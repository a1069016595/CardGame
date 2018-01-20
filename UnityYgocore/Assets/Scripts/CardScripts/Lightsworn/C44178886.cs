using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 光道武僧 艾琳
/// </summary>
public class C44178886 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCode(ComVal.code_BeforeReckonAttack);
        e1.SetCardEffectType(ComVal.cardEffectType_mustLauch);
        e1.SetLauchArea(ComVal.Area_Monster);
        e1.SetLauchPhase(ComVal.Phase_Battlephase);
        e1.SetCheckLauch(CheckLauch);
        e1.SetOperation(Operation);
        duel.ResignEffect(e1, card, player);


        LauchEffect e2 = new LauchEffect();
        e2.SetCategory(ComVal.category_disCard);
        e2.SetCheckLauch(CheckLauch1);
        e2.SetOperation(Operation1);
        e2.SetCode(ComVal.code_EnterEndPhase);
        e2.SetCardEffectType(ComVal.cardEffectType_mustLauch);
        e2.SetLauchArea(ComVal.Area_Monster);
        duel.ResignEffect(e2, card, player);
    }

    public void Operation1(IDuel duel, Card card, LauchEffect effect, Group target = null)
    {
        duel.AddFinishHandle();
        duel.DiscardFromDeck(3, card, effect, card.controller);
    }

    public bool CheckLauch1(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        if (card.controller.group_MainDeck.GroupNum > 0 && duel.IsPlayerRound(card.controller))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        AttackEvent e = duel.GetCurAttackEvent();
        Card c = e.Attackeder;
        duel.AddFinishHandle();
        duel.SendToMainDeck(ComVal.Area_Monster, c.ToGroup(), card, ComVal.reason_Effect, effect);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        if (duel.GetCurPhase() == ComVal.Phase_Battlephase)
        {
            AttackEvent e = duel.GetCurAttackEvent();
            if (e != null && e.Attacker == card)
            {
                if (e.Attackeder != null && !e.Attackeder.IsAttack())
                {
                    return true;
                }
            }
        }
        return false;
    }
}
