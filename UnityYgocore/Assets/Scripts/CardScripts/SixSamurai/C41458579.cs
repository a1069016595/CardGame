using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 六尺琼勾玉
/// </summary>
public class C41458579 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_destroy | ComVal.category_disAbleEffect);
        e1.SetCardEffectType(ComVal.cardEffectType_chooseLauch);
        e1.SetCode(ComVal.code_EffectLanch);
        e1.SetLauchArea(ComVal.Area_NormalTrap);
        e1.SetCheckLauch(CheckLauch);
        e1.SetOperation(Operation);
        duel.ResignEffect(e1, card, player);

    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Card c = duel.GetCurChain().GetLastEffect().ownerCard;
        duel.GetCurChain().DisableLastEffect();
        if (c.curArea.IsBind(ComVal.Area_Field))
        {
            duel.AddFinishHandle();
            duel.SendToGraveyard(ComVal.Area_Field, c.ToGroup(), card, ComVal.reason_EffectDestroy, effect);
        }
        else
        {
            duel.FinishHandle();
        }
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return duel.GetCurChain().GetLastEffect().ownerCard.controller != card.controller &&
            duel.GetCurChain().GetLastEffect().category.IsBind(ComVal.category_destroy)
            && duel.GetIncludeNameCardNumFromArea(ComStr.KeyWord_SixSamurai, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Monster, Fiter) > 0;
    }

    private bool Fiter(Card card)
    {
        return card.IsFaceUpInMonsterArea();
    }
}
