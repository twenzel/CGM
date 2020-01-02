using System.Collections.Generic;
using System.Linq;
using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=26
    /// </summary>
    public class PolyBezier : Command
    {
        /**
     * 1: discontinuous, 2: continuous, >2: reserved for registered values
     */
        public int ContinuityIndicator { get; set; }

        public List<BezierCurve> Curves { get; set; } = new List<BezierCurve>();

        public PolyBezier(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 26, container))
        {

        }

        public PolyBezier(CgmFile container, int continuityIndicator, IEnumerable<BezierCurve> curves)
            : this(container)
        {
            ContinuityIndicator = continuityIndicator;
            Curves.AddRange(curves);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            ContinuityIndicator = reader.ReadIndex();

            if (ContinuityIndicator == 1)
            {
                Assert(((reader.Arguments.Length - reader.CurrentArg) / reader.SizeOfPoint()) % 4 == 0, $"invalid PolyBezier args for _continuityIndicator {ContinuityIndicator}");

                var n = ((reader.Arguments.Length - reader.CurrentArg) / reader.SizeOfPoint()) / 4;

                var point = 0;
                while (point < n)
                {
                    var curve = new BezierCurve
                    {
                        reader.ReadPoint(),
                        reader.ReadPoint(),
                        reader.ReadPoint(),
                        reader.ReadPoint()
                    };
                    Curves.Add(curve);
                    point++;
                }
            }
            else if (this.ContinuityIndicator == 2)
            {
                Assert(((reader.Arguments.Length - reader.CurrentArg - 1) / reader.SizeOfPoint()) % 3 == 0, $"invalid PolyBezier args for _continuityIndicator {ContinuityIndicator}");
                var n = ((reader.Arguments.Length - reader.CurrentArg - 1) / reader.SizeOfPoint()) / 3;

                var point = 0;
                while (point < n)
                {
                    var curve = new BezierCurve();
                    if (point == 0)
                        curve.Add(reader.ReadPoint());
                    //else
                    //{
                    //    var lastCurve = Curves[Curves.Count - 1];
                    //    curve.Add(lastCurve[2]);
                    //}
                    curve.Add(reader.ReadPoint());
                    curve.Add(reader.ReadPoint());
                    curve.Add(reader.ReadPoint());

                    Curves.Add(curve);
                    point++;
                }
            }
            else
                reader.Unsupported("unsupported continuity indicator " + ContinuityIndicator);
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex(ContinuityIndicator);
            foreach (var curve in Curves)
            {
                foreach (var p in curve)
                    writer.WritePoint(p);
            }
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" POLYBEZIER {WriteInt(ContinuityIndicator)}");

            //for (int i = 0; i < P1.Length; i++)
            //{
            //    writer.Write($" {WritePoint(P1[i])} {WritePoint(P2[i])} {WritePoint(P3[i])} {WritePoint(P4[i])}");
            //}

            foreach (var curve in Curves)
            {
                var line = string.Join("", curve.Select(p => $" {WritePoint(p)}"));
                writer.Write(line);
            }

            writer.WriteLine(";");
        }
    }
}
