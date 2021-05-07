using dipl.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Key]
        public int Id { get; set; }
        public bool IsLiked { get; set; }
        public byte[] Image { get; set; }
        public string Name { get; private set; }
        public string SourceUrl { get; private set; }

        public Audio()
        {

        }

        public Audio(string SourceUrl, bool IsLiked)
        {
            this.SourceUrl = SourceUrl;
            this.IsLiked = IsLiked;
            Name = Path.GetFileNameWithoutExtension(SourceUrl);
            Image = GetImage();
        }

        public byte[] GetImage()
        {
            TagLib.File file_TAG;
            try
            {
                file_TAG = TagLib.File.Create(SourceUrl);
                if (file_TAG.Tag.Pictures.Length >= 1)
                {
                    return file_TAG.Tag.Pictures[0].Data.Data;
                }
                else
                {
                    return ((ImageSource)(new BitmapImage(new Uri("../../Assets/anime.jpg", UriKind.Relative)))).ToBytes();
                }
            }
            ///////////////////////////////////////////////////////////////////////////////////убрать когда подключу плеер
            catch { return ((ImageSource)(new BitmapImage(new Uri("../../Assets/anime.jpg", UriKind.Relative)))).ToBytes(); }
        }
    }
}
