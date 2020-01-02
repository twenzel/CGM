namespace codessentials.CGM.Commands
{
    public class CharacterSetIndex : GenericIndexCommand
    {
        public CharacterSetIndex(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 19, container), "charsetindex")
        {

        }

        public CharacterSetIndex(CGMFile container, int index)
            : this(container)
        {
            Index = index;
        }
    }
}