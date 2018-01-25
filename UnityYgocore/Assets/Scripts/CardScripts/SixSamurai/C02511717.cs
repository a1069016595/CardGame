using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 真六武众-阴鬼
/// </summary>
public class C02511717 : ICardScripts
{


    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCardEffectType(ComVal.cardEffectType_mustToChooseLauch);
        e1.SetCategory(ComVal.category_spSummon);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCode(ComVal.code_NormalSummon);
        e1.SetLauchArea(ComVal.Area_Monster);
        e1.SetOperation(Operation);
        duel.ResignEffect(e1, card, player);

        StateEffect e2 = new StateEffect();
        e2.SetRangeArea(ComVal.Area_Monster);
        e2.SetCardEffectType(ComVal.cardEffectType_normalStateEffect | ComVal.cardEffectType_Single | ComVal.cardEffectType_unableReset);
        e2.SetCategory(ComVal.category_stateEffect);
        e2.SetTarget(card);
        e2.SetStateEffectType(ComVal.stateEffectType_addAfkVal);
        e2.SetGetStateEffectVal(GetAddAfkVal);
        duel.ResignEffect(e2, card, player);
    }

    private int GetAddAfkVal(IDuel duel, Card card, StateEffect e)
    {
        if (duel.GetIncludeNameCardNumFromArea(ComStr.KeyWord_SixSamurai, false,
            card.controller, ComVal.cardType_Monster, ComVal.Area_Monster, Fiter1, true, card) >0)
        {
            return 1500;
        }
        else
        {
            return 0;
        }
    }

    private bool Fiter1(Card card)
    {
        return card.IsFaceUpInMonsterArea();
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_SixSamurai, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Hand, Fiter);
        GroupCardSelectBack callBack = delegate(Group val)
        {
            normalDele d = delegate()
            {
                duel.FinishHandle();
            };
            duel.SpeicalSummon(ComVal.Area_Hand, val.GetCard(0), card.controller, card, ComVal.reason_Effect, effect, 0, d);
        };
        duel.SelectCardFromGroup(g, callBack, 1, card.controller);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return duel.GetIncludeNameCardNumFromArea(ComStr.KeyWord_SixSamurai, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Hand, Fiter) > 0
               && card.controller.group_MonsterCard.GroupNum < 5 && code.group.ContainCard(card);
    }

    private bool Fiter(Card card)
    {
       
        return card.GetCurLevel() <= 4 && card.controller.CanSpSummon(card);
    }
}
