using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 卡片组
/// </summary>
public class Group
{
    public static Group Empty()
    {
        return new Group();
    }

    public List<Card> cardList;

    public List<string> GetCardIDList()
    {
        List<string> list = new List<string>();
        foreach (var item in cardList)
        {
            list.Add(item.cardID);
        }
        return list;
    }

    public int GroupNum
    {
        get
        {
            return cardList.Count;
        }
    }

    public string[] ToArray()
    {
        string[] a = new string[cardList.Count];
        for (int i = 0; i < cardList.Count; i++)
        {
            a[i] = cardList[i].cardID;
        }
        return a;
    }

    public void RemoveCard(Card card)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            Card item = cardList[i];
            if (item == card)
            {
                cardList.Remove(card);
                return;
            }
        }
    }

    public bool isEmpty()
    {
        if (cardList.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Card this[int index] 
    {
        get { return cardList[index]; }
    }

    /// <summary>
    /// 第一张牌的序号为1
    /// </summary>
    public Group()
    {
        cardList = new List<Card>();
    }

    public Group(List<Card> list)
    {
        cardList = new List<Card>();
        for (int i = 0; i < list.Count; i++)
        {
            cardList.Add(list[i]);
        }
    }

    /// <summary>
    /// 对卡片进行排序
    /// <para>根据卡牌种类 星阶来排</para>
    /// </summary>
    public void SortGroup()
    {
        // cardList.Sort();
        cardList = SortCardList(cardList);
    }
    /// <summary>
    /// 筛选出符合种族的怪兽卡片
    /// </summary>
    /// <param name="ff"></param>
    public Group SiftingGroupInRace(int race)
    {
        Group a = new Group();
        for (int i = 0; i < cardList.Count; i++)
        {
            if (ComVal.isBind(cardList[i].race, race))
            {
                a.AddCard(cardList[i]);
            }
        }
        return a;
    }
    /// <summary>
    /// 筛选出符合星级的怪兽卡片
    /// </summary>
    /// <returns></returns>
    public Group SiftingGroupInLevel(int level, bool isLessThan)
    {
        Group a = new Group();
        if (isLessThan)
        {
            for (int i = 0; i < cardList.Count; i++)
            {
                if (cardList[i].GetCurLevel() <= level)
                {
                    a.AddCard(cardList[i]);
                }
            }
        }
        else
        {
            for (int i = 0; i < cardList.Count; i++)
            {
                if (cardList[i].GetCurLevel() == level)
                {
                    a.AddCard(cardList[i]);
                }
            }
        }
        return a;
    }

    public Group SiftingGroupInArea(int area)
    {
        Group a = new Group();
        foreach (var item in cardList)
        {
            if (ComVal.isBind(item.curArea, area))
                a.AddCard(item);
        }
        return a;
    }

    /// <summary>
    /// 会改变传入的区域列表
    /// </summary>
    /// <param name="area"></param>
    /// <returns></returns>
    public Group SiftingGroupInArea(List<int> area)
    {
        for (int i = 0; i < area.Count; i++)
        {
            if (!cardList[i].curArea.IsBind(area[i]))
            {
                cardList.RemoveAt(i);
                area.RemoveAt(i);
                i--;
            }
        }
        return new Group(cardList);
    }

    /// <summary>
    /// 筛选出符合属性的怪兽卡片
    /// </summary>
    /// <param name="attr"></param>
    /// <returns></returns>
    public Group SiftingGroupInAttr(int attr)
    {
        Group a = new Group();
        foreach (var item in cardList)
        {
            if (ComVal.isBind(item.GetCurAttribute(), attr))
                a.AddCard(item);
        }
        return a;
    }

    /// <summary>
    /// 获取最后一张牌
    /// <para>设置墓地，除外区的显示</para>
    /// <para>卡片数为0时返回空卡片</para>
    /// </summary>
    /// <returns></returns>
    public Card GetLastCard()
    {
        if (isEmpty())
        {
            Card empty = Card.Empty();
            return empty;
        }
        Card card = cardList[cardList.Count - 1];
        if (card == null)
        {
            Debug.Log("error");
            return null;
        }
        else
            return card;
    }

    /// <summary>
    /// 获取第一张牌
    /// <para>用于抽牌</para>
    /// <para>卡片数为0时返回空卡片</para>
    /// </summary>
    /// <returns></returns>
    public Card GetFirstCard()
    {
        if (isEmpty())
        {
            return null;
        }
        Card card = cardList[0];
        if (card == null)
        {
            Debug.Log("error");
            return null;
        }
        else
            return card;
    }

    public Group GetFirstAmountCard(int num)
    {
        Group result = new Group();
        for (int i = 0; i < num; i++)
        {
            if (i < cardList.Count)
            {
                result.AddCard(cardList[i]);
            }
        }
        return result;
    }

    /// <summary>
    /// 按照序号获取一张牌
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public Card GetCard(int val)
    {
        Card card = cardList[val];

        if (card == null)
        {
            Debug.Log("error");
            return null;
        }
        else
            return card;
    }
    /// <summary>
    /// 会设置区域序号
    /// </summary>
    /// <param name="card"></param>
    public void AddAndSetCard(Card card)
    {
        cardList.Add(card);
        card.SetAreaRank(cardList.Count - 1);
    }

    public void InsertCard(Card card, int index)
    {
        cardList.Insert(index, card);
    }

    /// <summary>
    /// 不会设置区域序号
    /// </summary>
    /// <param name="card"></param>
    /// <param name="rank">rank为0则不会设置区域序号</param>
    public void AddCard(Card card)
    {
        cardList.Add(card);
    }

    /// <summary>
    /// 洗牌
    /// </summary>
    public void Shuffle()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            int a = Random.Range(0, cardList.Count);
            Card card = cardList[a];
            cardList[a] = cardList[cardList.Count - i - 1];
            cardList[cardList.Count - i - 1] = card;
        }
    }

    /// <summary>
    /// 会重新设置卡片区域序号
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public Card RemoveCard(int val)
    {
        Card card = cardList[val];
        cardList.RemoveAt(val);
        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].SetAreaRank(i);
        }
        return card;
    }

    public Card RemoveFirstCard()
    {
        Card card = cardList[0];
        cardList.RemoveAt(0);
        return card;
    }

    public List<int> GetRankList()
    {
        List<int> list = new List<int>();
        for (int i = 0; i < cardList.Count; i++)
        {
            list.Add(cardList[i].areaRank);
        }
        return list;
    }
    /// <summary>
    /// 是否在手牌或场地上
    /// </summary>
    /// <returns></returns>
    public bool IsHandAndField()
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            if (!ComVal.isBind(cardList[i].curArea, ComVal.Area_Hand | ComVal.Area_Monster | ComVal.Area_Trap))
                return false;
        }
        return true;
    }

    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public static List<Card> SortCardList(List<Card> list)
    {
        List<Card> normalMonsterList = new List<Card>();
        List<Card> monsterList = new List<Card>();
        List<Card> spellList = new List<Card>();
        List<Card> trapList = new List<Card>();
        List<Card> xyzMonsterList = new List<Card>();
        List<Card> fusionMonsterList = new List<Card>();
        List<Card> synchroMonsterList = new List<Card>();

        List<Card> resultList = new List<Card>();

        for (int i = 0; i < list.Count; i++)
        {
            Card card = list[i];
            if (card.IsMonster())
            {
                if (!ComVal.isInExtra(card.cardType) && card.cardType != ComVal.CardType_Monster_Normal)
                {
                    bool isInsert = false;
                    for (int j = 0; j < monsterList.Count; j++)//有同名则插入其后面
                    {
                        if (card.cardID == monsterList[j].cardID)
                        {
                            monsterList.Insert(j, card);
                            isInsert = true;
                            break;
                        }
                    }
                    if (!isInsert)
                    {
                        for (int x = 0; x < monsterList.Count; x++)
                        {
                            if (card.level < monsterList[x].level)
                            {
                                monsterList.Insert(x, card);
                                isInsert = true;
                                break;
                            }
                        }
                    }
                    if (!isInsert)
                    {
                        monsterList.Add(card);

                    }
                }
                else if (card.cardType == ComVal.CardType_Monster_Normal)
                {
                    bool isInsert = false;
                    for (int j = 0; j < normalMonsterList.Count; j++)//有同名则插入其后面
                    {
                        if (card.cardID == normalMonsterList[j].cardID)
                        {
                            normalMonsterList.Insert(j, card);
                            isInsert = true;
                            break;
                        }
                    }
                    if (!isInsert)
                    {
                        for (int x = 0; x < normalMonsterList.Count; x++)
                        {
                            if (card.level < normalMonsterList[x].level)
                            {
                                normalMonsterList.Insert(x, card);
                                isInsert = true;
                                break;
                            }
                        }
                    }
                    if (!isInsert)
                    {
                        normalMonsterList.Add(card);
                    }
                }
                else if (card.cardType == ComVal.CardType_Monster_Fusion)
                {
                    bool isInsert = false;
                    for (int j = 0; j < fusionMonsterList.Count; j++)//有同名则插入其后面
                    {
                        if (card.cardID == fusionMonsterList[j].cardID)
                        {
                            fusionMonsterList.Insert(j, card);
                            isInsert = true;
                            break;
                        }
                    }
                    if (!isInsert)
                    {
                        for (int x = 0; x < fusionMonsterList.Count; x++)
                        {
                            if (card.level < fusionMonsterList[x].level)
                            {
                                fusionMonsterList.Insert(x, card);
                                isInsert = true;
                                break;
                            }
                        }
                    }
                    if (!isInsert)
                    {
                        fusionMonsterList.Add(card);

                    }
                }
                else if (card.cardType == ComVal.CardType_Monster_Synchro)
                {
                    bool isInsert = false;
                    for (int j = 0; j < synchroMonsterList.Count; j++)//有同名则插入其后面
                    {
                        if (card.cardID == synchroMonsterList[j].cardID)
                        {
                            synchroMonsterList.Insert(j, card);
                            isInsert = true;
                            break;
                        }
                    }
                    if (!isInsert)
                    {
                        for (int x = 0; x < synchroMonsterList.Count; x++)
                        {
                            if (card.level < synchroMonsterList[x].level)
                            {
                                synchroMonsterList.Insert(x, card);
                                isInsert = true;
                                break;
                            }
                        }
                    }
                    if (!isInsert)
                    {
                        synchroMonsterList.Add(card);

                    }
                }
                else if (card.cardType == ComVal.CardType_Monster_XYZ)
                {
                    bool isInsert = false;
                    for (int j = 0; j < xyzMonsterList.Count; j++)//有同名则插入其后面
                    {
                        if (card.cardID == xyzMonsterList[j].cardID)
                        {
                            xyzMonsterList.Insert(j, card);
                            isInsert = true;
                            break;
                        }
                    }
                    if (!isInsert)
                    {
                        for (int x = 0; x < xyzMonsterList.Count; x++)
                        {
                            if (card.level < xyzMonsterList[x].level)
                            {
                                xyzMonsterList.Insert(x, card);
                                isInsert = true;
                                break;
                            }
                        }
                    }
                    if (!isInsert)
                    {
                        xyzMonsterList.Add(card);

                    }
                }
            }
            else if (card.isSpell())
            {
                bool isInsert = false;
                for (int j = 0; j < spellList.Count; j++)//有同名则插入其后面
                {
                    if (card.cardID == spellList[j].cardID)
                    {
                        spellList.Insert(j, card);
                        isInsert = true;
                        break;
                    }
                }
                if (!isInsert)
                {
                    spellList.Add(card);
                }
            }
            else if (card.IsTrap())
            {
                bool isInsert = false;
                for (int j = 0; j < trapList.Count; j++)//有同名则插入其后面
                {
                    if (card.cardID == trapList[j].cardID)
                    {
                        trapList.Insert(j, card);
                        isInsert = true;
                        break;
                    }
                }
                if (!isInsert)
                {
                    trapList.Add(card);
                }
            }

        }
        resultList.AddRange(normalMonsterList);
        resultList.AddRange(monsterList);
        resultList.AddRange(fusionMonsterList);
        resultList.AddRange(synchroMonsterList);
        resultList.AddRange(xyzMonsterList);
        resultList.AddRange(spellList);
        resultList.AddRange(trapList);
        return resultList;
    }

    public void AddList(List<Card> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Card card = list[i];
            cardList.Add(card);
        }
    }

    public int GetAreaNum(int area)
    {
        int a = 0;
        for (int i = 0; i < cardList.Count; i++)
        {
            Card card = cardList[i];
            if (ComVal.isBind(area, card.curArea))
            {
                a++;
            }
        }
        return a;
    }

    public Group GetAreaGroup(int area)
    {
        Group g = new Group();
        for (int i = 0; i < cardList.Count; i++)
        {
            Card card = cardList[i];
            if (ComVal.isBind(area, card.curArea))
            {
                g.AddCard(card);
            }
        }
        return g;
    }

    public List<Card> GetFitlerList(Filter f)
    {
        List<Card> result = new List<Card>();
        foreach (var item in cardList)
        {
            if (f(item))
            {
                result.Add(item);
            }
        }
        return result;
    }

    public Group GetFitlerGroup(CardFilter f)
    {
        Group result = new Group();
        foreach (var item in cardList)
        {
            if (f.isFit(item))
            {
                result.AddCard(item);
            }
        }
        return result;
    }

    public List<Card> GetFitlerList(CardFilter fiter)
    {
        List<Card> result = new List<Card>();
        foreach (var item in cardList)
        {
            if (fiter.isFit(item))
            {
                result.Add(item);
            }
        }
        return result;
    }

    public Group GetPlayerGroup(Player p)
    {
        Group g = new Group();
        foreach (var item in cardList)
        {
            if (item.controller == p)
                g.AddCard(item);
        }
        return g;
    }
    /// <summary>
    /// 获取可融合的卡片组
    /// </summary>
    /// <param name="g"></param>
    /// <returns></returns>
    public Group GetCanFusionGroup(Group g)
    {
        Group tg = new Group();
        foreach (var item in cardList)
        {
            if (item.CanFusion(g))
                tg.AddCard(item);
        }
        return tg;
    }

    public bool ContainCard(Card c)
    {
        return cardList.Contains(c);
    }

    public IEnumerator GetEnumerator()
    {
        foreach (Card item in cardList)
        {
            yield return item;
        }
    }

    internal Group GetFitlerGroup(Filter f)
    {
        Group result = new Group();
        foreach (var item in cardList)
        {
            if (f(item))
            {
                result.AddCard(item);
            }
        }
        return result;
    }

    public List<Card> ToList()
    {
        List<Card> result = new List<Card>();
        foreach (var item in cardList)
        {
            result.Add(item);
        }
        return result;
    }

    /// <summary>
    /// 获取卡片种类数量，如裁决之龙
    /// </summary>
    /// <returns></returns>
    public int GetTypeNum()
    {
        List<string> list = new List<string>();
        for (int i = 0; i < cardList.Count; i++)
        {
            if(!list.Contains(cardList[i].cardID))
            {
                list.Add(cardList[i].cardID);
            }
        }
        return list.Count;
    }

    public List<int> GetAreaList()
    {
        List<int> result = new List<int>();
        for (int i = 0; i < cardList.Count; i++)
        {
            result.Add(cardList[i].curArea);
        }
        return result;
    }

}
