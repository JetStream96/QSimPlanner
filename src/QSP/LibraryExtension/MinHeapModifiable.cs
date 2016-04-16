using System.Collections.Generic;

namespace QSP.LibraryExtension
{
    public class MinHeap<TKey, TValue>
    {
        //TKey: Need to be unique for each entry.
        //TValue: Used to sort the heap, can be updated

        private List<KeyValuePair<TKey, TValue>> content;
        private Dictionary<TKey, int> indexInList;

        private Comparer<TValue> valueComp;

        public MinHeap() : this(Comparer<TValue>.Default)
        {
        }

        public MinHeap(Comparer<TValue> item)
        {
            content = new List<KeyValuePair<TKey, TValue>>();
            indexInList = new Dictionary<TKey, int>();
            valueComp = Comparer<TValue>.Default;
        }

        private void BubbleDown(int index)
        {
            int length = content.Count;
            int leftChildIndex = 2 * index + 1;
            int rightChildIndex = 2 * index + 2;

            if (leftChildIndex >= length)
            {
                return;
            }

            int minIndex = index;

            if (valueComp.Compare(content[index].Value, content[leftChildIndex].Value) > 0)
            {
                minIndex = leftChildIndex;
            }

            if ((rightChildIndex < length) && 
                valueComp.Compare(content[minIndex].Value, content[rightChildIndex].Value) > 0)
            {
                minIndex = rightChildIndex;
            }

            if (minIndex != index)
            {
                swapNodes(index, minIndex);
                BubbleDown(minIndex);
            }
        }

        private void BubbleUp(int index)
        {
            if (index == 0)
            {
                return;
            }
            int parentIndex = (index - 1) / 2;

            if (valueComp.Compare(content[parentIndex].Value, content[index].Value) > 0)
            {
                swapNodes(parentIndex, index);
                BubbleUp(parentIndex);
            }
        }

        private void Heapify()
        {
            int length = content.Count;
            for (int i = length / 2 - 1; i >= 0; i--)
            {
                BubbleDown(i);
            }
        }

        public void Insert(TKey key, TValue value)
        {
            int len = content.Count;
            content.Add(new KeyValuePair<TKey, TValue>(key, value));
            indexInList.Add(key, len);
            BubbleUp(len);
        }

        public KeyValuePair<TKey, TValue> GetMin()
        {
            return content[0];
        }

        private void swapNodes(int index1, int index2)
        {
            indexInList[content[index1].Key] = index2;
            indexInList[content[index2].Key] = index1;

            var temp = content[index1];
            content[index1] = content[index2];
            content[index2] = temp;
        }

        public void DeleteMin()
        {
            int length = content.Count;
            if (length < 2)
            {
                if (length == 0)
                {
                    return;
                }
                else  // length ==1
                {
                    indexInList.Remove(content[0].Key);
                    content.RemoveAt(0);
                    return;
                }
            }
            else
            {
                indexInList.Remove(content[0].Key);
                content[0] = content[length - 1];
                indexInList[content[0].Key] = 0;

                content.RemoveAt(length - 1);
                BubbleDown(0);
            }
        }

        public KeyValuePair<TKey, TValue> PopMin()
        {
            var x = GetMin();
            DeleteMin();
            return x;
        }

        public KeyValuePair<TKey, TValue> GetElement(TKey key)
        {
            return content[indexInList[key]];
        }

        public int Count
        {
            get
            {
                return content.Count;
            }
        }

        public bool ItemExists(TKey key)
        {
            return indexInList.ContainsKey(key);
        }

        public void ReplaceValue(TKey key, TValue newValue)
        {
            int i = indexInList[key];
            content[i] = new KeyValuePair<TKey, TValue>(content[i].Key, newValue);
            BubbleUp(i);
            BubbleDown(i);
        }

        public int IsOrdered()
        {
            int index = 0;
            int length = content.Count;
            int leftChildIndex = 0;
            int rightChildIndex = 0;

            for (index = 0; index <= length - 1; index++)
            {
                leftChildIndex = 2 * index + 1;
                rightChildIndex = 2 * index + 2;

                if (leftChildIndex < length && 
                    valueComp.Compare(content[leftChildIndex].Value, content[index].Value) < 0)
                {
                    return leftChildIndex;
                }

                if (rightChildIndex < length && 
                    valueComp.Compare(content[rightChildIndex].Value, content[index].Value) < 0)
                {
                    return rightChildIndex;
                }
            }
            return -1;
        }
    }
}
