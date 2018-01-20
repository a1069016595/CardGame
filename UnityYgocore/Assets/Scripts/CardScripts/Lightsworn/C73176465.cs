using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 光道弓手 费莉丝
/// </summary>
public class C73176465 : ICardScripts
{
    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_spSummon);
        e1.SetCode(ComVal.code_ToGraveyard);
        e1.SetCardEffectType(ComVal.cardEffectType_mustLauch);
        e1.SetCheckLauch(CheckLauch);
        e1.SetOperation(Operation);
        e1.SetLauchArea(ComVal.Area_Graveyard);
        duel.ResignEffect(e1, card, player);

        StateEffect e2 = new StateEffect();
        e2.SetTarget(card);
        e2.SetCardEffectType(ComVal.cardEffectType_Single | ComVal.cardEffectType_normalStateEffect | ComVal.cardEffectType_unableReset);
        e2.SetStateEffectType(ComVal.stateEffectType_unableNormalSummon);
        duel.ResignEffect(e2, card, player);

        LauchEffect e3 = new LauchEffect();
        e3.SetCategory(ComVal.category_destroy | ComVal.category_disCard);
        e3.SetCode(ComVal.code_NoCode);
        e3.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e3.SetLauchArea(ComVal.Area_Monster);
        e3.SetCost(Cost);
        e3.SetCheckLauch(CheckLauch1);
        e3.SetOperation(Operation1);
        duel.ResignEffect(e3, card, player);
    }

    private void Operation1(IDuel duel, Card card, LauchEffect effect, Group target = null)
    {
        normalDele d=delegate
        {
            duel.AddFinishHandle();
            duel.DiscardFromDeck(3,card,effect,card.controller);
        };

        GroupCardSelectBack callBack = delegate(Group val)
        {
            duel.AddDelegate(d, true);
            duel.SendToGraveyard(ComVal.Area_Monster, val, card, ComVal.reason_EffectDestroy, effect);
        };

        Group g = duel.GetIncludeNameCardFromArea("", false, duel.GetOpsitePlayer(card.controller), ComVal.cardType_Monster, ComVal.Area_Monster);
        duel.SelectCardFromGroup(g, callBack, 1, card.controller);
    }

    private bool CheckLauch1(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return duel.GetIncludeNameCardFromArea("", false, duel.GetOpsitePlayer(card.controller), ComVal.cardType_Monster, ComVal.Area_Monster).GroupNum>0;
    }

    private void Cost(IDuel duel, Card card, LauchEffect effect)
    {
        duel.AddFinishHandle();
        duel.SendToGraveyard(ComVal.Area_Monster, card.ToGroup(), card, ComVal.reason_Cost, effect);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        if (duel.MonsterAreaIsFull(card.controller))
        {
            duel.FinishHandle();
            return;
        }
        normalDele d = delegate
        {
            duel.FinishHandle();
        };
        duel.SpeicalSummon(ComVal.Area_Graveyard, card, card.controller, card, ComVal.reason_Effect, effect, 0, d);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return code.group.ContainCard(card) && card.previousArea == ComVal.Area_MainDeck;
    }

}
