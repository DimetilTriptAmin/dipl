﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace dipl.Models
{
    public sealed class Audio
    {
        public bool IsLiked { get; set; }
        public ImageSource Image { get; set; }
        public string Name { get; private set; }
        public string SourceUrl { get; private set; }

        public Audio(string sourceurl, bool isLiked)
        {
            SourceUrl = sourceurl;
            IsLiked = isLiked;
            Name = Path.GetFileNameWithoutExtension(SourceUrl);
            Image = GetImage();
        }

        public ImageSource GetImage()
        {
            TagLib.File file_TAG;
            try
            {
                file_TAG = TagLib.File.Create(SourceUrl);
                if (file_TAG.Tag.Pictures.Length >= 1)
                {
                    var bin = file_TAG.Tag.Pictures[0].Data.Data;
                    var imageSource = new BitmapImage();
                    imageSource.BeginInit();
                    imageSource.StreamSource = new MemoryStream(bin);
                    imageSource.EndInit();
                    return imageSource;
                }
                else
                {
                    return new BitmapImage(new Uri("../../Assets/lp.jpg", UriKind.Relative));
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////убрать когда подключу плеер
            catch { return new BitmapImage(new Uri("../../Assets/lp.jpg", UriKind.Relative)); }
        }
    }
}
