using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 黑洞
/// </summary>
public class C53129443 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_destroy);
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e1.SetCode(ComVal.code_NoCode);
        e1.SetLauchArea(ComVal.Area_Hand | ComVal.Area_Field);
        e1.SetCheckLauch(CheckLauch);
        e1.SetOperation(Operation);
        duel.ResignEffect(e1, card, player);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group g = duel.GetIncludeNameCardFromArea("", true, null, ComVal.cardType_Monster, ComVal.Area_Monster);

        duel.AddFinishHandle();
        duel.SendToGraveyard(ComVal.Area_Field, g, card, ComVal.reason_EffectDestroy, effect);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        Group g = duel.GetIncludeNameCardFromArea("", true, null, ComVal.cardType_Monster, ComVal.Area_Monster);
        return g.GroupNum > 0;
    }
}
