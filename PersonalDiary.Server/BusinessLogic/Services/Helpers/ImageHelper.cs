using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace BusinessLogic.Services.Helpers
{
    public static class ImageHelper
    {
        public static Image Base64ToImage(string base645)
        {
            byte[] bytess = Encoding.Default.GetBytes(base645);
            string base64 = Encoding.UTF8.GetString(bytess);
            string StrAfterSlash = base64.Split('/')[1];
            string ImageFormat = StrAfterSlash.Split(';')[0];
            string CleanFaceData = base64.Replace($"data:image/{ImageFormat };base64,", string.Empty);
            CleanFaceData = CleanFaceData.Replace(' ', '+').Replace('-', '+').Replace('_', '/').PadRight(4 * ((CleanFaceData.Length + 3) / 4), '=');

            byte[] bytes = Convert.FromBase64String(CleanFaceData);
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            return image;
        }

        public static Image BytesToImage(byte[] bytes)
        {
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms, false, false);
            }

            return image;

        }

        public static string ImageToBase64(Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public static Image<Bgr, Byte> ImageToEmguCVImage(Image imageIn)
        {
            Bitmap bmpImage = new Bitmap(imageIn);
            Image<Bgr, Byte> imageOut = bmpImage.ToImage<Bgr, Byte>();

            return imageOut;
        }

        public static Image EmguCVImagetoImage(Image<Bgr, Byte> imageIn)
        {
            Bitmap bmpImage = imageIn.ToBitmap<Bgr, Byte>();
            Image imageOut = bmpImage;

            return imageOut;
        }
    }
}
