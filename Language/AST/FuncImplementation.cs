using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Language.AST
{
    using Semantic.Data;

    public class FuncImplementation : AstBase
    {
        public List<Symbol> Arguments { get; set; } = new List<Symbol>(); 

        public SymbolType ReturnType { get; set; }

        public string Name { get; set; }
    }
}
