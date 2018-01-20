using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 红莲魔龙
/// </summary>
public class C70902743 : ICardScripts {


    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        card.SetMaterialFilter2(F1, F2);
    }

    private bool F1(Card card)
    {
        return card.IsAdjust();
    }

    private bool F2(Card card)
    {
        return card.IsAdjustOutSide();
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
       
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return true;
    }
}
