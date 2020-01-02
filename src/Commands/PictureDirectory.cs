using System.Collections.Generic;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=1, ElementId=24
    /// </summary>
    public class PictureDirectory : Command
    {
        public enum Type
        {
            UI8 = 0,
            UI16,
            UI32
        }

        public class PDInfo
        {
            public string Identifier { get; set; }
            public int Location { get; set; }
            public int Directory { get; set; }
        }

        public Type Value { get; set; }
        public List<PDInfo> Infos { get; set; } = new List<PDInfo>();

        public PictureDirectory(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 24, container))
        {

        }

        public PictureDirectory(CGMFile container, Type type, IEnumerable<PDInfo> infos)
            : this(container)
        {
            Value = type;
            Infos.AddRange(infos);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Value = (Type)reader.ReadEnum();

            while (reader.CurrentArg < reader.Arguments.Length)
            {
                var info = new PDInfo
                {
                    Identifier = reader.ReadFixedString(),
                    Location = reader.ReadInt(),
                    Directory = reader.ReadInt()
                };

                Infos.Add(info);
            }
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteEnum((int)Value);
            foreach (var val in Infos)
            {
                writer.WriteFixedString(val.Identifier);
                writer.WriteInt(val.Location);
                writer.WriteInt(val.Directory);
            }
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" PICDIR {WriteEnum(Value)}");

            foreach (var info in Infos)
            {
                writer.Write($" {WriteString(info.Identifier)} {WriteInt(info.Location)} {WriteInt(info.Directory)}");
            }

            writer.WriteLine(";");
        }
    }
}
