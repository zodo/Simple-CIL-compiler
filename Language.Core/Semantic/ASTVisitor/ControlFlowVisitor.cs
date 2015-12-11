namespace Language.Semantic.ASTVisitor
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;

    using AST;
    using AST.Expressions;
    using AST.LeftExprSide;

    using Interfaces;

    using Language.AST;
    using Language.AST.Expressions;
    using Language.AST.Statements;

    public class ControlFlowVisitor : IAstVisitor
    {
        private readonly Stack<StatementBase> _callStack = new Stack<StatementBase>(); 

        private readonly Stack<CodeBlock> _codeBlocksStack = new Stack<CodeBlock>();


        private int _count = 0;

        private void VisitNext(StatementBase stm)
        {
            var st = NextStms(stm);
            if (st != null)
            {
                Visit((dynamic)st);
            }
        }

        private StatementBase NextStms(StatementBase stm)
        {
            var next = _codeBlocksStack.Peek().Statements.SkipWhile(x => x != stm).Skip(1).FirstOrDefault()
                       ?? NextReturnStm();
            _count++;
            return next;
            //if (next != null)
            //{
            //    Debug.WriteLine(next.Node.Token.ToString());
            //    var v = Visit((dynamic)next);
            //    if (v is StatementBase)
            //    {
            //        return new List<StatementBase> {v};
            //    }
            //    return v;
            //}
            //return new List<StatementBase>();
        }

        private StatementBase NextReturnStm()
        {
            //if (_callStack.Any())
            //{
            //    _count++;
            //    _codeBlocksStack.Pop();
            //    return _callStack.Pop();
            //}
            _count++;
            return null;
        }

        public dynamic Visit(Program program)
        {
            Visit((dynamic)program.FuncImplementations.Single(x => x.Name == "main"));
            return null;
        }

        public dynamic Visit(GlobalVariable variable)
        {
            return null;
        }

        public dynamic Visit(FuncImplementation impl)
        {
            Visit(impl.Code);
            return null;
        }

        public dynamic Visit(FuncDeclaration decl)
        {
            return null;
        }

        public dynamic Visit(Arguments arg)
        {
            return null;
        }

        public dynamic Visit(CallOrAssign stm)
        {
            stm.OrderNumber = _count;

            _callStack.Push(stm);
            Visit((dynamic)stm.LeftSideExpr);
            
            VisitNext(stm);
            
            return null;
        }

        public dynamic Visit(CodeBlock stm)
        {
            stm.OrderNumber = _count;
            _codeBlocksStack.Push(stm);
            Visit((dynamic)stm.Statements.First());
            return null;
        }

        public dynamic Visit(DoWhileStm stm)
        {

            return null;
        }

        public dynamic Visit(ForStm stm)
        {
            return null;
        }

        public dynamic Visit(IfStm stm)
        {
            stm.OrderNumber = _count;
            Visit((dynamic)stm.Condition);
            
            VisitNext(stm);
            return null;
        }

        public dynamic Visit(OperStm stm)
        {
            stm.OrderNumber = _count;
            VisitNext(stm);
            return null;
        }

        public dynamic Visit(ReturnStm stm)
        {
            stm.OrderNumber = _count;
            //Visit((dynamic)NextReturnStm());
            return null;
        }

        public dynamic Visit(StatementBase stm)
        {
            throw new System.NotImplementedException();
        }

        public dynamic Visit(LeftSideExprArray left)
        {
            return null;
        }

        public dynamic Visit(LeftSideExprBase left)
        {
            return null;
        }

        public dynamic Visit(LeftSideExprCall left)
        {
            Visit((dynamic)left.CallFunc);
            return null; 
        }

        public dynamic Visit(LeftSideExprVariable left)
        {
            return null;
        }

        public dynamic Visit(AddExpr expr)
        {
            Visit((ExpressionBase)expr);
            return null;
        }

        public dynamic Visit(AndOrExpr expr)
        {
            Visit((ExpressionBase)expr);
            return null;
        }

        public dynamic Visit(CallFuncExpr expr)
        {
            Visit((dynamic)expr.Function.Code);
            return null;
        }

        public dynamic Visit(CompareExpr expr)
        {
            Visit((ExpressionBase)expr);
            return null;
        }

        public dynamic Visit(CallCustomExpr expr)
        {
            Visit((ExpressionBase)expr);
            return null;
        }

        public dynamic Visit(ExpressionBase expr)
        {
            if (expr.First != null)
            {
                Visit((dynamic)expr.First);
            }
            if (expr.Second != null)
            {
                Visit((dynamic)expr.Second);
            }
            if (expr.Third != null)
            {
                Visit((dynamic)expr.Third);
            }
            return null;
        }

        public dynamic Visit(GetArrayExpr expr)
        {
            Visit((ExpressionBase)expr);
            Visit((dynamic)expr.Index);
            return null;
        }

        public dynamic Visit(GetVariableExpr expr)
        {
            Visit((ExpressionBase)expr);
            return null;
        }

        public dynamic Visit(LiteralExpr expr)
        {
            Visit((ExpressionBase)expr);
            return null;
        }

        public dynamic Visit(MultPowExpr expr)
        {
            Visit((ExpressionBase)expr);
            return null;
        }

        public dynamic Visit(NotExpr expr)
        {
            Visit((ExpressionBase)expr);
            return null;
        }

        public dynamic Visit(TernaryExpr expr)
        {
            Visit((ExpressionBase)expr);
            return null;
        }

        public dynamic Visit(UnaryExpr expr)
        {
            Visit((ExpressionBase)expr);
            return null;
        }
    }
}
