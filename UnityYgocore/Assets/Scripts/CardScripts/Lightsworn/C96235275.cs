using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 光道圣骑士 简
/// </summary>
public class C96235275 : ICardScripts
{
    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        StateEffect e1 = new StateEffect();
        e1.SetRangeArea(ComVal.Area_Monster);
        e1.SetCategory(ComVal.category_stateEffect);
        e1.SetCardEffectType(ComVal.cardEffectType_Single | ComVal.cardEffectType_normalStateEffect | ComVal.cardEffectType_unableReset);
        e1.SetStateEffectType(ComVal.stateEffectType_addAfkVal);
        e1.SetTarget(card);
        e1.SetStateEffectVal(300);
        e1.SetCondtion(Condition);
        duel.ResignEffect(e1, card, player);

        LauchEffect e2 = new LauchEffect();
        e2.SetCategory(ComVal.category_disCard);
        e2.SetCheckLauch(CheckLauch);
        e2.SetOperation(Operation);
        e2.SetCode(ComVal.code_EnterEndPhase);
        e2.SetCardEffectType(ComVal.cardEffectType_mustLauch);
        e2.SetLauchArea(ComVal.Area_Monster);
        duel.ResignEffect(e2, card, player);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group target = null)
    {
        duel.AddFinishHandle();
        duel.DiscardFromDeck(2, card, effect, card.controller);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
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

    bool Condition(IDuel duel, Card card, BaseEffect e,Card targetCard)
    {
        if (duel.GetCurPhase() == ComVal.Phase_Battlephase)
        {
            if (duel.GetCurAttackEvent()!=null&& duel.GetCurAttackEvent().Attacker == card && duel.GetCurAttackEvent().Attacker != null)
            {
                return true;
            }
        }
        return false;
    }
}
