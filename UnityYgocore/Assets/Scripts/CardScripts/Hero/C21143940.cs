using UnityEngine;
using System.Collections;
/// <summary>
/// 假面变化
/// </summary>
public class C21143940 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCode(ComVal.code_FreeCode);
        e1.SetCategory(ComVal.category_toGrave | ComVal.category_spSummon);
        e1.SetLauchArea(ComVal.Area_Hand | ComVal.Area_Trap);
        e1.SetCardEffectType(ComVal.cardEffectType_chooseLauch | ComVal.cardEffectType_normalLauch);
        e1.SetGetTarget(GetTarget);
        e1.SetOperation(Operation);
        e1.SetCheckLauch(CheckLauch);
        duel.ResignEffect(e1, card, player);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Card c = group.GetCard(0);
        if (c.curArea != ComVal.Area_Monster)
        {
            duel.FinishHandle();
            return;
        }
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_MaskHERO, false, card.ownerPlayer, ComVal.CardType_Monster_Fusion, ComVal.Area_Extra);
        g = g.SiftingGroupInAttr(c.GetCurAttribute());
        g = card.controller.GetCanSpSummonGroup(g);
        GroupCardSelectBack d = delegate(Group g1)
        {
            Card c1=g1.GetCard(0);

            normalDele dele = delegate
            {
                duel.FinishHandle();
            };
            duel.SpeicalSummon(ComVal.Area_Extra,c1, card.ownerPlayer, card, ComVal.reason_Effect, effect, 0,dele);
        };
        normalDele d1 = delegate
        {
            duel.SelectCardFromGroup(g, d, 1, card.controller);
        };
        duel.AddDelegate(d1);
        duel.SendToGraveyard(ComVal.Area_Monster, group, card, ComVal.reason_Effect, effect);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        int num = duel.GetIncludeNameCardNumFromArea(ComStr.KeyWord_Hero, false, card.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_Monster, filter, false, null, null);
        if (num == 0)
            return false;
        Group g2 = GettargetGroup(duel, card);
        return g2.GroupNum > 0;
    }

    public void GetTarget(IDuel duel, Card card, LauchEffect effect, GroupCardSelectBack dele)
    {
        Group g2 = GettargetGroup(duel, card);
        duel.SelectCardFromGroup(g2, dele, 1, card.controller);
    }

    private Group GettargetGroup(IDuel duel, Card card)
    {
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_Hero, false, card.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_Monster, filter, false, null, null);
        Group g1 = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_MaskHERO, false, card.ownerPlayer, ComVal.CardType_Monster_Fusion, ComVal.Area_Extra);
        g1 = card.ownerPlayer.GetCanSpSummonGroup(g1);
        Group g2 = new Group();
        int val = 0;
        foreach (var item in g1.cardList)
        {
            val = ComVal.Add(item.GetCurAttribute(), val);
        }
        foreach (var item in g.cardList)
        {
            if (ComVal.isBind(item.GetCurAttribute(), val))
                g2.AddCard(item);
        }
        return g2;
    }

    bool filter(Card card)
    {
        return card.IsFaceUp();
    }
}
