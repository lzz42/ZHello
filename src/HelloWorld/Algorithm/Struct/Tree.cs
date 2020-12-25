using System;
using System.Diagnostics;

namespace ZHello.Algorithm.Struct
{
    /// <summary>
    /// 二叉树
    /// </summary>
    /// <typeparam name="K"></typeparam>
    public class BTree<K>
        where K : IComparable
    {
        public K Key { get; set; }
        public BTree<K> Left { get; set; }
        public BTree<K> Right { get; set; }
    }

    /// <summary>
    /// 左孩子右兄弟的有根树
    /// </summary>
    /// <typeparam name="K"></typeparam>
    public class NTree<K>
        where K : IComparable
    {
        public K Key { get; set; }

        /// <summary>
        /// 节点x左侧孩子节点,无孩子节点为NIL
        /// </summary>
        public NTree<K> LeftChild { get; set; }

        /// <summary>
        /// 节点x右侧兄弟节点,无右侧兄弟节点为NIL
        /// </summary>
        public NTree<K> RightSibling { get; set; }
    }

    public static class P10_4
    {
        /// <summary>
        /// 遍历顺序
        /// </summary>
        public enum TraverseOrder
        {
            /// <summary>
            /// 先序遍历 根-左子-右子
            /// </summary>
            PreOrder = 1,

            /// <summary>
            /// 中序遍历 左子-根-右子
            /// </summary>
            InOrder = 2,

            /// <summary>
            /// 后序遍历 左子-右子-根
            /// </summary>
            PostOrder = 3,

            /// <summary>
            /// 层级优先遍历
            /// </summary>
            LevelOrder = 4,
        }

        /// <summary>
        /// 10.4.2
        /// n结点的二叉树，使用O(n)的递归过程，遍历所有的节点的关键字
        /// </summary>
        /// <typeparam name="N"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="tree"></param>
        /// <param name="node"></param>
        public static void PrintBTree<K>(BTree<K> tree, TraverseOrder order)
            where K : IComparable
        {
            if (tree == null)
                return;
            switch (order)
            {
                case TraverseOrder.PreOrder:
                    Trace.WriteLine(tree.Key);
                    PrintBTree(tree.Left, order);
                    PrintBTree(tree.Right, order);
                    break;

                case TraverseOrder.InOrder:
                    PrintBTree(tree.Left, order);
                    Trace.WriteLine(tree.Key);
                    PrintBTree(tree.Right, order);
                    break;

                case TraverseOrder.PostOrder:
                    PrintBTree(tree.Left, order);
                    PrintBTree(tree.Right, order);
                    Trace.WriteLine(tree.Key);
                    break;

                case TraverseOrder.LevelOrder:
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 10.4.3
        /// n结点的二叉树，使用O(n)的非递归过程，遍历所有的节点的关键字
        /// </summary>
        /// <typeparam name="N"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="tree"></param>
        /// <param name="node"></param>
        public static void PrintBTree<N, K>(BTree<K> tree, TraverseOrder order)
            where K : IComparable
        {
            if (tree == null)
                return;
            var stack = new Stack<BTree<K>>(1024);
            switch (order)
            {
                case TraverseOrder.PreOrder:
                    //前序遍历
                    while (tree != null || !stack.IsEmpty())
                    {
                        while (tree != null)
                        {
                            Trace.WriteLine(tree.Key);
                            stack.Push(tree);
                            tree = tree.Left;
                        }
                        if (!stack.IsEmpty())
                        {
                            tree = stack.Pop();
                            tree = tree.Right;
                        }
                    }
                    break;
                case TraverseOrder.InOrder:
                    //中序遍历
                    while (tree != null || !stack.IsEmpty())
                    {
                        while (tree != null)
                        {
                            stack.Push(tree);
                            tree = tree.Left;
                        }
                        if (!stack.IsEmpty())
                        {
                            tree = stack.Pop();
                            Trace.WriteLine(tree.Key);
                            tree = tree.Right;
                        }
                    }
                    break;
                case TraverseOrder.PostOrder:
                    //后序遍历
                    var stack2 = new Stack<BTree<K>>(1024);
                    while (tree != null || !stack.IsEmpty())
                    {
                        while (tree != null)
                        {
                            stack.Push(tree);
                            stack2.Push(tree);
                            tree = tree.Right;
                        }
                        if (!stack.IsEmpty())
                        {
                            tree = stack.Pop();
                            tree = tree.Left;
                        }
                    }
                    while (!stack2.IsEmpty())
                    {
                        tree = stack2.Pop();
                        Trace.WriteLine(tree.Key);
                    }
                    break;
                case TraverseOrder.LevelOrder:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 10.4.4
        /// n结点的任意有根树，使用O(n)的算法，遍历所有的节点的关键字
        /// 该树以左孩子右兄弟表示
        /// </summary>
        /// <typeparam name="N"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="tree"></param>
        /// <param name="node"></param>
        public static void PrintNTree<K>(NTree<K> tree, TraverseOrder order)
            where K : IComparable
        {
            if (tree == null)
                return;
            switch (order)
            {
                case TraverseOrder.PreOrder:
                    Trace.WriteLine(tree.Key);
                    PrintNTree(tree.LeftChild, order);
                    PrintNTree(tree.RightSibling, order);
                    break;
                case TraverseOrder.InOrder:
                    PrintNTree(tree.LeftChild, order);
                    Trace.WriteLine(tree.Key);
                    PrintNTree(tree.RightSibling, order);
                    break;
                case TraverseOrder.PostOrder:
                    PrintNTree(tree.LeftChild, order);
                    PrintNTree(tree.RightSibling, order);
                    Trace.WriteLine(tree.Key);
                    break;
                case TraverseOrder.LevelOrder:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 10.4.5
        /// n结点的二叉树，使用O(n)的非递归算法，遍历所有的节点的关键字
        /// 要求：除树本身存储空间外只能使用固定量的额外存储空间，且不能修改树
        /// </summary>
        /// <typeparam name="N"></typeparam>
        /// <typeparam name="K"></typeparam>
        /// <param name="tree"></param>
        /// <param name="node"></param>
        public static void PrintBTree5<K>(BTree<K> tree)
            where K : IComparable
        {
            if (tree == null)
                return;
        }
    }
}