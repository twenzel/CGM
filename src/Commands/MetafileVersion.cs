namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, Element=1
    /// </remarks>
    public class MetafileVersion : Command
    {
        public int Version { get; private set; }

        public MetafileVersion(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 1, container))
        {
        }

        public MetafileVersion(CGMFile container, int version)
            : this(container)
        {
            Version = version;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Version = reader.ReadInt();

        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteInt(Version);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" mfversion {Version};");
        }

        public override string ToString()
        {
            return $"MetafileVersion {Version}";
        }
    }
}
