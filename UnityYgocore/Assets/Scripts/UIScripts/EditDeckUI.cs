using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
/// <summary>
/// 主卡组 额外卡组窗口
/// </summary>
public class EditDeckUI : MonoBehaviour
{

    #region 单例
    private static EditDeckUI instance;

    public EditDeckUI()
    {
        instance = this;
    }

    public static EditDeckUI GetInstance()
    {
        return instance;
    }
    #endregion

    public GameObject mainDeckGroup;
    public GameObject extraDeckGroup;
    public GameObject cardPrefeb;

    public List<GameObject> mainDeckList;
    public List<GameObject> extraDeckList;

    public Deck curDeck;

    public CheckDrapCard mainCheckDrapCard;
    public CheckDrapCard extraCheckDrapCard;

    public Text mainDeckText;
    public Text extraDeckText;

    public Card_Edit curCard;

    List<GameObject> waitToUseList;

    public void Init()
    {
        cardPrefeb = Resources.Load("Prefebs/prefeb_editCard") as GameObject;
        curDeck = new Deck();
        mainCheckDrapCard = transform.FindChild("MainDeck").GetComponent<CheckDrapCard>();
        extraCheckDrapCard = transform.FindChild("ExtraDeck").GetComponent<CheckDrapCard>();
        mainDeckText = transform.FindChild("mainDeckText").GetComponent<Text>();
        extraDeckText = transform.FindChild("extraDeckText").GetComponent<Text>();
        waitToUseList = new List<GameObject>();
    }

    public void SetCurCard(Card_Edit obj)
    {
        curCard = obj;
    }

    /// <summary>
    /// 显示卡组
    /// <para>这会隐藏已存在的卡片</para>
    /// </summary>
    /// <param name="deck"></param>
    public void ShowDeck(Deck deck)
    {
        curDeck = deck;
        Group mainGroup = curDeck.mainDeck;
        Group extraGroup = curDeck.extraDeck;
        for (int i = 0; i < mainGroup.cardList.Count; i++)
        {

            Card theCard = mainGroup.cardList[i];

            if (i < mainDeckList.Count)
            {
                Card_Edit theCard_edit = mainDeckList[i].GetComponent<Card_Edit>();
                theCard_edit.SetTexture(theCard.cardID, true);
                theCard_edit.SetActive(true);
            }
            else
            {
                GameObject obj = ComMethod.InitGameObject(cardPrefeb, mainDeckGroup.transform);
                Card_Edit card = obj.GetComponent<Card_Edit>();
                card.Init();

                card.SetTexture(theCard.cardID, true);
                mainDeckList.Add(obj);
            }
        }
        for (int i = 0; i < extraGroup.cardList.Count; i++)
        {

            Card theCard = extraGroup.cardList[i];

            if (i < extraDeckList.Count)
            {
                Card_Edit theCard_edit = extraDeckList[i].GetComponent<Card_Edit>();
                theCard_edit.SetTexture(theCard.cardID, true);
                theCard_edit.SetActive(true);
            }
            else
            {
                GameObject obj = ComMethod.InitGameObject(cardPrefeb, extraDeckGroup.transform);
                Card_Edit card = obj.GetComponent<Card_Edit>();
                card.Init();

                card.SetTexture(theCard.cardID, true);
                extraDeckList.Add(obj);
            }
        }

        if (mainDeckList.Count > mainGroup.cardList.Count)
        {
            for (int i = mainGroup.cardList.Count; i < mainDeckList.Count; i++)
            {
                mainDeckList[i].SetActive(false);
            }
        }
        if (extraDeckList.Count > extraGroup.cardList.Count)
        {
            for (int i = extraGroup.cardList.Count; i < extraDeckList.Count; i++)
            {
                extraDeckList[i].SetActive(false);
            }
        }
        UpdateText();
    }

