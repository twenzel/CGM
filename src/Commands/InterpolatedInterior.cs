﻿using System;
using System.Collections.Generic;
using codessentials.CGM.Classes;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=43
    /// </summary>
    public class InterpolatedInterior : Command
    {
        public int Style { get; set; }
        public List<double> GeoX { get; set; } = new List<double>();
        public List<double> GeoY { get; set; } = new List<double>();
        public List<double> StageDesignators { get; set; } = new List<double>();
        public List<CgmColor> Colors { get; set; } = new List<CgmColor>();

        public InterpolatedInterior(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 43, container))
        {

        }

        public InterpolatedInterior(CgmFile container, int style, IEnumerable<double> geoX, IEnumerable<double> geoY, IEnumerable<double> stageDesignators, IEnumerable<CgmColor> colors)
            : this(container)
        {
            Style = style;
            GeoX.AddRange(geoX);
            GeoY.AddRange(geoY);
            StageDesignators.AddRange(stageDesignators);
            Colors.AddRange(colors);

            if (GeoX.Count != GeoY.Count)
                throw new ArgumentException("Amount of geoX does not match geoY!");
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Style = reader.ReadIndex();

            var length = (Style == 1) ? 1 : 2;
            for (var i = 0; i < length; i++)
            {
                GeoX.Add(reader.ReadSizeSpecification(_container.InteriorStyleSpecificationMode));
                GeoY.Add(reader.ReadSizeSpecification(_container.InteriorStyleSpecificationMode));
            }

            var numberOfStages = reader.ReadInt();

            for (var i = 0; i < numberOfStages; i++)
                StageDesignators.Add(reader.ReadReal());

            var colorLength = (Style == 3) ? 3 : numberOfStages + 1;

            for (var i = 0; i < colorLength; i++)
                Colors.Add(reader.ReadColor());
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteIndex(Style);
            for (var i = 0; i < GeoX.Count; i++)
            {
                writer.WriteSizeSpecification(GeoX[i], _container.InteriorStyleSpecificationMode);
                writer.WriteSizeSpecification(GeoY[i], _container.InteriorStyleSpecificationMode);
            }

            writer.WriteInt(StageDesignators.Count);
            foreach (var val in StageDesignators)
                writer.WriteReal(val);

            foreach (var val in Colors)
                writer.WriteColor(val);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write($" INTERPINT {WriteIndex(Style)}");

            for (var i = 0; i < GeoX.Count; i++)
            {
                writer.Write($" {WriteVDC(GeoX[i])} {WriteVDC(GeoY[i])}");
            }

            writer.Write($" {WriteInt(StageDesignators.Count)}");

            foreach (var val in StageDesignators)
                writer.Write($" {WriteReal(val)}");

            foreach (var val in Colors)
                writer.Write($" {WriteColor(val)}");

            writer.WriteLine(";");
        }
    }
}
