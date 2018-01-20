using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于存放静态方法
/// </summary>
public class DuelRule
{

    /// <summary>
    /// 检测祭品召唤
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public static bool CheckCanNormalSummon(Card card, Player player)
    {
        if (player.CanNormalSummon() && card.CanNormalSummon())
        {
            List<Card> monsterList = player.group_MonsterCard.GetCardList();
            int num = 0;
            for (int i = 0; i < monsterList.Count; i++)
            {
                if (monsterList[i].CanBeSacrifice())
                {
                    num++;
                }
            }
            return num >= card.GetSacrificeNum();
        }
        return false;
    }

    public static List<Card> GetCardListFromPlayer(int area, Player player)
    {
        List<Card> list = new List<Card>();
        if (ComVal.isBind(ComVal.Area_Graveyard, area))
            list.AddRange(player.group_Graveyard.cardList);
        if (ComVal.isBind(ComVal.Area_Extra, area))
            list.AddRange(player.group_ExtraDeck.cardList);
        if (ComVal.isBind(ComVal.Area_Hand, area))
            list.AddRange(player.group_HandCard.cardList);
        if (ComVal.isBind(ComVal.Area_Remove, area))
            list.AddRange(player.group_Remove.cardList);
        if (ComVal.isBind(ComVal.Area_MainDeck, area))
            list.AddRange(player.group_MainDeck.cardList);
        if (ComVal.isBind(ComVal.Area_Monster, area))
            list.AddRange(player.group_MonsterCard.GetCardList());
        if (ComVal.isBind(ComVal.Area_NormalTrap, area))
            list.AddRange(player.group_TrapCard.GetCardList());
        if (area.IsBind(ComVal.Area_FieldSpell))
        {
            if (player.fieldSpell != null)
            {
                list.Add(player.fieldSpell);
            }
        }
        return list;
    }

    /// <summary>
    /// 用于检测卡片是否都在手牌上
    /// </summary>
    /// <returns></returns>
    public static bool CardListIsHand(List<Card> list)
    {
        foreach (var item in list)
        {
            if (item.curArea != ComVal.Area_Hand)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 将卡片list转换为卡片区域序号的list
    /// </summary>
    /// <param name="cardList"></param>
    /// <returns></returns>
    public static List<int> ChangeCardListToRankList(List<Card> cardList)
    {
        List<int> list = new List<int>();
        foreach (var item in cardList)
        {
            list.Add(item.areaRank);
        }
        return list;
    }
}
