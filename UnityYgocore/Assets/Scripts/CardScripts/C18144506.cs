using UnityEngine;
/// <summary>
/// 鹰身女妖的羽毛扫
/// </summary>
public class C18144506 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_destroy);
        e1.SetCode(ComVal.code_NoCode);
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e1.SetLauchArea(ComVal.Area_Trap | ComVal.Area_Hand);
        e1.SetOperation(Operation);
        e1.SetCheckLauch(CheckLauch);
        duel.ResignEffect(e1, card, player);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group a = duel.GetIncludeNameCardFromArea("", false, card.opponentPlayer, 0, ComVal.Area_Trap, filer, false, null, null);
        duel.AddFinishHandle();
        duel.SendToGraveyard(ComVal.Area_Trap, a, card, ComVal.reason_EffectDestroy, effect);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        int a = duel.GetIncludeNameCardNumFromArea("", false, card.opponentPlayer, 0, ComVal.Area_Trap, filer, false, null, null);
        return a > 0;
    }

    bool filer(Card card)
    {
        return card.CanDestroy();
    }
}