using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 光道兽 沃尔夫
/// </summary>
public class C58996430 : ICardScripts
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
