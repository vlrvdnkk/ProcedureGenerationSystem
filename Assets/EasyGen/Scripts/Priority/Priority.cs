using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priority
{
    public int PriorityLevel { get; private set; } = 0;
    public IndexedSet PrioritySet { get; private set; } = null;

    public Priority(int priorityLevel = 0, IndexedSet prioritySet = null)
    {
        PriorityLevel = priorityLevel;
        PrioritySet = prioritySet;
    }

    public void SetPriority(int priorityLevel, IndexedSet prioritySet)
    {
        if (PriorityLevel != priorityLevel || PrioritySet != prioritySet)
        {
            PriorityLevel = priorityLevel;
            PrioritySet = prioritySet;
        }
    }
}
