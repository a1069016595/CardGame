using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 诸刃的活人剑术
/// </summary>
public class C21007444 : ICardScripts
{


    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch | ComVal.cardEffectType_chooseLauch);
        e1.SetCategory(ComVal.category_revived);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCode(ComVal.code_FreeCode);
        e1.SetGetTarget(GetTarget);
        e1.SetLauchArea(ComVal.Area_NormalTrap);
        e1.SetOperation(Operation);
        duel.ResignEffect(e1, card, player);
    }

    private void GetTarget(IDuel duel, Card card, LauchEffect effect, GroupCardSelectBack dele)
    {
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_SixSamurai, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Graveyard, Fiter);
        duel.SelectCardFromGroup(g, dele, 2, card.controller);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        GroupCardSelectBack d = delegate(Group val)
        {
            card.EffectDataGroup = val;

            normalDele d1 = delegate
            {
                Group result = new Group();
                normalDele d2 = delegate
                {
                    List<Card> list = result.ToList();
                    int reduceLP = 0;
                    foreach (var item in list)
                    {
                        reduceLP += item.GetCurAfk();
                    }
                    duel.ReduceLP(reduceLP, card.controller, ComVal.reason_Effect, card, effect);
                    duel.FinishHandle();
                };
                duel.AddDelegate(d2, true);
                Group target = card.EffectDataGroup.GetFitlerGroup(Fiter1);
                result = duel.SendToGraveyard(ComVal.Area_Monster, target, card, ComVal.reason_EffectDestroy, effect);
            };
            duel.AddDelayAction(d1, ComVal.resetEvent_LeaveEndPhase, 0);
            duel.FinishHandle();
        };
        duel.SpeicalSummon(ComVal.Area_Graveyard, group, card.controller, card, ComVal.reason_Effect, effect, ComVal.CardPutType_UpRightFront, d);
    }


    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        int cardNum = duel.GetIncludeNameCardNumFromArea(ComStr.KeyWord_SixSamurai, false, card.controller, ComVal.cardType_Monster, ComVal.Area_Graveyard, Fiter);
        return cardNum >= 2;
    }


    private bool Fiter1(Card card)
    {
        return card.curArea.IsBind(ComVal.Area_Monster);
    }

    private bool Fiter(Card card)
    {
        return card.ownerPlayer.CanSpSummon(card)&&card.CanDestroy();
    }
}
