using UnityEngine;
/// <summary>
/// 元素英雄 天空侠
/// </summary>
public class C40044918 : ICardScripts
{
    /// <summary>
    /// 生成效果
    /// </summary>
    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        //检索
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_search);
        e1.SetCardEffectType(ComVal.cardEffectType_chooseLauch);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCode(ComVal.code_NormalSummon | ComVal.code_SpecialSummon);
        e1.SetOperation(Operation);
        e1.SetDescribe(ComStr.EffectDescribe_40044918_2);
        e1.SetLauchArea(ComVal.Area_Monster);
        e1.SetGetTarget(GetTarget);
        duel.ResignEffect(e1, card, card.ownerPlayer);

        //破坏后场
        LauchEffect e2 = new LauchEffect();
        e2.SetCategory(ComVal.category_destroy);
        e2.SetCardEffectType(ComVal.cardEffectType_chooseLauch);
        e2.SetCheckLauch(CheckLauch1);
        e2.SetCode(ComVal.code_NormalSummon | ComVal.code_SpecialSummon);
        e2.SetOperation(Operation1);
        e2.SetDescribe(ComStr.EffectDescribe_40044918_1);
        e2.SetLauchArea(ComVal.Area_Monster);
        e2.SetGetTarget(GetTarget1);
        duel.ResignEffect(e2, card, card.ownerPlayer);
    }

    public void GetTarget(IDuel duel, Card card, LauchEffect effect, GroupCardSelectBack dele)
    {
        Group a = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_Hero, false, effect.ownerCard.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_MainDeck);
        duel.SelectCardFromGroup(a, dele, 1, card.controller);
    }

    public void GetTarget1(IDuel duel, Card card, LauchEffect effect, GroupCardSelectBack dele)
    {
        int a = duel.GetIncludeNameCardNumFromArea(ComStr.KeyWord_Hero, false, card.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_Monster, null, true, card, null);
        Group b = duel.GetIncludeNameCardFromArea("", true, null, 0, ComVal.Area_Trap);
        int val = a > b.GroupNum ? b.GroupNum : a;
        duel.SelectCardFromGroup(b, dele, val, card.controller);
    }

    public bool CheckLauch(IDuel duel, Card theCard, LauchEffect effect, Code code)
    {
        if (!code.group.ContainCard(theCard))
        {
            return false;
        }
        int a = duel.GetIncludeNameCardNumFromArea(ComStr.KeyWord_Hero, false, theCard.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_MainDeck);
        return a > 0;
    }

    public void Operation(IDuel duel, Card theCard, LauchEffect effect, Group group = null)
    {
        Card targetCard = group.GetCard(0);
        duel.AddFinishHandle();
        duel.AddCardToHandFromMainDeck(targetCard, effect.ownerCard.ownerPlayer, effect.ownerCard, effect);
    }

    public bool CheckLauch1(IDuel duel, Card theCard, LauchEffect effect, Code code)
    {
        if (!code.group.ContainCard(theCard))
        {
            return false;
        }
        int a = duel.GetIncludeNameCardNumFromArea(ComStr.KeyWord_Hero, false, theCard.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_Monster, null, true, theCard, null);
        int b = duel.GetIncludeNameCardNumFromArea("", true, null, 0, ComVal.Area_Trap);
        return a != 0 && b != 0;
    }
    public void Operation1(IDuel duel, Card card, LauchEffect effect, Group theGroup = null)
    {
        duel.AddFinishHandle();
        duel.SendToGraveyard(ComVal.Area_Trap, theGroup, effect.ownerCard, ComVal.reason_EffectDestroy, effect);
    }
}

