using System;
using System.Collections.Generic;
using System.Text;

namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=1, Element=14
    /// </summary>
    public class CharacterSetList : Command
    {
        public enum Type
        {
            _94_CHAR_G_SET,
            _96_CHAR_G_SET,
            _94_CHAR_MBYTE_G_SET,
            _96_CHAR_MBYTE_G_SET,
            COMPLETE_CODE
        }

        public List<KeyValuePair<Type, string>> CharacterSets { get; }

        public CharacterSetList(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.MetafileDescriptorElements, 14, container))
        {

        }

        public CharacterSetList(CGMFile container, KeyValuePair<Type, string>[] items)
            : this(container)
        {
            CharacterSets.AddRange(items);
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            while (reader.CurrentArg < reader.ArgumentsCount)
            {
                var value = reader.ReadEnum();
                Type characterType;
                switch (value)
                {
                    case 0: // 94 character G set
                        characterType = Type._94_CHAR_G_SET;
                        break;
                    case 1: // 96 character G set
                        characterType = Type._96_CHAR_G_SET;
                        break;
                    case 2: // 94 character multibyte G set
                        characterType = Type._94_CHAR_MBYTE_G_SET;
                        break;
                    case 3: // 96 character multibyte G set
                        characterType = Type._96_CHAR_MBYTE_G_SET;
                        break;
                    case 4:
                        characterType = Type.COMPLETE_CODE;
                        break;
                    default:
                        // XXX: which default to use?
                        characterType = Type.COMPLETE_CODE;
                        reader.Unsupported("unsupported character set type " + value);
                        break;
                }

                var characterSetDesignation = reader.ReadFixedString();
                CharacterSets.Add(new KeyValuePair<Type, string>(characterType, characterSetDesignation));

                // TODO:ss
                //if (characterSetDesignation.Length > 2)
                //{
                //    int c = characterSetDesignation[0];
                //    if (c == 27)
                //    {
                //        // 27 == ESC
                //        c = characterSetDesignation[1];
                //        if (c == 22)
                //        {
                //            int revNumber = characterSetDesignation[2];
                //        }
                //    }
                //}
            }
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            foreach (var pair in CharacterSets)
            {
                writer.WriteEnum((int)pair.Key);
                writer.WriteString(pair.Value);
            }
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.Write(" charsetlist");

            foreach (var pair in CharacterSets)
            {
                switch (pair.Key)
                {
                    case Type._94_CHAR_G_SET:
                        writer.Write(" STD94 ");
                        break;
                    case Type._96_CHAR_G_SET:
                        writer.Write(" STD96 ");
                        break;
                    case Type._94_CHAR_MBYTE_G_SET:
                        writer.Write(" STD94MULTIBYTE ");
                        break;
                    case Type._96_CHAR_MBYTE_G_SET:
                        writer.Write(" STD96MULTIBYTE ");
                        break;
                    case Type.COMPLETE_CODE:
                        writer.Write(" COMPLETECODE ");
                        break;
                    default:
                        throw new NotImplementedException($"Charsetlist type {pair.Key} not supported.");
                }

                writer.Write(WriteString(pair.Value));
            }

            writer.WriteLine(";");

            // writer.WriteLine($" fontlist '{string.Join("', '", _fontNames)}';");
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("CharacterSetList ");

            foreach (var pair in CharacterSets)
            {
                sb.Append($"[{pair.Key},{pair.Value}]");
            }

            return sb.ToString();
        }        
    }
}