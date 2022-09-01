using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WPF.Drawing.Demo.Utility
{
    public static class BitmapUtility
    {
        /// <summary>
        /// 将图片源的某个区域剪切到目标图片的指定区域
        /// </summary>
        /// <param name="source">源图</param>
        /// <param name="sourceRect">被剪切区域</param>
        /// <param name="des">目标图</param>
        /// <param name="desRect">目标图区域</param>
        public static void Cut(WriteableBitmap source, Rect sourceRect, WriteableBitmap des, Rect desRect)
        {
            try
            {
                des.Clear();

                using (des.GetBitmapContext())
                {
                    using (source.GetBitmapContext())
                    {
                        des.Blit(desRect, source, sourceRect, WriteableBitmapExtensions.BlendMode.Additive);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
