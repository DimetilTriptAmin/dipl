using dipl.Models.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Timers;
using WMPLib;

namespace dipl.Models
{
    public enum Status
    {
        Undefined,
        Paused,
        Stopped,
        Playing,
        Ended,
        Transitioning,
        Ready
    }


    public sealed class AudioPlayer
    {
        public readonly WindowsMediaPlayer wmp;
        public readonly Timer Timer;

        #region Props
        private ObservableCollection<Audio> _queue;
        public ObservableCollection<Audio> Queue
        {
            get => _queue;
            set
            {
                if (value.Count != 0)
                {
                    _queue = value;
                    OnQueueChanged();
                }
            }
        }
        public Audio CurrentAudio => Queue?.Count == 0? new Audio() : Queue?[CurrentIndex];
        public int CurrentIndex { get; set; }
        public int SavedVolume { get; set; }


        public Status PlayingStatus
        {
            get
            {
                switch (wmp.playState)
                {
                    case WMPPlayState.wmppsStopped:
                        return Status.Stopped;
                    case WMPPlayState.wmppsPaused:
                        return Status.Paused;
                    case WMPPlayState.wmppsPlaying:
                        return Status.Playing;
                    case WMPPlayState.wmppsMediaEnded:
                        return Status.Ended;
                    case WMPPlayState.wmppsTransitioning:
                        return Status.Transitioning;
                    case WMPPlayState.wmppsReady:
                        return Status.Ready;
                    default:
                        return Status.Undefined;
                }
            }
        }

        public double Position
        {
            get { return wmp.controls.currentPosition; }
            set { wmp.controls.currentPosition = value; }
        }

        public TimeSpan PositionTime => TimeSpan.FromMinutes((int)wmp.controls.currentPosition);

        public int Volume
        {
            get { return wmp.settings.volume; }
            set { wmp.settings.volume = value; }
        }

        public bool AutoNext { get; set; } = true;

        public bool AutoRestart { get; set; }
        #endregion

        public AudioPlayer(ObservableCollection<Audio> queue)
        {
            wmp = new WindowsMediaPlayer();
            Volume = 100;
            wmp.PlayStateChange += (e) =>
            {
                if (PlayingStatus == Status.Undefined)
                    return;
                PlayingStatusChanged?.Invoke();
            };


            Timer = new Timer() { Interval = 50 };
            Timer.Elapsed += (s, e) =>
            {
                ProgressChanged?.Invoke();

                if (PlayingStatus == Status.Stopped || PlayingStatus == Status.Ended)
                    ((Timer)s).Stop();

                if ((PlayingStatus == Status.Stopped || PlayingStatus == Status.Ended) && AutoRestart)
                {
                    SelectAudio(CurrentIndex);
                    return;
                }

                if ((PlayingStatus == Status.Stopped || PlayingStatus == Status.Ended) && AutoNext)
                    SelectAudio(++CurrentIndex);
            };
            Queue = queue;
            SelectAudio(0);
            Pause();
            Stop();
        }

        public void SelectAudio(int index)
        {

            CurrentIndex = index;

            if (Queue == null || Queue.Count==0)
                return;

            if (CurrentIndex >= Queue.Count)
                CurrentIndex = 0;

            if (CurrentIndex < 0)
                CurrentIndex = Queue.Count - 1;

            if (!File.Exists(CurrentAudio.SourceUrl))
            {
                DataHandler.DeleteAudio(CurrentAudio);
                Queue.RemoveAt(CurrentIndex);
                SelectAudio(CurrentIndex);
            }

            if (Queue.Count == 0)
                return;

            wmp.currentMedia = wmp.newMedia(CurrentAudio.SourceUrl);
            ProgressChanged?.Invoke();
            AudioSelected?.Invoke();
            if (App.CurrentAccount.Playlists[1].Audios.Count >= 30) App.CurrentAccount.Playlists[1].Audios.RemoveAt(29);
            if (!App.CurrentAccount.Playlists[1].Audios.Any(x => x.SourceUrl == CurrentAudio.SourceUrl))
            {
                App.CurrentAccount.Playlists[1].Audios.Add(CurrentAudio);
                DataHandler.AddAudio(App.CurrentAccount.Playlists[1], CurrentAudio);
            }
            Timer.Start();
            AutoNext = true;
        }

        #region Controls

        public void Play()
        {
            if (Queue == null || Queue.Count == 0)
                return;

            Timer.Start();
            wmp.controls.play();
        }

        public void Pause()
        {
            Timer.Stop();
            wmp.controls.pause();
        }

        public void Stop()
        {
            Timer.Stop();
            wmp.controls.stop();
        }

        public void Back()
        {
            if (this.PositionTime.TotalSeconds > 10) this.SelectAudio(this.CurrentIndex);
            else SelectAudio(CurrentIndex - 1);
        }

        public void Forward()
        {
            SelectAudio(CurrentIndex + 1);
        }

        public void MixPlaylist()
        {
            Random rnd = new Random();
            int irnd;
            Audio buff;

            for (int i = Queue.Count - 1; i > 0; i -= 2)
            {
                irnd = rnd.Next(0, i);
                buff = Queue[i];
                Queue[i] = Queue[irnd];
                Queue[irnd] = buff;
            }
            SelectAudio(0);
        }

        public void OnQueueChanged()
        {
            SelectAudio(0);
        }
        #endregion

        #region Events

        public event Action PlayingStatusChanged;

        public event Action ProgressChanged;

        public event Action AudioSelected;

        #endregion
    }
}
