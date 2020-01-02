using System.Collections.Generic;
using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=27
    /// </summary>
    public class PolySymbol : Command
    {
        public int Index { get; set; }
        public List<CgmPoint> Points { get; set; } = new List<CgmPoint>();

        public PolySymbol(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 27, container))
        {

        }

        public PolySymbol(CgmFile container, int index, CgmPoint[] points)
            : this(container)
        {
            Index = index;
            Points.AddRange(points);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Index = reader.ReadIndex();
            while (reader.CurrentArg < reader.Arguments.Length)
            {
                Points.Add(reader.ReadPoint());
            }
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex(Index);
            foreach (var p in Points)
                writer.WritePoint(p);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" SYMBOL {WriteIndex(Index)}");

            foreach (var p in Points)
                writer.Write($" {WritePoint(p)}");

            writer.WriteLine(";");
        }
    }
}
