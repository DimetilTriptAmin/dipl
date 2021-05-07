using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace dipl.Models.Data
{
	public static class ByteImageConverter
	{
		public static ImageSource ToImage(this byte[] imageData)
		{
			BitmapImage biImg = new BitmapImage();
			MemoryStream ms = new MemoryStream(imageData);
			biImg.BeginInit();
			biImg.StreamSource = ms;
			biImg.EndInit();

			ImageSource imgSrc = biImg as ImageSource;

			return imgSrc;
		}
	}

	public static class ImageByteConverter
	{
		public static byte[] ToBytes(this ImageSource imageSource)
		{
			byte[] bytes = null;
			var bitmapSource = imageSource as BitmapSource;

			if (bitmapSource != null)
			{
				var encoder = new JpegBitmapEncoder();
				encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

				using (var stream = new MemoryStream())
				{
					encoder.Save(stream);
					bytes = stream.ToArray();
				}
			}

			return bytes;
		}
	}
}
