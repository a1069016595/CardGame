using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    float interval;
    float timer;

    public Timer(float val)
    {
        interval = val;
    }

    public bool Update()
    {
        timer += Time.deltaTime;
        if (timer > interval)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Reset()
    {
        timer = 0;
    }
}
