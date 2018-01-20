using UnityEngine;
using System.Collections;
/// <summary>
/// 银河旋风
/// </summary>
public class C05133471 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_destroy);
        e1.SetCode(ComVal.code_NoCode);
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e1.SetLauchArea(ComVal.Area_Hand | ComVal.Area_Trap);
        e1.SetOperation(Operation);
        e1.SetCheckLauch(CheckLauch);
        e1.SetGetTarget(GetTarget);
        duel.ResignEffect(e1, card, player);

        LauchEffect e2 = new LauchEffect();
        e2.SetCategory(ComVal.category_destroy);
        e2.SetCode(ComVal.code_NoCode);
        e2.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e2.SetLauchArea(ComVal.Area_Graveyard);
        e2.SetOperation(Operation1);
        e2.SetCheckLauch(CheckLauch1);
        e2.SetGetTarget(GetTarget1);
        e2.SetCost(Cost);
        e2.SetEffectID("05133471");
        duel.ResignEffect(e2, card, player);

        duel.ResignEffectLauchLimitCounter(player,"05133471", 1);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        duel.AddFinishHandle();
        duel.SendToGraveyard(ComVal.Area_Trap, group, card, ComVal.reason_EffectDestroy, effect);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        int num = duel.GetIncludeNameCardNumFromArea("", true, null, 0, ComVal.Area_Trap, filter, true, card, null);
        return num > 0;
    }

    public void GetTarget(IDuel duel, Card card, LauchEffect effect, GroupCardSelectBack dele)
    {
        Group g = duel.GetIncludeNameCardFromArea("", true, null, 0, ComVal.Area_Trap, filter, true, card, null);
        duel.SelectCardFromGroup(g, dele, 1, card.controller);
    }

    bool filter(Card card)
    {

        return !card.IsFaceUp() && card.CanDestroy();
    }

    public void Operation1(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        duel.AddFinishHandle();
        duel.SendToGraveyard(ComVal.Area_Trap, group, card, ComVal.reason_EffectDestroy, effect);
    }

    public bool CheckLauch1(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        if (card.stayturn == 0)
        {
            return false;
        }
        int num = duel.GetIncludeNameCardNumFromArea("", true, null,0, ComVal.Area_Trap, filter1, false, null, null);
        return num > 0;
    }

    public void GetTarget1(IDuel duel, Card card, LauchEffect effect, GroupCardSelectBack dele)
    {
        Group g = duel.GetIncludeNameCardFromArea("", true, null,0, ComVal.Area_Trap, filter1, false, null, null);
        duel.SelectCardFromGroup(g, dele, 1, card.controller);
    }

    bool filter1(Card card)
    {
        return !card.IsFaceUp() && card.CanDestroy();
    }
    public void Cost(IDuel duel, Card card, LauchEffect effect)
    {
        Group g = new Group();
        g.AddCard(card);
        duel.AddFinishHandle();
        duel.SendToRemove(ComVal.Area_Graveyard, g, card, ComVal.reason_Cost, effect);
    }
}
