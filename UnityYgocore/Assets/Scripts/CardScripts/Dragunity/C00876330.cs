using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 龙骑兵团武器-银槲剑
/// </summary>
public class C00876330 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        SpSummonEffect e1 = new SpSummonEffect();
        e1.SetPutType(0);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCost(Cost);
        duel.ResignEffect(e1, card, player);

        LauchEffect e2 = new LauchEffect();
        e2.SetCategory(ComVal.category_equipCard);
        e2.SetCheckLauch(CheckLauch1);
        e2.SetCardEffectType(ComVal.cardEffectType_chooseLauch);
        e2.SetCode(ComVal.code_NormalSummon | ComVal.code_SpecialSummon);
        e2.SetLauchArea(ComVal.Area_Monster);
        e2.SetOperation(Operation);
        e2.SetGetTarget(GetTarget);

        duel.ResignEffect(e2, card, player);
    }

    private bool CheckLauch1(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_DragUnity, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Graveyard, Fiter);
        return code.group.ContainCard(card) && g.GroupNum > 0 && card.previousArea == ComVal.Area_Hand;
    }

    private void GetTarget(IDuel duel, Card card, LauchEffect effect, GroupCardSelectBack dele)
    {
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_DragUnity, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Graveyard, Fiter);
        duel.SelectCardFromGroup(g, dele, 1, card.controller);
    }

    private bool Fiter(Card card)
    {
        return card.race == ComVal.CardRace_Dragon;
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Card c = group.GetFirstCard();
        if (!card.curArea.IsBind(ComVal.Area_Monster) || !c.curArea.IsBind(ComVal.Area_Graveyard))
        {
            duel.FinishHandle();
            return;
        }
        normalDele d1 = delegate
        {
            StateEffect e1 = new StateEffect();
            e1.SetCardEffectType(ComVal.cardEffectType_equip | ComVal.cardEffectType_Single);
            e1.SetCategory(ComVal.category_equipCard);
            e1.SetEquipCard(card, c);
            e1.SetRangeArea(ComVal.Area_Trap);
            duel.ResignEffect(e1, c, card.controller);

            duel.FinishHandle();
        };
        duel.AddDelegate(d1, true);
        duel.EquipCardFromArea(ComVal.Area_Graveyard, c, card.controller, card, effect);
    }


    private void Cost(IDuel duel, Card card, LauchEffect effect)
    {
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_DragUnity, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Field);

        GroupCardSelectBack callBack=delegate(Group val)
        {
            duel.AddFinishHandle();
            duel.SendToGraveyard(ComVal.Area_Field,val,card,ComVal.reason_Effect,effect);
        };
        duel.SelectCardFromGroup(g, callBack, 1, card.controller);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return duel.GetIncludeNameCardNumFromArea(ComStr.KeyWord_DragUnity, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Field) > 0;
    }
}
