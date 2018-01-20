using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 编辑总UI
/// </summary>
public class EditUI : BaseUI
{
    #region 单例
    private static EditUI instance;

    public EditUI()
    {
        instance = this;
    }

    public static EditUI GetInstance()
    {
        return instance;
    }
    #endregion

    public Dropdown deckNameDropDown;

    public SelectLoadDeckUI selectLoadDeckUI;
    public EditDeckUI editDeckUI;
    public SearchCardUI searchCardUI;
    public SearchResultCardUI searchResultCardUI;
    public DragCardUI dragCardUI;

    public Deck curDeck;

    public override void Init()
    {
        curDeck = new Deck();

        selectLoadDeckUI = SelectLoadDeckUI.GetInstance();
        editDeckUI = EditDeckUI.GetInstance();
        searchCardUI = SearchCardUI.GetInstance();
        searchResultCardUI = SearchResultCardUI.GetInstance();
        dragCardUI = DragCardUI.GetInstance();

        editDeckUI.Init();
        searchCardUI.Init();
        searchResultCardUI.Init();
        selectLoadDeckUI.Init();
        dragCardUI.Init();
    }

    /// <summary>
    /// 保存卡组
    /// </summary>
    /// <param name="deckName"></param>
    public bool SaveDeck(string deckName)
    {
        if (editDeckUI.curDeck.isNull())
        {
            return false;
        }
        DeckLoad.SaveDeck(editDeckUI.curDeck, deckName);
        return true;
    }
    /// <summary>
    /// 显示选择的卡组
    /// </summary>
    /// <param name="deckName"></param>
    public void ShowDeck(string deckName)
    {
        Deck deck = new Deck();
        deck = DeckLoad.LoadDeck(deckName);
        editDeckUI.ShowDeck(deck);
    }
    /// <summary>
    /// 排序卡组
    /// </summary>
    public void SortDeck()
    {
        editDeckUI.SortDeck();
    }
    /// <summary>
    /// 增加卡片到卡组
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isMain"></param>
    public void AddCardToDeck(string id)
    {
        editDeckUI.AddCardToDeck(id);
    }

    public void ClearDeck()
    {
        curDeck.ClearDeck();
        editDeckUI.ShowDeck(curDeck);
    }

    public void DeleteDeck(string str)
    {
        DeckLoad.DeleteDeck(str);
    }

    /// <summary>
    /// 查找卡片
    /// </summary>
    /// <param name="cardType"></param>
    /// <param name="cardAttr"></param>
    /// <param name="cardRace"></param>
    /// <param name="cardLevel"></param>
    /// <param name="keyWord"></param>
    public void FindCard(int cardType, int cardAttr, int cardRace, int cardLevel, string keyWord)
    {
        List<string> list = new List<string>();
        list = LoadXml.SearchCard(cardType, cardAttr, cardRace, cardLevel, keyWord);

        List<Card> cardList = new List<Card>();
        foreach (var item in list)
        {
            cardList.Add(LoadXml.GetCard(item));
        }
        cardList = Group.SortCardList(cardList);

        list = new List<string>();
        for (int i = 0; i < cardList.Count; i++)
        {
            list.Add(cardList[i].cardID);
        }
        searchResultCardUI.ShowSearchCard(list);
    }

    /// <summary>
    /// 显示拖拽的卡片
    /// </summary>
    /// <param name="id"></param>
    public void ShowDragCardUI(string id)
    {
        dragCardUI.StartDrap(id);
    }

    public void RemoveCardFromDeck(GameObject obj)
    {
        editDeckUI.RemoveCard(obj);
    }
}
