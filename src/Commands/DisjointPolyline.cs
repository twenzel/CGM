using codessentials.CGM.Classes;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=2
    /// </summary>
    public class DisjointPolyline : Command
    {
        public List<KeyValuePair<CGMPoint, CGMPoint>> Lines { get; set; } = new List<KeyValuePair<CGMPoint, CGMPoint>>();

        public DisjointPolyline(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 2, container))
        {

        }

        public DisjointPolyline(CGMFile container, KeyValuePair<CGMPoint, CGMPoint>[] points)
            : this(container)
        {
            Lines.AddRange(points);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            var n = reader.ArgumentsCount / reader.SizeOfPoint();
            Debug.Assert(n % 2 == 0);

            for (var i = 0; i < (n / 2); i++)
            {
                var p1 = reader.ReadPoint();
                var p2 = reader.ReadPoint();
                Lines.Add(new KeyValuePair<CGMPoint, CGMPoint>(p1, p2));
            }
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            foreach (var pair in Lines)
            {
                writer.WritePoint(pair.Key);
                writer.WritePoint(pair.Value);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("DisjointPolyline [");
            for (var i = 0; i < Lines.Count; i++)
            {
                sb.Append("(");
                sb.Append(Lines[i].Key.X).Append(",");
                sb.Append(Lines[i].Key.Y).Append(",");
                sb.Append(Lines[i].Value.X).Append(",");
                sb.Append(Lines[i].Value.Y);
                sb.Append(")");
            }
            sb.Append("]");
            return sb.ToString();

        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write(" DISJTLINE");

            foreach (var pair in Lines)
            {
                writer.Write($" {WritePoint(pair.Key)} {WritePoint(pair.Value)}");
            }

            writer.WriteLine(";");
        }
    }
}