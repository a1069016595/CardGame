using UnityEngine;
using System.Collections;
/// <summary>
/// 奈落的落穴
/// </summary>
public class C29401950 : ICardScripts
{


    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetLauchArea(ComVal.Area_Trap);
        e1.SetCategory(ComVal.category_destroy | ComVal.category_remove);
        e1.SetCode(ComVal.code_NormalSummon | ComVal.code_SpecialSummon | ComVal.code_TurnBackSummon);
        e1.SetCardEffectType(ComVal.cardEffectType_mustToChooseLauch);
        e1.SetOperation(Operation);
        e1.SetCheckLauch(CheckLauch);
        duel.ResignEffect(e1, card, player);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group g = new Group();
        Code code = duel.GetCurCode();
        g = code.group.GetFitlerGroup(filter);
        if (g.GroupNum != 0)
        {
            duel.AddFinishHandle();
            duel.SendToRemove(ComVal.Area_Monster, g, card, ComVal.reason_Effect, effect);
        }
        else
        {
            duel.FinishHandle();
        }
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        Group g = code.group.GetFitlerGroup(filter);

        return g.GetPlayerGroup(duel.GetOpsitePlayer(card.controller)).GroupNum > 0;
    }

    bool filter(Card card)
    {
        return card.IsFaceUp() && card.GetCurAfk() > 1500 && card.CanDestroy() && card.curArea == ComVal.Area_Monster;
    }
}
