using System.Collections.Generic;
using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=8
    /// </summary>
    public class PolygonSet : Command
    {
        public enum EdgeFlag
        {
            INVIS = 0,
            VIS = 1,
            CLOSEINVIS = 2,
            CLOSEVIS = 3,
        }

        public List<KeyValuePair<EdgeFlag, CgmPoint>> Set { get; set; } = new List<KeyValuePair<EdgeFlag, CgmPoint>>();

        public PolygonSet(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 8, container))
        {

        }

        public PolygonSet(CgmFile container, IEnumerable<KeyValuePair<EdgeFlag, CgmPoint>> values)
            : this(container)
        {
            Set.AddRange(values);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Assert(reader.Arguments.Length % (reader.SizeOfPoint() + reader.SizeOfEnum()) == 0, "Invalid amount of arguments");
            var n = reader.Arguments.Length / (reader.SizeOfPoint() + reader.SizeOfEnum());

            for (var i = 0; i < n; i++)
            {
                var p = reader.ReadPoint();
                var edgeOutFlag = (EdgeFlag)reader.ReadEnum();
                Set.Add(new KeyValuePair<EdgeFlag, CgmPoint>(edgeOutFlag, p));
            }
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            foreach (var pair in Set)
            {
                writer.WritePoint(pair.Value);
                writer.WriteEnum((int)pair.Key);
            }
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" POLYGONSET");

            foreach (var pair in Set)
                writer.Write($" {WritePoint(pair.Value)} {WriteEnum(pair.Key)}");

            writer.WriteLine(";");
        }

        public override string ToString()
        {
            return "PolygonSet";
        }
    }
}
