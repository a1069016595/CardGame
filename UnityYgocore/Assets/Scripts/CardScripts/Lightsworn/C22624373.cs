using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 光道魔术师 丽拉
/// </summary>
public class C22624373 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e1.SetCode(ComVal.code_NoCode);
        e1.SetCategory(ComVal.category_destroy);
        e1.SetLauchArea(ComVal.Area_Monster);
        e1.SetOperation(Operation);
        e1.SetCheckLauch(CheckLauch);
        duel.ResignEffect(e1,card,player);

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
        duel.DiscardFromDeck(3, card, effect, card.controller);
    }

    private bool CheckLauch1(IDuel duel, Card card, LauchEffect effect, Code code)
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
        Group g = duel.GetIncludeNameCardFromArea("", false, duel.GetOpsitePlayer(card.controller), 0, ComVal.Area_Trap);

        GroupCardSelectBack callBack = delegate(Group val)
        {
            duel.AddFinishHandle();
            duel.SendToGraveyard(ComVal.Area_Trap, val, card, ComVal.reason_EffectDestroy, effect);
        };
        duel.SelectCardFromGroup(g, callBack, 1, card.controller);
        duel.ChangeMonsterType(ComVal.CardPutType_layFront, card);

        StateEffect e1 = new StateEffect();
        e1.SetRangeArea(ComVal.Area_Monster);
        e1.SetCategory(ComVal.category_stateEffect | ComVal.category_limitTime);
        e1.SetCardEffectType(ComVal.cardEffectType_Single | ComVal.cardEffectType_normalStateEffect);
        e1.SetStateEffectType(ComVal.stateEffectType_unableChangeType);
        e1.SetTarget(card);
        e1.SetResetCode(ComVal.resetEvent_LeaveEndPhase, 3);
        duel.ResignEffect(e1, card, card.controller);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        Group g=duel.GetOpsitePlayer(card.controller).group_TrapCard.GetGroup();
        return card.curPlaseState == ComVal.CardPutType_UpRightFront && g.GroupNum > 0;
    }
}
