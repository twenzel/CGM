using System;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=6
    /// </summary>
    public class AppendText : Command
    {
        public enum FinalEnum
        {
            NOTFINAL = 0,
            FINAL = 1
        }

        private FinalEnum _final;
        private string _text;

        public AppendText(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 6, container))
        {
           
        }

        public AppendText(CGMFile container, FinalEnum final, string text)
            :this(container)
        {
            _final = final;
            _text = text;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _final = (FinalEnum)reader.ReadEnum();
            _text = reader.ReadString();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)_final);
            writer.WriteString(_text);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" APNDTEXT {WriteEnum(_final)} {WriteString(_text)};");
        }

        public FinalEnum Final => _final;
        public string Text => _text;
    }
}