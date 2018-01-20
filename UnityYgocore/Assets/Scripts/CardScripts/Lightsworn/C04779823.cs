using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 光道主·大天使 米迦勒
/// </summary>
public class C04779823 : ICardScripts
{
    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        card.SetMaterialFilter2(F1, F2);

        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_remove);
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e1.SetLauchArea(ComVal.Area_Monster);
        e1.SetCost(Cost);
        e1.SetCheckLauch(CheckLauch);
        e1.SetOperation(Operation);
        e1.SetGetTarget(GetTarget);
        duel.ResignEffect(e1, card, player);
        card.SetCardCountLimit(new LauchEffect[1] { e1 }, 1);

        LauchEffect e2 = new LauchEffect();
        e2.SetCardEffectType(ComVal.cardEffectType_chooseLauch);
        e2.SetCategory(ComVal.category_addAttackVal);
        e2.SetCode(ComVal.code_ToGraveyard);
        e2.SetLauchArea(ComVal.Area_Graveyard);
        e2.SetGetTarget(GetTarget1);
        e2.SetCheckLauch(CheckLauch1);
        e2.SetOperation(Operation1);
        duel.ResignEffect(e2, card, player);

        LauchEffect e3 = new LauchEffect();
        e3.SetCategory(ComVal.category_disCard);
        e3.SetCheckLauch(CheckLauch3);
        e3.SetOperation(Operation3);
        e3.SetCode(ComVal.code_EnterEndPhase);
        e3.SetCardEffectType(ComVal.cardEffectType_mustLauch);
        e3.SetLauchArea(ComVal.Area_Monster);
        duel.ResignEffect(e3, card, player);
    }

    private void Operation3(IDuel duel, Card card, LauchEffect effect, Group target = null)
    {
        duel.AddFinishHandle();
        duel.DiscardFromDeck(3, card, effect, card.controller);
    }

    private bool CheckLauch3(IDuel duel, Card card, LauchEffect effect, Code code)
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

    private void Operation1(IDuel duel, Card card, LauchEffect effect, Group target = null)
    {
        normalDele d = delegate
        {
            duel.AddLP(target.GroupNum*300, card.controller, ComVal.reason_Effect, card, effect);
            duel.FinishHandle();
        };
        duel.AddDelegate(d, true);
        duel.SendToMainDeck(ComVal.Area_Graveyard, target, card, ComVal.reason_Effect, effect);
    }

    private bool CheckLauch1(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return code.group.ContainCard(card) && code.reason.reason.IsBind(ComVal.reason_Destroy);
    }

    private void GetTarget1(IDuel duel, Card card, LauchEffect effect, GroupCardSelectBack dele)
    {
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_Lightsworn, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Graveyard, null, true, card);
        duel.SelectCardFromGroup(g, dele, -1, card.controller);
    }

    private void GetTarget(IDuel duel, Card card, LauchEffect effect, GroupCardSelectBack dele)
    {
        Group g = duel.GetIncludeNameCardFromArea("", true, null, 0, ComVal.Area_Field, null, true, card);
        duel.SelectCardFromGroup(g, dele, 1, card.controller);
    }

    private void Cost(IDuel duel, Card card, LauchEffect effect)
    {
        duel.ReduceLP(1000, card.controller, ComVal.reason_Cost, card, effect);
        duel.FinishHandle();
    }
    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        duel.AddFinishHandle();
        duel.SendToRemove(ComVal.Area_Field, group, card, ComVal.reason_Effect, effect);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return duel.GetIncludeNameCardNumFromArea("", true, null, 0, ComVal.Area_Field, null, true, card) > 0 && card.controller.LP > 1000;
    }

    private bool F2(Card card)
    {
        return card.IsAdjustOutSide() && card.GetCurAttribute().IsBind(ComVal.CardAttr_Light);
    }

    private bool F1(Card card)
    {
        return card.IsAdjust();
    }
}
