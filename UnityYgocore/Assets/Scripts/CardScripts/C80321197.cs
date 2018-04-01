using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 深红剑士
/// </summary>
public class C80321197 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        card.SetMaterialFilter2(F1, F2);

        LauchEffect e1 = new LauchEffect();
        e1.SetCardEffectType(ComVal.cardEffectType_mustLauch);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCode(ComVal.code_ToGraveyard);
        e1.SetLauchArea(ComVal.Area_Monster);
        e1.SetOperation(Operation);
        duel.ResignEffect(e1, card, player);
    }

    private bool F1(Card card)
    {
        return card.IsAdjust();
    }

    private bool F2(Card card)
    {
        return card.IsAdjustOutSide();
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        LimitPlayerEffect e1 = new LimitPlayerEffect();
        e1.SetCategory(ComVal.category_limitEffect | ComVal.category_limitTime);
        e1.SetLimitEffectType(ComVal.limitEffectType_spSummonLimit);
        e1.SetResetCode(ComVal.resetEvent_LeaveEndPhase, 1);
        e1.SetTargetType(TargetPlayerType.other);
        e1.SetLimitEffectHandler(LimitEffectHandler);
        duel.ResignEffect(e1, card, card.controller);

        duel.FinishHandle();
    }

    private object LimitEffectHandler(IDuel duel, Card c, LimitPlayerEffect e)
    {
         if(duel.IsPlayerRound(duel.GetOpsitePlayer( e.ownerCard.controller)))
         {
             if(c.GetCurLevel()>=5)
             {
                 return false;
             }
         }
         return true;
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        if(code.reason.reason.IsBind(ComVal.reason_AttackDestroy))
        {
            AttackEvent e = duel.GetCurAttackEvent();
           if( e.Attacker==card)
           {
               return true;
           }
        }
        return false;
    }
}
