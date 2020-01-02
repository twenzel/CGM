namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, Element=8
    /// </remarks>
    public class ColourIndexPrecision : Command
    {
        public int Precision { get; private set; }

        public ColourIndexPrecision(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 8, container))
        {

        }


        public ColourIndexPrecision(CgmFile container, int precision)
            : this(container)
        {
            Precision = precision;
            AssertPrecision();
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Precision = reader.ReadInt();
            _container.ColourIndexPrecision = Precision;

            AssertPrecision();
        }

        private void AssertPrecision()
        {
            Assert(Precision == 8 || Precision == 16 || Precision == 24 || Precision == 32, "Invalid ColourIndexPrecision");
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteInt(Precision);
            _container.ColourIndexPrecision = Precision;
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" colrindexprec {WriteValue(Precision)};");
        }

        public static string WriteValue(int precision)
        {
            int val;

            if (precision == 8)
                val = sbyte.MaxValue;
            else if (precision == 16)
                val = short.MaxValue;
            else if (precision == 24)
                val = ushort.MaxValue;
            else
                val = int.MaxValue;

            return $"{val}";
        }

        public override string ToString()
        {
            return $"ColourIndexPrecision {Precision}";
        }        
    }
}