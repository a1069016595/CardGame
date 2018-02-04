using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 龙之溪谷
/// </summary>
public class C62265044 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e1.SetCode(ComVal.code_NoCode);
        e1.SetLauchArea(ComVal.Area_Hand);
        e1.SetOperation(Operation);
        e1.SetCheckLauch(CheckLauch);
        duel.ResignEffect(e1, card, player);

        LauchEffect e2 = new LauchEffect();
        e2.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e2.SetCode(ComVal.code_NoCode);
        e2.SetLauchArea(ComVal.Area_FieldSpell);
        e2.SetOperation(Operation1);
        e2.SetCheckLauch(CheckLauch1);
        e2.SetCost(Cost);
        e2.SetDescribe("从卡组把1只4星以下的名字带有「龙骑兵团」的怪兽加入手卡");
        duel.ResignEffect(e2, card, player);

        LauchEffect e3 = new LauchEffect();
        e3.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e3.SetCode(ComVal.code_NoCode);
        e3.SetLauchArea(ComVal.Area_FieldSpell);
        e3.SetOperation(Operation2);
        e3.SetCheckLauch(CheckLauch2);
        e3.SetCost(Cost);
        e3.SetDescribe("从卡组把1只龙族怪兽送去墓地");
        duel.ResignEffect(e3, card, player);

        card.SetCardCountLimit(e2, e3 , 1);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return true;
    }

    private bool CheckLauch2(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return card.controller.group_HandCard.GroupNum > 0 &&
            duel.GetIncludeNameCardFromArea("", false, card.controller, ComVal.cardType_Monster, ComVal.Area_MainDeck, Fiter1).GroupNum > 0;
    }

    private bool Fiter1(Card card)
    {
        return card.race.IsBind(ComVal.CardRace_Dragon);
    }

    private void Operation2(IDuel duel, Card card, LauchEffect effect, Group target = null)
    {
        Group g = duel.GetIncludeNameCardFromArea("", false, card.controller, ComVal.cardType_Monster, ComVal.Area_MainDeck, Fiter1);

        GroupCardSelectBack callBack = delegate(Group val)
        {
            duel.AddFinishHandle();
            duel.SendToGraveyard(ComVal.Area_MainDeck, val, card, ComVal.reason_Effect, effect);
        };

        duel.SelectCardFromGroup(g, callBack, 1, card.controller);
    }

    private void Cost(IDuel duel, Card card, LauchEffect effect)
    {
        GroupCardSelectBack d = delegate(Group g)
        {
            duel.AddFinishHandle();
            duel.SendToGraveyard(ComVal.Area_Hand, g, card, ComVal.reason_Cost, effect);
        };
        duel.SelectCardFromGroup(card.controller.group_HandCard, d, 1, card.controller);
    }

    private void Operation1(IDuel duel, Card card, LauchEffect effect, Group target = null)
    {
        Group g = duel.GetIncludeNameCardFromArea("", false, card.controller, ComVal.cardType_Monster, ComVal.Area_MainDeck, Fiter);

        GroupCardSelectBack callBack = delegate(Group val)
        {
            duel.AddFinishHandle();
            duel.AddCardToHandFromMainDeck(val.GetCard(0), card.controller, card, effect);
        };

        duel.SelectCardFromGroup(g, callBack, 1, card.controller);
    }


    public bool CheckLauch1(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return card.controller.group_HandCard.GroupNum > 0 &&
            duel.GetIncludeNameCardFromArea("", false, card.controller, ComVal.cardType_Monster, ComVal.Area_MainDeck, Fiter).GroupNum > 0;
    }

    private bool Fiter(Card card)
    {
        return card.GetCurLevel() <= 4 && card.ContainName(ComStr.KeyWord_DragUnity);
    }


    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group)
    {
        duel.FinishHandle();
    }
}