    /// <summary>
    /// 增加卡片
    /// </summary>
    /// <param name="isMain">是否为主卡组</param>
    /// <param name="id">卡片id</param>
    public void AddCardToDeck(string id)
    {

        Card card = LoadXml.GetCard(id);
        bool isMain = !ComVal.isInExtra(card.cardType);

        int cardNum = 0;
        if (isMain)
        {
            foreach (var item in curDeck.mainDeck.cardList)
            {
                if (item.cardID == card.cardID)
                {
                    cardNum++;
                }
            }
        }
        else
        {
            foreach (var item in curDeck.extraDeck.cardList)
            {
                if (item.cardID == card.cardID)
                {
                    cardNum++;
                }
            }
        }
        if (cardNum >= 3)
        {
            return;
        }
        if (isMain)
        {
            if (curCard != null)
            {
                int val = GetObjVal(curCard.gameObject, isMain);
                AddToDeckFromPool(card, mainDeckGroup.transform,true);
                curDeck.mainDeck.InsertCard(card, val);
                for (int i = val; i < curDeck.mainDeck.GroupNum; i++)
                {
                    mainDeckList[i].GetComponent<Card_Edit>().SetTexture(curDeck.mainDeck.GetCard(i).cardID, true);
                }
                UpdateText();
                return;
            }
            if (mainDeckList.Count > curDeck.mainDeck.cardList.Count)
            {
                Card_Edit theCard_edit = mainDeckList[curDeck.mainDeck.cardList.Count].GetComponent<Card_Edit>();
                theCard_edit.SetTexture(card.cardID, true);
                theCard_edit.SetActive(true);

            }
            else if (mainDeckList.Count == curDeck.mainDeck.cardList.Count)
            {
                AddToDeckFromPool(card, mainDeckGroup.transform,true);
            }
            else
            {
                Debug.Log("error");
            }

            curDeck.mainDeck.AddCard(card);
        }
        else
        {
            if (curCard != null)
            {
               int val = GetObjVal(curCard.gameObject, isMain);
                AddToDeckFromPool(card, extraDeckGroup.transform,false);
                curDeck.extraDeck.InsertCard(card, val);
                for (int i = val; i < curDeck.extraDeck.GroupNum; i++)
                {
                    extraDeckList[i].GetComponent<Card_Edit>().SetTexture(curDeck.extraDeck.GetCard(i).cardID, true);
                }
                UpdateText();
                return;
            }
            if (extraDeckList.Count > curDeck.extraDeck.cardList.Count)
            {
                Card_Edit theCard_edit = extraDeckList[curDeck.extraDeck.cardList.Count].GetComponent<Card_Edit>();
                theCard_edit.SetTexture(card.cardID, true);
                theCard_edit.SetActive(true);
            }
            else if (extraDeckList.Count == curDeck.extraDeck.cardList.Count)
            {
                AddToDeckFromPool(card, extraDeckGroup.transform,false);
            }
            else
            {
                Debug.Log("error");
            }
            curDeck.extraDeck.AddCard(card);
        }
        UpdateText();
    }

    private void AddToDeckFromPool(Card card, Transform target,bool isMain)
    {
        if (waitToUseList.Count == 0)
        {
            GameObject obj = ComMethod.InitGameObject(cardPrefeb, target);
            Card_Edit editCard = obj.GetComponent<Card_Edit>();
            editCard.Init();
            editCard.SetTexture(card.cardID, true);
            if (isMain)
                mainDeckList.Add(obj);
            else
                extraDeckList.Add(obj);
        }
        else
        {
            GameObject obj = waitToUseList[0];
            waitToUseList.RemoveAt(0);
            Card_Edit editCard = obj.GetComponent<Card_Edit>();
            obj.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            editCard.SetTexture(card.cardID, true);
            obj.transform.SetParent(target);
            if (isMain)
                mainDeckList.Add(obj);
            else
                extraDeckList.Add(obj);
        }
    }

    /// <summary>
    /// 排序卡组
    /// </summary>
    public void SortDeck()
    {
        curDeck.SortDeck();
        Group mainGroup = curDeck.mainDeck;
        Group extraGroup = curDeck.extraDeck;
        for (int i = 0; i < mainGroup.cardList.Count; i++)
        {
            Card_Edit card = mainDeckList[i].GetComponent<Card_Edit>();
            card.SetTexture(mainGroup.GetCard(i).cardID, true);
        }
        for (int i = 0; i < extraGroup.cardList.Count; i++)
        {
            Card_Edit card = extraDeckList[i].GetComponent<Card_Edit>();
            card.SetTexture(extraGroup.GetCard(i).cardID, true);
        }
    }

    /// <summary>
    /// 移除卡片
    /// </summary>
    public void RemoveCard(GameObject obj)
    {
        if (mainDeckList.Contains(obj))
        {
            for (int i = 0; i < mainDeckList.Count; i++)
            {
                if (mainDeckList[i] == obj)
                {
                    mainDeckList.RemoveAt(i);
                    curDeck.mainDeck.RemoveCard(i);
                    break;
                }
            }
        }
        else if (extraDeckList.Contains(obj))
        {
            for (int i = 0; i < extraDeckList.Count; i++)
            {
                if (extraDeckList[i] == obj)
                {
                    extraDeckList.RemoveAt(i);
                    curDeck.extraDeck.RemoveCard(i);
                    break;
                }
            }
        }
        else
        {
            Debug.Log("error");
        }
        obj.transform.SetParent(this.transform);
        obj.transform.position = new Vector3(1000, 1000, 1000);
        waitToUseList.Add(obj);
        UpdateText();
    }

    int GetObjVal(GameObject obj, bool isMain)
    {
        if (isMain)
        {
            for (int i = 0; i < mainDeckList.Count; i++)
            {
                if (mainDeckList[i] == obj)
                    return i;
            }
        }
        else
        {
            for (int i = 0; i < extraDeckList.Count; i++)
            {
                if (extraDeckList[i] == obj)
                    return i;
            }
        }
        return -1;
    }

    private void UpdateText()
    {
        mainDeckText.text = "主卡组:" + curDeck.mainDeck.GroupNum.ToString();
        extraDeckText.text = "额外卡组:" + curDeck.extraDeck.GroupNum.ToString();
    }
}
