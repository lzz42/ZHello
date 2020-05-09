#pragma warning disable CS3003,CS3001

using System;
using System.Collections.Generic;

namespace ZHello.Algorithm
{
    public interface ICache<T, TK, TV>
        where T : IDataItem<TK, TV>
    {
        void Clear();

        void Set(T t);

        bool Get(ref T t);
    }

    public interface IDataItem<TK, TV>
    {
        TK Key { get; set; }
        TV Value { get; set; }

        IDataItem<TK, TV> Make(TK tK, TV tV);
    }

    public class DataItem<TK, TV> : IDataItem<TK, TV>
        where TK : class
    {
        public DataItem()
        {
        }

        public TK Key { get; set; }
        public TV Value { get; set; }

        public IDataItem<TK, TV> Make(TK tK, TV tV)
        {
            return new DataItem<TK, TV>() { Key = tK, Value = tV };
        }
    }

    public abstract class Cache<T, TK, TV> : ICache<T, TK, TV>
        where T : IDataItem<TK, TV>
    {
        public abstract void Clear();

        public abstract bool Get(ref T t);

        public abstract void Set(T t);
    }

    public static class LruFunction
    {
        public static void Sort<T>(LinkedList<T> link, LinkedListNode<T> rNode)
        {
            if (link == null)
            {
                link = new LinkedList<T>();
            }
            link.Remove(rNode);
            link.AddFirst(rNode);
        }

        public static bool SearchData<T, TK, TV>(LinkedList<T> link, TK tK, out LinkedListNode<T> rNode)
            where T : IDataItem<TK, TV>
        {
            if (link.Count > 0)
            {
                return SearchData<T, TK, TV>(link.First, tK, out rNode);
            }
            else
            {
                rNode = null;
                return false;
            }
        }

        public static bool SearchData<T, TK, TV>(LinkedListNode<T> node, TK tK, out LinkedListNode<T> rNode)
            where T : IDataItem<TK, TV>
        {
            if (node != null)
            {
                if (node.Value.Key.Equals(tK))
                {
                    rNode = node;
                    return true;
                }
                else
                {
                    return SearchData<T, TK, TV>(node.Next, tK, out rNode);
                }
            }
            rNode = null;
            return false;
        }

        public static bool SearchHistoryData<T, TK, TV>(LinkedListNode<Tuple<T, int>> node, TK tK, out LinkedListNode<Tuple<T, int>> rNode)
            where T : IDataItem<TK, TV>
        {
            if (node != null)
            {
                if (node.Value.Item1.Key.Equals(tK))
                {
                    rNode = node;
                    return true;
                }
                else
                {
                    return SearchHistoryData<T, TK, TV>(node.Next, tK, out rNode);
                }
            }
            rNode = null;
            return false;
        }

        public static bool SearchHistoryData<T, TK, TV>(LinkedList<Tuple<T, int>> link, TK tK, out LinkedListNode<Tuple<T, int>> rNode)
            where T : IDataItem<TK, TV>
        {
            if (link.Count > 0)
            {
                return SearchHistoryData<T, TK, TV>(link.First, tK, out rNode);
            }
            else
            {
                rNode = null;
                return false;
            }
        }

        public static bool SearchQueue<T, TK, TV>(Queue<T> queue, TK tK, out T t)
            where T : IDataItem<TK, TV>
        {
            var result = false;
            t = default(T);
            if (queue != null && queue.Count > 0)
            {
                var len = queue.Count;
                for (int i = 0; i < len; i++)
                {
                    var temp = queue.Dequeue();
                    if (temp.Key.Equals(tK))
                    {
                        t = temp;
                        result = true;
                    }
                    else
                    {
                        queue.Enqueue(temp);
                    }
                }
            }
            return result;
        }
    }

