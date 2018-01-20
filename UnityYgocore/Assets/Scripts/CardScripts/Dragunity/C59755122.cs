using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 龙骑兵团-方阵龙
/// </summary>
public class C59755122 : ICardScripts
{


    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_spSummon);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e1.SetCode(ComVal.code_NoCode);
        e1.SetLauchArea(ComVal.Area_Trap);
        e1.SetOperation(Operation);

        duel.ResignEffect(e1, card, player);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        if (card.curArea.IsBind(ComVal.Area_Trap))
        {
            normalDele d = delegate
            {
                duel.FinishHandle();
            };
            duel.SpeicalSummon(ComVal.Area_NormalTrap, card, card.controller, card, ComVal.reason_Effect, effect, 0, d);
        }
        else
        {
            duel.FinishHandle();
        }
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return card.IsEquipCard();
    }
}
