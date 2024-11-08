using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T> where T : IComparable<T>
{
    List<T> heap = new List<T>();

    public int Count { get { return heap.Count; } }

    public void Push(T data)
    {
        heap.Add(data);

        int now = heap.Count - 1; // starting from last index

        while (now > 0)
        {
            int next = (now - 1) / 2; // index of parent node
            if (heap[now].CompareTo(heap[next]) < 0)
                break;

            T temp = heap[now];
            heap[now] = heap[next];
            heap[next] = temp;

            now = next;
        }
    }

    public T Pop()
    {
        T root = heap[0];

        int lastIndex = heap.Count - 1;
        heap[0] = heap[lastIndex];
        heap.RemoveAt(lastIndex);
        lastIndex--;

        int now = 0; // starting from the root
        while (true)
        {
            int leftChild = now * 2 + 1;
            int rightChild = now * 2 + 2;

            int next = now;

            if (leftChild <= lastIndex && heap[next].CompareTo(heap[leftChild]) < 0)
                next = leftChild;

            if (rightChild <= lastIndex && heap[next].CompareTo(heap[rightChild]) < 0)
                next = rightChild;

            if (next == now)
                break;

            T temp = heap[now];
            heap[now] = heap[next];
            heap[next] = temp;

            now = next;
        }

        return root;
    }
}
