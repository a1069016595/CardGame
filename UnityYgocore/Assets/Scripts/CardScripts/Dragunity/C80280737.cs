using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 爆裂模式
/// </summary>
public class C80280737 : ICardScripts
{
    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch | ComVal.cardEffectType_chooseLauch);
        e1.SetCategory(ComVal.category_spSummon);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCode(ComVal.code_FreeCode);
        e1.SetCost(Cost);
        e1.SetLauchArea(ComVal.Area_Trap);
        e1.SetOperation(Operation);
        duel.ResignEffect(e1, card, player);
    }

    private void Cost(IDuel duel, Card card, LauchEffect effect)
    {
        Group g = GetCanReleaseGroup(duel, card);

        GroupCardSelectBack callBack = delegate(Group val)
        {
            card.EffectDataCard = val.GetCard(0);
            duel.AddFinishHandle();
            duel.SendToGraveyard(ComVal.Area_Field, val, card, ComVal.reason_Realease, effect);
        };

        duel.SelectCardFromGroup(g, callBack, 1, card.controller);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Card effectDataCard = card.EffectDataCard;
        Group mainDeckG = duel.GetIncludeNameCardFromArea(effectDataCard.cardName, false, card.controller, ComVal.cardType_Monster, ComVal.Area_MainDeck, Fiter1);
        if(mainDeckG.GroupNum==0)
        {
            duel.FinishHandle();
            return;
        }
        normalDele d=delegate
        {
            duel.FinishHandle();
        };
        duel.SpeicalSummon(ComVal.Area_MainDeck, mainDeckG.GetCard(0), card.controller, card, ComVal.reason_Effect, effect, ComVal.CardPutType_UpRightFront, d);
    }

    private bool Fiter1(Card card)
    {
        return card.controller.CanSpSummon(card) && card.ContainName("爆裂体");
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return GetCanReleaseGroup(duel, card).GroupNum > 0;
    }

    private Group GetCanReleaseGroup(IDuel duel, Card card)
    {
        Group resultG = new Group();
        Group g = duel.GetIncludeNameCardFromArea("", false, card.controller, ComVal.CardType_Monster_Synchro, ComVal.Area_Monster);
        Group mainDeckG = duel.GetIncludeNameCardFromArea("爆裂体", false, card.controller, ComVal.cardType_Monster, ComVal.Area_MainDeck, Fiter);
        foreach (var item in g.cardList)
        {
            foreach (var mainDeckCard in mainDeckG.cardList)
            {
                if (mainDeckCard.cardName.Contains(item.cardName) && !resultG.ContainCard(item))
                {
                    resultG.AddCard(item);
                }
            }
        }
        return resultG;
    }

    private bool Fiter(Card card)
    {
        return card.controller.CanSpSummon(card);
    }
}
