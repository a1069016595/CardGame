
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DeleQueue
{
    public bool IsFree=true;

    Queue<object> mQueue = new Queue<object>();

    public DeleQueue()
    {

    }

    public void Add(normalDele obj)
    {
        mQueue.Enqueue(obj);
    }

    public void Update()
    {
        if(IsFree)
        {
            normalDele dele = (normalDele)mQueue.Dequeue();
            IsFree = false;
            dele();
        }
       
    }

    public void SetFree()
    {
        IsFree = true;
    }
}