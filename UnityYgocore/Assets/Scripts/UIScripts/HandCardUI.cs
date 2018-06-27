///------------------------------------------------------//
///根据其名字来确定是那个玩家的
/// 
///------------------------------------------------------//
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// 处理手牌的增加减少
/// </summary>
public class HandCardUI : MonoBehaviour
{
    private GameObject cardPrefeb;

    private float cardSizeX;
    private float cardSizeY;


    public List<Card_Hand> cardList;

    public SelectCardMgr selectCardMgr;


    public bool isInSelect;

    bool isMy;

    float animTime = 0.1f;
    public void Init()
    {
        selectCardMgr = SelectCardMgr.GetInstance();
        cardPrefeb = Resources.Load("Prefebs/prefeb_handCard") as GameObject;
        cardSizeX = cardPrefeb.GetComponent<Card_Hand>().rectTransform.sizeDelta.x;
        cardSizeY = cardPrefeb.GetComponent<Card_Hand>().rectTransform.sizeDelta.y;
        cardList = new List<Card_Hand>();

        if (this.name == "MHandCard")
            isMy = true;
        else
            isMy = false;
    }

    #region 选择卡片

    /// <summary>
    /// 进入选择卡片状态
    /// </summary>
    /// <param name="cardType"></param>
    /// <param name="num"></param>
    /// <param name="dele"></param>
    public void SelectFieldCard(List<int> list,bool isMySelect)
    {
        if (list.Count == 0)
        {
            return;
        }
        isInSelect = true;

        for (int i = 0; i < list.Count; i++)
        {
            Card_Hand cardHand = cardList[list[i]];
            if (cardHand == null)
            {
                Debug.Log("error");
                return;
            }
            cardHand.EnterSelectState(isMySelect);
        }
    }

    /// <summary>
    /// 结束卡片选择状态
    /// </summary>
    public void EndSelectCard()
    {
        isInSelect = false;
        foreach (var item in cardList)
        {
            if (item != null)
            {
                Card_Hand card = item.GetComponent<Card_Hand>();
                card.EndSelectState();
            }
        }
    }

