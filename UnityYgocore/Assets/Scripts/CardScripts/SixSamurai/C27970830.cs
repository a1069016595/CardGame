using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 六武之门
/// </summary>
public class C27970830 : ICardScripts
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
        e2.SetCardEffectType(ComVal.cardEffectType_mustNotInChain);
        e2.SetCategory(ComVal.category_pointer);
        e2.SetCheckLauch(CheckLauch1);
        e2.SetCode(ComVal.code_NormalSummon | ComVal.code_SpecialSummon);
        e2.SetLauchArea(ComVal.Area_NormalTrap);
        e2.SetOperation(Operation1);
        duel.ResignEffect(e2, card, player);

        LauchEffect e3 = new LauchEffect();//加攻
        e3.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e3.SetCode(ComVal.code_NoCode);
        e3.SetLauchArea(ComVal.Area_NormalTrap);
        e3.SetOperation(Operation2);
        e3.SetCost(Cost2);
        e3.SetCheckLauch(CheckLauch2);
        e3.SetCategory(ComVal.category_pointer);
        e3.SetGetTarget(GetTarget2);
        duel.ResignEffect(e3, card, player);

        LauchEffect e4 = new LauchEffect();//回手牌
        e4.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e4.SetCode(ComVal.code_NoCode);
        e4.SetLauchArea(ComVal.Area_NormalTrap);
        e4.SetOperation(Operation3);
        e4.SetCost(Cost3);
        e3.SetCheckLauch(CheckLauch3);
        e4.SetCategory(ComVal.category_pointer|ComVal.category_toHand);
        duel.ResignEffect(e4, card, player);

        LauchEffect e5 = new LauchEffect();//复生
        e5.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e5.SetCode(ComVal.code_NoCode);
        e5.SetLauchArea(ComVal.Area_NormalTrap);
        e5.SetOperation(Operation4);
        e5.SetCost(Cost4);
        e3.SetCheckLauch(CheckLauch4);
        e5.SetCategory(ComVal.category_pointer|ComVal.category_spSummon);
        duel.ResignEffect(e5, card, player);
    }

    #region

    private bool CheckLauch4(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return card.GetPointerNum(ComStr.Pointer_Samurai) >= 6 && GetTargetGroup4(duel, card).GroupNum > 0;
    }

    private void Cost4(IDuel duel, Card card, LauchEffect effect)
    {
        card.RemovePoint(ComStr.Pointer_Samurai, 6);
        duel.FinishHandle();
    }

    private void Operation4(IDuel duel, Card card, LauchEffect effect, Group target = null)
    {
        Group g = GetTargetGroup4(duel, card);
        GroupCardSelectBack callBack = delegate(Group val)
        {
            Card c = val.GetFirstCard();

            normalDele d = delegate()
            {
                duel.FinishHandle();
            };
            duel.SpeicalSummon(ComVal.Area_Graveyard,c,card.controller,card,ComVal.reason_Effect,effect,0,d);
        };
        duel.SelectCardFromGroup(g,callBack,1,card.controller);
    }

    private Group GetTargetGroup4(IDuel duel,Card card)
    {
       return duel.GetIncludeNameCardFromArea(ComStr.KeyWord_Shien, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Graveyard);
    }

    #endregion

    #region

    private bool CheckLauch3(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return card.GetPointerNum(ComStr.Pointer_Samurai) >= 4 && GetTargetGroup3(duel, card).GroupNum > 0;
    }

    private void Cost3(IDuel duel, Card card, LauchEffect effect)
    {
        card.RemovePoint(ComStr.Pointer_Samurai, 4);
        duel.FinishHandle();
    }

    private void Operation3(IDuel duel, Card card, LauchEffect effect, Group target = null)
    {
        Group g = GetTargetGroup3(duel, card);
        if(g.GroupNum==0)
        {
            duel.FinishHandle();
            return;
        }
        GroupCardSelectBack callBack=delegate(Group val)
        {
            Card c = val.GetFirstCard();
            duel.AddFinishHandle();
            duel.AddCardToHandFromArea(c.curArea, c, card.controller, card, effect);
        };

        duel.SelectCardFromGroup(g, callBack, 1, card.controller);
    }

    private Group GetTargetGroup3(IDuel duel,Card card)
    {
        return duel.GetIncludeNameCardFromArea(ComStr.KeyWord_SixSamurai, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Graveyard | ComVal.Area_MainDeck);
    }
    #endregion

    #region
    private void GetTarget2(IDuel duel, Card card, LauchEffect effect, GroupCardSelectBack dele)
    {
        Group g = GetEffectTargetGroup(duel, card);
        duel.SelectCardFromGroup(g, dele, 1, card.controller);
    }

    private bool CheckLauch2(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        int num = GetEffectTargetGroup(duel,card).GroupNum;
        return card.GetPointerNum(ComStr.Pointer_Samurai) >= 2 && num >= 1; 
    }

    private void Cost2(IDuel duel, Card card, LauchEffect effect)
    {
        card.RemovePoint(ComStr.Pointer_Samurai, 2);
        duel.FinishHandle();
    }

    private void Operation2(IDuel duel, Card card, LauchEffect effect, Group target = null)
    {
        Card targetCard = target.GetFirstCard();

        StateEffect e1 = new StateEffect();
        e1.SetRangeArea(ComVal.Area_Monster);
        e1.SetCategory(ComVal.category_stateEffect | ComVal.category_limitTime);
        e1.SetCardEffectType(ComVal.cardEffectType_Single | ComVal.cardEffectType_normalStateEffect);
        e1.SetStateEffectType(ComVal.stateEffectType_addAfkVal);
        e1.SetTarget(targetCard);
        e1.SetStateEffectVal(500);
        e1.SetResetCode(ComVal.resetEvent_LeaveEndPhase, 1);

        duel.ResignEffect(e1, card, card.controller);
        duel.FinishHandle();
    }

    private Group GetEffectTargetGroup(IDuel duel,Card card)
    {
        return duel.GetIncludeNameCardFromArea("", false, card.controller, ComVal.cardType_Monster, ComVal.Area_Monster, Fiter2);
    }

    private bool Fiter2(Card card)
    {
        return (card.ContainName(ComStr.KeyWord_SixSamurai) || card.ContainName(ComStr.KeyWord_Shien)) && card.IsFaceUpInMonsterArea();
    }
    #endregion


    private void Operation1(IDuel duel, Card card, LauchEffect effect, Group target = null)
    {
        card.AddPointer(ComStr.Pointer_Samurai, 2, 1000);
    }

    private bool CheckLauch1(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        Group g = code.group.GetFitlerGroup(Fiter);
        return g.GroupNum >= 1;
    }

    private bool Fiter(Card card)
    {
        return card.ContainName(ComStr.KeyWord_SixSamurai);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        duel.FinishHandle();
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return true;
    }
}
