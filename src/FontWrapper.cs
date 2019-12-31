using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM
{
    /// <summary>
    /// Helper class to store a font and a flag telling if the font is using symbol encoding. 
    /// </summary>
    internal class FontWrapper
    {
        public Font font;
        public bool useSymbolEncoding;

        public FontWrapper(Font font, bool useSymbolEncoding)
        {
            this.font = font;
            this.useSymbolEncoding = useSymbolEncoding;
        }
    }
}
