using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 光之援军
/// </summary>
public class C94886282 : ICardScripts
{
    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_disCard | ComVal.category_search);
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e1.SetCode(ComVal.code_NoCode);
        e1.SetLauchArea(ComVal.Area_Hand | ComVal.Area_Field);
        e1.SetCost(Cost);
        e1.SetOperation(Operation);
        e1.SetCheckLauch(CheckLauch);
        duel.ResignEffect(e1, card, player);
    }

    private void Cost(IDuel duel, Card card, LauchEffect effect)
    {
        duel.AddFinishHandle();
        duel.DiscardFromDeck(3, card, effect, card.controller);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_Lightsworn, false, card.controller, ComVal.cardType_Monster, ComVal.Area_MainDeck, Filter);
        if(g.GroupNum==0)
        {
            duel.FinishHandle();
            return;
        }
        else
        {
            GroupCardSelectBack callBack=delegate(Group val)
            {
                duel.AddFinishHandle();
                duel.AddCardToHandFromMainDeck(val.GetFirstCard(), card.controller, card, effect);
            };
            duel.SelectCardFromGroup(g, callBack, 1, card.controller);
        }
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_Lightsworn, false, card.controller, ComVal.cardType_Monster, ComVal.Area_MainDeck, Filter);
        return g.GroupNum > 0 && card.controller.group_MainDeck.GroupNum > 3;
    }

    private bool Filter(Card card)
    {
        return card.GetCurLevel() <= 4;
    }
}
