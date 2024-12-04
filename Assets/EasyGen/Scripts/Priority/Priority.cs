using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priority
{
    public int PriorityLevel { get; private set; } = 0;
    public SparseSet PrioritySet { get; private set; } = null;

    public Priority(int priorityLevel = 0, SparseSet prioritySet = null)
    {
        PriorityLevel = priorityLevel;
        PrioritySet = prioritySet;
    }

    public void SetPriority(int priorityLevel, SparseSet prioritySet)
    {
        if (PriorityLevel != priorityLevel || PrioritySet != prioritySet)
        {
            PriorityLevel = priorityLevel;
            PrioritySet = prioritySet;
        }
    }
}
