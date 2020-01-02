namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=10
    /// </summary>
    public class TextFontIndex : GenericIndexCommand
    {
        public TextFontIndex(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 10, container), "TEXTFONTINDEX")
        {

        }

        public TextFontIndex(CgmFile container, int index)
            : this(container)
        {
            SetValue(index);
        }
    }
}
