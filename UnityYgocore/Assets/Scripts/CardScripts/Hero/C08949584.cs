using UnityEngine;
using System.Collections;

/// <summary>
/// 英雄到来
/// </summary>
public class C08949584 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e1.SetCategory(ComVal.category_spSummon);
        e1.SetLauchArea(ComVal.Area_Hand | ComVal.Area_Trap);
        e1.SetCode(ComVal.code_NoCode);
        e1.SetCost(Cost);
        e1.SetOperation(Operation);
        e1.SetCheckLauch(CheckLauch);
        duel.ResignEffect(e1, card, player);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_ElementalHERO, false, card.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_MainDeck, fitler1, false, null, null);
        
        Group g1 = card.ownerPlayer.GetCanSpSummonGroup(g);
        GroupCardSelectBack d1 = delegate(Group g2)
        {
            Card c = g2.GetCard(0);

            normalDele dele = delegate
            {
                duel.FinishHandle();
            };
            duel.SpeicalSummon(ComVal.Area_MainDeck,c, card.ownerPlayer, card, ComVal.reason_Effect, effect, 0, dele);
        };
        duel.SelectCardFromGroup(g1, d1, 1, card.controller);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        int num = duel.GetIncludeNameCardNumFromArea("", false, card.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_Monster, fitler, false, null, null);
        Group g1 = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_ElementalHERO, false, card.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_MainDeck, fitler1, false, null, null);
        return card.ownerPlayer.CanSpSummon(g1) && num == 0;
    }

    public void Cost(IDuel duel, Card card, LauchEffect effect)
    {
        Player player = card.ownerPlayer;
        duel.ReduceLP(player.LP / 2, player,ComVal.reason_Cost,card,effect);
        duel.FinishHandle();
    }

    bool fitler(Card card)
    {
        if (!card.IsMonster())
        {
            return false;
        }
        if (card.curPlaseState == ComVal.CardPutType_UpRightFront || card.curPlaseState == ComVal.CardPutType_layFront)
            return card.CanSpSummon();
        else
            return false;
    }

    bool fitler1(Card card)
    {
        if (!card.IsMonster())
        {
            return false;
        }
        if (card.GetCurLevel() <= 4)
            return card.CanSpSummon();
        else
            return false;
    }
}
