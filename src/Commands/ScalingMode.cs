using System.Text;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=2, Element=1
    /// </summary>
    public class ScalingMode : Command
    {
        public enum Mode
        {
            ABSTRACT,
            METRIC
        }

        public Mode Value { get; set; }
        public double MetricScalingFactor { get; set; }

        public ScalingMode(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 1, container))
        {

        }

        public ScalingMode(CgmFile container, Mode mode, double factor)
            : this(container)
        {
            Value = mode;
            MetricScalingFactor = factor;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            var mod = reader.ArgumentsCount > 0 ? reader.ReadEnum() : 0;
            if (mod == 0)
            {
                Value = Mode.ABSTRACT;
            }
            else if (mod == 1)
            {
                Value = Mode.METRIC;
            }

            if (reader.CurrentArg < reader.ArgumentsCount - 2)
                MetricScalingFactor = reader.ReadFloatingPoint();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)Value);

            if (Value == Mode.METRIC)
                writer.WriteFloatingPoint(MetricScalingFactor);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            if (Value == Mode.ABSTRACT)
                writer.WriteLine($"  scalemode abstract;");
            else
                writer.WriteLine($"  scalemode metric, {WriteDouble(MetricScalingFactor)};");
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("ScalingMode mode=").Append(Value);
            if (Value == Mode.METRIC)
            {
                sb.Append(" metricScalingFactor=").Append(MetricScalingFactor);
            }
            return sb.ToString();
        }
    }
}
