namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=2, Element=9
    /// </remarks>
    public class DeviceViewportSpecificationMode : Command
    {
        public enum Mode
        {
            FRACTION, //FractionOfDrawingSurface,
            MM, //MillimetersWithScaleFactor,
            PHYDEVCOORD //PhysicalDeviceCoordinates
        }

        public Mode Value { get; private set; }
        public double MetricScaleFactor { get; private set; }

        public DeviceViewportSpecificationMode(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 9, container))
        {

        }

        public DeviceViewportSpecificationMode(CGMFile container, Mode mode, double factor)
            : this(container)
        {
            Value = mode;
            MetricScaleFactor = factor;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            var enumValue = reader.ReadEnum();
            switch (enumValue)
            {
                case 0:
                    Value = Mode.FRACTION;
                    break;
                case 1:
                    Value = Mode.MM;
                    break;
                case 2:
                    Value = Mode.PHYDEVCOORD;
                    break;
                default:
                    reader.Unsupported("unsupported mode " + enumValue);
                    break;
            }

            if (_container.RealPrecisionProcessed)
                MetricScaleFactor = reader.ReadReal();
            else
                MetricScaleFactor = reader.ReadFloatingPoint32();

            _container.DeviceViewportSpecificationMode = Value;
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)Value);
            _container.DeviceViewportSpecificationMode = Value;

            if (_container.RealPrecisionProcessed)
                writer.WriteReal(MetricScaleFactor);
            else
                writer.WriteFloatingPoint32(MetricScaleFactor);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" DEVVPMODE {WriteEnum(Value)} {WriteReal(MetricScaleFactor)}");
        }

        public override string ToString()
        {
            return $"DeviceViewportSpecificationMode [mode={Value}, metricScaleFactor={MetricScaleFactor}]";
        }
    }
}
