using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 爆裂兽
/// </summary>
public class C03431737 : ICardScripts
{


    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e1.SetCategory(ComVal.category_search);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCode(ComVal.code_NoCode);
        e1.SetCost(Cost);
        e1.SetLauchArea(ComVal.Area_Hand);
        e1.SetOperation(Operation);

        duel.ResignEffect(e1, card, player);
    }

    private void Cost(IDuel duel, Card card, LauchEffect effect)
    {
        duel.AddFinishHandle();
        duel.SendToGraveyard(ComVal.Area_Hand, card.ToGroup(), card, ComVal.reason_Cost, effect);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group g = duel.GetIncludeNameCardFromArea("", false, card.controller, 0, ComVal.Area_MainDeck, Fiter);
        duel.AddFinishHandle();
        duel.AddCardToHandFromMainDeck(g.GetCard(0), card.controller, card, effect);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return duel.GetIncludeNameCardNumFromArea("", false, card.controller, 0, ComVal.Area_MainDeck, Fiter) > 0;
    }

    private bool Fiter(Card card)
    {
        return card.cardName == "爆裂模式";
    }
}
