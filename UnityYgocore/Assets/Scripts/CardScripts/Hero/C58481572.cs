using UnityEngine;
/// <summary>
/// 假面英雄 暗爪
/// </summary>
public class C58481572 : ICardScripts
{
    public void InitialEffect(Card card, Player player, IDuel duel)
    {

        LimitPlayerEffect e1 = e1 = new LimitPlayerEffect();
        e1.SetCategory(ComVal.category_limitEffect);
        e1.SetTargetType(TargetPlayerType.other);
        e1.SetLimitEffectType(ComVal.limitEffectType_sendToRemove);
        e1.SetCondtion(condition);
        duel.ResignEffect(e1, card, player);

        LauchEffect e2 = new LauchEffect();
        e2.SetCategory(ComVal.category_remove);
        e2.SetLauchArea(ComVal.Area_Monster);
        e2.SetCode(ComVal.code_AddCardToHand);
        e2.SetCardEffectType(ComVal.cardEffectType_mustToChooseLauch);
        e2.SetOperation(Operation);
        e2.SetCheckLauch(CheckLauch);
        duel.ResignEffect(e2, card, player);
        card.SetCardCountLimit(e2 , 1);
    }

    private bool condition(IDuel duel, Card card, BaseEffect e,Card targetCard)
    {
        card = e.ownerCard;
        return card.IsFaceUpInMonsterArea();
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        if (card.opponentPlayer.GetHandCardNum() == 0)
        {
            duel.FinishHandle();
            return;
        }
        GroupCardSelectBack callBack = delegate(Group g)
        {
            duel.AddFinishHandle();
            duel.SendToRemove(ComVal.Area_Hand, g, card, ComVal.reason_Effect, effect);
        };
        duel.SelectCardFromGroup(card.opponentPlayer.GetHandCardGroup(), callBack, 1, card.controller);
    }


    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        if (!(card.IsInField() && card.IsFaceUp()))
        {
            return false;
        }
        foreach (var item in code.group.cardList)
        {
            if (item.ownerPlayer == card.opponentPlayer && item.previousArea.IsBind(ComVal.Area_MainDeck))
            {
                
                return true;
            }
        }
        return false;
    }
}
