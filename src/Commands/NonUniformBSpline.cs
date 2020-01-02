using System.Collections.Generic;
using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=24
    /// </summary>
    public class NonUniformBSpline : Command
    {
        public int SplineOrder { get; set; }
        public List<CgmPoint> Points { get; set; } = new List<CgmPoint>();
        public List<double> Knots { get; set; } = new List<double>();
        public double StartValue { get; set; }
        public double EndValue { get; set; }

        public NonUniformBSpline(CgmFile container)
            : this(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 24, container))
        {

        }

        public NonUniformBSpline(CommandConstructorArguments args)
           : base(args)
        {

        }

        public NonUniformBSpline(CgmFile container, int splineOrder, IEnumerable<CgmPoint> points, IEnumerable<double> knots, double start, double end)
            : this(container)
        {
            SetValues(splineOrder, points, knots, start, end);
        }

        protected void SetValues(int splineOrder, IEnumerable<CgmPoint> points, IEnumerable<double> knots, double start, double end)
        {
            SplineOrder = splineOrder;
            Points.AddRange(points);
            Knots.AddRange(knots);
            StartValue = start;
            EndValue = end;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            SplineOrder = reader.ReadInt();

            var numberOfControlPoints = reader.ReadInt();

            for (var i = 0; i < numberOfControlPoints; i++)
                Points.Add(reader.ReadPoint());

            for (var i = 0; i < numberOfControlPoints + SplineOrder; i++)
                Knots.Add(reader.ReadReal());

            StartValue = reader.ReadReal();
            EndValue = reader.ReadReal();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteInt(SplineOrder);
            writer.WriteInt(Points.Count);
            foreach (var val in Points)
                writer.WritePoint(val);
            foreach (var val in Knots)
                writer.WriteReal(val);
            writer.WriteReal(StartValue);
            writer.WriteReal(EndValue);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" NUB {WriteInt(SplineOrder)} {WriteInt(Points.Count)}");

            foreach (var val in Points)
                writer.Write($" {WritePoint(val)}");

            foreach (var val in Knots)
                writer.Write($" {WriteReal(val)}");

            writer.WriteLine($" {WriteReal(StartValue)} {WriteReal(EndValue)};");
        }
    }
}
