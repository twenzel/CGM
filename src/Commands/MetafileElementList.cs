namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, Element=11
    /// </remarks>
    public class MetafileElementList : Command
    {
        public const string DRAWINGSET = "DRAWINGSET";
        public const string DRAWINGPLUS = "DRAWINGPLUS";
        public const string VERSION2 = "VERSION2";
        public const string EXTDPRIM = "EXTDPRIM";
        public const string VERSION2GKSM = "VERSION2GKSM";
        public const string VERSION3 = "VERSION3";
        public const string VERSION4 = "VERSION4";

        public MetafileElementList(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 11, container))
        {

        }

        public MetafileElementList(CGMFile container, string element)
            : this(container)
        {
            Elements = new[] { element };
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            var nElements = reader.ReadInt();

            Elements = new string[nElements];
            for (var i = 0; i < nElements; i++)
            {
                var code1 = reader.ReadIndex();
                var code2 = reader.ReadIndex();
                if (code1 == -1)
                {
                    switch (code2)
                    {
                        case 0:
                            Elements[i] = DRAWINGSET;
                            break;
                        case 1:
                            Elements[i] = DRAWINGPLUS;
                            break;
                        case 2:
                            Elements[i] = VERSION2;
                            break;
                        case 3:
                            Elements[i] = EXTDPRIM;
                            break;
                        case 4:
                            Elements[i] = VERSION2GKSM;
                            break;
                        case 5:
                            Elements[i] = VERSION3;
                            break;
                        case 6:
                            Elements[i] = VERSION4;
                            break;
                        default:
                            reader.Unsupported("unsupported meta file elements set " + code2);
                            break;
                    }
                }
                else
                {
                    // note: here, we can easily determine if a class/element is implemented or not                    
                    Elements[i] = $" ({code1},{code2})";
                }
            }
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteInt(Elements.Length);

            foreach (var elem in Elements)
            {
                if (elem == DRAWINGSET)
                {
                    writer.WriteInt(-1);
                    writer.WriteInt(0);
                }
                else if (elem == DRAWINGPLUS)
                {
                    writer.WriteInt(-1);
                    writer.WriteInt(1);
                }
                else if (elem == VERSION2)
                {
                    writer.WriteInt(-1);
                    writer.WriteInt(2);
                }
                else if (elem == EXTDPRIM)
                {
                    writer.WriteInt(-1);
                    writer.WriteInt(3);
                }
                else if (elem == VERSION2GKSM)
                {
                    writer.WriteInt(-1);
                    writer.WriteInt(4);
                }
                else if (elem == VERSION3)
                {
                    writer.WriteInt(-1);
                    writer.WriteInt(5);
                }
                else if (elem == VERSION4)
                {
                    writer.WriteInt(-1);
                    writer.WriteInt(6);
                }
                else
                {
                    var val = elem.Replace("(", "").Replace(")", "").Trim();
                    var separatorIndex = val.IndexOf(",");
                    writer.WriteInt(int.Parse(val.Substring(0, separatorIndex)));
                    writer.WriteInt(int.Parse(val.Substring(separatorIndex + 1)));
                }
            }
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" mfelemlist '{string.Join("', '", Elements)}';");
        }

        public override string ToString()
        {
            return "MetafileElementList " + string.Join(" ", Elements);
        }

        public string[] Elements { get; private set; }
    }
}
