namespace codessentials.CGM.Commands
{
    public class CharacterOrientation : Command
    {
        public double Xup { get; private set; }
        public double yup { get; private set; }
        public double Xbase { get; private set; }
        public double Ybase { get; private set; }

        public CharacterOrientation(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 16, container))
        {

        }

        public CharacterOrientation(CgmFile container, double xUp, double yUp, double xBase, double yBase)
            : this(container)
        {
            Xup = xUp;
            yup = yUp;
            Xbase = xBase;
            Ybase = yBase;
        }

        public override void ReadFromBinary(IBinaryReader reader)
        {
            Xup = reader.ReadVdc();
            yup = reader.ReadVdc();
            Xbase = reader.ReadVdc();
            Ybase = reader.ReadVdc();
        }

        public override void WriteAsBinary(IBinaryWriter writer)
        {
            writer.WriteVdc(Xup);
            writer.WriteVdc(yup);
            writer.WriteVdc(Xbase);
            writer.WriteVdc(Ybase);
        }

        public override void WriteAsClearText(IClearTextWriter writer)
        {
            writer.WriteLine($"  CHARORI {WriteVDC(Xup)} {WriteVDC(yup)}, {WriteVDC(Xbase)} {WriteVDC(Ybase)};");
        }

        public override string ToString()
        {
            return $"CharacterOrientation xUp = {Xup}";
        }

        public bool IsDownToUp()
        {
            return Xup == -1 && yup == 0 && Xbase == 0 && Ybase == 1;
        }

        public bool IsLeftToRight()
        {
            return Xup == 0 && yup == 1 && Xbase == 1 && Ybase == 0;
        }
    }
}