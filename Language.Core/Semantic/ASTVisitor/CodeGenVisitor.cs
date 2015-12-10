namespace Language.Semantic.ASTVisitor
{
    using System;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Security.Policy;

    using AST;
    using AST.Expressions;
    using AST.LeftExprSide;

    using Data;

    using Language.AST;
    using Language.AST.Expressions;
    using Language.AST.Statements;
    using TriAxis.RunSharp;

    public class CodeGenVisitor : IAstVisitor
    {
        private readonly Program _program;

        private MethodGen _currentMethod;

        private CodeGen _codeGen => _currentMethod;

        private TypeGen _currentProgram;

        private AssemblyGen _assemblyGen;

        public CodeGenVisitor(Program program)
        {
            _program = program;
        }

        public void GenerateAssembly(string path)
        {
            _assemblyGen = new AssemblyGen("output", new CompilerOptions {OutputPath = path, SymbolInfo = true, TargetFrameworkName = "4.5", TargetFrameworkDisplayName = "4.5"});
            using (_assemblyGen.Namespace("Language"))
            {
                _currentProgram = _assemblyGen.Public.Class("Program");
                {
                    Visit(_program);
                }
                var type = _currentProgram.GetCompletedType(true);
                _currentProgram = _assemblyGen.Public.Class("Runner");
                {
                    CodeGen method = _currentProgram.Public.Static.Method(typeof(void), "Main");
                    {
                        Operand obj = method.Local(_currentProgram.ExpressionFactory.New(type));
                        method.Invoke(obj, "main");
                        method.Invoke(typeof(Console), "ReadKey");
                    }
                }
            }
            _assemblyGen.Save();
            
        }



        public dynamic Visit(AddExpr expr)
        {
            var left = _codeGen.Local(Visit((dynamic)expr.First)) as Operand;
            var right = _codeGen.Local(Visit((dynamic)expr.Second)) as Operand;
            switch (expr.Type)
            {
                case AddType.Plus:
                    return left + right;
                case AddType.Minus:
                    return left - right;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public dynamic Visit(AndOrExpr expr)
        {
            var left = _codeGen.Local(Visit((dynamic)expr.First)) as Operand;
            var right = _codeGen.Local(Visit((dynamic)expr.Second)) as Operand;
            switch (expr.Type)
            {
                case AndOrOperation.Or:
                    return left || right;
                case AndOrOperation.And:
                    return left && right;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public dynamic Visit(CallFuncExpr expr)
        {
            var objects = expr.Arguments.Values.Select(x => Visit((dynamic)x)).Cast<Operand>().ToArray();

            return _codeGen.This().Invoke(expr.Function.Name, objects);
        }

        public dynamic Visit(CompareExpr expr)
        {
            var left = (Visit((dynamic)expr.First)) as Operand;
            var right = (Visit((dynamic)expr.Second)) as Operand;
            switch (expr.Type)
            {
                case CompareType.Eq:
                    return left == right;
                case CompareType.NotEq:
                    return left != right;
                case CompareType.More:
                    return left > right;
                case CompareType.Less:
                    return left< right;
                case CompareType.MoreEq:
                    return left >= right;
                case CompareType.LeesEq:
                    return left <= right;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public dynamic Visit(ConsoleReadExpr expr)
        {
            if (expr.Type == SymbolType.String)
            {
                return _assemblyGen.StaticFactory.Invoke(typeof(Console), "ReadLine");
            }
            var str = _assemblyGen.StaticFactory.Invoke(typeof(Console), "ReadLine");
            return _assemblyGen.StaticFactory.Invoke(typeof(Convert), "ToInt32", str);
        }

        public dynamic Visit(ExpressionBase expr)
        {
            throw new System.NotImplementedException();
        }

        public dynamic Visit(GetArrayExpr expr)
        {
            throw new System.NotImplementedException();
        }

        public dynamic Visit(GetVariableExpr expr)
        {
            var symbol = expr.Namespace.Symbols.SingleOrDefault(x => x.Name == expr.Name);
            //var local = _codeGen.Local(symbol.CodeGenField);
            return symbol.CodeGenField as Operand;
        }

        public dynamic Visit(LiteralExpr expr)
        {
            return _codeGen.Local(expr.Value);
        }

        public dynamic Visit(MultPowExpr expr)
        {
            var left = _codeGen.Local(Visit((dynamic)expr.First)) as Operand;
            var right = _codeGen.Local(Visit((dynamic)expr.Second)) as Operand;
            switch (expr.Type)
            {
                case MultPowDivType.Mult:
                    return left * right;
                case MultPowDivType.Div:
                    var d = _assemblyGen.StaticFactory.Invoke(typeof(Convert), "ToDouble", left);
                    return d / right;
                case MultPowDivType.IntDiv:
                    var res = left / right;
                    return _assemblyGen.StaticFactory.Invoke(typeof(Convert), "ToInt32", res);
                case MultPowDivType.Mod:
                    return left.Modulus(right);
                case MultPowDivType.Pow:
                    return _assemblyGen.StaticFactory.Invoke(typeof(Math), "Pow", left, right);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public dynamic Visit(NotExpr expr)
        {
            var v = _codeGen.Local(Visit((dynamic)expr.First)) as Operand;
            return v.LogicalNot();
        }

        public dynamic Visit(TernaryExpr expr)
        {
            var cond = _codeGen.Local(Visit((dynamic)expr.First)) as Operand;
            var first = _codeGen.Local(Visit((dynamic)expr.Second)) as Operand;
            var second = _codeGen.Local(Visit((dynamic)expr.Third)) as Operand;
            return cond.Conditional(first, second);
        }

        public dynamic Visit(UnaryExpr expr)
        {
            var first = Visit((dynamic)expr.First) as Operand;
            switch (expr.Type)
            {
                case UnaryType.PlusPlus:
                    return first.PostIncrement();
                case UnaryType.Minus:
                    return -first;
                case UnaryType.MinusMinus:
                    return first.PreDecrement();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public dynamic Visit(CallOrAssign stm)
        {
            if (stm.AssignExpression == null)
            {
                Visit((dynamic)stm.LeftSideExpr);
                return null;
            }

            string name = Visit((dynamic)stm.LeftSideExpr).Name;

            var symbol = stm.Namespace.Symbols.SingleOrDefault(x => x.Name == name);
            if (ReferenceEquals(symbol.CodeGenField, null))
            {
                symbol.CodeGenField = _codeGen.Local(Visit((dynamic)stm.AssignExpression));
            }
            else
            {
                _codeGen.Assign(symbol.CodeGenField, Visit((dynamic)stm.AssignExpression));
            }
            return null;
        }

        public dynamic Visit(CodeBlock stm)
        {
            foreach (var statement in stm.Statements)
            {
                //_codeGen.BeginScope();
                Visit((dynamic)statement);
                //_codeGen.End();
            }
            return null;
        }

        public dynamic Visit(DoWhileStm stm)
        {
            _codeGen.While(Visit((dynamic)stm.Condition));
            {
                Visit((dynamic)stm.Statements);
            }
            _codeGen.End();
            return null;
        }

        public dynamic Visit(ForStm stm)
        {
            throw new System.NotImplementedException();
        }

        public dynamic Visit(IfStm stm)
        {
            _codeGen.If(Visit((dynamic)stm.Condition));
            {
                Visit((dynamic)stm.IfTrue);
            }
            if (stm.IfFalse != null)
            {
                _codeGen.Else();
                {
                    Visit((dynamic)stm.IfFalse);
                }
            }
            _codeGen.End();
            return null;
        }

        public dynamic Visit(OperStm stm)
        {
            if (stm.Operation == "write")
            {
                var objects = stm.Arguments?.Values.Select(x => Visit((dynamic)x)).Cast<Operand>().ToArray() ?? new Operand[0];
                _codeGen.WriteLine(objects);
            }
            return null;
        }

        public dynamic Visit(ReturnStm stm)
        {
            var type = stm.ReturnExpression.GetExprType();
            if (type == SymbolType.Void || type == SymbolType.Unknown)
            {
                _codeGen.Return();
            }
            else
            {
                var operand = Visit((dynamic)stm.ReturnExpression);
                _codeGen.Return(operand);
            }
            
            return null;
        }

        public dynamic Visit(StatementBase stm)
        {
            throw new System.NotImplementedException();
        }

        public dynamic Visit(Program program)
        {
            foreach (var t in program.GlobalVariables)
            {
                Visit(t);
            }
            foreach (var impl in program.FuncImplementations)
            {
                _currentMethod = _currentProgram.Public.Method(impl.ReturnType.CodeGenType(), impl.Name);
                Visit((dynamic)impl);
            }

            return null;
        }

        public dynamic Visit(GlobalVariable variable)
        {
            if (variable.Value.Value != null)
            {
                variable.Value.CodeGenField = _currentProgram.Public.Field(variable.Value.Type.CodeGenType(), variable.Value.Name, variable.Value.Value) as FieldGen;
                
            }
            else
            {
                variable.Value.CodeGenField = _currentProgram.Public.Field(variable.Value.Type.CodeGenType(), variable.Value.Name) as FieldGen;
            }

            return null;
        }

        public dynamic Visit(FuncImplementation impl)
        {
            impl.CodeGenMethod = _currentMethod;
            foreach (var param in impl.Parameters)
            {
                _currentMethod.Parameter(param.Value.CodeGenType(), param.Key);
            }
            foreach (var param in impl.Parameters)
            {
                impl.Namespace.Symbols
                   .SingleOrDefault(x => x.Name == param.Key)
                   .CodeGenField = _codeGen.Arg(param.Key);
            }
            if (impl.ReturnExpression != null)
            {
                _codeGen.Return(Visit((dynamic)impl.ReturnExpression));
            }
            if (impl.Code != null)
            {
                foreach (var statement in impl.Code.Statements)
                {
                    Visit((dynamic)statement);
                }
            }
            return null;
        }

        public dynamic Visit(FuncDeclaration decl)
        {
            throw new System.NotImplementedException();
        }

        public dynamic Visit(Arguments arg)
        {
            throw new System.NotImplementedException();
        }

        public dynamic Visit(LeftSideExprArray left)
        {
            throw new System.NotImplementedException();
        }

        public dynamic Visit(LeftSideExprBase left)
        {
            throw new System.NotImplementedException();
        }

        public dynamic Visit(LeftSideExprCall left)
        {
            var objects = left.CallFunc.Arguments.Values.Select(x => Visit((dynamic)x)).Cast<Operand>().ToArray();
            _codeGen.Invoke(_codeGen.This(), left.CallFunc.Function.Name, objects);
            
            return null;
        }

        public dynamic Visit(LeftSideExprVariable left)
        {
            return new { left.Name};
        }
    }
}
