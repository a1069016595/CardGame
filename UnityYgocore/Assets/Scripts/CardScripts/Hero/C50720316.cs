using UnityEngine;
/// <summary>
/// 元素英雄 影雾女郎
/// </summary>
public class C50720316 : ICardScripts
{
    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetEffectID("50720316");
        e1.SetCategory(ComVal.category_search | ComVal.category_toHand);
        e1.SetCode(ComVal.code_ToGraveyard);
        e1.SetCardEffectType(ComVal.cardEffectType_mustToChooseLauch);
        e1.SetCheckLauch(CheckLauch);
        e1.SetOperation(Operation);
        e1.SetLauchArea(ComVal.Area_Graveyard);
        duel.ResignEffect(e1, card, player);

        LauchEffect e2 = new LauchEffect();
        e2.SetEffectID("50720316");
        e2.SetCategory(ComVal.category_search | ComVal.category_toHand);
        e2.SetCode(ComVal.code_SpecialSummon);
        e2.SetCardEffectType(ComVal.cardEffectType_mustToChooseLauch);
        e2.SetCheckLauch(CheckLauch1);
        e2.SetOperation(Operation1);
        e2.SetLauchArea(ComVal.Area_Monster);
        duel.ResignEffect(e2, card, player);

        duel.ResignEffectLauchLimitCounter(player,"50720316", 1);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group theGroup = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_Hero, false, effect.ownerCard.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_MainDeck, null, true, null, effect.ownerCard.cardID);
        GroupCardSelectBack CallBack = delegate(Group targetGroup)
        {
            Card targetCard = targetGroup.GetCard(0);
            duel.AddFinishHandle();
            duel.AddCardToHandFromMainDeck( targetCard, effect.ownerCard.ownerPlayer, effect.ownerCard, effect);
        };
        duel.SelectCardFromGroup(theGroup, CallBack, 1,card.controller);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        int num = duel.GetIncludeNameCardNumFromArea(ComStr.KeyWord_Hero, false, card.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_MainDeck, null, true, null, card.cardID);
        return num != 0 && code.group.ContainCard(card);
    }

    public void Operation1(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group theGroup = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_Change, false, effect.ownerCard.ownerPlayer, ComVal.CardType_Spell_Quick, ComVal.Area_MainDeck);
        GroupCardSelectBack CallBack = delegate(Group targetGroup)
        {
            Card targetCard = targetGroup.GetCard(0);
            duel.AddFinishHandle();
            duel.AddCardToHandFromMainDeck(targetCard, effect.ownerCard.ownerPlayer, effect.ownerCard, effect);
        };
        duel.SelectCardFromGroup(theGroup, CallBack, 1,card.controller);
    }

    public bool CheckLauch1(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        Group a = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_Change, false, card.ownerPlayer, ComVal.CardType_Spell_Quick, ComVal.Area_MainDeck);
        if (a.GroupNum == 0)
            return false;
        else
            return true && code.group.ContainCard(card);
    }
}

