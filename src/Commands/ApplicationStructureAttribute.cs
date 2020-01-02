using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=9, ElementId=1
    /// </summary>
    public class ApplicationStructureAttribute : Command
    {
        public string AttributeType { get; private set; }
        public StructuredDataRecord Data { get; private set; }

        public ApplicationStructureAttribute(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.ApplicationStructureDescriptorElements, 1, container))
        {
           
        }

        public ApplicationStructureAttribute(CGMFile container, string attributeType, StructuredDataRecord sdr)
            :this(container)
        {
            AttributeType = attributeType;
            Data = sdr;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            AttributeType = reader.ReadFixedString();
            Data = reader.ReadSDR();            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteFixedString(AttributeType);
            writer.WriteSDR(Data);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" APSATTR {WriteString(AttributeType)} ");
            WriteSDR(writer, Data);
            writer.WriteLine(";");
        }
    }
}