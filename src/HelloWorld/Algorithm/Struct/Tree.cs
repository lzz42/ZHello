using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZHello.Algorithm.Struct
{
    public class TNode<K>
        where K : IComparable
    {
        public static TNode<K> NIL = null;
        public K Key { get; set; }
        public TNode<K> P { get; set; }
    }

    public class BTNode<K> : TNode<K>
        where K : IComparable
    {
        public BTNode<K> Left { get; set; }
        public BTNode<K> Right { get; set; }
    }

    public class BTree<N, K>
        where K : IComparable
        where N : BTNode<K>
    {
        public N NIL = null;

        public BTree()
        {
            Root = NIL;
        }

        public N Root { get; set; }

        public bool IsEmpty()
        {
            return Root == NIL;
        }

        public bool IsRoot(N x)
        {
            return x.P == NIL;
        }
    }

    public class NTNode<K> : TNode<K>
        where K : IComparable
    {
        /// <summary>
        /// 节点x左侧孩子节点,无孩子节点为NIL
        /// </summary>
        public NTNode<K> LeftChild { get; set; }

        /// <summary>
        /// 节点x右侧兄弟节点,无右侧兄弟节点为NIL
        /// </summary>
        public NTNode<K> RightSibling { get; set; }
    }

    public class NTree<N, K>
        where K : IComparable
        where N : NTNode<K>
    {
        public static N NIL = null;
        public N Root { get; set; }
    }

    public static class P10_4
    {
        /// <summary>
        /// 10.4.2
        /// </summary>
        /// <typeparam name="N"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="tree"></param>
        /// <param name="node"></param>
        public static void PrintBTree<N, K>(BTree<N, K> tree, BTNode<K> node = null)
            where K : IComparable
            where N : BTNode<K>
        {
            if (node == null)
            {
                if (tree == null)
                    return;
                if (tree.IsEmpty())
                    return;
                PrintBTree(tree, tree.Root);
            }
            else
            {
                Trace.WriteLine(node.Key);
                if (node.Left != null)
                    PrintBTree(tree, node.Left);
                if (node.Right != null)
                    PrintBTree(tree, node.Right);
            }
        }

        /// <summary>
        /// 10.4.3
        /// </summary>
        /// <typeparam name="N"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="tree"></param>
        /// <param name="node"></param>
        public static void PrintBTree<N, K>(BTree<N, K> tree)
            where K : IComparable
            where N : BTNode<K>
        {
            if (tree == null)
                return;
            if (tree.IsEmpty())
                return;
            var node = (BTNode<K>)tree.Root;
            var stack = new Stack<BTNode<K>>(1024);
            while (node != null || !stack.IsEmpty())
            {
                while (node != null)
                {
                    Trace.WriteLine(node.Key);
                    stack.Push(node);
                    node = node.Left;
                }
                if (!stack.IsEmpty())
                {
                    node = stack.Pop();
                    node = node.Right;
                }
            }
        }

        /// <summary>
        /// 10.4.4
        /// </summary>
        /// <typeparam name="N"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="tree"></param>
        /// <param name="node"></param>
        public static void PrintNTree<N, K>(NTree<N, K> tree, NTNode<K> node = null)
            where K : IComparable
            where N : NTNode<K>
        {
            if (node == null)
            {
                if (tree == null)
                    return;
                PrintNTree(tree, tree.Root);
            }
            else
            {
                Trace.WriteLine(node.Key);
                if (node.LeftChild != null)
                    PrintNTree(tree, node.LeftChild);
                if (node.RightSibling != null)
                    PrintNTree(tree, node.RightSibling);
            }
        }

        /// <summary>
        /// 10.4.5
        /// </summary>
        /// <typeparam name="N"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="tree"></param>
        /// <param name="node"></param>
        public static void PrintBTree5<N, K>(BTree<N, K> tree)
            where K : IComparable
            where N : BTNode<K>
        {
            if (tree == null)
                return;
            if (tree.IsEmpty())
                return;
            var node = tree.Root;
            //
        }

    }
}
