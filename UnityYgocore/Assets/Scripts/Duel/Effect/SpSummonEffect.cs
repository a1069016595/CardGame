using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpSummonEffect : BaseEffect
{
    private CheckLaunch checkLauch;
    private Cost cost;
    private int putType;

    public SpSummonEffect()
    {
        SetType(EffectType.SpsummonEffect);
    }

    public void SetCost(Cost dele)
    {
        cost = dele;
    }

    public void SetCheckLauch(CheckLaunch dele)
    {
        checkLauch = dele;
    }

    public void SetPutType(int putType)
    {
        this.putType = putType;
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        if (!code.code.IsBind(ComVal.code_NoCode))
        {
            Debug.Log("error");
        }
        return checkLauch(duel, card, effect, code);
    }

    public void Cost(IDuel duel, Card c, LauchEffect e)
    {
        if (cost == null)
        {
            duel.FinishHandle();
            return;
        }
        cost(duel, c, e);
    }

    public int PutType
    {
        get { return putType; }
    }
}
