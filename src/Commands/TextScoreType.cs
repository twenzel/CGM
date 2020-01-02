using System.Collections.Generic;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=41
    /// </summary>
    public class TextScoreType : Command
    {
        public class TSInfo
        {
            public int Type { get; set; }
            public bool Indicator { get; set; }
        }

        public List<TSInfo> Infos { get; set; } = new List<TSInfo>();

        public TextScoreType(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 41, container))
        {

        }

        public TextScoreType(CGMFile container, IEnumerable<TSInfo> infos)
            : this(container)
        {
            Infos.AddRange(infos);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            while (reader.CurrentArg < reader.Arguments.Length)
            {
                var info = new TSInfo
                {
                    Type = reader.ReadIndex(),
                    Indicator = reader.ReadBool()
                };

                Infos.Add(info);
            }
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            foreach (var info in Infos)
            {
                writer.WriteIndex(info.Type);
                writer.WriteBool(info.Indicator);
            }
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write(" TEXTSCORETYPE");
            foreach (var info in Infos)
            {
                writer.Write($" {WriteIndex(info.Type)} {WriteBool(info.Indicator)}");
            }

            writer.WriteLine(";");
        }
    }
}
