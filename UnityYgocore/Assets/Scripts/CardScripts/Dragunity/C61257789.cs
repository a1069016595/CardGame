using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 星尘龙/爆裂体
/// </summary>
public class C61257789 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {



        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_destroy | ComVal.category_disAbleEffect);
        e1.SetCardEffectType(ComVal.cardEffectType_chooseLauch);
        e1.SetCode(ComVal.code_EffectLanch);
        e1.SetLauchArea(ComVal.Area_Monster);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCost(Cost);
        e1.SetOperation(Operation);
        duel.ResignEffect(e1, card, player);

        LauchEffect e2 = new LauchEffect();
        e2.SetCategory(ComVal.category_revived);
        e2.SetCardEffectType(ComVal.cardEffectType_mustToChooseLauch);
        e2.SetCode(ComVal.code_EnterEndPhase);
        e2.SetLauchArea(ComVal.Area_Graveyard);
        e2.SetCheckLauch(CheckLauch1);
        e2.SetOperation(Operation1);
        duel.ResignEffect(e2, card, player);

        StateEffect e3 = new StateEffect();
        e3.SetTarget(card);
        e3.SetCardEffectType(ComVal.cardEffectType_Single | ComVal.cardEffectType_normalStateEffect | ComVal.cardEffectType_unableReset);
        e3.SetStateEffectType(ComVal.stateEffectType_unableNormalSummon);
        duel.ResignEffect(e3, card, player);
    }

    private void Operation1(IDuel duel, Card card, LauchEffect effect, Group target = null)
    {
        normalDele d = delegate
        {
            duel.FinishHandle();
        };
        duel.SpeicalSummon(ComVal.Area_Graveyard, card, card.controller, card, ComVal.reason_Effect, effect, 0, d);
    }

    private bool CheckLauch1(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        if (card.changeAreaReason != null)
        {
            if (card.changeAreaReason.card == card && card.stayturn == 0)
            {
                return true;
            }
        }
        return false;
    }

    private bool F1(Card card)
    {
        return card.IsAdjust();
    }

    private bool F2(Card card)
    {
        return card.IsAdjustOutSide();
    }

    private void Cost(IDuel duel, Card card, LauchEffect effect)
    {
        duel.AddFinishHandle();
        duel.SendToGraveyard(ComVal.Area_Monster, card.ToGroup(), card, ComVal.reason_EffectDestroy, effect);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Card c = duel.GetCurChain().GetLastEffect().ownerCard;
        duel.GetCurChain().DisableLastEffect();
        if (c.curArea.IsBind(ComVal.Area_Field))
        {
            duel.AddFinishHandle();
            duel.SendToGraveyard(ComVal.Area_Field, c.ToGroup(), card, ComVal.reason_EffectDestroy, effect);
        }
        else
        {
            duel.FinishHandle();
        }
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        if (duel.GetCurChain().GetLastEffect() != null)
        {
            return true;
        }
        return false;
    }
}
