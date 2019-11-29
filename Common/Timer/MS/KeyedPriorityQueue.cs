namespace MS
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Runtime.InteropServices;

    /// <summary>
    /// 对timer对象的管理。·
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <typeparam name="P"></typeparam>
    [Serializable]
    public class KeyedPriorityQueue<K, V, P> where V : class
    {
        private List<HeapNode<K, V, P>> heap;
        private HeapNode<K, V, P> placeHolder;
        private Comparer<P> priorityComparer;
        private int size;//堆大小

        public event EventHandler<KeyedPriorityQueueHeadChangedEventArgs<V>> FirstElementChanged;

        public KeyedPriorityQueue()
        {
            this.heap = new List<HeapNode<K, V, P>>();
            this.priorityComparer = Comparer<P>.Default;
            this.placeHolder = new HeapNode<K, V, P>();
            this.heap.Add(new HeapNode<K, V, P>());
        }

        /// <summary>
        /// 清除重置
        /// </summary>
        public void Clear()
        {
            this.heap.Clear();
            this.size = 0;
        }

        /// <summary>
        /// 出队
        /// </summary>
        /// <returns></returns>
        public V Dequeue()
        {
            V local = (this.size < 1) ? default(V) : this.DequeueImpl();
            V newHead = (this.size < 1) ? default(V) : this.heap[1].Value;
            this.RaiseHeadChangedEvent(default(V), newHead);
            return local;
        }

        /// <summary>
        /// 出队时，后面先级最大值调制第一位子
        /// </summary>
        /// <returns></returns>
        private V DequeueImpl()
        {
            V local = this.heap[1].Value;
            this.heap[1] = this.heap[this.size];
            this.heap[this.size--] = this.placeHolder;//容量减小。
            this.Heapify(1);
            return local;
        }

        /// <summary>
        /// 入队排定优先级
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="priority"></param>
        public void Enqueue(K key, V value, P priority)
        {
            V local = (this.size > 0) ? this.heap[1].Value : default(V);
            int num = ++this.size;//1，2，3
            int num2 = num / 2;//0，1
            if (num == this.heap.Count)
            {
                this.heap.Add(this.placeHolder);
            }
            while ((num > 1) && this.IsHigher(priority, this.heap[num2].Priority))//排定优先级，由低到高。
            {
                this.heap[num] = this.heap[num2];
                num = num2;
                num2 = num / 2;
            }
            this.heap[num] = new HeapNode<K, V, P>(key, value, priority);//1，
            V newHead = this.heap[1].Value;
            if (!newHead.Equals(local))
            {
                this.RaiseHeadChangedEvent(local, newHead);//重新改变优先事件
            }
        }

        public V FindByPriority(P priority, Predicate<V> match)
        {
            if (this.size >= 1)
            {
                return this.Search(priority, 1, match);
            }
            return default(V);
        }

        /// <summary>
        /// 优先级排序
        /// </summary>
        /// <param name="i"></param>
        private void Heapify(int i)
        {
            int num = 2 * i;//2
            int num2 = num + 1;//3
            int j = i;//1
            if ((num <= this.size) && this.IsHigher(this.heap[num].Priority, this.heap[i].Priority))
            {
                j = num;//2
            }
            if ((num2 <= this.size) && this.IsHigher(this.heap[num2].Priority, this.heap[j].Priority))
            {
                j = num2;
            }
            if (j != i)
            {
                this.Swap(i, j);
                this.Heapify(j);
            }
        }

        protected virtual bool IsHigher(P p1, P p2)//小于-比较优先级大小
        {
            return (this.priorityComparer.Compare(p1, p2) < 1);
        }

        /// <summary>
        /// 取出优先级最大的元素
        /// </summary>
        /// <returns></returns>
        public V Peek()//每次返回第一个
        {
            if (this.size >= 1)
            {
                return this.heap[1].Value;
            }
            return default(V);
        }

        private void RaiseHeadChangedEvent(V oldHead, V newHead)
        {
            if (oldHead != newHead)
            {
                EventHandler<KeyedPriorityQueueHeadChangedEventArgs<V>> firstElementChanged = this.FirstElementChanged;
                if (firstElementChanged != null)
                {
                    firstElementChanged(this, new KeyedPriorityQueueHeadChangedEventArgs<V>(oldHead, newHead));
                }
            }
        }

        /// <summary>
        /// 移除对应Key值的元素。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public V Remove(K key)
        {
            if (this.size >= 1)
            {
                V oldHead = this.heap[1].Value;
                for (int i = 1; i <= this.size; i++)
                {
                    if (this.heap[i].Key.Equals(key))
                    {
                        V local2 = this.heap[i].Value;
                        this.Swap(i, this.size);
                        this.heap[this.size--] = this.placeHolder;
                        this.Heapify(i);
                        V local3 = this.heap[1].Value;
                        if (!oldHead.Equals(local3))
                        {
                            this.RaiseHeadChangedEvent(oldHead, local3);
                        }
                        return local2;
                    }
                }
            }
            return default(V);
            //List<string> strList = new List<string>();
            //strList.Find((string a) =>
            //{
            //    if (a == "")
            //    {
            //        return true;
            //    }
            //    return false;
            //});
        }

        /// <summary>
        /// 查找对应匹配的优先级元素
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="i"></param>
        /// <param name="match"></param>
        /// <returns></returns>
        private V Search(P priority, int i, Predicate<V> match)
        {
            V local = default(V);
            if (this.IsHigher(this.heap[i].Priority, priority))
            {
                if (match(this.heap[i].Value))
                {
                    local = this.heap[i].Value;
                }
                int num = 2 * i;
                int num2 = num + 1;
                if ((local == null) && (num <= this.size))
                {
                    local = this.Search(priority, num, match);
                }
                if ((local == null) && (num2 <= this.size))
                {
                    local = this.Search(priority, num2, match);
                }
            }
            return local;
        }

        /// <summary>
        /// 元素交换
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private void Swap(int i, int j)
        {
            HeapNode<K, V, P> node = this.heap[i];
            this.heap[i] = this.heap[j];
            this.heap[j] = node;
        }

        /// <summary>
        /// 容量
        /// </summary>
        public int Count
        {
            get
            {
                return this.size;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyCollection<K> Keys
        {
            get
            {
                List<K> list = new List<K>();
                for (int i = 1; i <= this.size; i++)
                {
                    list.Add(this.heap[i].Key);
                }
                return new ReadOnlyCollection<K>(list);
            }
        }

        public ReadOnlyCollection<V> Values
        {
            get
            {
                List<V> list = new List<V>();
                for (int i = 1; i <= this.size; i++)
                {
                    list.Add(this.heap[i].Value);
                }
                return new ReadOnlyCollection<V>(list);
            }
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        private struct HeapNode<KK, VV, PP>
        {
            public KK Key;
            public VV Value;
            public PP Priority;
            public HeapNode(KK key, VV value, PP priority)
            {
                this.Key = key;
                this.Value = value;
                this.Priority = priority;
            }
        }

    }
}
