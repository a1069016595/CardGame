using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 星球改造
/// </summary>
public class C73628505 : ICardScripts
{


    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e1.SetCategory(ComVal.category_search);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCode(ComVal.code_NoCode);
        e1.SetLauchArea(ComVal.Area_NormalTrap | ComVal.Area_Hand);
        e1.SetOperation(Operation);

        duel.ResignEffect(e1, card, player);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group g = duel.GetIncludeNameCardFromArea("", false, card.controller, ComVal.CardType_Spell_Field, ComVal.Area_MainDeck);

        GroupCardSelectBack callBack = delegate(Group val)
        {
            duel.AddFinishHandle();
            duel.AddCardToHandFromMainDeck(val.GetCard(0), card.controller, card, effect);
        };
        duel.SelectCardFromGroup(g, callBack, 1, card.controller);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return duel.GetIncludeNameCardNumFromArea("", false, card.controller, ComVal.CardType_Spell_Field, ComVal.Area_MainDeck) > 0;
    }
}
