using System;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=0, ElementId=21
    /// </remarks>
    public class BeginApplicationStructure : Command
    {
        public enum InheritanceFlag
        {
            STLIST = 0,
            APS = 1
        }

        private string _id;
        private string _type;
        private InheritanceFlag _flag;

        public BeginApplicationStructure(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.DelimiterElement, 21, container))
        {
           
        }

        public BeginApplicationStructure(CGMFile container, string id, string type, InheritanceFlag flag)
            :this(container)
        {
            _id = id;
            _type = type;
            _flag = flag;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _id = reader.ReadFixedString();
            _type = reader.ReadFixedString();
            _flag = (InheritanceFlag)reader.ReadEnum();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteFixedString(_id);
            writer.WriteFixedString(_type);
            writer.WriteEnum((int)_flag);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" BEGAPS {WriteString(_id)} {WriteString(_type)} {WriteEnum(_flag)};");
        }

        public override string ToString()
        {
            return $"Begin Application Structure {_id}, {_type}";
        }

        public string Id => _id;
        public string Type => _type;
        public InheritanceFlag Flag => _flag;
    }
}