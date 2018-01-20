using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 二重电光
/// </summary>
public class C33846209 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_destroy | ComVal.category_drawCard);
        e1.SetCardEffectType(ComVal.cardEffectType_chooseLauch);
        e1.SetCheckLauch(CheckLauch);
        e1.SetGetTarget(GetTarget);
        e1.SetCost(Cost);
        e1.SetCardEffectType(ComVal.cardEffectType_chooseLauch | ComVal.cardEffectType_normalLauch);
        e1.SetCode(ComVal.code_FreeCode);
        e1.SetLauchArea(ComVal.Area_Trap | ComVal.Area_Hand);
        e1.SetOperation(Operation);
        duel.ResignEffect(e1, card, player);
    }

    private void Cost(IDuel duel, Card card, LauchEffect effect)
    {
        Group g = duel.GetIncludeNameCardFromArea("", false, card.controller, 0, ComVal.Area_Monster);
        g = g.GetFitlerGroup(Fiter);

        GroupCardSelectBack callBack = delegate(Group _g)
        {
            duel.SendToGraveyard(ComVal.Area_Monster, _g, card, ComVal.reason_Realease, effect);
        };

        duel.SelectCardFromGroup(g, callBack, 1, card.controller);
    }

    private void GetTarget(IDuel duel, Card card, LauchEffect effect, GroupCardSelectBack dele)
    {
        Group g = duel.GetIncludeNameCardFromArea("", true, null, 0, ComVal.Area_Field,null,true,card);
        duel.SelectCardFromGroup(g, dele, 1, card.controller);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        if (!card.CanDestroy())
        {
            duel.FinishHandle();
            return;
        }
        normalDele d = delegate
        {
            duel.AddFinishHandle();
            duel.DrawCard(card.controller, 1, card, effect);
        };
        duel.AddDelegate(d, true);
        duel.SendToGraveyard(ComVal.Area_Field, group, card, ComVal.reason_Destroy, effect);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        Group g = duel.GetIncludeNameCardFromArea("", false, card.controller, 0, ComVal.Area_Monster);
        return duel.GetIncludeNameCardFromArea("", true, null, 0, ComVal.Area_Field,null,true,card).GroupNum > 1 && g.GetFitlerGroup(Fiter).GroupNum > 0;
    }

    private bool Fiter(Card card)
    {
        return card.cardType.IsBind(ComVal.CardType_Monster_Double) && card.IsFaceUp() && card.GetCurLevel() == 4;
    }
}
