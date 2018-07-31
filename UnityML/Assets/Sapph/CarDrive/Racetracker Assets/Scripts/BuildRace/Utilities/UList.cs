using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UList<T> : List<T>
{
    public new void Add(T item)
    {
        if (Contains(item) == false)
        {
            base.Add(item);
        }
    }
}