    public class LRU<T, TK, TV> : Cache<T, TK, TV>
        where T : IDataItem<TK, TV>
    {
        private LinkedList<T> mLink;
        public uint Cap { get; private set; }

        public LRU(int capacity)
        {
            System.Diagnostics.Contracts.Contract.Requires(capacity > 0);
            Cap = (uint)capacity;
            mLink = new LinkedList<T>();
        }

        public override void Clear()
        {
            mLink.Clear();
        }

        /// <summary>
        /// 访问数据项
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public override bool Get(ref T t)
        {
            t.Value = default(TV);
            LinkedListNode<T> rNode;
            if (LruFunction.SearchData<T, TK, TV>(mLink, t.Key, out rNode))
            {
                //将查找的节点移动到链表头部
                LruFunction.Sort(mLink, rNode);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加数据项
        /// </summary>
        /// <param name="t"></param>
        public override void Set(T t)
        {
            while (mLink.Count > Cap)
            {
                mLink.RemoveLast();
            }
            LinkedListNode<T> rNode;
            if (LruFunction.SearchData<T, TK, TV>(mLink, t.Key, out rNode))
            {
                //将查找的节点移动到链表头部
                LruFunction.Sort(mLink, rNode);
            }
            else
            {
                mLink.RemoveLast();
                mLink.AddFirst(t);
            }
        }
    }

    public class LRU_K<T, TK, TV> : Cache<T, TK, TV>
        where T : IDataItem<TK, TV>
    {
        private LinkedList<Tuple<T, int>> mLinkedList_his { get; set; }
        private LinkedList<T> mLink { get; set; }

        public uint K { get; private set; }

        public uint Cap { get; private set; }

        public uint CapHistory { get; private set; }

        public LRU_K(int cap, int capHistory, uint k)
        {
            System.Diagnostics.Contracts.Contract.Requires(cap > 0);
            System.Diagnostics.Contracts.Contract.Requires(capHistory > 0);
            System.Diagnostics.Contracts.Contract.Requires(k > 0);
            Cap = (uint)cap;
            CapHistory = (uint)capHistory;
            K = k;
            mLinkedList_his = new LinkedList<Tuple<T, int>>();
            mLink = new LinkedList<T>();
        }

        public override void Clear()
        {
            mLinkedList_his.Clear();
            mLink.Clear();
        }

        private void PushHistory(T t)
        {
            while (mLinkedList_his.Count >= CapHistory)
            {
                mLinkedList_his.RemoveLast();
            }

            if (mLinkedList_his.Count >= CapHistory)
            {
                //TODO:删除末尾的节点
                mLinkedList_his.RemoveLast();
                PushHistory(t);
            }
            else
            {
                mLinkedList_his.AddFirst(new Tuple<T, int>(t, 0));
            }
        }

        public override bool Get(ref T t)
        {
            var result = false;
            t.Value = default(TV);
            LinkedListNode<T> rNode;
            if (LruFunction.SearchData<T, TK, TV>(mLink, t.Key, out rNode))
            {
                LruFunction.Sort(mLink, rNode);
                result = true;
            }
            else
            {
                LinkedListNode<Tuple<T, int>> rHisNode;
                if (LruFunction.SearchHistoryData<T, TK, TV>(mLinkedList_his, t.Key, out rHisNode))
                {
                    var k = rHisNode.Value.Item2 + 1;
                    if (k >= K)
                    {
                        mLinkedList_his.Remove(rHisNode);
                        Set(rHisNode.Value.Item1);
                    }
                    else
                    {
                        rHisNode.Value = new Tuple<T, int>(rHisNode.Value.Item1, k);
                        LruFunction.Sort(mLinkedList_his, rHisNode);
                    }
                    result = true;
                }
            }
            return result;
        }

        public override void Set(T t)
        {
            while (mLink.Count > Cap)
            {
                mLink.RemoveLast();
            }
            mLink.AddFirst(t);
        }
    }

    public class Two_Queues<T, TK, TV> : Cache<T, TK, TV>
        where T : IDataItem<TK, TV>
    {
        private Queue<T> mHisQueue { get; set; }
        private LinkedList<T> mLink { get; set; }
        private int Cap { get; set; }
        private int CapHistory { get; set; }

        public Two_Queues(int cap, int capHistory)
        {
            Cap = cap;
            CapHistory = capHistory;
            mHisQueue = new Queue<T>();
            mLink = new LinkedList<T>();
        }

        public override void Clear()
        {
            if (mHisQueue != null)
            {
                mHisQueue.Clear();
            }
            if (mLink != null)
            {
                mLink.Clear();
            }
        }

        public override bool Get(ref T t)
        {
            var result = false;
            LinkedListNode<T> rNode;
            T temp;
            if (LruFunction.SearchData<T, TK, TV>(mLink, t.Key, out rNode))
            {
                LruFunction.Sort(mLink, rNode);
                result = true;
            }
            else if (LruFunction.SearchQueue<T, TK, TV>(mHisQueue, t.Key, out temp))
            {
                mHisQueue.Enqueue(temp);
                t = temp;
                result = true;
            }
            return result;
        }

        public override void Set(T t)
        {
            T temp;
            if (LruFunction.SearchQueue<T, TK, TV>(mHisQueue, t.Key, out temp))
            {
                while (mLink.Count > Cap)
                {
                    mLink.RemoveLast();
                }
                mLink.AddFirst(t);
            }
            else
            {
                while (mHisQueue.Count > CapHistory)
                {
                    mHisQueue.Dequeue();
                }
                mHisQueue.Enqueue(t);
            }
        }
    }

    public class Multi_Queue<T, TK, TV> : Cache<T, TK, TV>
        where T : IDataItem<TK, TV>
    {
        private int Priority { get; set; }

        private int K { get; set; }

        public int Cap { get; set; }

        public int CapHistory { get; set; }

        public LinkedList<T>[] mListLinks { get; set; }

        public LinkedList<Tuple<T, int>> mLink_History { get; set; }

        public Multi_Queue(int k, int cap, int capHistory)
        {
            System.Diagnostics.Contracts.Contract.Requires(k > 0);
            System.Diagnostics.Contracts.Contract.Requires(cap > 0);
            System.Diagnostics.Contracts.Contract.Requires(capHistory > 0);
            K = k;
            Cap = cap;
            CapHistory = capHistory;
            mLink_History = new LinkedList<Tuple<T, int>>();
            mListLinks = new LinkedList<T>[K];
            for (int i = 0; i < K; i++)
            {
                mListLinks[i] = new LinkedList<T>();
            }
        }

        public override void Clear()
        {
            mLink_History.Clear();
            for (int i = 0; i < mListLinks.Length; i++)
            {
                if (mListLinks != null)
                {
                    mListLinks[i].Clear();
                }
            }
        }

        public override bool Get(ref T t)
        {
            var result = false;

            return result;
        }

        public override void Set(T t)
        {
        }
    }

    public class DataCache_LRU<TKey, TData>
        where TKey : IComparable
    {
        private Dictionary<TKey, TData> Datas { get; set; }

        private TKey[] keys { get; set; }

        public void Insert(TKey key, TData data)
        {
        }

        public TData Get(TKey key)
        {
            return default(TData);
        }
    }
}