using System;
using UnityEngine;

public class PriorityHeap<TPriority> where TPriority : IComparable<TPriority>
{
    private struct Item
    {
        public Vector2Int key;
        public TPriority priority;

        public Item(Vector2Int key, TPriority priority)
        {
            this.key = key;
            this.priority = priority;
        }
    }

    private Item[] heap;
    private int[,] indices; // -1 означает, что элемент отсутствует
    private int count;
    private int capacity;

    public PriorityHeap(BoundsInt bounds)
    {
        capacity = bounds.size.x * bounds.size.y + 1;
        heap = new Item[capacity];
        count = 0;

        indices = new int[bounds.size.x, bounds.size.y];
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                indices[x, y] = -1;
            }
        }
    }

    public bool IsEmpty()
    {
        return count == 0;
    }

    public void Clear()
    {
        count = 0;

        for (int x = 0; x < indices.GetLength(0); x++)
        {
            for (int y = 0; y < indices.GetLength(1); y++)
            {
                indices[x, y] = -1;
            }
        }
    }

    public int Count => count;

    public TPriority TopPriority()
    {
        return heap[1].priority;
    }

    public void Enqueue(Vector2Int key, TPriority priority)
    {
        count++;

        if (count == capacity)
        {
            capacity *= 2;
            Array.Resize(ref heap, capacity);
        }

        heap[count] = new Item(key, priority);
        indices[key.x, key.y] = count;

        RestoreHeapUpwards(count);
    }

    public Vector2Int Dequeue()
    {
        Vector2Int topKey = heap[1].key;
        indices[topKey.x, topKey.y] = -1;

        heap[1] = heap[count];
        indices[heap[1].key.x, heap[1].key.y] = 1;
        count--;

        RestoreHeapDownwards(1);

        return topKey;
    }

    public bool Update(Vector2Int key, TPriority newPriority)
    {
        int idx = indices[key.x, key.y];
        if (idx == -1)
        {
            return false;
        }

        int cmp = newPriority.CompareTo(heap[idx].priority);
        heap[idx].priority = newPriority;

        if (cmp < 0)
        {
            RestoreHeapDownwards(idx);
        }
        else if (cmp > 0)
        {
            RestoreHeapUpwards(idx);
        }
        return true;
    }

    public void Remove(Vector2Int key)
    {
        int idx = indices[key.x, key.y];
        if (idx == -1)
        {
            return;
        }

        if (idx == count)
        {
            count--;
            indices[key.x, key.y] = -1;
            return;
        }

        indices[key.x, key.y] = -1;

        int cmp = heap[count].priority.CompareTo(heap[idx].priority);

        heap[idx] = heap[count];
        indices[heap[idx].key.x, heap[idx].key.y] = idx;
        count--;

        if (cmp < 0)
        {
            RestoreHeapDownwards(idx);
        }
        else if (cmp > 0)
        {
            RestoreHeapUpwards(idx);
        }
    }

    private void RestoreHeapUpwards(int idx)
    {
        if (idx == 1)
            return;

        int parentIdx = idx / 2;
        if (heap[parentIdx].priority.CompareTo(heap[idx].priority) < 0)
        {
            Swap(idx, parentIdx);
            RestoreHeapUpwards(parentIdx);
        }
    }

    private void RestoreHeapDownwards(int idx)
    {
        int leftIdx = idx * 2;
        int rightIdx = leftIdx + 1;

        if (leftIdx > count)
        {
            return;
        }

        int largestIdx = leftIdx;
        if (rightIdx <= count && heap[rightIdx].priority.CompareTo(heap[leftIdx].priority) > 0)
        {
            largestIdx = rightIdx;
        }

        if (heap[largestIdx].priority.CompareTo(heap[idx].priority) > 0)
        {
            Swap(idx, largestIdx);
            RestoreHeapDownwards(largestIdx);
        }
    }

    private void Swap(int idx1, int idx2)
    {
        (heap[idx1], heap[idx2]) = (heap[idx2], heap[idx1]);
        indices[heap[idx1].key.x, heap[idx1].key.y] = idx1;
        indices[heap[idx2].key.x, heap[idx2].key.y] = idx2;
    }

    public override string ToString()
    {
        string result = $"Count: {count}  ";
        for (int i = 1; i <= count; i++)
        {
            result += $"[{heap[i].key} | {heap[i].priority}], ";
        }

        return result;
    }
}