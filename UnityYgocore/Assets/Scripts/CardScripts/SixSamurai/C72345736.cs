using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 六武众的团结
/// </summary>
public class C72345736 : ICardScripts
{


    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCardEffectType(ComVal.cardEffectType_notInChain);
        e1.SetCategory(ComVal.category_pointer);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCode(ComVal.code_NormalSummon | ComVal.code_SpecialSummon);
        e1.SetLauchArea(ComVal.Area_NormalTrap);
        e1.SetOperation(Operation);
        duel.ResignEffect(e1, card, player);

        LauchEffect e2 = new LauchEffect();
        e2.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e2.SetCode(ComVal.code_NoCode);
        e2.SetCategory(ComVal.category_drawCard);
        e2.SetCheckLauch(CheckLauch1);
        e2.SetCost(Cost);
        e2.SetLauchArea(ComVal.Area_NormalTrap);
        e2.SetOperation(Operation1);

        LauchEffect e3 = new LauchEffect();
        e3.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e3.SetCode(ComVal.code_NoCode);
        e3.SetLauchArea(ComVal.Area_Hand);
        e3.SetOperation(Operation2);
        e3.SetCheckLauch(CheckLauch2);
        duel.ResignEffect(e3, card, player);
    }

    private void Operation2(IDuel duel, Card card, LauchEffect effect, Group target = null)
    {
        duel.FinishHandle();
    }

    private bool CheckLauch2(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return true;
    }

    private bool CheckLauch1(IDuel duel, Card card, LauchEffect effect, Code code)
    {
       return card.GetPointerNum(ComStr.Pointer_Samurai)>0;
    }

    private void Cost(IDuel duel, Card card, LauchEffect effect)
    {
        duel.AddFinishHandle();
        duel.SendToGraveyard(ComVal.Area_NormalTrap, card.ToGroup(), card, ComVal.reason_Cost, effect);
    }

    private void Operation1(IDuel duel, Card card, LauchEffect effect, Group target = null)
    {
        duel.AddFinishHandle();
        duel.DrawCard(card.controller, card.GetPointerNum(ComStr.Pointer_Samurai), card, effect);
    }



    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        card.AddPointer(ComStr.Pointer_Samurai, 1, 2);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        Group g = code.group.GetFitlerGroup(Fiter);
        return g.GroupNum > 1;
    }

    private bool Fiter(Card card)
    {
        return card.ContainName(ComStr.KeyWord_SixSamurai);
    }
}
