using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public delegate bool Condition(IDuel duel, Card card, BaseEffect e,Card targetCard);
public delegate void Cost(IDuel duel, Card card, LauchEffect effect);
public delegate void Target(IDuel duel, Card card, LauchEffect effect, GroupCardSelectBack dele);
public delegate void Operation(IDuel duel, Card card, LauchEffect effect, Group target = null);
public delegate bool CheckLaunch(IDuel duel, Card card, LauchEffect effect, Code code);
public delegate int GetStateEffectVal(IDuel duel, Card card, StateEffect e);
public delegate object LimitEffectHandler(IDuel duel,Card c,LimitPlayerEffect e);

public enum EffectType
{
    AreaEffect,
    LimitPlayerEffect,
    LauchEffect,
    StateEffect,
    SpsummonEffect,
    LinkEffect,
    EquipEffect,
}

public enum TargetPlayerType
{
    my,
    other,
    both,
}


public class BaseEffect
{
    public EffectType type;
    public string describe;
    public Card ownerCard;
    public int resetCode;
    public int startRoundCount;
    public int resetRoundCount;
    public bool isDisable;

    protected Duel duel;

    public int cardEffectType;//效果特性

    public int category;

    #region Set

    protected void SetType(EffectType type)
    {
        this.type = type;
    }

    public void SetDescribe(string describe)
    {
        this.describe = describe;
    }

    public void SetDisable()
    {
        isDisable = true;
    }

    public void SetResetCode(int resetCode, int roundCount)
    {
        this.resetCode = resetCode;
        this.resetRoundCount = roundCount;
    }

    /// <summary>
    /// 设置效果特性 如触发效果 普通效果
    /// </summary>
    /// <param name="cardEffectType"></param>
    public virtual void SetCardEffectType(int cardEffectType)
    {
        this.cardEffectType = cardEffectType;
    }

    public void SetCategory(int category)
    {
        this.category = category;
    }

    #endregion

    public bool IsBindCardEffectType(int cardEffectType)
    {
        return this.cardEffectType.IsBind(cardEffectType);
    }

    public void Init(Card ownerCard)
    {
        duel = Duel.GetInstance();
        this.ownerCard = ownerCard;
        isDisable = false;
        this.startRoundCount = duel.roundCount;
    }

    public bool IsBindType(EffectType type)
    {
        return this.type == type;
    }

    public void CheckReset(int eventCode)
    {
        if (ComVal.isBind(ComVal.resetEvent_Phase, eventCode))
        {
            if (resetCode <= eventCode)
            {
                int val = startRoundCount + resetRoundCount;
                if (val == duel.roundCount)
                {
                    isDisable = true;
                }
                //else if (duel.roundCount > val)
                //{
                //    Debug.Log(ownerCard.cardID);
                //    Debug.Log("error");
                //}
            }
        }
        else
        {
            if (resetCode == eventCode)
            {
                int val = startRoundCount + resetRoundCount;
                if (val == duel.roundCount)
                {
                    isDisable = true;
                }
                //else if (val > duel.roundCount)
                //{
                //    Debug.Log("error");
                //}
            }
        }
    }
}