namespace codessentials.CGM.Commands
{
    public class CharacterSetIndex : GenericIndexCommand
    {
        public CharacterSetIndex(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 19, container), "charsetindex")
        {

        }

        public CharacterSetIndex(CgmFile container, int index)
            : this(container)
        {
            Index = index;
        }
    }
}