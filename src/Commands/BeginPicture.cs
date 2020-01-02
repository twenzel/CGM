namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=3
    /// </remarks>
    public class BeginPicture : Command
    {
        public string PictureName { get; private set; }

        public BeginPicture(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 3, container))
        {

        }

        public BeginPicture(CgmFile container, string name)
            : this(container)
        {
            PictureName = name;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            PictureName = reader.ArgumentsCount > 0 ? reader.ReadString() : "";
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteString(PictureName);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"\n BEGPIC '{PictureName}';");
        }

        public override string ToString()
        {
            return "BeginPicture " + PictureName;
        }
    }
}