using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=2, Element=19
    /// </remarks>
    public class GeometricPatternDefinition : Command
    {
        public int PatternIndex { get; set; }
        public int Identifier { get; set; }
        public CgmPoint FirstCorner { get; set; }
        public CgmPoint SecondCorner { get; set; }

        public GeometricPatternDefinition(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 19, container))
        {

        }

        public GeometricPatternDefinition(CgmFile container, int patternIndex, int id, CgmPoint first, CgmPoint second)
            : this(container)
        {
            PatternIndex = patternIndex;
            Identifier = id;
            FirstCorner = first;
            SecondCorner = second;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            PatternIndex = reader.ReadIndex();
            Identifier = reader.ReadName();
            FirstCorner = reader.ReadPoint();
            SecondCorner = reader.ReadPoint();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex(PatternIndex);
            writer.WriteName(Identifier);
            writer.WritePoint(FirstCorner);
            writer.WritePoint(SecondCorner);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  GEOPATDEF {WriteIndex(PatternIndex)} {WriteName(Identifier)} {WritePoint(FirstCorner)} {WritePoint(SecondCorner)};");
        }
    }
}