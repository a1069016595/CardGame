using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 用于表示场地上的卡片组合
/// </summary>
public class FieldCardGroup
{
    public Card[] cardList;

    private int num;

    public int GroupNum
    {
        get { return num; }
    }

    public FieldCardGroup(int val)
    {
        cardList = new Card[val];
        num = 0;
    }
    /// <summary>
    /// 不会设置区域序号
    /// </summary>
    /// <param name="val"></param>
    /// <param name="card"></param>
    public void AddCard(int val, Card card)
    {
      
        cardList[val] = card;
        //Debug.Log(GetCard(val).cardName);
        num++;
    }

    public Card RemoveCard(int val)
    {
        Card card = cardList[val];
        cardList[val] = null;
        num--;
        return card;
    }

    public Card GetCard(int val)
    {
        Card card = cardList[val];
        return card;
    }

    public List<Card> GetCardList()
    {
        List<Card> list = new List<Card>();
        foreach (var item in cardList)
        {
            if (item != null)
            {
                list.Add(item);
            }
        }
        return list;
    }

    public Group GetGroup()
    {
        Group result = new Group();
        for (int i = 0; i < cardList.Length; i++)
        {
            if (cardList[i] != null)
                result.AddCard(cardList[i]);
        }
        return result;
    }
}