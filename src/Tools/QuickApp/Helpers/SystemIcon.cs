using System;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing.Imaging;
using Microsoft.Win32;

namespace QuickApp.Helpers
{
    /// <summary>
    /// 系统文件(夹)图标
    /// </summary>
    public class SystemIcon
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }
        [DllImport("Shell32.dll", EntryPoint = "SHGetFileInfo", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(string pszPath, uint dwFileAttributes, ref SHFILEINFO psfi, uint cbFileInfo, uint uFlags);
        [DllImport("User32.dll", EntryPoint = "DestroyIcon")]
        public static extern int DestroyIcon(IntPtr hIcon);
        [DllImport("shell32.DLL", EntryPoint = "ExtractAssociatedIcon")]
        private static extern int ExtractAssociatedIconA(int hInst, string lpIconPath, ref int lpiIcon); //声明函数
        private static System.IntPtr thisHandle;

        #region API参数的常量定义

        public enum FileInfoFlags : uint
        {
            SHGFI_ICON = 0x000000100,                 //get icon
            SHGFI_DISPLAYNAME = 0x000000200,          //get display name
            SHGFI_TYPENAME = 0x000000400,             //get type name
            SHGFI_ATTRIBUTES = 0x000000800,           //get attributes
            SHGFI_ICONLOCATION = 0x000001000,         //get icon location
            SHGFI_EXETYPE = 0x000002000,              //get exe type
            SHGFI_SYSICONINDEX = 0x000004000,         //get system icon index
            SHGFI_LINKOVERLAY = 0x000008000,          //put a link overlay on icon
            SHGFI_SELECTED = 0x000010000,             //show icon in selected state
            SHGFI_ATTR_SPECIFIED = 0x000020000,       //get only specified attributes
            SHGFI_LARGEICON = 0x000000000,            //get large icon
            SHGFI_SMALLICON = 0x000000001,            //get small icon
            SHGFI_OPENICON = 0x000000002,             //get open icon
            SHGFI_SHELLICONSIZE = 0x000000004,        //get shell size icon
            SHGFI_PIDL = 0x000000008,                 //pszPath is a pidl
            SHGFI_USEFILEATTRIBUTES = 0x0000000010,   //use passed dwFileAttribute
            SHGFI_ADDOVERLAYS = 0x000000020,          //apply the appropriate overlays
            SHGFI_OVERLAYINDEX = 0x000000040          //Get the index of the overlya
        }

        public enum FileAttributeFlags : uint
        {
            FILE_ATTRIBUTE_ReadOnly = 0x00000001,
            FILE_ATTRIBUTE_HIDDEN = 0x00000002,
            FILE_ATTRIBUTE_SYSTEM = 0x00000004,
            FILE_ATTRIBUTE_DIRECTORY = 0x00000010,
            FILE_ATTRIBUTE_ARCHIVE = 0x00000020,
            FILE_ATTRIBUTE_DEVICE = 0x00000040,
            FILE_ATTRIBUTE_NORMAL = 0x00000080,
            FILE_ATTRIBUTE_TEMPORARY = 0x00000100,
            FILE_ATTRIBUTE_SPARSE_FILE = 0x00000200,
            FILE_ATTRIBUTE_REPARSE_POINT = 0x00000400,
            FILE_ATTRIBUTE_COMPRESSED = 0x00000800,
            FILE_ATTRIBUTE_OFFLINE = 0x00001000,
            FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 0x00002000,
            FILE_ATTRIBUTE_ENCRYPTED = 0x00004000
        }
        /// <summary>
        /// 获取文件(夹)的关联图标
        /// </summary>
        /// <param name="isLargeIcon">是否返回大图标</param>
        /// <param name="fileName">文件名，为空返回文件夹关联图标，否则返回对应文件关联图标</param>
        /// <returns>获取到的图标</returns>
        public static ImageSource GetImageSource(bool isLargeIcon, string fileName = "")
        {
            Icon icon = GetIcon(isLargeIcon, fileName);
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                new Int32Rect(0, 0, icon.Width, icon.Height),
                BitmapSizeOptions.FromEmptyOptions());
        }


        /// <summary>
        /// 获取文件(夹)的关联图标
        /// </summary>
        /// <param name="isLargeIcon">是否返回大图标</param>
        /// <param name="fileName">文件名，为空返回文件夹关联图标，否则返回对应文件关联图标</param>
        /// <returns>获取到的图标</returns>
        public static Icon GetIcon(bool isLargeIcon, string fileName = "")
        {
            uint uFlags = (uint)FileInfoFlags.SHGFI_ICON;
            if (isLargeIcon)
            {
                uFlags |= (uint)FileInfoFlags.SHGFI_LARGEICON;
            }
            else
            {
                uFlags |= (uint)FileInfoFlags.SHGFI_SMALLICON;
            }
            if (string.IsNullOrWhiteSpace(fileName) == false)
            {
                uFlags |= (uint)FileInfoFlags.SHGFI_USEFILEATTRIBUTES;
            }
            return GetIcon(fileName, uFlags);
        }

        /// <summary>
        /// 获取文件(夹)关联图标
        /// </summary>
        /// <param name="pszPath">文件(夹)名称</param>
        /// <param name="uFlags"></param>
        /// <returns>图标</returns>
        private static Icon GetIcon(string pszPath, uint uFlags)
        {
            SHFILEINFO shfi = new SHFILEINFO();
            IntPtr hI = SHGetFileInfo(pszPath, 0, ref shfi, (uint)Marshal.SizeOf(shfi),
                    uFlags);
            if (hI.Equals(IntPtr.Zero))
            {
                return null;
            }
            Icon icon = Icon.FromHandle(shfi.hIcon).Clone() as Icon;
            DestroyIcon(shfi.hIcon);        //释放资源
            return icon;
        }

        public static System.Drawing.Icon GetLnkIcon(string s)//S是要获取文件路径，返回ico格式文件
        {
            int RefInt = 0;
            thisHandle = new IntPtr(ExtractAssociatedIconA(0, s, ref RefInt));
            return System.Drawing.Icon.FromHandle(thisHandle);
        }

        public static ImageSource GetImageSourceFromLnk(string s)
        {
            var icon = GetLnkIcon(s);
            return GetImageSourceFromIcon(icon);
        }


        #endregion

        #region BitmapImage与byte[]相互转换

        /// <summary>
        /// 获取图标资源
        /// </summary>
        /// <param name="imgFilePath"></param>
        /// <returns></returns>
        public static ImageSource GetImageSource(string imgFilePath)
        {
            BitmapFrame imgSource = null;
            using (FileStream fs = new FileStream(imgFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ImageSourceConverter isc = new ImageSourceConverter();
                byte[] arr = new byte[fs.Length];
                fs.Read(arr, 0, (int)fs.Length);
                MemoryStream stream = new MemoryStream(arr);
                imgSource = isc.ConvertFrom(stream) as BitmapFrame;
            }
            return imgSource;
        }


        public static ImageSource GetImageSourceFromIcon(Icon icon)
        {
            return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                icon.Handle,
                new Int32Rect(0, 0, icon.Width, icon.Height),
                BitmapSizeOptions.FromEmptyOptions());
        }

        //byte[]转换为BitmapImage
        public static BitmapImage ByteArrayToBitmapImage(byte[] byteArrasy)
        {
            BitmapImage bmp = null;
            try
            {
                bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.StreamSource = new MemoryStream(byteArrasy);
                bmp.EndInit();
            }
            catch
            {
                bmp = null;
            }

            return bmp;
        }

        //BitmapImage转换为byte[]
        public static byte[] BitmapImageToByteArray(string path)
        {
            byte[] byteArray = null;

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    byteArray = new byte[fs.Length];
                    fs.Read(byteArray, 0, byteArray.Length);
                }
            }
            catch (Exception)
            {
                byteArray = null;
            }

            return byteArray;
        }

        //BitmapImage转换为byte[]
        public static byte[] BitmapImageToByteArray(BitmapImage bmp)
        {
            byte[] byteArray = null;

            try
            {
                Stream sMarket = bmp.StreamSource;
                if (sMarket != null && sMarket.Length > 0)
                {
                    //行重要，因为Position经常位于Stream的末尾，导致下面读取到的长度为0
                    sMarket.Position = 0;
                    using (BinaryReader br = new BinaryReader(sMarket))
                    {
                        byteArray = br.ReadBytes((int)sMarket.Length);
                    }
                }
            }
            catch
            {
                byteArray = null;
            }

            return byteArray;
        }

        /// <summary>
        /// 缩放图片
        /// </summary>
        /// <param name="oldData"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <returns></returns>
        public static byte[] ResizeImage(byte[] oldData, int newWidth, int newHeight)
        {
            try
            {
                string tmpFile1 = System.IO.Path.GetTempFileName() + ".png";
                string tmpFile2 = System.IO.Path.GetTempFileName() + ".png";
                using (FileStream fs = new FileStream(tmpFile1, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    fs.Write(oldData, 0, oldData.Length);
                }
                GenerateHighThumbnail(tmpFile1, tmpFile2, 16, 16);
                byte[] newData = null;
                using (FileStream fs = new FileStream(tmpFile2, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    newData = new byte[fs.Length];
                    fs.Read(newData, 0, newData.Length);
                }
                if (System.IO.File.Exists(tmpFile1))
                    System.IO.File.Delete(tmpFile1);
                if (System.IO.File.Exists(tmpFile2))
                    System.IO.File.Delete(tmpFile2);
                return newData;
            }
            catch
            {

            }
            return null;
        }

        public static void GenerateHighThumbnail(string oldImagePath, string newImagePath, int width, int height)
        {
            System.Drawing.Image oldImage = System.Drawing.Image.FromFile(oldImagePath);
            int newWidth = AdjustSize(width, height, oldImage.Width, oldImage.Height).Width;
            int newHeight = AdjustSize(width, height, oldImage.Width, oldImage.Height).Height;
            //。。。。。。。。。。。
            System.Drawing.Image thumbnailImage = oldImage.GetThumbnailImage(newWidth, newHeight, new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback), IntPtr.Zero);
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(thumbnailImage);
            //处理JPG质量的函数
            System.Drawing.Imaging.ImageCodecInfo ici = GetEncoderInfo("image/jpeg");
            if (ici != null)
            {
                System.Drawing.Imaging.EncoderParameters ep = new System.Drawing.Imaging.EncoderParameters(1);
                ep.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)100);
                bm.Save(newImagePath, ici, ep);
                //释放所有资源，不释放，可能会出错误。
                ep.Dispose();
                ep = null;
            }
            ici = null;
            bm.Dispose();
            bm = null;
            thumbnailImage.Dispose();
            thumbnailImage = null;
            oldImage.Dispose();
            oldImage = null;
        }
        private static bool ThumbnailCallback()
        {
            return false;
        }
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        public struct PicSize
        {
            public int Width;
            public int Height;
        }
        public static PicSize AdjustSize(int spcWidth, int spcHeight, int orgWidth, int orgHeight)
        {
            PicSize size = new PicSize();
            // 原始宽高在指定宽高范围内，不作任何处理 
            if (orgWidth <= spcWidth && orgHeight <= spcHeight)
            {
                size.Width = orgWidth;
                size.Height = orgHeight;
            }
            else
            {
                // 取得比例系数 
                float w = orgWidth / (float)spcWidth;
                float h = orgHeight / (float)spcHeight;
                // 宽度比大于高度比 
                if (w > h)
                {
                    size.Width = spcWidth;
                    size.Height = (int)(w >= 1 ? Math.Round(orgHeight / w) : Math.Round(orgHeight * w));
                }
                // 宽度比小于高度比 
                else if (w < h)
                {
                    size.Height = spcHeight;
                    size.Width = (int)(h >= 1 ? Math.Round(orgWidth / h) : Math.Round(orgWidth * h));
                }
                // 宽度比等于高度比 
                else
                {
                    size.Width = spcWidth;
                    size.Height = spcHeight;
                }
            }
            return size;
        }

        #endregion
    }
}
