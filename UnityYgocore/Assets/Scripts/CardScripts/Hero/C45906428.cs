/// <summary>
/// 奇迹融合
/// </summary>
public class C45906428 : ICardScripts
{

    public void InitialEffect(Card card, Player player, IDuel duel)
    {
        LauchEffect e1 = new LauchEffect();
        e1.SetCategory(ComVal.category_remove|ComVal.category_fusionSummon);
        e1.SetLauchArea(ComVal.Area_Hand | ComVal.Area_Trap);
        e1.SetCode(ComVal.code_NoCode);
        e1.SetCheckLauch(CheckLauch);
        e1.SetOperation(Operation);
        e1.SetCardEffectType(ComVal.cardEffectType_normalLauch);
        duel.ResignEffect(e1, card, player);
    }

    public void Operation(IDuel duel, Card card, LauchEffect effect, Group group = null)
    {
        Group g1 = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_ElementalHERO, false, card.ownerPlayer, ComVal.CardType_Monster_Fusion, ComVal.Area_Extra);
        Group g2 = duel.GetIncludeNameCardFromArea("", false, card.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_Monster | ComVal.Area_Graveyard);
        g1 = g1.GetCanFusionGroup(g2);
        g1 = card.controller.GetCanSpSummonGroup(g1);
        GroupCardSelectBack callBack = delegate(Group theGroup)
        {
            Card c = theGroup.GetCard(0);
            Group g3 = c.cardMaterialFitler.GetGroup(g2);

            GroupCardSelectBack callBack1 = delegate(Group theGroup1)
            {
                

                normalDele finish = delegate
                {
                    duel.FinishHandle();
                };
                normalDele d = delegate
                {
                    duel.SpeicalSummon(ComVal.Area_Extra, c, card.ownerPlayer, card, ComVal.reason_FusionSummon, effect, 0, finish);
                };
                duel.AddDelegate(d);
                duel.SendToRemove(ComVal.Area_Graveyard | ComVal.Area_Monster, theGroup1, card, ComVal.reason_FusionMaterial, effect);
            };
            duel.SelectFusionMaterialFromGroup(g3, callBack1, c);
        };
        duel.SelectCardFromGroup(g1, callBack, 1, card.controller);
    }

    public bool CheckLauch(IDuel duel, Card card, LauchEffect effect, Code code)
    {
        Group g = duel.GetIncludeNameCardFromArea("", false, card.ownerPlayer, ComVal.cardType_Monster, ComVal.Area_Monster | ComVal.Area_Graveyard);
        Group g1 = duel.GetIncludeNameCardFromArea(ComStr.KeyWord_ElementalHERO, false, card.ownerPlayer, ComVal.CardType_Monster_Fusion, ComVal.Area_Extra);
        for (int i = 0; i < g1.GroupNum; i++)
        {
            Card theCard = g1.GetCard(i);
            if (theCard.CanFusion(g)&&card.controller.CanSpSummon(theCard))
            {
                return true;
            }
        }
        return false;
    }
}
