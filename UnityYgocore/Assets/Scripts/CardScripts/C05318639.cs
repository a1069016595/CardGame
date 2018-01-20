using UnityEngine;
/// <summary>
/// 旋风
/// </summary>
public class C05318639 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_destroy);
        e1.SetCheckLauch(CheckLauch);
        e1.SetGetTarget(GetTarget);
        e1.SetOperation(Operation);
        e1.SetCardEffectType(ComVal.cardEffectType_chooseLauch | ComVal.cardEffectType_normalLauch);
        e1.SetCode(ComVal.code_FreeCode);
        e1.SetLauchArea(ComVal.Area_Trap | ComVal.Area_Hand);
        duel.ResignEffect(e1, card, player);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        duel.AddFinishHandle();
        duel.SendToGraveyard(ComVal.Area_Trap, group, card, ComVal.reason_EffectDestroy, effect);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        int a = duel.GetIncludeNameCardNumFromArea("", true, null, 0, ComVal.Area_Trap, Filter, true, card, null);
        return a > 0;
    }

    public void GetTarget(IDuel duel, Card card, LauchEffect effect, GroupCardSelectBack dele)
    {
        Group a = duel.GetIncludeNameCardFromArea("", true, null, 0, ComVal.Area_Trap, Filter, true, card, null);
        duel.SelectCardFromGroup(a, dele, 1, card.controller);
    }

    bool Filter(Card card)
    {
        return card.CanDestroy();
    }
}
