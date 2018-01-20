using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 裁决之龙
/// </summary>
public class C57774843 : ICardScripts
{
    public void InitialEffect(Card card, Player player, IDuel duel)
    {


        SpSummonEffect e1 = new SpSummonEffect();
        e1.SetPutType(0);
        e1.SetCheckLauch(CheckLauch);
        duel.ResignEffect(e1, card, player);

        LauchEffect e2 = new LauchEffect();
        e2.SetCategory(ComVal.category_destroy);
        e2.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e2.SetLauchArea(ComVal.Area_Monster);
        e2.SetCode(ComVal.code_NoCode);
        e2.SetCost(Cost);
        e2.SetOperation(Operation);
        e2.SetCheckLauch(CheckLauch2);
        duel.ResignEffect(e2, card, player);

        StateEffect e3 = new StateEffect();
        e3.SetTarget(card);
        e3.SetCardEffectType(ComVal.cardEffectType_Single | ComVal.cardEffectType_normalStateEffect | ComVal.cardEffectType_unableReset);
        e3.SetStateEffectType(ComVal.stateEffectType_unableNormalSummon);
        duel.ResignEffect(e3, card, player);

        LauchEffect e4 = new LauchEffect();
        e4.SetCategory(ComVal.category_disCard);
        e4.SetCheckLauch(CheckLauch1);
        e4.SetOperation(Operation1);
        e4.SetCode(ComVal.code_EnterEndPhase);
        e4.SetCardEffectType(ComVal.cardEffectType_mustLauch);
        e4.SetLauchArea(ComVal.Area_Monster);
        duel.ResignEffect(e4, card, player);
    }

    private bool CheckLauch2(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        Group g = duel.GetIncludeNameCardFromArea("", true, null, 0, ComVal.Area_Field, null, true, card);
        return card.controller.LP > 1000 && g.GroupNum > 0;
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group target = null)
    {
        Group g = duel.GetIncludeNameCardFromArea("", true, null, 0, ComVal.Area_Field, null, true, card);
        duel.AddFinishHandle();
        duel.SendToGraveyard(ComVal.Area_Field, g, card, ComVal.reason_EffectDestroy, effect);
    }

    private void Cost(IDuel duel, Card card, LauchEffect effect)
    {
        duel.ReduceLP(1000, card.controller, ComVal.reason_Cost, card, effect);
        duel.FinishHandle();
    }



    public void Operation1(IDuel duel, Card card, LauchEffect effect, Group target = null)
    {
        duel.AddFinishHandle();
        duel.DiscardFromDeck(4, card, effect, card.controller);
    }

    public bool CheckLauch1(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        if (card.controller.group_MainDeck.GroupNum > 0 && duel.IsPlayerRound(card.controller))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_Lightsworn, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Graveyard);
        Debug.Log(g.GetTypeNum());
        return g.GetTypeNum() >= 4;
    }
}
