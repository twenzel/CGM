using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace codessentials.CGM.Commands
{
    public class ColourTable : Command
    {
        private int _startIndex;
        private Color[] _colors;

        public ColourTable(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 34, container))
        {
           
        }


        public ColourTable(CGMFile container, int startIndex, Color[] colors)
            :this(container)
        {
            _startIndex = startIndex;
            _colors = colors;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _startIndex = reader.ReadColorIndex();

            // don't assert here: there can be extra parameters. Why?!?

            var n = (reader.ArgumentsCount - reader.CurrentArg) / reader.SizeOfDirectColor();
            _colors = new Color[n];
            for (var i = 0; i < n; i++)
            {
                _colors[i] = reader.ReadDirectColor();
            }

            // don't ensure here -> see above
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteColorIndex(_startIndex);
            foreach (var col in _colors)
                writer.WriteDirectColor(col);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($"  colrtable {_startIndex} ");

            var first = true;
            foreach (var col in _colors)
            {
                if (!first)
                    writer.Write(",\n              ");

                if (first)
                    first = false;
                
                writer.Write(" "+WriteColor(col, _container.ColourModel));
            }

            writer.WriteLine(";");
        }

        public Color GetColor(int index)
        {
            return _colors[index + _startIndex];
        }

        public int StartIndex => _startIndex;
        public Color[] Colors => _colors;
    }
}