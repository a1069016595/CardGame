using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 强欲而谦虚之壶
/// </summary>
public class C98645731 : ICardScripts
{
    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_search);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e1.SetCode(ComVal.code_NoCode);
        e1.SetLauchArea(ComVal.Area_Trap | ComVal.Area_Hand);
        e1.SetEffectID("98645731");
        e1.SetOperation(Operation);

        duel.ResignEffect(e1, card, player);
        duel.ResignEffectLauchLimitCounter(player, "98645731", 1);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        LimitPlayer(duel, card);

        Group g = card.controller.group_MainDeck.GetFirstAmountCard(3);
        if (g.GroupNum == 0)
        {
            duel.FinishHandle();
            return;
        }
        GroupCardSelectBack callBack = delegate(Group val)
        {
            duel.AddFinishHandle();
            duel.AddCardToHandFromMainDeck(val.GetCard(0), card.controller, card, effect);
        };
        duel.SelectCardFromGroup(g, callBack, 1, card.controller);
    }

    private static void LimitPlayer(IDuel duel, Card card)
    {
        LimitPlayerEffect e1 = new LimitPlayerEffect();
        e1.SetCategory(ComVal.category_limitEffect | ComVal.category_limitTime);
        e1.SetLimitEffectType(ComVal.limitEffectType_unableSpSummon);
        e1.SetResetCode(ComVal.resetEvent_LeaveEndPhase, 0);
        e1.SetTargetType(TargetPlayerType.my);
        duel.ResignEffect(e1, card, card.controller);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return card.controller.MainDeckNum > 3 && !card.controller.WhetherBeSpSummon();
    }
}
