using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 光道召唤师 露米娜丝
/// </summary>
public class C95503687 : ICardScripts
{
    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_spSummon);
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e1.SetCode(ComVal.code_NoCode);
        e1.SetLauchArea(ComVal.Area_Monster);
        e1.SetCost(Cost);
        e1.SetCheckLauch(CheckLauch);
        e1.SetOperation(Operation);
        card.SetCardCountLimit(new LauchEffect[1] { e1 }, 1);
        duel.ResignEffect(e1, card, player);

        LauchEffect e2 = new LauchEffect();
        e2.SetCategory(ComVal.category_disCard);
        e2.SetCheckLauch(CheckLauch1);
        e2.SetOperation(Operation2);
        e2.SetCode(ComVal.code_EnterEndPhase);
        e2.SetCardEffectType(ComVal.cardEffectType_mustLauch);
        e2.SetLauchArea(ComVal.Area_Monster);
        duel.ResignEffect(e2, card, player);
    }

    private bool CheckLauch1(IDuel duel, Card card, LauchEffect effect, Code code)
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

    private void Operation2(IDuel duel, Card card, LauchEffect effect, Group target = null)
    {
        duel.AddFinishHandle();
        duel.DiscardFromDeck(3, card, effect, card.controller);
    }

    private void Cost(IDuel duel, Card card, LauchEffect effect)
    {
        Group a = card.controller.group_HandCard;
        GroupCardSelectBack callBack = delegate(Group group)
        {
            duel.AddFinishHandle();
            duel.SendToGraveyard(ComVal.Area_Hand, group, card, ComVal.reason_Cost, effect);
        };
        duel.SelectCardFromGroup(a, callBack, 1, card.controller);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        GroupCardSelectBack callBack = delegate(Group g)
        {
            Card c = g.GetFirstCard();
            normalDele d = delegate
            {
                duel.FinishHandle();
            };
            duel.SpeicalSummon(ComVal.Area_Graveyard, c, card.controller, card, ComVal.reason_Effect, effect, 0, d);
        };
        duel.SelectCardFromGroup(card.controller.group_Graveyard.GetFitlerGroup(fiter), callBack, 1, card.controller);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return card.controller.group_HandCard.GroupNum > 0 && card.controller.group_Graveyard.GetFitlerGroup(fiter).GroupNum > 0;
    }

    private bool fiter(Card card)
    {
        return card.level <= 4 && card.ContainName(ComStr.KeyWord_Lightsworn);
    }
}
