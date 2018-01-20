using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StateEffect : BaseEffect,ICloneable
{
    public int stateEffectType;
    public int rangeArea;
    public int stateEffectVal;

    public GetStateEffectVal getStateEffectVal;
    public Condition condition;//状态效果条件

    public Card targetCard;
    public Group targetGroup;

    public Card equipCard;
    
    public StateEffect()
    {
        SetType(EffectType.StateEffect);
    }

    #region 设置

    public void SetStateEffectType(int stateEffectType)
    {
        this.stateEffectType = stateEffectType;
    }

    public void SetStateEffectVal(int stateEffectVal)
    {
        this.stateEffectVal = stateEffectVal;
    }

    public void SetGetStateEffectVal(GetStateEffectVal getStateEffectVal)
    {
        this.getStateEffectVal = getStateEffectVal;
    }

    public void SetResetCode(int resetCode)
    {
        this.resetCode = resetCode;
    }

    /// <summary>
    /// 生效区域 装备效果为装备卡所在的区域
    /// </summary>
    /// <param name="rangeArea"></param>
    public void SetRangeArea(int rangeArea)
    {
        this.rangeArea = rangeArea;
    }

    public void SetCondtion(Condition condition)
    {
        this.condition = condition;
    }

    public void SetTarget(Card c)
    {
        this.targetCard = c;
    }

    public void SetTarget(Group g)
    {
        this.targetGroup = g;
    }

    public void SetEquipCard(Card target,Card equipCard)
    {
        this.targetCard = target;
        this.equipCard = equipCard;
    }
    #endregion

    #region 获取

    public int GetStateEffectVal(Card c)
    {
        if(getStateEffectVal!=null)
        {
            return getStateEffectVal(duel, c, this);
        }
        return stateEffectVal;
    }

    public bool IsValid(Card c)
    {
        if(condition!=null)
        {
            return condition(duel, c, this,c) && !isDisable;
        }
        else
        {
            return !isDisable;
        }
    }

    public bool IsBindStateEffectType(int val)
    {
        return stateEffectType==val;
    }

    #endregion

    #region 操作

    public object Clone()
    {
        return MemberwiseClone();
    }
    #endregion

}
