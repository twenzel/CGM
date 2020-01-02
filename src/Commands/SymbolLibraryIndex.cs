namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=48
    /// </summary>
    public class SymbolLibraryIndex : GenericIndexCommand
    {
        public SymbolLibraryIndex(CgmFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 48, container), "SYMBOLINDEX")
        {
        }

        public SymbolLibraryIndex(CgmFile container, int index)
            : this(container)
        {
            SetValue(index);
        }
    }
}
