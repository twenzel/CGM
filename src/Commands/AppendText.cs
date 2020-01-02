namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=6
    /// </summary>
    public class AppendText : Command
    {
        public enum FinalType
        {
            NOTFINAL = 0,
            FINAL = 1
        }

        public FinalType Final { get; private set; }
        public string Text { get; private set; }

        public AppendText(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 6, container))
        {

        }

        public AppendText(CgmFile container, FinalType final, string text)
            : this(container)
        {
            Final = final;
            Text = text;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Final = (FinalType)reader.ReadEnum();
            Text = reader.ReadString();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)Final);
            writer.WriteString(Text);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" APNDTEXT {WriteEnum(Final)} {WriteString(Text)};");
        }
    }
}