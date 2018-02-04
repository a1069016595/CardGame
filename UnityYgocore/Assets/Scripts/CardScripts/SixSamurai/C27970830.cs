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
        e2.SetCardEffectType(ComVal.cardEffectType_notInChain);
        e2.SetCategory(ComVal.category_pointer);
        e2.SetCheckLauch(CheckLauch1);
        e2.SetCode(ComVal.code_NormalSummon | ComVal.code_SpecialSummon);
        e2.SetLauchArea(ComVal.Area_NormalTrap);
        e2.SetOperation(Operation1);
        duel.ResignEffect(e2, card, player);


    }

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
