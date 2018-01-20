using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 光道猎犬 雷光
/// </summary>
public class C21502796 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_destroy | ComVal.category_disCard);
        e1.SetCheckLauch(CheckLauch);
        e1.SetOperation(Operation);
        e1.SetCode(ComVal.code_TurnBack | ComVal.code_TurnBackSummon);
        e1.SetCardEffectType(ComVal.cardEffectType_mustLauch);
        e1.SetLauchArea(ComVal.Area_Monster);
        duel.ResignEffect(e1, card, player);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group g = duel.GetIncludeNameCardFromArea("", true, card.controller, 0, ComVal.Area_Field);
        normalDele d = delegate
        {
            duel.AddFinishHandle();
            duel.DiscardFromDeck(3, card, effect, card.controller);
        };

        GetMes d1 = delegate(bool val)
        {
            if (val)
            {
                GroupCardSelectBack d2 = delegate(Group target)
                {
                    duel.AddDelegate(d, true);
                    duel.SendToGraveyard(ComVal.Area_Field, target, card, ComVal.reason_EffectDestroy, effect);
                };
                duel.SelectCardFromGroup(g, d2, 1, card.controller);
            }
            else
            {
                d();
            }
        };
        if (g.GroupNum > 0)
        {
            duel.ShowDialogBox(card, d1, card.controller.isMy);
        }
        else
        {
            d();
        }
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        if (code.group.ContainCard(card))
        {
            return true;
        }
        return false;
    }
}
