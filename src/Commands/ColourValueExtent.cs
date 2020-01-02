using System.Text;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, Element=10
    /// </remarks>
    public class ColourValueExtent : Command
    {
        public int[] MinimumColorValueRGB { get; private set; }
        public int[] MaximumColorValueRGB { get; private set; }
        public double FirstComponentScale { get; private set; }
        public double SecondComponentScale { get; private set; }
        public double ThirdComponentScale { get; private set; }

        public ColourValueExtent(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 10, container))
        {

        }

        public ColourValueExtent(CgmFile container, int[] minimumColorValueRGB, int[] maximumColorValueRGB, double firstComponentScale, double second, double third)
            : this(container)
        {
            MinimumColorValueRGB = minimumColorValueRGB;
            MaximumColorValueRGB = maximumColorValueRGB;
            FirstComponentScale = firstComponentScale;
            SecondComponentScale = second;
            ThirdComponentScale = third;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            var colorModel = _container.ColourModel;
            MinimumColorValueRGB = new int[] { };
            MaximumColorValueRGB = new int[] { };

            if (colorModel == ColourModel.Model.RGB)
            {
                var precision = _container.ColourPrecision;

                MinimumColorValueRGB = new int[] { reader.ReadUInt(precision), reader.ReadUInt(precision), reader.ReadUInt(precision) };
                MaximumColorValueRGB = new int[] { reader.ReadUInt(precision), reader.ReadUInt(precision), reader.ReadUInt(precision) };

                _container.ColourValueExtentMinimumColorValueRGB = MinimumColorValueRGB;
                _container.ColourValueExtentMaximumColorValueRGB = MaximumColorValueRGB;
            }
            else if (colorModel == ColourModel.Model.CIELAB ||
                    colorModel == ColourModel.Model.CIELUV ||
                    colorModel == ColourModel.Model.RGB_RELATED)
            {
                FirstComponentScale = reader.ReadReal();
                SecondComponentScale = reader.ReadReal();
                ThirdComponentScale = reader.ReadReal();
            }
            else
            {
                reader.Unsupported("unsupported color model " + colorModel);
            }
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            if (_container.ColourModel == ColourModel.Model.RGB)
            {
                var precision = _container.ColourPrecision;
                writer.WriteUInt(MinimumColorValueRGB[0], precision);
                writer.WriteUInt(MinimumColorValueRGB[1], precision);
                writer.WriteUInt(MinimumColorValueRGB[2], precision);
                writer.WriteUInt(MaximumColorValueRGB[0], precision);
                writer.WriteUInt(MaximumColorValueRGB[1], precision);
                writer.WriteUInt(MaximumColorValueRGB[2], precision);

                _container.ColourValueExtentMinimumColorValueRGB = MinimumColorValueRGB;
                _container.ColourValueExtentMaximumColorValueRGB = MaximumColorValueRGB;
            }
            else if (_container.ColourModel == ColourModel.Model.CIELAB ||
                    _container.ColourModel == ColourModel.Model.CIELUV ||
                    _container.ColourModel == ColourModel.Model.RGB_RELATED)
            {
                writer.WriteReal(FirstComponentScale);
                writer.WriteReal(SecondComponentScale);
                writer.WriteReal(ThirdComponentScale);
            }
            else
            {
                writer.Unsupported("unsupported color model " + _container.ColourModel);
            }
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" colrvalueext ");

            if (_container.ColourModel == ColourModel.Model.RGB)
            {
                writer.Write($"{MinimumColorValueRGB[0]} {MinimumColorValueRGB[1]} {MinimumColorValueRGB[2]}");
                writer.Write($", {MaximumColorValueRGB[0]} {MaximumColorValueRGB[1]} {MaximumColorValueRGB[2]}");
            }
            else if (_container.ColourModel == ColourModel.Model.CMYK)
            {
                writer.Write($"{MinimumColorValueRGB[0]} {MinimumColorValueRGB[1]} {MinimumColorValueRGB[2]} {MinimumColorValueRGB[3]}");
                writer.Write($", {MaximumColorValueRGB[0]} {MaximumColorValueRGB[1]} {MaximumColorValueRGB[2]} {MaximumColorValueRGB[3]}");
            }
            else
            {
                writer.Write($"{AsColorValue(FirstComponentScale)} {AsColorValue(SecondComponentScale)} {AsColorValue(ThirdComponentScale)}");
            }

            writer.WriteLine(";");
        }

        private static string AsColorValue(double value)
        {
            return value.ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(".", ",");
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("ColourValueExtent");
            if (_container.ColourModel == ColourModel.Model.RGB)
            {
                sb.Append(" min RGB=(").Append(MinimumColorValueRGB[0]).Append(",");
                sb.Append(MinimumColorValueRGB[1]).Append(",");
                sb.Append(MinimumColorValueRGB[2]).Append(")");

                sb.Append(" max RGB=(").Append(MaximumColorValueRGB[0]).Append(",");
                sb.Append(MaximumColorValueRGB[1]).Append(",");
                sb.Append(MaximumColorValueRGB[2]).Append(")");
            }
            else if (_container.ColourModel == ColourModel.Model.CMYK)
            {
                // unsupported
            }
            else if (_container.ColourModel == ColourModel.Model.CIELAB ||
                    _container.ColourModel == ColourModel.Model.CIELUV ||
                    _container.ColourModel == ColourModel.Model.RGB_RELATED)
            {
                sb.Append(" first=").Append(FirstComponentScale);
                sb.Append(" second=").Append(SecondComponentScale);
                sb.Append(" third=").Append(ThirdComponentScale);
            }
            return sb.ToString();
        }
    }
}