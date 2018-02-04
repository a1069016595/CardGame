
using UnityEngine;

/// <summary>
/// 光道暗杀者 莱登
/// </summary>
public class C77558536 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_disCard);
        e1.SetLauchArea(ComVal.Area_Monster);
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e1.SetLauchPhase(ComVal.Phase_Mainphase);
        e1.SetOperation(Operation);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCode(ComVal.code_NoCode);
        card.SetCardCountLimit( e1 , 1);
        duel.ResignEffect(e1, card, player);

        LauchEffect e2 = new LauchEffect();
        e2.SetCategory(ComVal.category_disCard);
        e2.SetCheckLauch(CheckLauch1);
        e2.SetOperation(Operation2);
        e2.SetCode(ComVal.code_EnterEndPhase);
        e2.SetCardEffectType(ComVal.cardEffectType_mustLauch);
        e2.SetLauchArea(ComVal.Area_Monster);
        duel.ResignEffect(e2, card, player);
    }

    private void Operation2(IDuel duel, Card card, LauchEffect effect, Group target = null)
    {
        duel.AddFinishHandle();
        duel.DiscardFromDeck(2, card, effect, card.controller);
    }

    private bool CheckLauch1(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        if (card.controller.group_MainDeck.GroupNum > 0&&duel.IsPlayerRound(card.controller))
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
        Group g = new Group();
        normalDele d = delegate
        {
            g = g.GetFitlerGroup(fiter);
            if (g.GroupNum > 0)
            {
                StateEffect e1 = new StateEffect();
                e1.SetRangeArea(ComVal.Area_Monster);
                e1.SetCategory(ComVal.category_stateEffect | ComVal.category_limitTime);
                e1.SetCardEffectType(ComVal.cardEffectType_Single | ComVal.cardEffectType_normalStateEffect);
                e1.SetStateEffectType(ComVal.stateEffectType_addAfkVal);
                e1.SetTarget(card);
                e1.SetStateEffectVal(200);
                e1.SetResetCode(ComVal.resetEvent_LeaveEndPhase, 2);
                duel.ResignEffect(e1, card, card.controller);
            }
            duel.FinishHandle();
        };
        duel.AddDelegate(d, true);
        g = duel.DiscardFromDeck(2, card, effect, card.controller);
    }

    private bool fiter(Card card)
    {
        return card.IsMonster() && card.ContainName(ComStr.KeyWord_Lightsworn);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        if (card.controller.group_MainDeck.GroupNum < 2)
        {
            return false;
        }
        return true;
    }
}
