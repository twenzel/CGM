namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, Element=7
    /// </remarks>
    public class ColourPrecision : Command
    {
        public int Precision { get; set; }

        public ColourPrecision(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 7, container))
        {

        }

        public ColourPrecision(CGMFile container, int precision)
            : this(container)
        {
            Precision = precision;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Precision = reader.ReadInt();
            _container.ColourPrecision = Precision;
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteInt(Precision);
            _container.ColourPrecision = Precision;
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" colrprec {WritValue(Precision)};");
        }

        public static string WritValue(int precision)
        {
            return $"{System.Math.Pow(2, precision) - 1}";
        }

        public override string ToString()
        {
            return $"ColourPrecision {Precision}";
        }        
    }
}