using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Security.Cryptography;
using System.Text;
using Serilog;

namespace Common
{
    public static partial class GlobalUtilities
    {
        /// <summary>
        /// Get the image bytes from a web request to an image at the provided URL
        /// </summary>
        /// <param name="url">The URL of the image</param>
        /// <returns>A byte array </returns>
        public static byte[] GetExternalImageBytes(string url)
        {
            WebResponse result = null;
            var bytes = new byte[0];

            try
            {
                WebRequest request  = WebRequest.Create(url);
                request.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
                result              = request.GetResponse();
                Stream stream       = result.GetResponseStream();
                BinaryReader br     = new BinaryReader(stream);
                byte[] rBytes       = br.ReadBytes(1000000);
                br.Close();
                result.Close();
                bytes               = new MemoryStream(rBytes, 0, rBytes.Length).ToArray();
            }
            catch
            {

            }
            finally
            {
                if (result != null) result.Close();
            }

            return bytes;
        }

        /// <summary>
        /// Get the bytes from a stream
        /// </summary>
        /// <param name="stream">The stream</param>
        /// <returns>A byte array </returns>
        public static byte[] GetBytesFromStream(Stream stream)
        {
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)bytes.Length);

            return bytes;
        }

        /// <summary>
        /// Get the image bytes from a web request to an image at the provided URL
        /// </summary>
        /// <param name="url">The URL of the image</param>
        /// <returns>A byte array </returns>
        public static string GetCustomerAvatarUrl(int customerID, AvatarType type = AvatarType.Default, bool cache = true, bool secure = true)
        {
            return "/profiles/avatar/{0}/{1}/{2}".FormatWith(customerID, AvatarType.Default, cache);
        }
        
        /// <summary>
        /// Resize an image so that the image dimensions fall between the provided maximum width and height.
        /// </summary>
        /// <param name="imageBytes">The image bytes</param>
        /// <param name="maxWidth">The maximum width of the image</param>
        /// <param name="maxHeight">The maximum height of the image</param>
        /// <returns></returns>
        public static byte[] ResizeImage(byte[] imageBytes, int maxWidth, int maxHeight)
        {
            using (var ms = new MemoryStream(imageBytes))
            using (var bmp = new Bitmap(ms))
            {
                ImageFormat format = bmp.RawFormat;
                decimal ratio;
                int newWidth = 0;
                int newHeight = 0;

                if (bmp.Width > maxWidth || bmp.Height > maxHeight)
                {
                    if (bmp.Width > bmp.Height)
                    {
                        ratio = (decimal)maxWidth / bmp.Width;
                        newWidth = maxWidth;
                        decimal lnTemp = bmp.Height * ratio;
                        newHeight = (int)lnTemp;
                    }
                    else
                    {
                        ratio = (decimal)maxHeight / bmp.Height;
                        newHeight = maxHeight;
                        decimal lnTemp = bmp.Width * ratio;
                        newWidth = (int)lnTemp;
                    }
                }

                if (newWidth == 0) newWidth = bmp.Width;
                if (newHeight == 0) newHeight = bmp.Height;

                using (var bmpOut = new Bitmap(newWidth, newHeight))
                using (var msOut = new MemoryStream())
                {
                    Graphics g = Graphics.FromImage(bmpOut);

                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
                    g.DrawImage(bmp, 0, 0, newWidth, newHeight);

                    bmpOut.Save(msOut, ImageFormat.Jpeg);

                    return (msOut.ToArray());
                }
            }
        }
        
        /// <summary>
        /// Resize an image so that the image dimensions fall between the provided maximum width and height.
        /// </summary>
        /// <param name="imageBytes">The image bytes</param>
        /// <param name="maxWidth">The maximum width of the image</param>
        /// <param name="maxHeight">The maximum height of the image</param>
        /// <returns></returns>
        public static byte[] ResizeImage(byte[] imageBytes, AvatarType type)
        {
            int maxWidth;
            int maxHeight;

            switch (type)
            {
                case AvatarType.Tiny:
                    maxWidth = maxHeight = 16;
                    break;
                case AvatarType.Small:
                    maxWidth = maxHeight = 50;
                    break;
                case AvatarType.Large:
                    maxWidth = maxHeight = 300;
                    break;
                case AvatarType.Default:
                default:
                    maxWidth = maxHeight = 100;
                    break;
            }

            using (var ms = new MemoryStream(imageBytes))
            using (var bmp = new Bitmap(ms))
            {
                ImageFormat format = bmp.RawFormat;
                decimal ratio;
                int newWidth = 0;
                int newHeight = 0;

                if (bmp.Width > maxWidth || bmp.Height > maxHeight)
                {
                    if (bmp.Width > bmp.Height)
                    {
                        ratio = (decimal)maxWidth / bmp.Width;
                        newWidth = maxWidth;
                        decimal lnTemp = bmp.Height * ratio;
                        newHeight = (int)lnTemp;
                    }
                    else
                    {
                        ratio = (decimal)maxHeight / bmp.Height;
                        newHeight = maxHeight;
                        decimal lnTemp = bmp.Width * ratio;
                        newWidth = (int)lnTemp;
                    }
                }

                if (newWidth == 0) newWidth = bmp.Width;
                if (newHeight == 0) newHeight = bmp.Height;

                using (var bmpOut = new Bitmap(newWidth, newHeight))
                using (var msOut = new MemoryStream())
                {
                    Graphics g = Graphics.FromImage(bmpOut);

                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.FillRectangle(Brushes.White, 0, 0, newWidth, newHeight);
                    g.DrawImage(bmp, 0, 0, newWidth, newHeight);

                    bmpOut.Save(msOut, ImageFormat.Jpeg);

                    return (msOut.ToArray());
                }
            }
        }
        
        /// <summary>
        /// Crops an image found at the provided URL to the provided dimensions, starting at the provided X and Y position.
        /// </summary>
        /// <param name="imageUrl">The URL where the image can be found at</param>
        /// <param name="width">The desired width of the cropped image</param>
        /// <param name="height">The desired height of the cropped image</param>
        /// <param name="X">The X position where the cropping will begin</param>
        /// <param name="Y">The Y position where the cropping will begin</param>
        /// <returns></returns>
        public static byte[] Crop(string imageUrl, int width, int height, int X, int Y)
        {
            // Download the image
            var webClient = new WebClient();
            byte[] imageBytes = webClient.DownloadData(imageUrl);

            return Crop(imageBytes, width, height, X, Y);
        }

        /// <summary>
        /// Crops a provided image to the provided dimensions, starting at the provided X and Y position.
        /// </summary>
        /// <param name="imageBytes">The image to be cropped</param>
        /// <param name="width">The desired width of the cropped image</param>
        /// <param name="height">The desired height of the cropped image</param>
        /// <param name="X">The X position where the cropping will begin</param>
        /// <param name="Y">The Y position where the cropping will begin</param>
        /// <returns></returns>
        public static byte[] Crop(byte[] imageBytes, int width, int height, int X, int Y)
        {
            // Convert the bytes into an Image
            MemoryStream ms = new MemoryStream(imageBytes);
            System.Drawing.Image returnImage = System.Drawing.Image.FromStream(ms);

            using (System.Drawing.Image OriginalImage = returnImage)
            {
                using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(width, height))
                {
                    bmp.SetResolution(OriginalImage.HorizontalResolution, OriginalImage.VerticalResolution);
                    using (System.Drawing.Graphics Graphic = System.Drawing.Graphics.FromImage(bmp))
                    {
                        Graphic.SmoothingMode = SmoothingMode.HighQuality;
                        Graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        Graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        Graphic.DrawImage(OriginalImage, new System.Drawing.Rectangle(0, 0, width, height), X, Y, width, height, System.Drawing.GraphicsUnit.Pixel);
                        MemoryStream nms = new MemoryStream();
                        bmp.Save(nms, OriginalImage.RawFormat);
                        return nms.GetBuffer();
                    }
                }
            }
        }

        private static bool AvatarExists(int customerID)
        {
            using (var conn = new SqlConnection(GlobalSettings.Exigo.Api.Sql.ConnectionStrings.SqlReporting))
            {
                conn.Open();

                var cmd = new SqlCommand("Select top 1 1 From ImageFiles Where Path=@FilePath", conn);
                cmd.Parameters.Add("@FilePath", System.Data.SqlDbType.NVarChar, 500).Value = "/customers/" + customerID;
                return cmd.ExecuteScalar() != null;
            }
        }

        public static bool InsertOrUpdateAvatarToReportingDatabase(int customerID, byte[] bytes)
        {
            try
            {
                var vectors = new int[] { 300, 100, 50, 16 };
                var files = new string[] { "avatar-lg.png", "avatar.png", "avatar-sm.png", "avatar-xs.png" };
                var sqlText = "";

                if (AvatarExists(customerID)) //update
                {
                    using (var conn = new SqlConnection(GlobalSettings.Exigo.Api.Sql.ConnectionStrings.SqlReporting))
                    {
                        conn.Open();
                        var cmd = new SqlCommand(sqlText, conn);
                        cmd.Parameters.Add("@FilePath", System.Data.SqlDbType.NVarChar, 500).Value = "/customers/" + customerID;
                        cmd.Parameters.Add("@Modified", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
                        for (int i = 0; i < files.Length; i++)
                        {
                            var localBytes = ResizeImage(bytes, vectors[i], vectors[i]);
                            cmd.Parameters.Add("@FileName" + i, System.Data.SqlDbType.NVarChar, 500).Value = files[i];
                            cmd.Parameters.Add("@File" + i, System.Data.SqlDbType.VarBinary).Value = localBytes;
                            sqlText += string.Format(" UPDATE ImageFiles SET ImageData = @File{0}, Size = {1}, ModifiedDate = @Modified WHERE Path=@FilePath AND Name=@FileName{0}", i, localBytes.Length);
                        }
                        cmd.CommandText = sqlText;
                        cmd.ExecuteNonQuery();
                    }
                }
                else //insert
                {
                    using (var conn = new SqlConnection(GlobalSettings.Exigo.Api.Sql.ConnectionStrings.SqlReporting))
                    {
                        conn.Open();
                        var cmd = new SqlCommand(sqlText, conn);
                        cmd.Parameters.Add("@FilePath", System.Data.SqlDbType.NVarChar, 500).Value = "/customers/" + customerID;
                        cmd.Parameters.Add("@Modified", System.Data.SqlDbType.DateTime).Value = DateTime.Now;
                        for (int i = 0; i < files.Length; i++)
                        {
                            var localBytes = ResizeImage(bytes, vectors[i], vectors[i]);
                            cmd.Parameters.Add("@FileName" + i, System.Data.SqlDbType.NVarChar, 500).Value = files[i];
                            cmd.Parameters.Add("@File" + i, System.Data.SqlDbType.VarBinary).Value = localBytes;
                            sqlText += string.Format(" INSERT INTO ImageFiles ([Path],[Name],[ModifiedDate],[Size],[ImageData]) VALUES (@FilePath,@FileName{0},@Modified,{1},@File{0}", i, localBytes.Length);
                        }
                        cmd.CommandText = sqlText;
                        cmd.ExecuteNonQuery();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error Updating Avatar Image");
                return false;
            }
        }

        public static bool InsertOrUpdateAvatarToAPI(int customerID, byte[] bytes)
        {
            var vectors = new int[] { 300, 100, 50, 16 };
            var files = new string[] { "avatar-lg.png", "avatar.png", "avatar-sm.png", "avatar-xs.png" };
            var imageApi = ExigoService.ExigoDAL.Images();
            var result = false;
            for (int i = 0; i < files.Length; i++)
                result = imageApi.SaveImage("/customers/" + customerID, files[i], ResizeImage(bytes, vectors[i], vectors[i]));
            return result;
        }
        public static string ImageToBase64(string path)
        {
            return "data:image/png;base64," + Convert.ToBase64String(File.ReadAllBytes(path));  
        }
    }
}