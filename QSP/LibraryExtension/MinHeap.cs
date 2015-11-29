using System.Collections.Generic;

namespace QSP.LibraryExtension
{
    public class MinHeap<T>
    {
        //TODO: This class is not used in the project (yet). It is not fully tested and may contain bugs.
        private List<T> content;
        private IComparer<T> customComparer;
        
        public MinHeap()
        {
            content = new List<T>();
            customComparer = Comparer<T>.Default;
        }

        public MinHeap(List<T> item, IComparer<T> comparer)
        {
            content = new List<T>(item);
            this.customComparer = comparer;
            Heapify();
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

            T val = content[index];
            T left = content[leftChildIndex];
            T right = content[rightChildIndex];

            if (customComparer.Compare (val, left) > 0)
            {
                minIndex = leftChildIndex;
            }

            T min = content[minIndex];

            if ((rightChildIndex < length) && customComparer.Compare (min, right) > 0)
            {
                minIndex = rightChildIndex;
            }

            if (minIndex != index)
            {
                T temp = content[index];
                content[index] = content[minIndex];
                content[minIndex] = temp;
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

            T parent = content[parentIndex];
            T val = content[index];

            if (customComparer.Compare (parent, val) > 0)
            {
                T temp = content[parentIndex];
                content[parentIndex] = content[index];
                content[index] = temp;
                BubbleUp(parentIndex);
            }
        }

        private void Heapify()
        {
            int length = content.Count;
            for (int i = (length / 2) - 1; i >= 0; i--)
            {
                BubbleDown(i);
            }
        }

        public void Insert(T newValue)
        {
            int length = content.Count;
            content.Add(newValue);
            BubbleUp(length);
        }

        public T GetMin()
        {
            return content[0];
        }

        public void DeleteMin()
        {
            int length = content.Count;
            if (length == 0)
            {
                return;
            }

            content[0] = content[length - 1];
            content.RemoveAt(length - 1);
            BubbleDown(0);
        }


    }
}
