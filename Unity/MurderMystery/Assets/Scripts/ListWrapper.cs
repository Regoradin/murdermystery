using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ListWrapper<T>
{
    [SerializeField]
    public List<T> InnerList;
    
    public T this[int key]
    {
        get
        {
            if (InnerList == null)
            {
                InnerList = new List<T>();
            }
            return InnerList[key];
        }
        set
        {
            InnerList[key] = value;
        }
    }

    public int Count
    {
        get
        {
            if (InnerList == null)
            {
                InnerList = new List<T>();
            }
            return InnerList.Count;
        }
    }
}
