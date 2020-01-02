using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=1, Element=22
    /// </summary>
    public class GlyphMapping : Command
    {
        public int CharacterSetIndex { get; set; }
        public CharacterSetList.Type Type { get; set; }
        public string SequenceTail { get; set; }
        public int OctetsPerCode { get; set; }
        public int GlyphSource { get; set; }
        public StructuredDataRecord CodeAssocs { get; set; }

        public GlyphMapping(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 22, container))
        {

        }

        public GlyphMapping(CGMFile container, int characterSetIndex, CharacterSetList.Type type, string sequenceTail, int octetsPerCode, int glyphSource, StructuredDataRecord codeAssocs)
            : this(container)
        {
            CharacterSetIndex = characterSetIndex;
            Type = type;
            SequenceTail = sequenceTail;
            OctetsPerCode = octetsPerCode;
            GlyphSource = glyphSource;
            CodeAssocs = codeAssocs;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            CharacterSetIndex = reader.ReadIndex();
            Type = (CharacterSetList.Type)reader.ReadEnum();
            SequenceTail = reader.ReadFixedString();
            OctetsPerCode = reader.ReadInt();
            GlyphSource = reader.ReadIndex();
            CodeAssocs = reader.ReadSDR();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex(CharacterSetIndex);
            writer.WriteEnum((int)Type);
            writer.WriteFixedString(SequenceTail);
            writer.WriteInt(OctetsPerCode);
            writer.WriteIndex(GlyphSource);
            writer.WriteSDR(CodeAssocs);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($"  GLYPHMAP {WriteIndex(CharacterSetIndex)} {WriteEnum(Type)} {WriteIndex(OctetsPerCode)} {WriteIndex(GlyphSource)}");
            WriteSDR(writer, CodeAssocs);
            writer.WriteLine(";");
        }
    }
}