using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 六武派二刀流
/// </summary>
public class C75525309 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch | ComVal.cardEffectType_chooseLauch);
        e1.SetCategory(ComVal.category_toHand);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCode(ComVal.code_NoCode | ComVal.code_FreeCode);
        e1.SetGetTarget(GetTarget);
        e1.SetLauchArea(ComVal.Area_NormalTrap);
        e1.SetOperation(Operation);
        duel.ResignEffect(e1, card, player);
    }

    private void GetTarget(IDuel duel, Card card, LauchEffect effect, GroupCardSelectBack dele)
    {
        Group g = duel.GetIncludeNameCardFromArea("", false, duel.GetOpsitePlayer(card.controller), 0, ComVal.Area_Field);
        duel.SelectCardFromGroup(g, dele, 2, card.controller);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        duel.AddFinishHandle();
        duel.AddCardToHandFromArea(ComVal.Area_Field, group, card.controller, card, effect);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return duel.GetIncludeNameCardNumFromArea(ComStr.KeyWord_SixSamurai, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Monster, Fiter) == 1
            && duel.GetIncludeNameCardNumFromArea("", false, duel.GetOpsitePlayer(card.controller), 0, ComVal.Area_Field) >= 2;
    }

    private bool Fiter(Card card)
    {
        return card.IsFaceUpInMonsterArea();
    }
}
