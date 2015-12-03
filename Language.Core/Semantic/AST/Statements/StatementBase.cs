namespace Language.AST
{
    using System.Collections.Generic;

    public abstract class StatementBase : AstBase
    {
        public virtual List<StatementBase> Children { get; set; } = new List<StatementBase>();

        public int OrderNumber { get; set; }
    }
}
