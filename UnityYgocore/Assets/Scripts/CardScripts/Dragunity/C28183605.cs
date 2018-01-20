using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 龙骑兵团-指挥官
/// </summary>
public class C28183605 : ICardScripts
{


    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_equipCard);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCardEffectType(ComVal.cardEffectType_chooseLauch);
        e1.SetCode(ComVal.code_NormalSummon);
        e1.SetLauchArea(ComVal.Area_Monster);
        e1.SetOperation(Operation);
        e1.SetGetTarget(GetTarget);

        duel.ResignEffect(e1, card, player);

        StateEffect e2 = new StateEffect();
        e2.SetRangeArea(ComVal.Area_Monster);
        e2.SetCardEffectType(ComVal.cardEffectType_normalStateEffect | ComVal.cardEffectType_Single | ComVal.cardEffectType_unableReset);
        e2.SetCategory(ComVal.category_stateEffect);
        e2.SetTarget(card);
        e2.SetStateEffectType(ComVal.stateEffectType_addAfkVal);
        e2.SetGetStateEffectVal(GetAddAfkVal);
        duel.ResignEffect(e2, card, player);
    }

    private int GetAddAfkVal(IDuel duel, Card card, StateEffect e)
    {
       return duel.GetIncludeNameCardFromArea(ComStr.KeyWord_DragUnity, false, card.controller, 0, ComVal.Area_Field).GroupNum*200;
    }

    private void GetTarget(IDuel duel, Card card, LauchEffect effect, GroupCardSelectBack dele)
    {
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_DragUnity, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Graveyard, Fiter);
        duel.SelectCardFromGroup(g, dele, 1, card.controller);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Card c=group.GetFirstCard();
        if(!card.curArea.IsBind(ComVal.Area_Monster)||!c.curArea.IsBind(ComVal.Area_Graveyard))
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
        duel.EquipCardFromArea(ComVal.Area_Graveyard, c, card.controller,card, effect);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_DragUnity, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Graveyard, Fiter);
        return code.group.ContainCard(card)&& g.GroupNum > 0;
    }

    private bool Fiter(Card card)
    {
        return card.GetCurLevel() <= 3 && card.race == ComVal.CardRace_Dragon;
    }
}
