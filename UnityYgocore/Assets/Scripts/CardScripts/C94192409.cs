
/// <summary>
/// 強制脱出装置
/// </summary>
public class C94192409 : ICardScripts {


    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetLauchArea(ComVal.Area_Trap);
        e1.SetCategory(ComVal.category_toHand);
        e1.SetCode(ComVal.code_FreeCode);
        e1.SetCardEffectType(ComVal.cardEffectType_chooseLauch|ComVal.cardEffectType_normalLauch);
        e1.SetCheckLauch(CheckLauch);
        e1.SetOperation(Operation);
        e1.SetGetTarget(GetTarget);
        duel.ResignEffect(e1, card, player);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Card target = group.GetCard(0);
        duel.AddFinishHandle();
        duel.AddCardToHandFromArea(ComVal.Area_Monster, target, target.ownerPlayer, card, effect);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        int num = duel.GetIncludeNameCardNumFromArea("", true, null, ComVal.cardType_Monster, ComVal.Area_Monster);
        return num > 0;
    }

    public void GetTarget(IDuel duel, Card card, LauchEffect effect, GroupCardSelectBack dele)
    {
        Group g = duel.GetIncludeNameCardFromArea("", true, null, ComVal.cardType_Monster, ComVal.Area_Monster);
        duel.SelectCardFromGroup(g, dele, 1,card.ownerPlayer);
    }

}
