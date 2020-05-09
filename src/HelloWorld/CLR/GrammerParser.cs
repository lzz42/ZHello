using System.Collections.Generic;
using System.Linq;

namespace ZHello.CLR
{
    /*
     语法分析->语法树
     语义分析->执行树
         */

    public abstract class Expression
    {
    }

    public class FilterExpression : Expression
    {
        //public
        public FilterExpression()
        {
        }
    }

    public abstract class FilterUtility<T>
    {
        public abstract Expression Filter(string expression);

        public abstract bool IsLogicalFilter(string expression);
    }

    public class TreeNodeFilter<T> : FilterUtility<T>
    {
        public const char And_Op = '&';
        public const char Or_Op = ' ';
        public const char Group_Start_Op = '(';
        public const char Group_End_Op = ')';

        private Queue<char> _inpust { get; set; }

        public override Expression Filter(string expression)
        {
            if (string.IsNullOrEmpty(expression))
                return null;
            var queue = new Queue<char>();

            return null;
        }

        public override bool IsLogicalFilter(string expression)
        {
            if (!string.IsNullOrEmpty(expression))
            {
                return expression.Contains(And_Op) || expression.Contains(Or_Op);
            }
            return false;
        }
    }

    public interface FilterNode
    {
        bool Contains(string key);

        void ModifyProperties(bool visiable);
    }

    public abstract class Context
    {
        //public char[]
    }

    public abstract class Token
    {
        public char[] TokenOperators { get; set; }
    }

    public class Intersection : Token
    {
        public ExpressionNode LeftNode { get; set; }
        public ExpressionNode RightNode { get; set; }
    }

    public class Union : Token
    {
        public ExpressionNode LeftNode { get; set; }
        public ExpressionNode RightNode { get; set; }
    }

    public class ExpressionNode
    {
        public Token Token { get; set; }
    }

    public class LeafExpressionNode
    {
    }

    /// <summary>
    /// 语法分析
    /// </summary>
    public class SyntaxAnalyzer
    {
    }

    /// <summary>
    /// 语法树
    /// </summary>
    public class SyntaxTree
    {
    }

    /// <summary>
    /// 语义分析
    /// </summary>
    public class SemanticAnalyzer
    {
    }

    public class SemanticTree
    {
    }
}