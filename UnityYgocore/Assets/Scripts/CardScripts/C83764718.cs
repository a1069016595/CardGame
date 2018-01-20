
using UnityEngine;
/// <summary>
/// 死者苏生
/// </summary>
public class C83764718 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_spSummon);
        e1.SetCode(ComVal.code_NoCode);
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e1.SetLauchArea(ComVal.Area_Trap | ComVal.Area_Hand);
        e1.SetCheckLauch(CheckLauch);
        e1.SetOperation(Operation);
        e1.SetGetTarget(GetTarget);
        e1.SetLauchPhase(ComVal.Phase_Mainphase);
        duel.ResignEffect(e1, card, player);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Card target = group.GetCard(0);
        if (target.curArea != ComVal.Area_Graveyard)
            return;
        normalDele dele = delegate
        {
            duel.FinishHandle();
        };
        target.controller = effect.ownerCard.controller;
        duel.SpeicalSummon(ComVal.Area_Graveyard, target, effect.ownerCard.ownerPlayer, effect.ownerCard, ComVal.reason_Effect, effect, 0, dele);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        int a = duel.GetIncludeNameCardNumFromArea("", true, null, ComVal.cardType_Monster, ComVal.Area_Graveyard);
        return a != 0 && !duel.MonsterAreaIsFull(card.ownerPlayer);
    }

    public void GetTarget(IDuel duel, Card card, LauchEffect effect, GroupCardSelectBack dele)
    {
        Group a = duel.GetIncludeNameCardFromArea("", true, null, ComVal.cardType_Monster, ComVal.Area_Graveyard,filer,false,null,null);
        duel.SelectCardFromGroup(a, dele, 1,card.ownerPlayer);
    }

    bool filer(Card card)
    {
        return card.CanSpSummon();
    }
}

