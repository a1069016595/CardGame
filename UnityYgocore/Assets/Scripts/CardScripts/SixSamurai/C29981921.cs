using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 真六武众-紫炎
/// </summary>
public class C29981921 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        card.SetMaterialFilter2(F1, F2);

        LauchEffect e1 = new LauchEffect();
        e1.SetCardEffectType(ComVal.cardEffectType_chooseLauch);
        e1.SetCode(ComVal.code_EffectLanch);
        e1.SetCategory(ComVal.category_destroy | ComVal.category_disAbleEffect);
        e1.SetCheckLauch(CheckLauch);
        e1.SetLauchArea(ComVal.Area_Field);
        e1.SetOperation(Operation);
        duel.ResignEffect(e1, card, player);

        card.SetCardCountLimit(e1, 1);
    }


    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Card c = duel.GetCurChain().GetLastEffect().ownerCard;
        duel.GetCurChain().DisableLastEffect();
        duel.AddFinishHandle();
        duel.SendToGraveyard(ComVal.Area_Field, c.ToGroup(), card, ComVal.reason_Effect, effect);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        if (duel.GetCurChain().GetLastEffect().ownerCard.cardType.IsBind(ComVal.cardType_Spell | ComVal.cardType_Trap))
        {
            return true;
        }
        return false;
    }


    private bool F1(Card card)
    {
        return card.IsAdjust() && card.race.IsBind(ComVal.CardRace_Warrior);
    }

    private bool F2(Card card)
    {
        return card.IsAdjustOutSide() && card.ContainName(ComStr.KeyWord_SixSamurai);
    }
}