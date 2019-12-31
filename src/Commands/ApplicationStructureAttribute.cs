using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=9, ElementId=1
    /// </summary>
    public class ApplicationStructureAttribute : Command
    {
        private string _attributeType;
        private StructuredDataRecord _data;

        public ApplicationStructureAttribute(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.ApplicationStructureDescriptorElements, 1, container))
        {
           
        }

        public ApplicationStructureAttribute(CGMFile container, string attributeType, StructuredDataRecord sdr)
            :this(container)
        {
            _attributeType = attributeType;
            _data = sdr;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _attributeType = reader.ReadFixedString();
            _data = reader.ReadSDR();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteFixedString(_attributeType);
            writer.WriteSDR(_data);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" APSATTR {WriteString(_attributeType)} ");
            WriteSDR(writer, _data);
            writer.WriteLine(";");
        }

        public string AttributeType => _attributeType;
        public StructuredDataRecord Data => _data;
    }
}