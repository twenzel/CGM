using System;
using System.Collections.Generic;
using System.Drawing;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=1, Element=20
    /// </summary>
    public class ColourCalibration : Command
    {
        public int CalibrationSelection { get; set; }
        public double ReferenceX { get; set; }
        public double ReferenceY { get; set; }
        public double ReferenceZ { get; set; }
        public double Xr { get; set; }
        public double Xg { get; set; }
        public double Xb { get; set; }
        public double Yr { get; set; }
        public double Yg { get; set; }
        public double Yb { get; set; }
        public double Zr { get; set; }
        public double Zg { get; set; }
        public double Zb { get; set; }
        public double Ra { get; set; }
        public double Rb { get; set; }
        public double Rc { get; set; }
        public double Ga { get; set; }
        public double Gb { get; set; }
        public double Gc { get; set; }
        public double Ba { get; set; }
        public double Bb { get; set; }
        public double Bc { get; set; }
        public int TableEntries { get; set; }
        public List<Tuple<double, double>> LookupR { get; set; } = new List<Tuple<double, double>>();
        public List<Tuple<double, double>> LookupG { get; set; } = new List<Tuple<double, double>>();
        public List<Tuple<double, double>> LookupB { get; set; } = new List<Tuple<double, double>>();
        public int NumberOfGridLocations { get; set; }
        public List<Color> CmykGridLocations { get; set; } = new List<Color>();
        public List<Tuple<double, double, double>> XyzGridLocations { get; set; } = new List<Tuple<double, double, double>>();

        public ColourCalibration(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 20, container))
        {

        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            CalibrationSelection = reader.ReadIndex();
            ReferenceX = reader.ReadReal();
            ReferenceY = reader.ReadReal();
            ReferenceZ = reader.ReadReal();
            Xr = reader.ReadReal();
            Xg = reader.ReadReal();
            Xb = reader.ReadReal();
            Yr = reader.ReadReal();
            Yg = reader.ReadReal();
            Yb = reader.ReadReal();
            Zr = reader.ReadReal();
            Zg = reader.ReadReal();
            Zb = reader.ReadReal();
            Ra = reader.ReadReal();
            Rb = reader.ReadReal();
            Rc = reader.ReadReal();
            Ga = reader.ReadReal();
            Gb = reader.ReadReal();
            Gc = reader.ReadReal();
            Ba = reader.ReadReal();
            Bb = reader.ReadReal();
            Bc = reader.ReadReal();
            TableEntries = reader.ReadInt();

            for (var i = 0; i < TableEntries; i++)
                LookupR.Add(new Tuple<double, double>(reader.ReadReal(), reader.ReadReal()));

            for (var i = 0; i < TableEntries; i++)
                LookupG.Add(new Tuple<double, double>(reader.ReadReal(), reader.ReadReal()));

            for (var i = 0; i < TableEntries; i++)
                LookupB.Add(new Tuple<double, double>(reader.ReadReal(), reader.ReadReal()));

            NumberOfGridLocations = reader.ReadInt();

            for (var i = 0; i < NumberOfGridLocations; i++)
                CmykGridLocations.Add(reader.ReadDirectColor());

            for (var i = 0; i < NumberOfGridLocations; i++)
                XyzGridLocations.Add(new Tuple<double, double, double>(reader.ReadReal(), reader.ReadReal(), reader.ReadReal()));
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex(CalibrationSelection);
            writer.WriteReal(ReferenceX);
            writer.WriteReal(ReferenceY);
            writer.WriteReal(ReferenceZ);
            writer.WriteReal(Xr);
            writer.WriteReal(Xg);
            writer.WriteReal(Xb);
            writer.WriteReal(Yr);
            writer.WriteReal(Yg);
            writer.WriteReal(Yb);
            writer.WriteReal(Zr);
            writer.WriteReal(Zg);
            writer.WriteReal(Zb);
            writer.WriteReal(Ra);
            writer.WriteReal(Rb);
            writer.WriteReal(Rc);
            writer.WriteReal(Ga);
            writer.WriteReal(Gb);
            writer.WriteReal(Gc);
            writer.WriteReal(Ba);
            writer.WriteReal(Bb);
            writer.WriteReal(Bc);
            writer.WriteInt(TableEntries);

            if (TableEntries != LookupR.Count)
                throw new InvalidOperationException($"The amount of table entries({TableEntries}) does not match the red lookup table entries({LookupR.Count})!");

            foreach (var pair in LookupR)
            {
                writer.WriteReal(pair.Item1);
                writer.WriteReal(pair.Item2);
            }

            if (TableEntries != LookupG.Count)
                throw new InvalidOperationException($"The amount of table entries({TableEntries}) does not match the green lookup table entries({LookupG.Count})!");

            foreach (var pair in LookupG)
            {
                writer.WriteReal(pair.Item1);
                writer.WriteReal(pair.Item2);
            }

            if (TableEntries != LookupB.Count)
                throw new InvalidOperationException($"The amount of table entries({TableEntries}) does not match the blue lookup table entries({LookupB.Count})!");

            foreach (var pair in LookupB)
            {
                writer.WriteReal(pair.Item1);
                writer.WriteReal(pair.Item2);
            }

            writer.WriteInt(NumberOfGridLocations);

            if (NumberOfGridLocations != CmykGridLocations.Count)
                throw new InvalidOperationException($"The number of grid locations({NumberOfGridLocations}) does not match the CMYK grid locations({CmykGridLocations.Count})!");

            foreach (var color in CmykGridLocations)
                writer.WriteDirectColor(color);

            if (NumberOfGridLocations != XyzGridLocations.Count)
                throw new InvalidOperationException($"The number of grid locations({NumberOfGridLocations}) does not match the XYZ grid locations({XyzGridLocations.Count})!");

            foreach (var pair in XyzGridLocations)
            {
                writer.WriteReal(pair.Item1);
                writer.WriteReal(pair.Item2);
                writer.WriteReal(pair.Item3);
            }
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" COLRCALIB {WriteInt(CalibrationSelection)} {WriteReal(ReferenceX)} {WriteReal(ReferenceY)} {WriteReal(ReferenceZ)}");

            writer.Write($" {WriteReal(Xr)} {WriteReal(Xg)} {WriteReal(Xb)} {WriteReal(Yr)} {WriteReal(Yg)} {WriteReal(Yb)} {WriteReal(Zr)} {WriteReal(Zg)} {WriteReal(Zb)}");
            writer.Write($" {WriteReal(Ra)} {WriteReal(Rb)} {WriteReal(Rc)} {WriteReal(Ga)} {WriteReal(Gb)} {WriteReal(Gc)} {WriteReal(Ba)} {WriteReal(Bb)} {WriteReal(Bc)}");

            writer.Write($" {WriteInt(TableEntries)}");

            foreach (var val in LookupR)
                writer.Write($" {WriteInt((int)val.Item1)} {WriteInt((int)val.Item2)}");

            foreach (var val in LookupG)
                writer.Write($" {WriteInt((int)val.Item1)} {WriteInt((int)val.Item2)}");

            foreach (var val in LookupB)
                writer.Write($" {WriteInt((int)val.Item1)} {WriteInt((int)val.Item2)}");

            writer.Write($" {WriteInt(NumberOfGridLocations)}");

            foreach (var val in CmykGridLocations)
                writer.Write($" {WriteColor(val, _container.ColourModel)}");

            foreach (var val in XyzGridLocations)
                writer.Write($" {WriteReal(val.Item1)} {WriteReal(val.Item2)} {WriteReal(val.Item3)}");

            writer.WriteLine(";");
        }        
    }
}