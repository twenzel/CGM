using codessentials.CGM.Classes;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=4, ElementId=9
    /// </summary>
    public class CellArray : Command
    {
        private int _representationFlag;
        private int _nx;
        private int _ny;
        private CGMPoint _p;
	    private CGMPoint _q;
	    private CGMPoint _r;
        private int _localColorPrecision;

        // either the colors are filled or the colorIndexes depending on the color selection mode
        private CGMColor[] _colors;

        public CellArray(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.GraphicalPrimitiveElements, 9, container))
        {
            
        }        

        public CellArray(CGMFile container, int repesentationFlag, int nx, int ny, CGMPoint p, CGMPoint q, CGMPoint r, int localColorPrecision, CGMColor[] colors)
            : this(container)
        {
            _representationFlag = repesentationFlag;
            _nx = nx;
            _ny = ny;
            _p = p;
            _q = q;
            _r = r;
            _localColorPrecision = localColorPrecision;
            _colors = colors;        
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            // 3P, 3I, E, CLIST
            _p = reader.ReadPoint();
            _q = reader.ReadPoint();
            _r = reader.ReadPoint();
            _nx = reader.ReadInt(); // number of cells per row
            _ny = reader.ReadInt(); // number of rows

            _localColorPrecision = reader.ReadInt();
            int localColorPrecision = _localColorPrecision;

            if (localColorPrecision == 0)
            {
                if (_container.ColourSelectionMode == ColourSelectionMode.Type.INDEXED)
                    localColorPrecision = _container.ColourIndexPrecision;
                else
                    localColorPrecision = _container.ColourPrecision;
            }

            _representationFlag = reader.ReadEnum();

            readColorList(localColorPrecision, reader);
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {            
            writer.WritePoint(_p);
            writer.WritePoint(_q);
            writer.WritePoint(_r);
            writer.WriteInt(_nx);
            writer.WriteInt(_ny);
            writer.WriteInt(_localColorPrecision);
            writer.WriteEnum(_representationFlag);

            WriteColorList(_localColorPrecision, writer);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($"  CELLARRAY {WritePoint(_p)} {WritePoint(_q)} {WritePoint(_r)}, {WriteInt(_nx)}, {WriteInt(_ny)}, ");

            if (_localColorPrecision == 0)
            {
                if (_container.ColourSelectionMode == ColourSelectionMode.Type.INDEXED)
                    writer.Write(ColourIndexPrecision.WriteValue(_container.ColourIndexPrecision));
                else
                    writer.Write(ColourPrecision.WritValue(_container.ColourPrecision));
            }
            else
                writer.Write(WriteInt(_localColorPrecision));

            for (int i = 0; i < _colors.Length; i++)
            {
                writer.Write($" {WriteColor(_colors[i])}");
                if (i < _colors.Length - 1)
                    writer.Write(" ");
            }            

            writer.WriteLine(";");
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("CellArray");
            sb.Append(" nx=").Append(_nx);
            sb.Append(" ny=").Append(_ny);
            sb.Append(" representation flag=").Append(_representationFlag);
            sb.Append(" p=").Append(_p).Append(",");
            sb.Append(" q=").Append(_q).Append(",");
            sb.Append(" r=").Append(_r);

            return sb.ToString();
        }

        private void readColorList(int localColorPrecision, IBinaryReader reader)
        {
            int nColor = _nx * _ny;
            _colors = new CGMColor[nColor];
                          
            if (_representationFlag == 0)
            {
                // run length list mode
                int c = 0;
                while (c < nColor)
                {
                    int numColors = reader.ReadInt();
                    CGMColor color = reader.ReadColor(localColorPrecision);

                    // don't directly fill the array with numColors in case we
                    // encounter a erroneous CGM file, e.g. SCHEMA03.CGM that
                    // returns an incorrect number of colors; only fill at most
                    // the number of colors left in the array
                    int maxIndex = System.Math.Min(numColors, nColor - c);
                    for (int i = 0; i < maxIndex; i++)
                    {
                        _colors[c++] = color;
                    }

                    if (c > 0 && c % _nx == 0)
                    {
                        // align on word at the end of a line
                        reader.AlignOnWord();
                    }
                }
            }
            else if (_representationFlag == 1)
            {
                // packed list mode
                int i = 0;
                for (int row = 0; row < _ny; row++)
                {
                    for (int col = 0; col < _nx; col++)
                    {
                        _colors[i++] = reader.ReadColor(localColorPrecision);
                    }
                    // align on word
                    reader.AlignOnWord();
                }
            }
            else
            {
                reader.Unsupported("unsupported representation flag " + _representationFlag);
            }
                
        }

        private void WriteColorList(int localColorPrecision, IBinaryWriter writer)
        {
            // run length list mode
            if (_representationFlag == 0)
            {
                int c = 0;
                foreach (var col in _colors)
                {
                    writer.WriteInt(1);
                    writer.WriteColor(col, _localColorPrecision);
                    c++;

                    if (c > 0 && c % _nx == 0)
                    {
                        // align on word at the end of a line
                        writer.FillToWord();
                    }
                }
               
            }
            // packed list mode
            else if (_representationFlag == 1)
            {                           
                // packed list mode
                int i = 0;
                for (int row = 0; row < _ny; row++)
                {
                    for (int col = 0; col < _nx; col++)
                    {
                        writer.WriteColor(_colors[i++], localColorPrecision);
                    }
                    // align on word
                    writer.FillToWord();
                }
            }
            else
                writer.Unsupported("unsupported representation flag " + _representationFlag);            
        }

        public int RepresentationFlag => _representationFlag;
        public int Nx => _nx;
        public int Ny => _ny;
        public CGMPoint P =>_p;
        public CGMPoint Q =>_q;
        public CGMPoint R => _r;
        public int LocalColorPrecision => _localColorPrecision;

        public CGMColor[] Colors => _colors;
    }
}