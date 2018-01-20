using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 太阳交换
/// </summary>
public class C00691925 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e1.SetCategory(ComVal.category_disCard | ComVal.category_drawCard);
        e1.SetCode(ComVal.code_NoCode);
        e1.SetLauchArea(ComVal.Area_Field | ComVal.Area_Hand);
        e1.SetCost(Cost);
        e1.SetCheckLauch(CheckLauch);
        e1.SetOperation(Operation);
        duel.ResignEffect(e1, card, player);
    }

    private void Cost(IDuel duel, Card card, LauchEffect effect)
    {
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_Lightsworn, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Hand);

        GroupCardSelectBack callBack = delegate(Group val)
        {
            duel.AddFinishHandle();
            duel.SendToGraveyard(ComVal.Area_Hand, val, card, ComVal.reason_Cost, effect);
        };
        duel.SelectCardFromGroup(g, callBack, 1, card.controller);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        normalDele d = delegate
        {
            duel.AddFinishHandle();
            duel.DiscardFromDeck(2, card, effect, card.controller);
        };
        duel.AddDelegate(d, true);
        duel.DrawCard(card.controller, 2, card, effect);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_Lightsworn, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Hand);
        return g.GroupNum > 0 && card.controller.group_MainDeck.GroupNum >= 2;
    }
}
