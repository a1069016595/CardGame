/// <summary>
/// 元素英雄 绝对零度侠 
/// </summary>
public class C40854197 : ICardScripts
{
    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        card.SetMaterialFilter2(Fiter1,Fiter2);

        StateEffect e1 = new StateEffect();
        e1.SetRangeArea(ComVal.Area_Monster);
        e1.SetCardEffectType(ComVal.cardEffectType_normalStateEffect | ComVal.cardEffectType_Single | ComVal.cardEffectType_unableReset);
        e1.SetCategory(ComVal.category_stateEffect);
        e1.SetTarget(card);
        e1.SetStateEffectType(ComVal.stateEffectType_addAfkVal);
        e1.SetGetStateEffectVal(GetAddAfkVal);
        duel.ResignEffect(e1, card, player);

        LauchEffect e2 = new LauchEffect();
        e2.SetCategory(ComVal.category_destroy);
        e2.SetCode(ComVal.code_LeaveField);
        e2.SetCardEffectType(ComVal.cardEffectType_mustLauch);
        e2.SetCheckLauch(CheckLauch);
        e2.SetOperation(Operation);
        duel.ResignEffect(e2, card, player);
    }

    private bool Fiter1(Card c)
    {
        return c.ContainName(ComStr.KeyWord_Hero);
    }

    private bool Fiter2(Card c)
    {
        return c.GetCurAttribute().IsBind(ComVal.CardAttr_Water);
    }




    private int GetAddAfkVal(IDuel duel, Card card, StateEffect e)
    {
        return duel.GetIncludeNameCardNumFromArea("", true, null, ComVal.cardType_Monster, ComVal.Area_Monster, filter, true, card) * 500;
    }

    private bool filter(Card card)
    {
        return (card.GetCurAttribute() & ComVal.CardAttr_Water) == 0 ? false : true;
    }


    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group g = duel.GetIncludeNameCardFromArea("", false, duel.GetOpsitePlayer(card.controller), ComVal.cardType_Monster, ComVal.Area_Monster);
        duel.AddFinishHandle();
        duel.SendToGraveyard(ComVal.Area_Monster, g, card, ComVal.reason_EffectDestroy, effect);
    }



    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return code.group.ContainCard(card) && card.previousArea.IsBind(ComVal.Area_Field);
    }
}
