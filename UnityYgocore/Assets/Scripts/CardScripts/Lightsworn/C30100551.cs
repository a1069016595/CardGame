using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 光道圣女 密涅瓦
/// </summary>
public class C30100551 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        card.SetXYZMaterialFiter(Filter, 2);

        LauchEffect e1 = new LauchEffect();
        e1.SetCost(Cost);
        e1.SetCategory(ComVal.category_drawCard | ComVal.category_disCard);
        e1.SetCheckLauch(CheckLauch);
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        e1.SetCode(ComVal.code_NoCode);
        e1.SetLauchArea(ComVal.Area_Monster);
        e1.SetOperation(Operation);

        LauchEffect e2 = new LauchEffect();
        e2.SetCategory(ComVal.category_destroy | ComVal.category_disCard);
        e2.SetCheckLauch(CheckLauch1);
        e2.SetCardEffectType(ComVal.cardEffectType_mustToChooseLauch);
        e2.SetCode(ComVal.code_ToGraveyard);
        e2.SetLauchArea(ComVal.Area_Graveyard);
        e2.SetOperation(Operation1);

        duel.ResignEffect(e1, card, player);
        duel.ResignEffect(e2, card, player);
    }

    private void Operation1(IDuel duel, Card card, LauchEffect effect, Group target = null)
    {
        Group g = new Group();
        normalDele d1 = delegate
        {
            GetMes d2 = delegate(bool val)
            {
                if (val)
                {
                    GroupCardSelectBack gCallBack = delegate(Group g2)
                    {
                        duel.AddFinishHandle();
                        duel.SendToGraveyard(ComVal.Area_Field, g2, card, ComVal.reason_EffectDestroy, effect);
                    };

                    Group selectGroup = duel.GetIncludeNameCardFromArea("", true, card.controller, 0, ComVal.Area_Field);
                    duel.SelectCardFromGroup(selectGroup, gCallBack, g.GroupNum, card.controller,false);
                }
                else
                {
                    duel.FinishHandle();
                }
            };

            g = g.GetFitlerGroup(cardFiter);
            if (g.GroupNum > 0)
            {
                duel.ShowDialogBox(card, d2, card.controller.isMy);
            }
            else
            {
                duel.FinishHandle();
            }
        };
        duel.AddDelegate(d1, true);
        g = duel.DiscardFromDeck(3, card, effect, card.controller);
    }

    private bool CheckLauch1(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return code.group.ContainCard(card) && 
                 code.reason.reason.IsBind(ComVal.reason_Destroy) && 
                 card.controller.group_MainDeck.GroupNum > 3;
    }

    private void Cost(IDuel duel, Card card, LauchEffect effect)
    {
        duel.AddFinishHandle();
        duel.RemoveXYZMaterial(card, 1, ComVal.reason_Cost, card, effect);
    }

    private bool Filter(Card card)
    {
        return card.GetCurLevel() == 4 && card.CanBeXYZMaterial();
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group g = new Group();
        normalDele d1 = delegate
        {
            duel.AddFinishHandle();
            g = g.GetFitlerGroup(cardFiter);
            duel.DrawCard(card.controller, g.GroupNum, card, effect);
        };
        duel.AddDelegate(d1, true);
        g = duel.DiscardFromDeck(3, card, effect, card.controller);
    }

    private bool cardFiter(Card card)
    {
        return card.ContainName(ComStr.KeyWord_Lightsworn);
    }

   

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        return card.XyzMaterialNum() > 0 && card.controller.group_MainDeck.GroupNum > 3;
    }
}
