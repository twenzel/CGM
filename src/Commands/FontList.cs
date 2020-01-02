using System.Collections.Generic;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=1, Element=13
    /// </summary>
    public class FontList : Command
    {
        public List<string> FontNames { get; set; } = new List<string>();

        //private FontWrapper[] _fonts;

        //private static Dictionary<string, FontWrapper> _fontMapping;
        //private static float DEFAULT_FONT_SIZE = 32;

        //static FontList()
        //{            
        //    _fontMapping = new Dictionary<string, FontWrapper>();
        //    _fontMapping.Add("times-roman", new FontWrapper(new Font(FontFamily.GenericSerif, DEFAULT_FONT_SIZE, FontStyle.Regular), false));
        //    _fontMapping.Add("times-bold", new FontWrapper(new Font(FontFamily.GenericSerif, DEFAULT_FONT_SIZE, FontStyle.Bold), false));
        //    _fontMapping.Add("times-italic", new FontWrapper(new Font(FontFamily.GenericSerif, DEFAULT_FONT_SIZE, FontStyle.Italic), false));
        //    _fontMapping.Add("times-bolditalic", new FontWrapper(new Font(FontFamily.GenericSerif, DEFAULT_FONT_SIZE, FontStyle.Bold | FontStyle.Italic), false));
        //    _fontMapping.Add("times-bold-italic", new FontWrapper(new Font(FontFamily.GenericSerif, DEFAULT_FONT_SIZE, FontStyle.Bold | FontStyle.Italic), false));

        //    _fontMapping.Add("helvetica", new FontWrapper(new Font(FontFamily.GenericSansSerif, DEFAULT_FONT_SIZE, FontStyle.Regular), false));
        //    _fontMapping.Add("helvetica-bold", new FontWrapper(new Font(FontFamily.GenericSansSerif, DEFAULT_FONT_SIZE, FontStyle.Bold), false));
        //    _fontMapping.Add("helvetica-oblique", new FontWrapper(new Font(FontFamily.GenericSansSerif, DEFAULT_FONT_SIZE, FontStyle.Italic), false));
        //    _fontMapping.Add("helvetica-boldoblique", new FontWrapper(new Font(FontFamily.GenericSansSerif, DEFAULT_FONT_SIZE, FontStyle.Bold | FontStyle.Italic), false));
        //    _fontMapping.Add("helvetica-bold-oblique", new FontWrapper(new Font(FontFamily.GenericSansSerif, DEFAULT_FONT_SIZE, FontStyle.Bold | FontStyle.Italic), false));

        //    _fontMapping.Add("courier", new FontWrapper(new Font(FontFamily.GenericMonospace, DEFAULT_FONT_SIZE, FontStyle.Regular), false));
        //    _fontMapping.Add("courier-bold", new FontWrapper(new Font(FontFamily.GenericMonospace, DEFAULT_FONT_SIZE, FontStyle.Bold), false));
        //    _fontMapping.Add("courier-italic", new FontWrapper(new Font(FontFamily.GenericMonospace, DEFAULT_FONT_SIZE, FontStyle.Italic), false));
        //    _fontMapping.Add("courier-oblique", new FontWrapper(new Font(FontFamily.GenericMonospace, DEFAULT_FONT_SIZE, FontStyle.Italic), false));
        //    _fontMapping.Add("courier-bolditalic", new FontWrapper(new Font(FontFamily.GenericMonospace, DEFAULT_FONT_SIZE, FontStyle.Bold | FontStyle.Italic), false));
        //    _fontMapping.Add("courier-boldoblique", new FontWrapper(new Font(FontFamily.GenericMonospace, DEFAULT_FONT_SIZE, FontStyle.Bold | FontStyle.Italic), false));
        //    _fontMapping.Add("courier-bold-italic", new FontWrapper(new Font(FontFamily.GenericMonospace, DEFAULT_FONT_SIZE, FontStyle.Bold | FontStyle.Italic), false));
        //    _fontMapping.Add("courier-bold-oblique", new FontWrapper(new Font(FontFamily.GenericMonospace, DEFAULT_FONT_SIZE, FontStyle.Bold | FontStyle.Italic), false));

        //    // this has to be a font that is able to display all characters, typically a unicode font
        //    _fontMapping.Add("symbol", new FontWrapper(new Font(FontFamily.GenericSerif, DEFAULT_FONT_SIZE, FontStyle.Regular), true));
        //}

        public FontList(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 13, container))
        {

        }

        public FontList(CGMFile container, string[] fonts)
            : this(container)
        {
            FontNames.AddRange(fonts);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            //int count = 0, i = 0;
            //while (i < reader.Arguments.Length)
            //{
            //    count++;
            //    i += reader.Arguments[i] + 1;
            //}

            //_fontNames = new string[count];
            //count = 0;
            //i = 0;
            //while (i < reader.Arguments.Length)
            //{
            //    char[] a = new char[reader.Arguments[i]];
            //    for (int j = 0; j < reader.Arguments[i]; j++)
            //        a[j] = (char)reader.Arguments[i + j + 1];
            //    _fontNames[count] = new string(a);
            //    count++;
            //    i += reader.Arguments[i] + 1;
            //}

            while (reader.CurrentArg < reader.ArgumentsCount)
                FontNames.Add(reader.ReadFixedString());

            //FontNames = new string[reader.ArgumentsCount];

            //for (int i = 0; i < reader.ArgumentsCount; i++)
            //{
            //    FontNames[i] = reader.ReadFixedString();
            //}






            //_fonts = new FontWrapper[_fontNames.Length];
            //i = 0;
            //foreach (var fontName in _fontNames)
            //{
            //    var key = normalizeFontName(fontName);

            //    if (_fontMapping.ContainsKey(key))
            //    {
            //        _fonts[i++] = _fontMapping[key];
            //    }
            //    else {
            //        FontFamily decodedFont = FontFamily.Families.FirstOrDefault( f => f.Name == fontName);
            //        FontStyle style = FontStyle.Regular;

            //        if (decodedFont == null && fontName.Contains(" "))
            //        {
            //            var values = fontName.Split(' ');
            //            var name = values[0];    
            //            decodedFont = FontFamily.Families.FirstOrDefault(f => f.Name == name);

            //            for(int v = 1; v<values.Length; v++)
            //            {
            //                if (values[v].Equals("bold", System.StringComparison.OrdinalIgnoreCase))
            //                    style = style | FontStyle.Bold;

            //                if (values[v].Equals("italic", System.StringComparison.OrdinalIgnoreCase) || values[v].Equals("oblique", System.StringComparison.OrdinalIgnoreCase))
            //                    style = style | FontStyle.Italic;
            //            }
            //        }

            //        if (decodedFont != null)
            //            // XXX: assume non symbolic encoding, is that right?
            //            _fonts[i++] = new FontWrapper(new Font(decodedFont, DEFAULT_FONT_SIZE, style), false);
            //        else
            //            _fonts[i++] = new FontWrapper(new Font(fontName, DEFAULT_FONT_SIZE), false);
            //    }
            //}
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            foreach (var name in FontNames)
                writer.WriteFixedString(name);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" fontlist '{string.Join("', '", FontNames)}';");
        }

        public override string ToString()
        {
            return "FontList " + string.Join(", ", FontNames);
        }
    }
}