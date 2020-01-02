using System.Collections.Generic;
using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=32
    /// </summary>
    public class PatternTable : Command
    {
        public int Index { get; set; }
        public int Nx { get; set; }
        public int Ny { get; set; }
        public int LocalColorPrecision { get; set; }
        public List<CgmColor> Colors { get; set; } = new List<CgmColor>();

        public PatternTable(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 32, container))
        {

        }

        public PatternTable(CgmFile container, int index, int nx, int ny, int localColorPrecision, IEnumerable<CgmColor> colors)
            : this(container)
        {
            Index = index;
            Nx = nx;
            Ny = ny;
            LocalColorPrecision = localColorPrecision;
            Colors.AddRange(colors);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Index = reader.ReadIndex();
            Nx = reader.ReadInt();
            Ny = reader.ReadInt();
            LocalColorPrecision = reader.ReadInt();

            var nColor = Nx * Ny;
            for (var i = 0; i < nColor; i++)
                Colors.Add(reader.ReadColor(LocalColorPrecision));
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex(Index);
            writer.WriteInt(Nx);
            writer.WriteInt(Ny);
            writer.WriteInt(LocalColorPrecision);
            foreach (var val in Colors)
                writer.WriteColor(val, LocalColorPrecision);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" PATTABLE {WriteIndex(Index)} {WriteInt(Nx)} {WriteInt(Ny)}");

            if (LocalColorPrecision == 0)
            {
                if (_container.ColourSelectionMode == ColourSelectionMode.Type.INDEXED)
                    writer.Write(ColourIndexPrecision.WriteValue(_container.ColourIndexPrecision));
                else
                    writer.Write(ColourPrecision.WritValue(_container.ColourPrecision));
            }
            else
                writer.Write(WriteInt(LocalColorPrecision));

            foreach (var color in Colors)
                writer.Write($" {WriteColor(color)}");

            writer.WriteLine(";");
        }
    }
}
