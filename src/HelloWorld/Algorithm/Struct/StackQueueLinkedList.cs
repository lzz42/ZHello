using System;

namespace ZHello.Algorithm.Struct
{
    #region Exceptions

    public class UnderflowException : Exception
    {
    }

    public class OverflowException : Exception
    {
    }

    public class EmptyDataException : Exception
    {
    }

    #endregion Exceptions

    public class Stack<T>
    {
        public Stack(int cap)
        {
            Cap = cap;
            Data = new T[cap];
            Top = -1;
        }

        private T[] Data { get; set; }
        private int Top { get; set; }
        private int Cap { get; set; }

        public void Push(T t)
        {
            if (Top > Cap - 1)
                throw new UnderflowException();
            Top++;
            Data[Top] = t;
        }

        public T Pop()
        {
            if (Top < 0)
                throw new UnderflowException();
            T t = default(T);
            t = Data[Top];
            Top--;
            return t;
        }

        public bool IsEmpty()
        {
            return Top <= 0;
        }
    }

    public class Queue<T>
    {
        public Queue(int cap)
        {
            Cap = cap;
            Data = new T[cap];
            Tail = 0;
            Head = 0;
            IsOver = false;
        }

        private int Cap { get; set; }
        private T[] Data { get; set; }
        private int Tail { get; set; }
        private int Head { get; set; }
        private bool IsOver { get; set; }

        /*
         low----------------hight
         -----tail-----head------
         */

        public void InQueue(T t)
        {
            if (Head - Tail <= 0 && IsOver)
                throw new OverflowException();
            Data[Head] = t;
            Head++;
            if (Head >= Cap)
            {
                IsOver = true;
                Head = Head % Cap;
            }
        }

        public void DeQueue(out T t)
        {
            if (Head <= Tail && !IsOver)
                throw new UnderflowException();
            t = Data[Tail];
            Tail++;
            if (Tail >= Cap)
            {
                IsOver = false;
                Tail = Tail % Cap;
            }
        }
    }

    public class Node<T, K>
        where K : IComparable
    {
        public K Key { get; set; }
        public Node<T, K> Next { get; set; }
        public Node<T, K> Prev { get; set; }
        public T Data { get; set; }
    }

    public class DoublyLinkedList<N, T, K>
        where N : Node<T, K>
        where K : IComparable
    {
        public DoublyLinkedList()
        {
            Head = new Node<T, K>();
            //哨兵sentinel 哑对象
            nil = new Node<T, K>();
            nil.Next = nil;
            nil.Prev = nil;
            /*
            NIL==nil
            head=nil.next
            head.prev=nil
            tail=nil.prev
            tail.next=nil
             */
        }

        #region 双向链表

        public static Node<T, K> NIL = null;
        public Node<T, K> Head { get; set; }

        public void Insert(Node<T, K> node)
        {
            node.Next = Head;
            if (Head != NIL)
                Head.Prev = node;
            Head = node;
            node.Prev = NIL;
        }

        public void Delete(Node<T, K> node)
        {
            if (node.Prev != NIL)
                node.Prev.Next = node.Next;
            else
                Head = node.Next;
            if (node.Next != NIL)
                node.Next.Prev = node.Prev;
        }

        public Node<T, K> Search(K k)
        {
            var x = Head;
            while (x != NIL && k.CompareTo(x.Key) == 0)
                x = x.Next;
            return x;
        }

        #endregion 双向链表

        #region 使用哨兵的双向链表

        private Node<T, K> nil { get; set; }

        public void Insert_S(Node<T, K> x)
        {
            nil.Prev.Next = x;
            x.Prev = nil.Prev;
            x.Next = nil;
            nil.Prev = x;
        }

        public void Delete_S(Node<T, K> x)
        {
            x.Next.Prev = x.Prev;
            x.Prev.Next = x.Next;
        }

        public Node<T, K> Search_S(K k)
        {
            var x = nil.Next;
            while (k.CompareTo(x.Key) != 0 && x != nil)
                x = x.Next;
            return x;
        }

        #endregion 使用哨兵的双向链表
    }
}