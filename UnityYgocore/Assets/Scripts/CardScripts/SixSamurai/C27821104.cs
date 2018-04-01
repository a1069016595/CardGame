using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 六武众的荒行
/// </summary>
public class C27821104 : ICardScripts
{
    public static Card mCard;

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch | ComVal.cardEffectType_chooseLauch);
        e1.SetCategory(ComVal.category_destroy | ComVal.category_spSummon);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCode(ComVal.code_FreeCode);
        e1.SetGetTarget(GetTarget);
        e1.SetLauchArea(ComVal.Area_NormalTrap | ComVal.Area_Hand);
        e1.SetOperation(Operation);
        duel.ResignEffect(e1, card, player);

    }


    private void GetTarget(IDuel duel, Card card, LauchEffect effect, GroupCardSelectBack dele)
    {
        Group result = GetTarget(duel, card);
        GroupCardSelectBack callBack = delegate(Group g)
        {
            card.EffectDataCard = g.GetFirstCard();
            dele(g);
        };

        duel.SelectCardFromGroup(result, callBack, 1, card.controller);
    }

    private static Group GetTarget(IDuel duel, Card card)
    {
        Group deckG = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_SixSamurai, false, card.controller, ComVal.cardType_Monster, ComVal.Area_MainDeck);
        Group fieldG = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_SixSamurai, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Monster);

        Group result = new Group();
        foreach (var item in deckG.cardList)
        {
            foreach (var fieldCard in fieldG.cardList)
            {
                if (!result.ContainCard(fieldCard) && fieldCard.GetCurAfk() == item.GetCurAfk() && fieldCard.cardName != item.cardName)
                {
                    result.AddCard(fieldCard);
                }
            }
        }
        return result;
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Card target = card.EffectDataCard;
        mCard = target;
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_SixSamurai, false, card.controller, ComVal.cardType_Monster, ComVal.Area_MainDeck, Fiter);
        if (!target.curArea.IsBind(ComVal.Area_Monster) || g.GroupNum == 0)
        {
            duel.FinishHandle();
            return;
        }

        GroupCardSelectBack callBack = delegate(Group val)
        {
            normalDele d = delegate
            {
                normalDele DestoryCard = delegate
                {
                    Card c = val.GetFirstCard();
                    duel.SendToGraveyard(ComVal.Area_Monster, c.ToGroup(), card, ComVal.reason_Effect, effect);
                };
                duel.AddDelayAction(DestoryCard, ComVal.resetEvent_LeaveEndPhase, 0);
                duel.FinishHandle();
            };
            duel.SpeicalSummon(ComVal.Area_MainDeck, val.GetFirstCard(), card.controller, card, ComVal.reason_Effect, effect, 0, d);
        };
        duel.SelectCardFromGroup(g, callBack, 1, card.controller);
    }

    private bool Fiter(Card card)
    {
        return card.GetCurAfk() == mCard.GetCurAfk() && card.cardName != mCard.cardName;
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return GetTarget(duel, card).GroupNum > 0 && card.controller.GetLeftMonsterAreaNum() > 0;
    }
}
