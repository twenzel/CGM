using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, Element=10
    /// </remarks>
    public class ColourValueExtent : Command
    {
        private int[] _minimumColorValueRGB;
        private int[] _maximumColorValueRGB;
        private double _firstComponentScale;
        private double _secondComponentScale;
        private double _thirdComponentScale;

        public ColourValueExtent(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 10, container))
        {
            
        }

        public ColourValueExtent(CGMFile container, int[] minimumColorValueRGB, int[] maximumColorValueRGB, double firstComponentScale, double second, double third)
            :this(container)
        {
            _minimumColorValueRGB = minimumColorValueRGB;
            _maximumColorValueRGB = maximumColorValueRGB;
            _firstComponentScale = firstComponentScale;
            _secondComponentScale = second;
            _thirdComponentScale = third;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            ColourModel.Model colorModel = _container.ColourModel;
            _minimumColorValueRGB = new int[] { };
            _maximumColorValueRGB = new int[] { };

            if (colorModel == ColourModel.Model.RGB)
            {
                int precision = _container.ColourPrecision;

                _minimumColorValueRGB = new int[] { reader.ReadUInt(precision), reader.ReadUInt(precision), reader.ReadUInt(precision) };
                _maximumColorValueRGB = new int[] { reader.ReadUInt(precision), reader.ReadUInt(precision), reader.ReadUInt(precision) };

                _container.ColourValueExtentMinimumColorValueRGB = _minimumColorValueRGB;
                _container.ColourValueExtentMaximumColorValueRGB = _maximumColorValueRGB;
            }
            else if (colorModel == ColourModel.Model.CIELAB ||
                    colorModel == ColourModel.Model.CIELUV ||
                    colorModel == ColourModel.Model.RGB_RELATED)
            {
                _firstComponentScale = reader.ReadReal();
                _secondComponentScale = reader.ReadReal();
                _thirdComponentScale = reader.ReadReal();
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
                int precision = _container.ColourPrecision;
                writer.WriteUInt(_minimumColorValueRGB[0], precision);
                writer.WriteUInt(_minimumColorValueRGB[1], precision);
                writer.WriteUInt(_minimumColorValueRGB[2], precision);
                writer.WriteUInt(_maximumColorValueRGB[0], precision);
                writer.WriteUInt(_maximumColorValueRGB[1], precision);
                writer.WriteUInt(_maximumColorValueRGB[2], precision);

                _container.ColourValueExtentMinimumColorValueRGB = _minimumColorValueRGB;
                _container.ColourValueExtentMaximumColorValueRGB = _maximumColorValueRGB;
            }              
            else if (_container.ColourModel == ColourModel.Model.CIELAB ||
                    _container.ColourModel == ColourModel.Model.CIELUV ||
                    _container.ColourModel == ColourModel.Model.RGB_RELATED)
            {
                writer.WriteReal(_firstComponentScale);
                writer.WriteReal(_secondComponentScale);
                writer.WriteReal(_thirdComponentScale);
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
                writer.Write($"{_minimumColorValueRGB[0]} {_minimumColorValueRGB[1]} {_minimumColorValueRGB[2]}");
                writer.Write($", {_maximumColorValueRGB[0]} {_maximumColorValueRGB[1]} {_maximumColorValueRGB[2]}");
            }
            else if (_container.ColourModel == ColourModel.Model.CMYK)
            {
                writer.Write($"{_minimumColorValueRGB[0]} {_minimumColorValueRGB[1]} {_minimumColorValueRGB[2]} {_minimumColorValueRGB[3]}");
                writer.Write($", {_maximumColorValueRGB[0]} {_maximumColorValueRGB[1]} {_maximumColorValueRGB[2]} {_maximumColorValueRGB[3]}");
            }
            else
            {
                writer.Write($"{AsColorValue(_firstComponentScale)} {AsColorValue(_secondComponentScale)} {AsColorValue(_thirdComponentScale)}");
            }

            writer.WriteLine(";");
        }

        private static string AsColorValue(double value)
        {
            return value.ToString(System.Globalization.CultureInfo.InvariantCulture).Replace(".", ",");
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("ColourValueExtent");
            if (_container.ColourModel == ColourModel.Model.RGB)
            {
                sb.Append(" min RGB=(").Append(_minimumColorValueRGB[0]).Append(",");
                sb.Append(_minimumColorValueRGB[1]).Append(",");
                sb.Append(_minimumColorValueRGB[2]).Append(")");

                sb.Append(" max RGB=(").Append(_maximumColorValueRGB[0]).Append(",");
                sb.Append(_maximumColorValueRGB[1]).Append(",");
                sb.Append(_maximumColorValueRGB[2]).Append(")");
            }
            else if (_container.ColourModel == ColourModel.Model.CMYK)
            {
                // unsupported
            }
            else if (_container.ColourModel == ColourModel.Model.CIELAB ||
                    _container.ColourModel == ColourModel.Model.CIELUV ||
                    _container.ColourModel == ColourModel.Model.RGB_RELATED)
            {
                sb.Append(" first=").Append(_firstComponentScale);
                sb.Append(" second=").Append(_secondComponentScale);
                sb.Append(" third=").Append(_thirdComponentScale);
            }
            return sb.ToString();
        }

        public int[] MinimumColorValueRGB => _minimumColorValueRGB;
        public int[] MaximumColorValueRGB =>_maximumColorValueRGB;
        public double FirstComponentScale => _firstComponentScale;
        public double SecondComponentScale =>_secondComponentScale;
        public double ThirdComponentScale => _thirdComponentScale;
    }
}