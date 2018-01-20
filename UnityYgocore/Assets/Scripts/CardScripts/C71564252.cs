using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 雷王
/// </summary>
public class C71564252 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LimitPlayerEffect e1 = new LimitPlayerEffect();
        e1.SetCategory(ComVal.category_limitEffect);
        e1.SetTargetType(TargetPlayerType.both);
        e1.SetLimitEffectType(ComVal.limitEffectType_unableSearchCardFromMainDeck);
        e1.SetCondtion(condition);
        duel.ResignEffect(e1, card, player);

        LauchEffect e2 = new LauchEffect();
        e2.SetCategory(ComVal.category_disAbleSpSummon);
        e2.SetCheckLauch(CheckLauch);
        e2.SetCode(ComVal.code_SpDeclaration);
        e2.SetCost(Cost);
        e2.SetOperation(Operation);
        e2.SetCardEffectType(ComVal.cardEffectType_mustToChooseLauch);
        e2.SetLauchArea(ComVal.Area_Monster);
        duel.ResignEffect(e2, card, player);
    }

    private void Cost(IDuel duel, Card card, LauchEffect effect)
    {
        duel.AddFinishHandle();
        duel.SendToGraveyard(ComVal.Area_Field, card.ToGroup(), card, ComVal.reason_Cost, effect);
    }

    private bool condition(IDuel duel, Card card, BaseEffect e, Card targetCard)
    {
        return e.ownerCard.IsFaceUpInMonsterArea();
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        duel.NegateSummon(card, effect);
        duel.FinishHandle();
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return card.ownerPlayer.CanSendToGraveyard() && !duel.GetCurChain().isHandle;
    }
}
