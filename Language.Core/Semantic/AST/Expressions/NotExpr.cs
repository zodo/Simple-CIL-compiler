﻿namespace Language.AST.Expressions
{
    using Semantic.ASTVisitor;

    public class NotExpr : ExpressionBase
    {
        public override dynamic Accept(IAstVisitor visitor)
        {
            return visitor.Visit(this);
        }

     }
}