using System.Collections.Generic;
using System;
using UnityEngine;

public class IndexedSet
{
    private int capacity;
    private int count;
    private int[] valuesArr; // Массив для значений
    private int[] indexArr; // Массив индексов

    public IndexedSet(int capacity, bool full)
    {
        this.capacity = capacity;
        valuesArr = new int[capacity];
        indexArr = new int[capacity];

        if (full)
        {
            count = capacity;
            for (int i = 0; i < capacity; i++)
            {
                valuesArr[i] = i;
                indexArr[i] = i;
            }
        }
        else
        {
            count = 0;
        }
    }

    public int Count => count;

    public IndexedSet Clone()
    {
        IndexedSet clone = new IndexedSet(capacity, false)
        {
            count = count
        };
        Array.Copy(valuesArr, 0, clone.valuesArr, 0, count);
        Array.Copy(indexArr, 0, clone.indexArr, 0, capacity);
        return clone;
    }

    public void Clear()
    {
        count = 0;
    }

    public void Intersect(IndexedSet other)
    {
        int i = 0;
        while (i < count)
        {
            int value = valuesArr[i];
            if (!other.Contains(value))
            {
                RemoveAt(i);
            }
            else
            {
                i++;
            }
        }
    }

    public int OverlapCount(IndexedSet other)
    {
        IndexedSet smaller = count < other.count ? this : other;
        IndexedSet larger = count < other.count ? other : this;

        int overlapCount = 0;
        for (int i = 0; i < smaller.count; i++)
        {
            if (larger.Contains(smaller.valuesArr[i]))
            {
                overlapCount++;
            }
        }
        return overlapCount;
    }

    public void Add(int value)
    {
        if (!Contains(value))
        {
            valuesArr[count] = value;
            indexArr[value] = count;
            count++;
        }
    }

    public void Remove(int value)
    {
        if (Contains(value))
        {
            int index = indexArr[value];
            int lastValue = valuesArr[--count];
            valuesArr[index] = lastValue;
            indexArr[lastValue] = index;
        }
    }

    public bool Contains(int value)
    {
        return value < capacity && indexArr[value] < count && valuesArr[indexArr[value]] == value;
    }

    public int GetDense(int idx)
    {
        if (idx >= count || idx < 0) throw new IndexOutOfRangeException();
        return valuesArr[idx];
    }

    public void RemoveAt(int idx)
    {
        if (idx >= count || idx < 0) throw new IndexOutOfRangeException();

        int lastValue = valuesArr[--count];
        valuesArr[idx] = lastValue;
        indexArr[lastValue] = idx;
    }

    public IEnumerator<int> GetEnumerator()
    {
        for (int i = 0; i < count; i++)
        {
            yield return valuesArr[i];
        }
    }

    public int[] ToArray()
    {
        int[] result = new int[count];
        Array.Copy(valuesArr, 0, result, 0, count);
        return result;
    }
}