using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于一个回合发动一次的效果计数
/// </summary>
public class EffectLauchLimitCounter
{
    List<EffectLauchCounter> counterList;

    public EffectLauchLimitCounter()
    {
        counterList = new List<EffectLauchCounter>();
    }

    public void AddCounter(string id, int maxLauchTime)
    {
        counterList.Add(new EffectLauchCounter(id, maxLauchTime));
    }

    public void ResetCounter()
    {
        foreach (var item in counterList)
        {
            item.ResetCounter();
        }
    }

    public void AddLauchTime(string effectID)
    {
        if(effectID==null)
        {
            return;
        }
        foreach (var item in counterList)
        {
            if (item.IsBindID(effectID))
            {
                item.AddLauchTime();
            }
        }
    }

    public bool CheckCanLauch(string effectID)
    {
        foreach (var item in counterList)
        {
            if (item.IsBindID(effectID))
            {
                return item.CanLauch();
            }
        }
        return true;
    }
}

public class EffectLauchCounter
{
    string effectId;
    int lauchTime;
    int maxLauchTime;

    public EffectLauchCounter(string id, int maxLauchTime)
    {
        effectId = id;
        this.maxLauchTime = maxLauchTime;
    }

    public void AddLauchTime()
    {
        lauchTime++;
    }

    public bool CanLauch()
    {
        return lauchTime < maxLauchTime;
    }

    public void ResetCounter()
    {
        lauchTime=0;
    }

    public bool IsBindID(string id)
    {
        return effectId==id;
    }
}