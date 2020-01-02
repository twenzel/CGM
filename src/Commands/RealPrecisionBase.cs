namespace codessentials.CGM.Commands
{
    public abstract class RealPrecisionBase : Command
    {
        public Precision Value { get; protected set; } = Precision.Fixed_32;

        protected RealPrecisionBase(CommandConstructorArguments args)
            : base(args)
        {

        }

        protected void SetValue(Precision precision)
        {
            Value = precision;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            var p1 = reader.ReadEnum();
            var p2 = reader.ReadInt();
            var p3 = reader.ReadInt();

            if (p1 == 0)
            {
                if (p2 == 9 && p3 == 23)
                {
                    Value = Precision.Floating_32;
                }
                else if (p2 == 12 && p3 == 52)
                {
                    Value = Precision.Floating_64;
                }
                else
                {
                    // use default
                    reader.Unsupported("unsupported real precision");
                    Value = Precision.Fixed_32;
                }
            }
            else if (p1 == 1)
            {
                if (p2 == 16 && p3 == 16)
                {
                    Value = Precision.Fixed_32;
                }
                else if (p2 == 32 && p3 == 32)
                {
                    Value = Precision.Fixed_64;
                }
                else
                {
                    // use default
                    reader.Unsupported("unsupported real precision");
                    Value = Precision.Fixed_32;
                }
            }
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            switch (Value)
            {
                case Precision.Floating_32:
                    writer.WriteInt(0);
                    writer.WriteInt(9);
                    writer.WriteInt(23);
                    break;
                case Precision.Floating_64:
                    writer.WriteInt(0);
                    writer.WriteInt(12);
                    writer.WriteInt(52);
                    break;
                case Precision.Fixed_32:
                    writer.WriteInt(1);
                    writer.WriteInt(16);
                    writer.WriteInt(16);
                    break;
                case Precision.Fixed_64:
                    writer.WriteInt(1);
                    writer.WriteInt(32);
                    writer.WriteInt(32);
                    break;
            }
        }
    }

    public enum Precision
    {
        Floating_32,
        Floating_64,
        Fixed_32,
        Fixed_64
    }
}
