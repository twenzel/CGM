namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, Element=3
    /// </remarks>
    public class VDCType : Command
    {
        public enum Type
        {
            Integer,
            Real
        }

        public Type Value { get; private set; } = Type.Integer;

        public VDCType(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 3, container))
        {

        }

        public VDCType(CgmFile container, Type type)
            : this(container)
        {
            Value = type;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            var p1 = reader.ReadEnum();
            if (p1 == 0)
                Value = Type.Integer;
            else if (p1 == 1)
                Value = Type.Real;
            else
                reader.Unsupported("VDC Type " + p1);

            _container.VDCType = Value;
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)Value);
            _container.VDCType = Value;
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            if (Value == Type.Integer)
            {
                writer.Info("Writing vdctype = real instead of integer (as read by the binary file) because of some problems using integer. If the CGM file could not be opened in any viewer please edit file and change vdctype.");
                writer.WriteLine($" vdctype {WriteEnum(Type.Real)};");
            }
            else
                writer.WriteLine($" vdctype {WriteEnum(Value)};");
        }

        public override string ToString()
        {
            return $"VDCType [{Value}]";
        }
    }
}
