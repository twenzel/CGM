using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace codessentials.CGM.Commands
{
    /// <remarks>
    /// Class=1, Element=8
    /// </remarks>
    public class ColourIndexPrecision : Command
    {
        private int _precision;       

        public ColourIndexPrecision(CGMFile container) 
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 8, container))
        {
           
        }


        public ColourIndexPrecision(CGMFile container, int precision)
            :this(container)
        {
            _precision = precision;
            AssertPrecision();
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            _precision = reader.ReadInt();
            _container.ColourIndexPrecision = _precision;

            AssertPrecision();
        }

        private void AssertPrecision()
        {
            Assert(_precision == 8 || _precision == 16 || _precision == 24 || _precision == 32, "Invalid ColourIndexPrecision");
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteInt(_precision);
            _container.ColourIndexPrecision = _precision;
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {            
            writer.WriteLine($" colrindexprec {WriteValue(_precision)};");
        }

        public static string WriteValue(int precision)
        {
            var val = 0;

            if (precision == 8)
                val = sbyte.MaxValue;
            else if (precision == 16)
                val = short.MaxValue;
            else if (precision == 24)
                val = ushort.MaxValue;
            else
                val = int.MaxValue;

            return $"{val}";
        }

        public override string ToString()
        {
            return $"ColourIndexPrecision {_precision}";
        }

        public int Precision => _precision;
    }
}