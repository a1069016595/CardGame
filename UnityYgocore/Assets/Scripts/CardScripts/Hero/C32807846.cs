
/// <summary>
/// 增援
/// </summary>
public class C32807846 : ICardScripts
{
    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_toHand | ComVal.category_search);
        e1.SetLauchArea(ComVal.Area_Trap | ComVal.Area_Hand);
        e1.SetOperation(Operation);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCode(ComVal.code_NoCode);
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        duel.ResignEffect(e1, card, player);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group a = duel.GetIncludeNameCardFromArea("", false, effect.ownerCard.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_MainDeck, filter, false, null, null);
        GroupCardSelectBack myDele = delegate(Group theGroup)
              {
                  Card target = theGroup.GetCard(0);
                  duel.AddFinishHandle();
                  duel.AddCardToHandFromMainDeck( target, effect.ownerCard.ownerPlayer, effect.ownerCard, effect);
              };
        duel.SelectCardFromGroup(a, myDele, 1, card.controller);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        Group a = duel.GetIncludeNameCardFromArea("", false, card.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_MainDeck, filter, false, null, null);
        return a.GroupNum > 0;
    }

    bool filter(Card card)
    {
        return ComVal.isBind(ComVal.CardRace_Warrior, card.race);
    }
}
