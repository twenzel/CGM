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

        private DataTypeSelector _locationDataTypeSelector;
        private List<ApplicationStructureInfo> _infos = new List<ApplicationStructureInfo>();

        public ApplicationStructureDirectory(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.PictureDescriptorElements, 20, container))
        {
            
        }

        public ApplicationStructureDirectory(CGMFile container, DataTypeSelector typeSelector, ApplicationStructureInfo[] infos)
            :this(container)
        {
            _locationDataTypeSelector = typeSelector;
            _infos.AddRange(infos);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _locationDataTypeSelector = (DataTypeSelector)reader.ReadEnum();

            while (reader.CurrentArg < reader.Arguments.Length)
            {
                var info = new ApplicationStructureInfo();
                info.Identifier = reader.ReadFixedString();
                info.Location = reader.ReadInt();

                _infos.Add(info);
            }            
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)_locationDataTypeSelector);
            foreach(var info in _infos)
            {
                writer.WriteFixedString(info.Identifier);
                writer.WriteInt(info.Location);
            }
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" APSDIR {WriteEnum(_locationDataTypeSelector)}");

            foreach (var info in _infos)
                writer.Write($" {WriteString(info.Identifier)} {WriteInt(info.Location)}");

            writer.WriteLine(";");
        }

        public DataTypeSelector TypeSelector => _locationDataTypeSelector;
        public List<ApplicationStructureInfo> Infos => _infos;
    }
}