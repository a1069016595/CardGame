using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 真六武众-辉斩
/// </summary>
public class C49721904 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        SpSummonEffect e1 = new SpSummonEffect();
        e1.SetCheckLauch(CheckLauch);
        e1.SetPutType(0);
        duel.ResignEffect(e1, card, player);

        StateEffect e2 = new StateEffect();
        e2.SetRangeArea(ComVal.Area_Monster);
        e2.SetCardEffectType(ComVal.cardEffectType_normalStateEffect | ComVal.cardEffectType_Single | ComVal.cardEffectType_unableReset);
        e2.SetCategory(ComVal.category_stateEffect);
        e2.SetTarget(card);
        e2.SetStateEffectType(ComVal.stateEffectType_addAfkVal);
        e2.SetGetStateEffectVal(GetAddAfkVal);
        duel.ResignEffect(e2, card, player);

        StateEffect e3 = (StateEffect)e2.Clone();
        e3.SetStateEffectType(ComVal.stateEffectType_addDefVal);
        duel.ResignEffect(e3, card, player);
    }

    private int GetAddAfkVal(IDuel duel, Card card, StateEffect e)
    {
        if (duel.GetIncludeNameCardNumFromArea(ComStr.KeyWord_SixSamurai, false,
           card.controller, ComVal.cardType_Monster, ComVal.Area_Monster, Fiter, true, card) >= 2)
        {
            return 300;
        }
        else
        {
            return 0;
        }
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {

    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return duel.GetIncludeNameCardNumFromArea(ComStr.KeyWord_SixSamurai, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Monster,
            Fiter, true, null, "49721904") > 0;
    }

    private bool Fiter(Card card)
    {
        return card.IsFaceUpInMonsterArea() ;
    }
}
