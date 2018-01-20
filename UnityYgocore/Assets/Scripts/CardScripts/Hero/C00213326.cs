
/// <summary>
/// E紧急呼唤
/// </summary>
public class C00213326 : ICardScripts
{
    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_toHand | ComVal.category_search);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCode(ComVal.code_NoCode);
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e1.SetOperation(Operation);
        e1.SetLauchArea(ComVal.Area_Hand | ComVal.Area_Trap);
        duel.ResignEffect(e1, card, player);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect,  Group group = null)
    {
        Group a = duel.GetIncludeNameCardFromArea("英雄",false, effect.ownerCard.ownerPlayer, ComVal.cardType_Monster,ComVal.Area_MainDeck);
        GroupCardSelectBack CallBack = delegate(Group theGroup)
        {
            Card targetCard = theGroup.GetCard(0);
            duel.AddFinishHandle();
            duel.AddCardToHandFromMainDeck(targetCard, effect.ownerCard.ownerPlayer, effect.ownerCard, effect);
           
        };
        duel.SelectCardFromGroup(a, CallBack, 1,card.controller);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        int a = duel.GetIncludeNameCardNumFromArea("英雄", false, card.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_MainDeck);
        return a == 0 ? false : true;
    }
}
