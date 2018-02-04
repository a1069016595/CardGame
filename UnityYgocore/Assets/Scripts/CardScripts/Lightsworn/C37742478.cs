using UnityEngine;

/// <summary>
/// 欧尼斯特
/// </summary>
public class C37742478 : ICardScripts
{
    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_addAttackVal);
        e1.SetCode(ComVal.code_BeforeReckonAttack);
        e1.SetLauchPhase(ComVal.Phase_Battlephase);
        e1.SetLauchArea(ComVal.Area_Hand);
        e1.SetCardEffectType(ComVal.cardEffectType_chooseLauch);
        e1.SetCost(Cost);
        e1.SetCheckLauch(CheckLauch);
        e1.SetOperation(Operation);
        duel.ResignEffect(e1, card, player);

        LauchEffect e2 = new LauchEffect();
        e2.SetCategory(ComVal.category_toHand);
        e2.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e2.SetCode(ComVal.code_NoCode);
        e2.SetLauchPhase(ComVal.Phase_Mainphase);
        e2.SetLauchArea(ComVal.Area_Monster);
        e2.SetCheckLauch(CheckLauch1);
        e2.SetOperation(Operation1);
        duel.ResignEffect(e2, card, player);
    }

    private void Operation1(IDuel duel, Card card, LauchEffect effect, Group target = null)
    {
        duel.AddFinishHandle();
        duel.AddCardToHandFromArea(ComVal.Area_Monster, card, card.ownerPlayer, card, effect);
    }

    private bool CheckLauch1(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return card.IsFaceUp();
    }

    private void Cost(IDuel duel, Card card, LauchEffect effect)
    {
        duel.AddFinishHandle();
        duel.SendToGraveyard(ComVal.Area_Hand, card.ToGroup(), card, ComVal.reason_Cost, effect);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        AttackEvent e = duel.GetCurAttackEvent();
        StateEffect e1 = new StateEffect();
        e1.SetRangeArea(ComVal.Area_Monster);
        e1.SetCategory(ComVal.category_stateEffect | ComVal.category_limitTime);
        e1.SetCardEffectType(ComVal.cardEffectType_Single | ComVal.cardEffectType_normalStateEffect);
        e1.SetStateEffectType(ComVal.stateEffectType_addAfkVal);

        if (e.Attacker.controller.isMy)
        {
            e1.SetTarget(e.Attacker);
            e1.SetStateEffectVal(e.Attackeder.GetCurAfk());
        }
        else
        {
            e1.SetTarget(e.Attackeder);
            e1.SetStateEffectVal(e.Attacker.GetCurAfk());
        }

        e1.SetResetCode(ComVal.resetEvent_LeaveEndPhase, 1);
        duel.ResignEffect(e1, card, card.controller);
        duel.FinishHandle();
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        if (duel.GetCurPhase() == ComVal.Phase_Battlephase)
        {
            AttackEvent e = duel.GetCurAttackEvent();
            if (e != null && !e.IsInvalid())
            {
                if (!e.IsDirectAttack())
                {
                    if (e.Attacker.controller == card.controller && e.Attacker.GetCurAttribute().IsBind(ComVal.CardAttr_Light))
                    {
                        return true;
                    }
                    if (e.Attackeder.controller.isMy && e.Attackeder.GetCurAttribute().IsBind(ComVal.CardAttr_Light))
                    {
                        return true;
                    }
                }

            }
        }
        return false;
    }
}
