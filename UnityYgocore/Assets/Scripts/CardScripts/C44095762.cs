using UnityEngine;
/// <summary>
/// 神圣防护罩 
/// </summary>
public class C44095762 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetLauchArea(ComVal.Area_Trap);
        e1.SetCategory(ComVal.category_destroy);
        e1.SetCode(ComVal.code_AttackDeclaration);
        e1.SetCardEffectType(ComVal.cardEffectType_chooseLauch);
        e1.SetCheckLauch(CheckLauch);
        e1.SetOperation(Operation);
        duel.ResignEffect(e1, card, player);
    }


    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group g = duel.GetIncludeNameCardFromArea("", false, card.opponentPlayer, ComVal.cardType_Monster, ComVal.Area_Monster, filter);
        duel.AddFinishHandle();
        duel.SendToGraveyard(ComVal.Area_Monster, g, card, ComVal.reason_EffectDestroy, effect);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        if(duel.GetCurAttackEvent().Attacker.controller==card.controller)
        {
            return false;
        }
        return duel.GetIncludeNameCardNumFromArea("", false, card.opponentPlayer, ComVal.cardType_Monster, ComVal.Area_Monster, filter) > 0;
    }

    private bool filter(Card card)
    {
        return card.curPlaseState == ComVal.CardPutType_UpRightFront;
    }
}
