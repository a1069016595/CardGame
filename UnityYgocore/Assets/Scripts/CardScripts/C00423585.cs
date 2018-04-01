using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 召唤僧
/// </summary>
public class C00423585 : ICardScripts
{
    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        //不能解放
        StateEffect e1 = new StateEffect();
        e1.SetRangeArea(ComVal.Area_Monster);
        e1.SetCategory(ComVal.category_stateEffect );
        e1.SetCardEffectType(ComVal.cardEffectType_Single | ComVal.cardEffectType_normalStateEffect|ComVal.cardEffectType_unableReset);
        e1.SetStateEffectType(ComVal.stateEffectType_unableRelease);
        e1.SetTarget(card);
        duel.ResignEffect(e1, card, player);

        //变防守
        LauchEffect e2 = new LauchEffect();
        e2.SetCategory(ComVal.category_position);
        e2.SetLauchArea(ComVal.Area_Monster);
        e2.SetCardEffectType(ComVal.cardEffectType_mustLauch);
        e2.SetCode(ComVal.code_NormalSummon );
        e2.SetOperation(Operation);
        e2.SetCheckLauch(CheckLauch);
        duel.ResignEffect(e2, card, player);

        //特殊召唤
        LauchEffect e3 = new LauchEffect();
        e3.SetCategory(ComVal.category_spSummon);
        e3.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e3.SetCode(ComVal.code_NoCode);
        e3.SetLauchArea(ComVal.Area_Monster);
        e3.SetOperation(Operation1);
        e3.SetCost(Cost);
        e3.SetCheckLauch(CheckLauch1);
        e3.SetLauchPhase(ComVal.Phase_Mainphase);
        card.SetCardCountLimit(e3 , 1);
        duel.ResignEffect(e3, card, player);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        duel.ChangeMonsterType(ComVal.CardPutType_layFront, effect.ownerCard);
        duel.FinishHandle();
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        if (code.group.ContainCard( effect.ownerCard)&&card.CanChangeType()&&card.curPlaseState==ComVal.CardPutType_UpRightFront)
        {
            return true;
        }
        else
        {
           
            return false;
        }
    }

    public void Operation1(IDuel duel, Card card, LauchEffect effect,  Group group = null)
    {
        Group b = duel.GetIncludeNameCardFromArea("", false, card.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_MainDeck, filer, false, null, null);
        GroupCardSelectBack callBack = delegate(Group targetGroup)
                {
                    Card target = targetGroup.GetCard(0);
                    normalDele norDele = delegate()
                    {
                        StateEffect e1 = new StateEffect();
                        e1.SetRangeArea(ComVal.Area_Monster);
                        e1.SetCategory(ComVal.category_stateEffect|ComVal.category_limitTime);
                        e1.SetCardEffectType(ComVal.cardEffectType_Single|ComVal.cardEffectType_normalStateEffect);
                        e1.SetTarget(target);
                        e1.SetStateEffectType(ComVal.stateEffectType_unableAttack);
                        e1.SetResetCode(ComVal.resetEvent_LeaveEndPhase,0);
                        duel.ResignEffect(e1, card, card.ownerPlayer);
                        duel.FinishHandle();
                    };
                    duel.SpeicalSummon(ComVal.Area_MainDeck,target, card.ownerPlayer, card, ComVal.reason_Effect, effect, 0, norDele);

                };
        duel.SelectCardFromGroup(b, callBack, 1, card.controller);
    }

    public bool CheckLauch1(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        int a = duel.GetIncludeNameCardNumFromArea("", false, card.ownerPlayer, ComVal.cardType_Spell, ComVal.Area_Hand);
        Group b = duel.GetIncludeNameCardFromArea("", false, card.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_MainDeck, filer, false, null, null);

        if (a == 0 || b.GroupNum == 0 || duel.MonsterAreaIsFull(card.ownerPlayer))
            return false;
        else
            return true && card.ownerPlayer.CanSpSummon(b);
    }

    public void Cost(IDuel duel, Card card, LauchEffect effect)
    {
        Group a = duel.GetIncludeNameCardFromArea("", false, card.ownerPlayer, ComVal.cardType_Spell, ComVal.Area_Hand);
        GroupCardSelectBack callBack = delegate(Group group)
        {
            duel.AddFinishHandle();
            duel.SendToGraveyard(ComVal.Area_Hand, group, card, ComVal.reason_Cost, effect);
        };
        duel.SelectCardFromGroup(a, callBack, 1, card.controller);
    }

    bool filer(Card card)
    {
        if (card.GetCurLevel() == 4)
        {
            return true;
        }
        else
            return false;
    }
}
