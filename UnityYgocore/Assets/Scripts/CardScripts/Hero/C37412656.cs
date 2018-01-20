using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 英雄爆破
/// </summary>
public class C37412656 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetLauchArea(ComVal.Area_Trap);
        e1.SetCategory(ComVal.category_toHand);
        e1.SetCode(ComVal.code_FreeCode);
        e1.SetCardEffectType(ComVal.cardEffectType_chooseLauch | ComVal.cardEffectType_normalLauch);
        e1.SetCheckLauch(CheckLauch);
        e1.SetOperation(Operation);
        e1.SetGetTarget(GetTarget);
        duel.ResignEffect(e1, card, player);
    }

    private void GetTarget(IDuel duel, Card card, LauchEffect effect, GroupCardSelectBack dele)
    {
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_ElementalHERO, false, card.controller, ComVal.CardType_Monster_Double | ComVal.CardType_Monster_Normal,
                                                ComVal.Area_Graveyard);
        duel.SelectCardFromGroup(g, dele, 1, card.controller);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Card c = group.GetCard(0);
        if (!c.curArea.IsBind(ComVal.Area_Graveyard))
        {
            duel.FinishHandle();
            return;
        }

        normalDele d = delegate
        {
            Group g = duel.GetIncludeNameCardFromArea("", false, duel.GetOpsitePlayer(card.controller), ComVal.cardType_Monster,
                                               ComVal.Area_Monster);
            Group targetG = new Group();
            for (int i = 0; i < g.cardList.Count; i++)
            {
                if (g.cardList[i].GetCurAfk() <= c.GetBaseAfk())
                {
                    targetG.AddCard(g.cardList[i]);
                }
            }
            if(targetG.GroupNum==0)
            {
                duel.FinishHandle();
                return;
            }
          
            GroupCardSelectBack callBack = delegate(Group val)
            {
                duel.AddFinishHandle();
                duel.SendToGraveyard(ComVal.Area_Monster, val, card, ComVal.reason_EffectDestroy, effect);
            };

            duel.SelectCardFromGroup(targetG, callBack, 1, card.controller);
        };
        duel.AddDelegate(d, true);
        duel.AddCardToHandFromArea(ComVal.Area_Graveyard, c, card.controller, card, effect);
    }


    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_ElementalHERO, false, card.controller, ComVal.CardType_Monster_Double | ComVal.CardType_Monster_Normal,
                                               ComVal.Area_Graveyard);
        return g.GroupNum > 0;
    }
}
