using System;
using System.Drawing;
using System.IO;

namespace MineX.Common.Utils
{
    public class BitmapConvert
    {
        public static Bitmap FromBase64String(string data)
        {
            byte[] bytes = Convert.FromBase64String(data);
            using (var stream = new MemoryStream(bytes))
            {
                return new Bitmap(stream);
            }
        }
    }
}
