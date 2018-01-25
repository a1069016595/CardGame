using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 六武众的隐退者
/// </summary>
public class C61737116 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        SpSummonEffect e1 = new SpSummonEffect();
        e1.SetPutType(0);
        e1.SetCheckLauch(CheckLauch);
        duel.ResignEffect(e1, card, player);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {

    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return duel.GetIncludeNameCardNumFromArea("", false, card.controller, ComVal.cardType_Monster, ComVal.Area_Monster) == 0
            && duel.GetIncludeNameCardNumFromArea("", false, duel.GetOpsitePlayer(card.controller), ComVal.cardType_Monster, ComVal.Area_Monster) > 0;
    }
}
