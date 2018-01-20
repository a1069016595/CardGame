using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 愚蠢的埋葬
/// </summary>

public class C81439173 : ICardScripts
{
    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e1.SetCategory(ComVal.category_toGrave);
        e1.SetCode(ComVal.code_NoCode);
        e1.SetLauchArea(ComVal.Area_Hand | ComVal.Area_Field);
        e1.SetCheckLauch(CheckLauch);
        e1.SetOperation(Operation);
        duel.ResignEffect(e1, card, player);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group g = card.controller.group_MainDeck;
        g= g.GetFitlerGroup(Filter);
       
        GroupCardSelectBack callBack=delegate(Group val)
        {
            duel.AddFinishHandle();
            duel.SendToGraveyard(ComVal.Area_MainDeck, val, card, ComVal.reason_Effect, effect);
        };
        duel.SelectCardFromGroup(g, callBack, 1, card.controller);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        Group g = card.controller.group_MainDeck;
        return g.GetFitlerGroup(Filter).GroupNum > 0;
    }

    private bool Filter(Card card)
    {
        return card.IsMonster();
    }
}
