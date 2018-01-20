using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// 生成卡片实例
/// </summary>
class CardFactroy
{
    public static Card GenerateCard(string id)
    {
        Card card = LoadXml.GetCard(id);
        Card result = new Card(card.cardID, card.cardName, card.cardDescribe, card.cardType);
        if (card.IsMonster())
        {
            result.SetMonsterAttribute(card.attribute, card.race, card.level, card.afk, card.def);
        }
        return result;
    }
}

