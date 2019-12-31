using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace codessentials.CGM.Export
{
    internal class WriterBucket : List<byte>
    {
        public void WriteWord(int data)
        {
            Add((byte)(data >> 8 & 0xff));
            Add((byte)(data & 0xff));
        }

        public void WriteString(string value)
        {
            //int len = value.Length;
            //var t = false;
            //if (len >> 1 << 1 != len)
            //{
            //    //value = value + "\0";
            //    //len++;
            //    t = true;
            //}


            foreach (var c in value)
                Add((byte)c);

            //for (int i = 0; i < num - 1; i += 2)
            //{
            //    this.WriteWord((int)((byte)value[i]) << 8 | (int)((byte)value[i + 1]));
            //}

            
        }

        public void WriteFloat(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            base.Add(bytes[3]);
            base.Add(bytes[2]);
            base.Add(bytes[1]);
            base.Add(bytes[0]);
        }

        public void SaveToStream(Stream stream)
        {
            stream.Write(this.ToArray(), 0, base.Count);
        }       
    }

}
