using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LauchEffect:BaseEffect
{
    public int lauchArea;
    public string effectID;//用于限制一回合一次的效果
    public Int64 code;
    public int lauchPhase;

    public Cost cost;
    public Target getTarget;
    public Operation operation;
    public CheckLaunch checkLauch;


    Group targetGroup;

    public LauchEffect ()
    {
        SetType(EffectType.LauchEffect);
    }

    #region 设置

    public override void SetCardEffectType(int cardEffectType)
    {
        base.SetCardEffectType(cardEffectType);
        if (cardEffectType == ComVal.cardEffectType_normalLauch)
        {
            SetCode(ComVal.code_NoCode);
            SetLauchPhase(ComVal.Phase_Mainphase);
        }
    }

    public void SetCost(Cost cost)
    {
        this.cost = cost;
    }

    public void SetGetTarget(Target getTarget)
    {
        this.getTarget = getTarget;
    }

    public void SetOperation(Operation operation)
    {
        this.operation = operation;
    }

    public void SetCheckLauch(CheckLaunch checkLauch)
    {
        this.checkLauch = checkLauch;
    }

    public void SetEffectID(string effectID)
    {
        this.effectID = effectID;
    }

    public void SetLauchPhase(int phase)
    {
        this.lauchPhase = phase;
    }

    public void SetCode(Int64 code)
    {
        this.code = code;
    }

    public void SetLauchArea(int area)
    {
        this.lauchArea = area;
    }
    #endregion

    #region  

    public bool IsTrigger()
    {
        return cardEffectType.IsBind(ComVal.cardEffectType_triggerEffect);
    }

    #endregion

    #region 操作

    /// <summary>
    /// 支付代价 增加发动次数
    /// </summary>
    /// <param name="dele"></param>
    public void Cost()
    {
        ownerCard.AddCardEffectLauchTime(this);
        if (cost != null)
        {
            cost(duel, ownerCard, this);
        }
        else
        {
            duel.FinishHandle();
        }
    }

    public void GetTarget(normalDele theDele)
    {
        if (getTarget == null)
        {
            return;
        }
        GroupCardSelectBack dele = delegate(Group group)
        {
            targetGroup = group;
            if (theDele != null)
                theDele();
        };
        getTarget(duel, ownerCard, this, dele);
    }


    /// <summary>
    /// 操作
    /// </summary>
    public void Operate()
    {
        operation(duel, ownerCard, this, targetGroup);
    }

    /// <summary>
    /// 是否可以发动效果
    /// </summary>
    /// <param name="_code"></param>
    /// <returns></returns>
    public bool CanBeLaunch(Code _code)
    {
        if (!ownerCard.CanLauchEffect(this) || !ownerCard.controller.CanLauchEffect(this))
        {
            return false;
        }
        if (ownerCard.IsMonster() && ownerCard.IsInField())
        {
            if (!ownerCard.IsFaceUp())
            {
                return false;
            }
        }
        if (lauchArea != 0)
        {
            if (!ComVal.isBind(ownerCard.curArea, lauchArea))
            {
                return false;
            }
        }
        if (!ComVal.isBind(_code.code, code))
        {
            return false;
        }
        if (lauchPhase != 0)
        {
            if (!ComVal.isBind(duel.currentPhase, lauchPhase))
            {
                return false;
            }
        }
        return checkLauch(duel, ownerCard, this, _code);
    }

    #endregion
}
