using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 连接效果 用于装备卡等
/// </summary>
public class LinkEffect : BaseEffect
{
    public Card targetCard;
    public Card selfCard;

    public override void SetCardEffectType(int cardEffectType)
    {
        base.SetCardEffectType(cardEffectType);
        SetType(EffectType.LinkEffect);
    }

    public void SetLinkCard(Card selfCard, Card targetCard)
    {
        this.selfCard = selfCard;
        this.targetCard = targetCard;
    }
}