    int GetCardRank(Card_Hand card)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            if (cardList[i] == card)
            {
                return i;
            }
        }
        Debug.Log("error");
        return -1;
    }

    /// <summary>
    /// 由card_field调用
    /// </summary>
    /// <param name="card"></param>
    public bool SelectCard(Card_Hand card)
    {
       return selectCardMgr.SelectCard(ComVal.Area_Hand, GetCardRank(card),isMy);
    }

    public bool DisSelectCard(Card_Hand card)
    {
        return selectCardMgr.DisSelectCard(ComVal.Area_Hand, GetCardRank(card),isMy);
    }

    #endregion
    /// <summary>
    /// 加入卡片
    /// </summary>
    /// <param name="id"></param>
    public void AddCard(string id, bool isMy)
    {
        Card_Hand prefeb = GameObject.Instantiate(cardPrefeb).GetComponent<Card_Hand>();

        prefeb.transform.SetParent(this.transform);
        float val = cardList.Count > 7 ? cardSizeX / 2 * 8 : cardSizeX / 2 * (cardList.Count + 1);
        if (!isMy)
            val = -val;
        prefeb.rectTransform.anchoredPosition3D = new Vector3(val, 0, 0);
        prefeb.rectTransform.sizeDelta = new Vector2(cardSizeX, cardSizeY);
        prefeb.rectTransform.localScale = Vector3.one;
        prefeb.Init(this, isMy);

        if (Duel.GetInstance().IsNetWork)
        {
            if (isMy)
                prefeb.SetTexture(id, true);
            else
                prefeb.SetOverTexture(id);
        }
        else
        {
            prefeb.SetTexture(id, true);
            //prefeb.SetOverTexture(id);
        }
        cardList.Add(prefeb);
        SortCard();
    }

    /// <summary>
    /// 通过list来加入卡片组
    /// </summary>
    /// <param name="list"></param>
    public void AddCard(List<string> list, bool isMy)
    {
        for (int i = 0; i < list.Count; i++)
        {
            string cardID = list[i];
            AddCard(cardID, isMy);
        }
    }

    /// <summary>
    /// 通过卡片的区域值来移除卡片
    /// </summary>
    /// <param name="val"></param>
    public void RemoveCard(int val)
    {
        Card_Hand card = cardList[val];
        cardList.RemoveAt(val);
        Destroy(card.gameObject);
        SortCard();
    }

    public void RemoveCard(List<int> list)
    {
        list.Sort();
        for (int i = list.Count - 1; i >= 0; i--)
        {
            int a = list[i];
            Debug.Log(a);
            RemoveCard(a);
        }
    }
    /// <summary>
    /// 排序 对方卡片要相反
    /// </summary>
    void SortCard()
    {
        if (cardList.Count == 0)
        {
            return;
        }
        if (cardList.Count <= 7)
        {
            float x = -(cardList.Count - 1) * cardSizeX / 2;
            if (isMy)
                cardList[0].rectTransform.DOLocalMove(new Vector3(x, 0, 0), animTime);
            else
                cardList[0].rectTransform.DOLocalMove(new Vector3(-x, 0, 0), animTime);
            for (int i = 1; i < cardList.Count; i++)
            {
                float val;
                if (isMy)
                    val = x + i * cardSizeX;
                else
                    val = -(x + i * cardSizeX);
                cardList[i].rectTransform.DOLocalMove(new Vector3(val, 0, 0), animTime);
            }
        }
        else
        {
            float x = -6 * cardSizeX / 2;
            float x1 = -x * 2 / (cardList.Count - 1);
            if (isMy)
                cardList[0].rectTransform.DOLocalMove(new Vector3(x, 0, 0), animTime);
            else
                cardList[0].rectTransform.DOLocalMove(new Vector3(-x, 0, 0), animTime);
            for (int i = 1; i < cardList.Count; i++)
            {
                float val;
                if (isMy)
                    val = x + i * x1;
                else
                    val = -(x + i * x1);
                cardList[i].rectTransform.DOLocalMove(new Vector3(val, 0, 0), animTime);
            }
        }
    }

    /// <summary>
    /// 获取卡片的区域序号
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public int GetCardRank(GameObject obj)
    {
        for (int i = 0; i < cardList.Count; i++)
        {
            if (cardList[i].gameObject == obj)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// 显示手牌的虚线框动画
    /// </summary>
    public void ShowCardDashAnim(List<int> list,bool isMySelect)
    {
        foreach (var item in list)
        {
            cardList[item].ShowDashAnim(isMySelect);
        }
    }

    public RectTransform GetCardRectRank(int rank)
    {
        return cardList[rank].GetComponent<RectTransform>();
    }

    public Vector3 GetReturnHandCardPos(int rank)
    {
        float val;
        if (rank<7)
        {
            float x = -(rank - 1) * cardSizeX / 2;

            if (rank == 0)
            {
                if (isMy)
                    return new Vector3(x, 0, 0);
                else
                    return new Vector3(-x, 0, 0);
            }
            else
            {
                if (isMy)
                    val = x + rank * cardSizeX;
                else
                    val = -(x + rank * cardSizeX);
                return new Vector3(val, 0, 0);
            }
        }
        else
        {
            float x = -6 * cardSizeX / 2;
            float x1 = -x * 2 / (rank - 1);
            if (rank == 0)
            {
                if (isMy)
                    return new Vector3(x, 0, 0);
                else
                    return new Vector3(-x, 0, 0);
            }
            else
            {
                if (isMy)
                    val = x + rank * x1;
                else
                    val = -(x + rank * x1);
                return new Vector3(val, 0, 0);
            }
        }
    }

    public void HideCardDashAnim()
    {
        foreach (var item in cardList)
        {
            item.HideDashAnim();
        }
    }
}
