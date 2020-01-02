using System.Collections.Generic;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=2, Element=20
    /// </remarks>
    public class ApplicationStructureDirectory : Command
    {
        public enum DataTypeSelector
        {
            UI8 = 0,
            UI16,
            UI32
        }

        public class ApplicationStructureInfo
        {
            public string Identifier { get; set; }
            public int Location { get; set; }
        }

        public DataTypeSelector TypeSelector { get; private set; }
        public List<ApplicationStructureInfo> Infos { get; }

        public ApplicationStructureDirectory(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 20, container))
        {
            
        }

        public ApplicationStructureDirectory(CGMFile container, DataTypeSelector typeSelector, ApplicationStructureInfo[] infos)
            :this(container)
        {
            TypeSelector = typeSelector;
            Infos.AddRange(infos);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            TypeSelector = (DataTypeSelector)reader.ReadEnum();

            while (reader.CurrentArg < reader.Arguments.Length)
            {
                var info = new ApplicationStructureInfo
                {
                    Identifier = reader.ReadFixedString(),
                    Location = reader.ReadInt()
                };

                Infos.Add(info);
            }            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)TypeSelector);
            foreach(var info in Infos)
            {
                writer.WriteFixedString(info.Identifier);
                writer.WriteInt(info.Location);
            }
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" APSDIR {WriteEnum(TypeSelector)}");

            foreach (var info in Infos)
                writer.Write($" {WriteString(info.Identifier)} {WriteInt(info.Location)}");

            writer.WriteLine(";");
        }
    }
}