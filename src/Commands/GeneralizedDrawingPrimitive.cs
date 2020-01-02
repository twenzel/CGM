using codessentials.CGM.Classes;
using System.Collections.Generic;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=10
    /// </summary>
    public class GeneralizedDrawingPrimitive : Command
    {
        public int Identifier { get; set; }
        public List<CgmPoint> Points { get; set; } = new List<CgmPoint>();
        public string DataRecord { get; set; }

        public GeneralizedDrawingPrimitive(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 10, container))
        {

        }

        public GeneralizedDrawingPrimitive(CgmFile container, int id, CgmPoint[] points, string data)
            : this(container)
        {
            Identifier = id;
            Points.AddRange(points);
            DataRecord = data;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Identifier = reader.ReadInt();
            var numberOfPoints = reader.ReadInt();

            for (var i = 0; i < numberOfPoints; i++)
            {
                Points.Add(reader.ReadPoint());
            }

            DataRecord = reader.ReadString();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteInt(Identifier);
            writer.WriteInt(Points.Count);
            foreach (var p in Points)
                writer.WritePoint(p);
            writer.WriteString(DataRecord);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" GDP {WriteInt(Identifier)}");

            foreach (var p in Points)
                writer.Write($" {WritePoint(p)}");

            writer.WriteLine($" {WriteString(DataRecord)};");
        }
    }
}