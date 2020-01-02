using codessentials.CGM.Classes;
using System.Text;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=9
    /// </summary>
    public class CellArray : Command
    {
        public int RepresentationFlag { get; private set; }
        public int Nx { get; private set; }
        public int Ny { get; private set; }
        public CGMPoint P { get; private set; }
        public CGMPoint Q { get; private set; }
        public CGMPoint R { get; private set; }
        public int LocalColorPrecision { get; private set; }

        /// <summary>
        /// either the colors are filled or the colorIndexes depending on the color selection mode
        /// </summary>
        public CGMColor[] Colors { get; private set; }

        public CellArray(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 9, container))
        {

        }

        public CellArray(CGMFile container, int repesentationFlag, int nx, int ny, CGMPoint p, CGMPoint q, CGMPoint r, int localColorPrecision, CGMColor[] colors)
            : this(container)
        {
            RepresentationFlag = repesentationFlag;
            Nx = nx;
            Ny = ny;
            P = p;
            Q = q;
            R = r;
            LocalColorPrecision = localColorPrecision;
            Colors = colors;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            // 3P, 3I, E, CLIST
            P = reader.ReadPoint();
            Q = reader.ReadPoint();
            R = reader.ReadPoint();
            Nx = reader.ReadInt(); // number of cells per row
            Ny = reader.ReadInt(); // number of rows

            LocalColorPrecision = reader.ReadInt();
            var localColorPrecision = LocalColorPrecision;

            if (localColorPrecision == 0)
            {
                if (_container.ColourSelectionMode == ColourSelectionMode.Type.INDEXED)
                    localColorPrecision = _container.ColourIndexPrecision;
                else
                    localColorPrecision = _container.ColourPrecision;
            }

            RepresentationFlag = reader.ReadEnum();

            readColorList(localColorPrecision, reader);
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WritePoint(P);
            writer.WritePoint(Q);
            writer.WritePoint(R);
            writer.WriteInt(Nx);
            writer.WriteInt(Ny);
            writer.WriteInt(LocalColorPrecision);
            writer.WriteEnum(RepresentationFlag);

            WriteColorList(LocalColorPrecision, writer);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($"  CELLARRAY {WritePoint(P)} {WritePoint(Q)} {WritePoint(R)}, {WriteInt(Nx)}, {WriteInt(Ny)}, ");

            if (LocalColorPrecision == 0)
            {
                if (_container.ColourSelectionMode == ColourSelectionMode.Type.INDEXED)
                    writer.Write(ColourIndexPrecision.WriteValue(_container.ColourIndexPrecision));
                else
                    writer.Write(ColourPrecision.WritValue(_container.ColourPrecision));
            }
            else
                writer.Write(WriteInt(LocalColorPrecision));

            for (var i = 0; i < Colors.Length; i++)
            {
                writer.Write($" {WriteColor(Colors[i])}");
                if (i < Colors.Length - 1)
                    writer.Write(" ");
            }

            writer.WriteLine(";");
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("CellArray");
            sb.Append(" nx=").Append(Nx);
            sb.Append(" ny=").Append(Ny);
            sb.Append(" representation flag=").Append(RepresentationFlag);
            sb.Append(" p=").Append(P).Append(",");
            sb.Append(" q=").Append(Q).Append(",");
            sb.Append(" r=").Append(R);

            return sb.ToString();
        }

        private void readColorList(int localColorPrecision, IBinaryReader reader)
        {
            var nColor = Nx * Ny;
            Colors = new CGMColor[nColor];

            if (RepresentationFlag == 0)
            {
                // run length list mode
                var c = 0;
                while (c < nColor)
                {
                    var numColors = reader.ReadInt();
                    var color = reader.ReadColor(localColorPrecision);

                    // don't directly fill the array with numColors in case we
                    // encounter a erroneous CGM file, e.g. SCHEMA03.CGM that
                    // returns an incorrect number of colors; only fill at most
                    // the number of colors left in the array
                    var maxIndex = System.Math.Min(numColors, nColor - c);
                    for (var i = 0; i < maxIndex; i++)
                    {
                        Colors[c++] = color;
                    }

                    if (c > 0 && c % Nx == 0)
                    {
                        // align on word at the end of a line
                        reader.AlignOnWord();
                    }
                }
            }
            else if (RepresentationFlag == 1)
            {
                // packed list mode
                var i = 0;
                for (var row = 0; row < Ny; row++)
                {
                    for (var col = 0; col < Nx; col++)
                    {
                        Colors[i++] = reader.ReadColor(localColorPrecision);
                    }
                    // align on word
                    reader.AlignOnWord();
                }
            }
            else
            {
                reader.Unsupported("unsupported representation flag " + RepresentationFlag);
            }

        }

        private void WriteColorList(int localColorPrecision, IBinaryWriter writer)
        {
            // run length list mode
            if (RepresentationFlag == 0)
            {
                var c = 0;
                foreach (var col in Colors)
                {
                    writer.WriteInt(1);
                    writer.WriteColor(col, LocalColorPrecision);
                    c++;

                    if (c > 0 && c % Nx == 0)
                    {
                        // align on word at the end of a line
                        writer.FillToWord();
                    }
                }

            }
            // packed list mode
            else if (RepresentationFlag == 1)
            {
                // packed list mode
                var i = 0;
                for (var row = 0; row < Ny; row++)
                {
                    for (var col = 0; col < Nx; col++)
                    {
                        writer.WriteColor(Colors[i++], localColorPrecision);
                    }
                    // align on word
                    writer.FillToWord();
                }
            }
            else
                writer.Unsupported("unsupported representation flag " + RepresentationFlag);
        }
    }
}