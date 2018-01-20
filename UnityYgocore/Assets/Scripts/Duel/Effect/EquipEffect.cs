using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipEffect : BaseEffect
{
    public Card targetCard;
    public Card equipCard;

    public override void SetCardEffectType(int cardEffectType)
    {
        base.SetCardEffectType(cardEffectType);
        SetType(EffectType.EquipEffect);
    }

    public void SetEquipCard(Card targetCard,Card equipCard)
    {
        this.targetCard = targetCard;
        this.equipCard = equipCard;
    }


}
