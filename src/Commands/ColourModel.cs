namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=1, Element=19
    /// </summary>
    public class ColourModel : Command
    {
        public enum Model
        {
            RGB = 1,
            CIELAB = 2,
            CIELUV = 3,
            CMYK = 4,
            RGB_RELATED = 5
        }

        public Model Value { get; private set; }

        public ColourModel(CgmFile container)
                : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 19, container))
        {

        }

        public ColourModel(CgmFile container, Model model)
            : this(container)
        {
            Value = model;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            var index = reader.ReadIndex();
            switch (index)
            {
                case 1:
                    _container.ColourModel = Model.RGB;
                    break;
                case 2:
                    _container.ColourModel = Model.CIELAB;
                    break;
                case 3:
                    _container.ColourModel = Model.CIELUV;
                    break;
                case 4:
                    _container.ColourModel = Model.CMYK;
                    break;
                case 5:
                    _container.ColourModel = Model.RGB_RELATED;
                    break;
                default:
                    reader.Unsupported("unsupported color mode " + index);
                    _container.ColourModel = Model.RGB;
                    break;
            }

            Value = _container.ColourModel;
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex((int)Value);
            _container.ColourModel = Value;
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($" COLRMODEL {WriteInt((int)Value)};");
        }

        public override string ToString()
        {
            return $"ColourModel {Value}";
        }
    }
}
