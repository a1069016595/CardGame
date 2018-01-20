using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 元素英雄 闪光侠
/// </summary>
public class C22061412 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        card.SetMaterialFilter2(Fiter1, Fiter2);

        StateEffect e1 = new StateEffect();
        e1.SetRangeArea(ComVal.Area_Monster);
        e1.SetCardEffectType(ComVal.cardEffectType_normalStateEffect | ComVal.cardEffectType_Single | ComVal.cardEffectType_unableReset);
        e1.SetCategory(ComVal.category_stateEffect);
        e1.SetTarget(card);
        e1.SetStateEffectType(ComVal.stateEffectType_addAfkVal);
        e1.SetGetStateEffectVal(GetAddAfkVal);
        duel.ResignEffect(e1, card, player);

        LauchEffect e2 = new LauchEffect();
        e2.SetCategory(ComVal.category_toHand);
        e2.SetCode(ComVal.code_ToGraveyard);
        e2.SetCardEffectType(ComVal.cardEffectType_mustToChooseLauch);
        e2.SetCheckLauch(CheckLauch);
        e2.SetOperation(Operation);
        duel.ResignEffect(e2, card, player);
    }

    private int GetAddAfkVal(IDuel duel, Card card, StateEffect e)
    {
        Group g = card.controller.group_Remove;
        return g.GetFitlerGroup(Fiter2).GroupNum * 300;
    }

    private bool Fiter2(Card card)
    {
        return card.ContainName(ComStr.KeyWord_ElementalHERO);
    }

    private bool Fiter1(Card card)
    {
        return card.GetCurAttribute().IsBind(ComVal.CardAttr_Light);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group g = card.controller.group_Remove;
        g = g.GetFitlerGroup(Fiter2);

        if (g.GroupNum == 0)
        {
            duel.FinishHandle();
            return;
        }
        GroupCardSelectBack callBack = delegate(Group val)
        {
            duel.AddFinishHandle();
            duel.AddCardToHandFromArea(ComVal.Area_Remove, val, card.controller, card, effect);
        };
        if (g.GroupNum < 2)
        {
            callBack(g);
            return;
        }
        duel.SelectCardFromGroup(g, callBack, 2, card.controller, false);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        if (!card.previousArea.IsBind(ComVal.Area_Field) || !code.group.ContainCard(card))
        {
            return false;
        }
        Group g = card.controller.group_Remove;
        g = g.GetFitlerGroup(Fiter2);
        return g.GroupNum > 0;
    }
}
