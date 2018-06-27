using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using Protocol;

public delegate void GroupCardSelectBack(Group theGroup );
//所有卡片的prefeb开始时便生成
public class SelectCardUI : DuelUIOpearate
{
    #region 单例
    private static SelectCardUI instance;

    public SelectCardUI()
    {
        instance = this;
    }

    public static SelectCardUI GetInstance()
    {
        return instance;
    }
    #endregion

    public Button leftButton;
    public Button rightButton;
    public Button applyButton;
    public RectTransform layoutGroup;
    public Text text;
    public GameObject cardPrefeb;

    /// <summary>
    /// 当前所有的卡片集合
    /// </summary>
    public Group currentCardGroup;
    /// <summary>
    /// 当前显示的卡片集合
    /// </summary>
    private Group curShowGroup;

    public int curPage;
    public int totalPage;
    public bool isMax;
    public List<Card_Select> cardList;
    public List<bool> choseList;

    public GroupCardSelectBack curCallBack;

    public int curSelectNum;
    public int curMaxSelectNum;
    RectTransform rectTransform;


    public void Init()
    {
        leftButton = this.transform.FindChild("leftButton").GetComponent<Button>();
        rightButton = this.transform.FindChild("rightButton").GetComponent<Button>();
        applyButton = this.transform.FindChild("applyButton").GetComponent<Button>();
        layoutGroup = this.transform.FindChild("layoutGroup").GetComponent<RectTransform>();
        text = this.transform.FindChild("text").GetComponent<Text>();
        rectTransform = transform.GetComponent<RectTransform>();
        cardPrefeb = Resources.Load("Prefebs/prefeb_selectUICard") as GameObject;
        cardList = new List<Card_Select>();
        curPage = 1;

        leftButton.onClick.AddListener(LeftButtonClick);
        rightButton.onClick.AddListener(RightButtonClick);
        applyButton.onClick.AddListener(ApplyButtonClick);


        InitCardGroup();
        this.gameObject.SetActive(false);

        AddHandler(DuelEvent.netEvent_ReciveSelectCardGroup, ReciveSelectCardGroup);
        AddHandler(DuelEvent.netEvent_ReciveSelectGroupCardCon, ReciveSelectCardGroupCon);

        AddHandler(DuelEvent.playBackEvent_SelectCardGroup, PlayBackSelectCardGrpup);

    }

    private void PlayBackSelectCardGrpup(params object[] args)
    {
        SelectCard((int)args[0]);
        HandleApplyButton();
    }

    private void ReciveSelectCardGroupCon(params object[] args)
    {
        DuelSelectGroupCardConDTO dto = (DuelSelectGroupCardConDTO)args[0];
        if (dto.CtrType == 1)
        {
            HandleTurnBack();
        }
        else if (dto.CtrType == 2)
        {
            HandleTurnForward();
        }
        else if (dto.CtrType == 3)
        {
            HandleApplyButton();
        }
        else
        {
            Debug.Log("error");
        }
    }

    private void ReciveSelectCardGroup(params object[] args)
    {
        DuelSelectGroupCardDTO dto = (DuelSelectGroupCardDTO)args[0];
        if (dto.isSelect)
        {
            SelectCard(dto.rank);
        }
        else
        {
            DisSelectCard(dto.rank);
        }
    }

    private void ApplyButtonClick()
    {
        if (curSelectNum == 0 && curMaxSelectNum != 0)
        {
            return;
        }

        if (CanNotControl())
        {
            return;
        }
        if (IsSendMes())
        {
            DuelEventSys.GetInstance.SendEvent(DuelEvent.netEvent_SendSelectGroupCardCon, 3);
        }

        HandleApplyButton();
    }

    private void HandleApplyButton()
    {
        if (isMax)
        {
            if (curSelectNum < curMaxSelectNum)
                return;
        }

        Tweener a = rectTransform.DOScaleY(0, 0.15f);
        a.SetEase(Ease.Linear);

        a.onComplete = delegate
        {
            Group theGroup = new Group();
            List<int> recordList = new List<int>();
            for (int i = 0; i < choseList.Count; i++)
            {
                if (choseList[i])
                {
                    Card card = currentCardGroup.GetCard(i);
                    theGroup.AddCard(card);
                    recordList.Add(i);
                }
            }
            this.gameObject.SetActive(false);
            Duel.GetInstance().SetNotSelect();

            eventSys.SendEvent(DuelEvent.duelEvent_RecordOperate, RecordEvent.recordEvent_SelectCardGroup, recordList);

            if (curCallBack != null)
            {
                curCallBack(theGroup);
            }
        };
    }

    /// <summary>
    /// 向后翻
    /// </summary>
    private void RightButtonClick()
    {
        if (totalPage == curPage)
        {
            return;
        }

        if (CanNotControl())
        {
            return;
        }
        if (IsSendMes())
        {
            DuelEventSys.GetInstance.SendEvent(DuelEvent.netEvent_SendSelectGroupCardCon, 1);
        }

        HandleTurnBack();
    }

    private void HandleTurnBack()
    {
        curPage++;
        curShowGroup = new Group();
        for (int i = (curPage - 1) * 10; i < curPage * 10; i++)
        {
            if (i >= currentCardGroup.GroupNum)
            {
                break;
            }
            curShowGroup.AddCard(currentCardGroup.GetCard(i));

        }
        Show(curShowGroup);
    }

