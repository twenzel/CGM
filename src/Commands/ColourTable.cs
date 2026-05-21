using System.Collections.Generic;
using System.Drawing;

namespace codessentials.CGM.Commands
{
    public class ColourTable : Command
    {
        public int StartIndex { get; private set; }
        public Color[] Colors { get; private set; }
        public static Dictionary<int, Color> ColorTable { get; private set; } = new Dictionary<int, Color>();

        public ColourTable(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 34, container))
        {

        }


        public ColourTable(CgmFile container, int startIndex, Color[] colors)
            : this(container)
        {
            StartIndex = startIndex;
            Colors = colors;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            StartIndex = reader.ReadColorIndex();

            // don't assert here: there can be extra parameters. Why?!?

            var n = (reader.ArgumentsCount - reader.CurrentArg) / reader.SizeOfDirectColor();
            Colors = new Color[n];
            for (var i = 0; i < n; i++)
            {
                Colors[i] = reader.ReadDirectColor();

                var colorTableIndex = StartIndex + i;
                if (ColorTable.ContainsKey(colorTableIndex))
                {
                    ColorTable[colorTableIndex] = Colors[i];
                }
                else
                {
                    ColorTable.Add(colorTableIndex, Colors[i]);
                }
            }

            // don't ensure here -> see above
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteColorIndex(StartIndex);
            foreach (var col in Colors)
                writer.WriteDirectColor(col);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($"  colrtable {StartIndex} ");

            var first = true;
            foreach (var col in Colors)
            {
                if (!first)
                    writer.Write(",\n              ");

                if (first)
                    first = false;

                writer.Write(" " + WriteColor(col, _container.ColourModel));
            }

            writer.WriteLine(";");
        }

        public Color GetColor(int index)
        {
            return ColorTable[index];
        }
    }
}
