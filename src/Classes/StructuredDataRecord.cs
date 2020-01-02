using System.Collections.Generic;
using System.Diagnostics;

namespace codessentials.CGM.Classes
{
    /// <summary>
    /// Structured Data Record container
    /// </summary>
    public class StructuredDataRecord
    {
        public enum StructuredDataType
        {
            SDR = 1,
            CI = 2,
            CD = 3,
            N = 4,
            E = 5,
            I = 6,
            RESERVED = 7,
            IF8 = 8,
            IF16 = 9,
            IF32 = 10,
            IX = 11,
            R = 12,
            S = 13,
            SF = 14,
            VC = 15,
            VDC = 16,
            CCO = 17,
            UI8 = 18,
            UI32 = 19,
            BS = 20,
            CL = 21,
            UI16 = 22
        }

        /// <summary>
        /// One entry in the structured data record
        /// </summary>
        [DebuggerDisplay("Type {Type}, count {Count}")]
        public class Member
        {
            public StructuredDataType Type { get; private set; }
            public int Count { get; private set; }
            public List<object> Data { get; private set; }

            public Member(StructuredDataType type, int count, List<object> data)
            {
                Type = type;
                Count = count;
                Data = data;
            }
        }

        public List<Member> Members { get; }

        public void Add(StructuredDataType type, int count, List<object> data)
        {
            Members.Add(new Member(type, count, data));
        }

        public void Add(StructuredDataType type, object[] data)
        {
            var list = new List<object>(data);
            Add(type, list.Count, list);
        }
    }
}
