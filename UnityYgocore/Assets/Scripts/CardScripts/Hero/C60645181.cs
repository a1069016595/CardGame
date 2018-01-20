using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 英豪冠军 断钢剑王
/// </summary>
public class C60645181 : ICardScripts
{
    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        card.SetXYZMaterialFiter(Fiter, 2);

        LauchEffect e1 = new LauchEffect();
        e1.SetCost(Cost);
        e1.SetCategory(ComVal.category_addAttackVal);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e1.SetLauchArea(ComVal.Area_Field);
        e1.SetOperation(Operation);

        duel.ResignEffect(e1, card, player);
    }

    private void Cost(IDuel duel, Card card, LauchEffect effect)
    {
        duel.AddFinishHandle();
        duel.RemoveXYZMaterial(card, 2, ComVal.reason_Cost, card, effect);
    }

    private bool Fiter(Card card)
    {
        return card.GetCurLevel() == 4 && card.CanBeXYZMaterial() && card.race.IsBind(ComVal.CardRace_Warrior);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        StateEffect e1 = new StateEffect();
        e1.SetRangeArea(ComVal.Area_Monster);
        e1.SetCategory(ComVal.category_stateEffect | ComVal.category_limitTime);
        e1.SetCardEffectType(ComVal.cardEffectType_Single | ComVal.cardEffectType_normalStateEffect);
        e1.SetStateEffectType(ComVal.stateEffectType_ChangeAttack);
        e1.SetTarget(card);
        e1.SetStateEffectVal(4000);
        e1.SetResetCode(ComVal.resetEvent_LeaveEndPhase, 2);
        duel.ResignEffect(e1, card, card.controller);
        duel.FinishHandle();
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return card.XyzMaterialNum() >=2;
    }
}
