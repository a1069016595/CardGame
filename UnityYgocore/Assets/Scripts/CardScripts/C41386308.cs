
/// <summary>
/// 数学家
/// </summary>
public class C41386308 : ICardScripts
{
    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_toGrave);
        e1.SetCardEffectType(ComVal.cardEffectType_chooseLauch);
        e1.SetCode(ComVal.code_NormalSummon);
        e1.SetCheckLauch(CheckLauch);
        e1.SetOperation(Operation);
        duel.ResignEffect(e1, card, player);
    }

    public bool CheckLauch(IDuel duel, Card theCard, LauchEffect effect, Code code)
    {
        if (!code.group.ContainCard(theCard))
        {
            return false;
        }
        int a = duel.GetIncludeNameCardNumFromArea("", false, theCard.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_MainDeck);
        return a != 0;
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group a = duel.GetIncludeNameCardFromArea("", false, effect.ownerCard.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_MainDeck);

        GroupCardSelectBack CallBack = delegate(Group theGroup)
        {
            Card targetCard = theGroup.GetCard(0);
            duel.AddFinishHandle();
            duel.SendToGraveyard(ComVal.Area_MainDeck, theGroup, effect.ownerCard, ComVal.reason_Effect, effect);
        };
        duel.SelectCardFromGroup(a, CallBack, 1, card.controller);
    }
}