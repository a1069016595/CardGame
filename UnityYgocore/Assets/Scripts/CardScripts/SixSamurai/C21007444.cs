using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 诸刃的活人剑术
/// </summary>
public class C21007444 : ICardScripts
{


    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch | ComVal.cardEffectType_chooseLauch);
        e1.SetCategory(ComVal.category_revived);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCode(ComVal.code_FreeCode);
        e1.SetGetTarget(GetTarget);
        e1.SetLauchArea(ComVal.Area_NormalTrap);
        e1.SetOperation(Operation);
        duel.ResignEffect(e1, card, player);
    }

    private void GetTarget(IDuel duel, Card card, LauchEffect effect, GroupCardSelectBack dele)
    {
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_SixSamurai, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Graveyard);
        duel.SelectCardFromGroup(g, dele, 2, card.controller);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
      // duel.SpeicalSummon()
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        int cardNum = duel.GetIncludeNameCardNumFromArea(ComStr.KeyWord_SixSamurai, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Graveyard);
        return cardNum >= 2;
    }
}