    /// <summary>
    /// 向前翻
    /// </summary>
    private void LeftButtonClick()
    {
        if (curPage == 1)
        {
            return;
        }
        if (CanNotControl())
        {
            return;
        }
        if (IsSendMes())
        {
            DuelEventSys.GetInstance.SendEvent(DuelEvent.netEvent_SendSelectGroupCardCon, 2);
        }

        HandleTurnForward();
    }

    private void HandleTurnForward()
    {
        curPage--;
        curShowGroup = new Group();
        for (int i = (curPage - 1) * 10; i < curPage * 10; i++)
        {
            curShowGroup.AddCard(currentCardGroup.GetCard(i));
        }
        Show(curShowGroup);
    }

    private void InitCardGroup()
    {
        curShowGroup = new Group();
        for (int i = 0; i < 10; i++)
        {
            GameObject card = GameObject.Instantiate(cardPrefeb);
            card.transform.SetParent(layoutGroup.transform);
            card.transform.localScale = Vector3.one;
            card.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            Card_Select card_select = card.GetComponent<Card_Select>();
            card_select.Init(i);
            cardList.Add(card_select);
            card_select.SetActive(false);
            card_select.event_disSelectCard += card_select_event_disSelectCard;
            card_select.event_selectCard += card_select_event_selectCard;
        }
    }

    void card_select_event_selectCard(int num, Card_Select obj)
    {
        if (curMaxSelectNum != -1 && curSelectNum == curMaxSelectNum)
        {
            return;
        }
        if (CanNotControl())
        {
            return;
        }
        obj.SetSelect();
        int i = (curPage - 1) * 10 + num;
        choseList[i] = true;
        curSelectNum++;


        if (IsSendMes())
        {
            DuelEventSys.GetInstance.SendEvent(DuelEvent.netEvent_SendSelectCardGroup, true, i);
        }
    }

    private void SelectCard(int rank)
    {
        cardList[rank].SetSelect();
        choseList[rank] = true;
        curSelectNum++;
    }

    private void DisSelectCard(int rank)
    {
        cardList[rank].SetDisSelect();
        choseList[rank] = false;
        curSelectNum--;
    }

    void card_select_event_disSelectCard(int num, Card_Select obj)
    {
        if (CanNotControl())
        {
            return;
        }
        obj.SetDisSelect();
        int i = (curPage - 1) * 10 + num;
        choseList[i] = false;
        curSelectNum--;

        if (IsSendMes())
        {
            DuelEventSys.GetInstance.SendEvent(DuelEvent.netEvent_SendSelectCardGroup, false, i);
        }
    }

    public void ShowSelectCardUI(Group cardGroup, GroupCardSelectBack callBack, int num, bool isMy, bool isMax)
    {
        curPage = 1;
        curSelectNum = 0;
        this.isMax = isMax;
        foreach (var item in cardList)
        {
            item.SetDisSelect();
        }

        isMySelect = isMy;
        Duel.GetInstance().SetSelect();
        rectTransform.localScale = new Vector3(0.5f, 0, 1);
        Tweener a = rectTransform.DOScaleY(0.5f, 0.15f);
        a.SetEase(Ease.Linear);

        curMaxSelectNum = num;
        this.gameObject.SetActive(true);
        if ((cardGroup.GroupNum % 10) != 0)
        {
            totalPage = cardGroup.GroupNum / 10 + 1;
        }
        else
        {
            totalPage = cardGroup.GroupNum / 10;
        }
        curCallBack = callBack;
        currentCardGroup = cardGroup;
        curShowGroup = new Group();
        choseList = new List<bool>();

        for (int i = 0; i < currentCardGroup.GroupNum; i++)
        {
            choseList.Add(false);
        }

        if (currentCardGroup.GroupNum <= 10)
        {
            for (int i = 0; i < currentCardGroup.GroupNum; i++)
            {
                Card card = currentCardGroup.GetCard(i);
                curShowGroup.AddCard(card);
            }
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                Card card = currentCardGroup.GetCard(i);
                curShowGroup.AddCard(card);
            }
        }

        Show(curShowGroup);


    }

    private void UpdateText()
    {
        text.text = curPage.ToString() + "/" + totalPage.ToString();
    }

    /// <summary>
    /// 显示卡组
    /// </summary>
    /// <param name="cardGroup"></param>
    private void Show(Group cardGroup)
    {
        for (int i = 0; i < cardGroup.GroupNum; i++)
        {
            Card_Select card_select = cardList[i];
            Card card = cardGroup.GetCard(i);
            card_select.SetActive(true);
            card_select.SetText(card.curArea, card.areaRank);
            card_select.SetTexture(card.cardID, false);
        }
        for (int i = cardGroup.GroupNum; i < 10; i++)
        {
            Card_Select card_select = cardList[i];
            card_select.SetActive(false);
        }
        for (int i = (curPage - 1) * 10; i < curPage * 10; i++)
        {
            if (i >= choseList.Count)
            {
                break;
            }
            if (choseList[i])
            {
                cardList[i % 10].SetSelect();
            }
            else
            {
                cardList[i % 10].SetDisSelect();
            }
        }
        UpdateText();
    }
}