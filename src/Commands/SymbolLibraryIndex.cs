namespace codessentials.CGM.Commands
{
    /// <summary>
    /// Class=5, ElementId=48
    /// </summary>
    public class SymbolLibraryIndex : GenericIndexCommand
    {
        public SymbolLibraryIndex(CGMFile container)
            : base(new CommandConstructorArguments(ClassCode.AttributeElements, 48, container), "SYMBOLINDEX")
        {
        }

        public SymbolLibraryIndex(CGMFile container, int index)
            : this(container)
        {
            SetValue(index);
        }
    }
}
