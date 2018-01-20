using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimitPlayerEffect : BaseEffect
{
    public int limitEffectType;

    public TargetPlayerType targetType;

    public LimitEffectHandler handler;

    public Condition condition;

    public LimitPlayerEffect()
    {
        SetType(EffectType.LimitPlayerEffect);
    }

    #region  设置

    public void SetLimitEffectType(int limitEffectType)
    {
        this.limitEffectType = limitEffectType;
    }

    public void SetLimitEffectHandler(LimitEffectHandler handler)
    {
        this.handler = handler;
    }

    public void SetTargetType(TargetPlayerType targetType)
    {
        this.targetType = targetType;
    }

    public void SetCondtion(Condition condition)
    {
        this.condition = condition;
    }

    #endregion


    public bool IsValid(Card targetCard)
    {
        if(condition!=null)
        {
            return condition(duel, ownerCard, this,targetCard) && !isDisable;
        }
        else
        {
            return !isDisable;
        }
    }

    public bool IsBindLimitEffectType(int val)
    {
        return this.limitEffectType==val;
    }

    public object LimitEffectHandle(Card c)
    {
        if (handler != null)
        {
            return handler(duel, c, this);
        }
        return true;
    }

    public List<Card> LimitEffectHandle(List<Card> list)
    {
        List<Card> result = new List<Card>();
        if (handler != null)
        {
            foreach (var item in list)
            {
                if ((bool)handler(duel, item, this))
                {
                    result.Add(item);
                }
            }
            return result;
        }
        else
        {
            return list;
        }

    }
}