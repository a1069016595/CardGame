using System.Collections.Generic;
/// <summary>
/// 元素英雄 烈焰侠  
/// </summary>
public class C63060238 : ICardScripts
{
    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        //检索
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_search | ComVal.category_toHand);
        e1.SetCheckLauch(CheckLauch);
        e1.SetOperation(Operation);
        e1.SetCode(ComVal.code_NotSpSummon | ComVal.code_SpecialSummon);
        e1.SetCardEffectType(ComVal.cardEffectType_mustToChooseLauch);
        e1.SetLauchArea(ComVal.Area_Monster);
        duel.ResignEffect(e1, card, player);

        //送去墓地
        LauchEffect e2 = new LauchEffect();
        e2.SetCategory(ComVal.category_toGrave);
        e2.SetCode(ComVal.code_NoCode);
        e2.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e2.SetLauchArea(ComVal.Area_Monster);
        e2.SetCheckLauch(CheckLauch1);
        e2.SetOperation(Operation1);
        e2.SetLauchPhase(ComVal.Phase_Mainphase);
        duel.ResignEffect(e2, card, player);

        card.SetCardCountLimit(e1, e2 , 1);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group g = duel.GetIncludeNameCardFromArea("24094653", false, card.ownerPlayer, ComVal.cardType_Spell, ComVal.Area_MainDeck);
        GroupCardSelectBack callBack = delegate(Group theGroup)
        {
            Card c = theGroup.GetCard(0);   
            duel.AddFinishHandle();
            duel.AddCardToHandFromMainDeck( c, card.ownerPlayer, card, effect);
        };
        duel.SelectCardFromGroup(g, callBack, 1, card.controller);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        int num = duel.GetIncludeNameCardNumFromArea("24094653", false, card.ownerPlayer, ComVal.cardType_Spell, ComVal.Area_MainDeck);
        return num > 0 && code.group.ContainCard(card);
    }

    public void Operation1(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group g = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_ElementalHERO, false, card.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_MainDeck, null, true, null, card.cardID);
        GroupCardSelectBack callBack = delegate(Group theGroup)
        {
            normalDele d = delegate
            {
                Card c = theGroup.GetCard(0);
                StateEffect e1 = new StateEffect();
                e1.SetRangeArea(ComVal.Area_Monster);
                e1.SetCategory(ComVal.category_stateEffect | ComVal.category_limitTime);
                e1.SetCardEffectType(ComVal.cardEffectType_Single | ComVal.cardEffectType_normalStateEffect);
                e1.SetStateEffectType(ComVal.stateEffectType_ChangeAttribute);
                e1.SetTarget(card);
                e1.SetStateEffectVal(c.attribute);

                e1.SetResetCode(ComVal.resetEvent_LeaveEndPhase, 0);

                StateEffect e2 = e1.Clone() as StateEffect;
                e2.SetStateEffectType(ComVal.stateEffectType_ChangeAttack);
                e2.SetStateEffectVal(c.afk);

                StateEffect e3 = e1.Clone() as StateEffect;
                e3.SetStateEffectType(ComVal.stateEffectType_ChangeDef);
                e3.SetStateEffectVal(c.def);

                duel.ResignEffect(e1, card, card.ownerPlayer);
                duel.ResignEffect(e2, card, card.ownerPlayer);
                duel.ResignEffect(e3, card, card.ownerPlayer);

                LimitPlayerEffect e4 = new LimitPlayerEffect();
                e4.SetCategory(ComVal.category_limitEffect);
                e4.SetTargetType(TargetPlayerType.my);
                e4.SetLimitEffectType(ComVal.limitEffectType_spSummonLimit);
                e4.SetLimitEffectHandler(limitEffectHandler);
                e4.SetResetCode(ComVal.resetEvent_LeaveEndPhase, 0);
                duel.ResignEffect(e4, card, card.ownerPlayer);
                duel.FinishHandle();
            };
            duel.AddDelegate(d);
            duel.SendToGraveyard(ComVal.Area_MainDeck, theGroup, card, ComVal.reason_Effect, effect);
        };
        duel.SelectCardFromGroup(g, callBack, 1, card.controller);
    }

    private object limitEffectHandler(IDuel duel, Card c, LimitPlayerEffect e)
    {
        return c.cardType.IsBind(ComVal.CardType_Monster_Fusion);
    }

    public bool CheckLauch1(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        int num = duel.GetIncludeNameCardNumFromArea(ComStr.KeyWord_ElementalHERO, false, card.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_MainDeck, null, true, null, card.cardID);
        return num > 0;
    }
}
