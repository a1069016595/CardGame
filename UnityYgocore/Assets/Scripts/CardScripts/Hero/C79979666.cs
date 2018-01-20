/// <summary>
/// 水泡侠
/// </summary>
public class C79979666 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        SpSummonEffect e1 = new SpSummonEffect();
        e1.SetCheckLauch(CheckLauch);
        e1.SetPutType(0);
        duel.ResignEffect(e1, card, player);

        LauchEffect e2 = new LauchEffect();
        e1.SetCategory(ComVal.category_drawCard);
        e2.SetCardEffectType(ComVal.cardEffectType_mustToChooseLauch);
        e2.SetCode(ComVal.code_SpecialSummon);
        e2.SetLauchArea(ComVal.Area_Monster);
        e2.SetCheckLauch(CheckLauch1);
        e2.SetOperation(Operation1);
        duel.ResignEffect(e2, card, player);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        normalDele norDele = delegate
        {
            duel.FinishHandle();
        };
        duel.SpeicalSummon(ComVal.Area_Hand,card, card.ownerPlayer, card, ComVal.reason_Effect, effect, 0, norDele);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        int a = duel.GetIncludeNameCardNumFromArea("", false, card.ownerPlayer, 0, ComVal.Area_Hand);
        return a == 1;
    }

    public void Operation1(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        duel.DrawCard(card.ownerPlayer, 2, card, effect);
        duel.FinishHandle();
    }
    public bool CheckLauch1(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        if (code.group.ContainCard(card))
        {
            int g = duel.GetIncludeNameCardNumFromArea("", false, card.ownerPlayer, 0, ComVal.Area_Hand, null,true, card, null);
            int g1 = duel.GetIncludeNameCardNumFromArea("", false, card.ownerPlayer, 0, ComVal.Area_Monster, null, true, card, null);
            return g == 0 && g1 == 0;
        }
        return false;
    }
}
