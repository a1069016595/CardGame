using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

//一页有7个prefeb
public class SearchResultCardUI : MonoBehaviour
{
    
    #region 单例
    private static SearchResultCardUI instance;

    public SearchResultCardUI()
    {
        instance = this;
    }

    public static SearchResultCardUI GetInstance()
    {
        return instance;
    }
    #endregion

    public Button leftButton;
    public Button rightButton;
    public Button applyButton;
    public RectTransform layoutGroup;
    public GameObject cardPrefeb;

    /// <summary>
    /// 当前所有的卡片集合
    /// </summary>
    public List<string> currentCardList;
    /// <summary>
    /// 当前显示的卡片集合
    /// </summary>
    private List<string> curShowList;

    public Text text;

    public int curPage;
    public int totalPage;
    public List<Card_SearchResult> cardList;

    public int prefebNum;

    public void Init()
    {
        leftButton = this.transform.FindChild("leftButton").GetComponent<Button>();
        rightButton = this.transform.FindChild("rightButton").GetComponent<Button>();
        text = transform.FindChild("text").GetComponent<Text>();

        layoutGroup = this.transform.FindChild("layoutGroup").GetComponent<RectTransform>();
        cardPrefeb = Resources.Load("Prefebs/prefeb_searchResultCard") as GameObject;
        cardList = new List<Card_SearchResult>();
        curPage = 1;

        leftButton.onClick.AddListener(LeftButtonClick);
        rightButton.onClick.AddListener(RightButtonClick);

        InitCardGroup();
        text.text = "0/0";
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
        curPage++;
        curShowList = new List<string>();
        for (int i = (curPage - 1) * 7; i < curPage * 7; i++)
        {
            if (i >= currentCardList.Count)
            {
                break;
            }
            curShowList.Add(currentCardList[i]);

        }
        Show(curShowList);
        UpdateText();
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
        curPage--;
        curShowList = new List<string>();
        for (int i = (curPage - 1) * 7; i < curPage * 7; i++)
        {
            curShowList.Add(currentCardList[i]);
        }
        Show(curShowList);
        UpdateText();
    }

    private void InitCardGroup()
    {
        curShowList = new List<string>();
        for (int i = 0; i < 7; i++)
        {
            GameObject card = GameObject.Instantiate(cardPrefeb);
            card.transform.SetParent(layoutGroup.transform);
            card.transform.localScale = Vector3.one;
            card.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            Card_SearchResult card_select = card.GetComponent<Card_SearchResult>();
            card_select.Init();
            cardList.Add(card_select);
            card_select.SetActive(false);
          
        }
    }

    /// <summary>
    /// 显示搜索到的卡片
    /// </summary>
    public void ShowSearchCard(List<string> list)
    {
       
        this.gameObject.SetActive(true);
        if ((list.Count % 7) != 0)
        {
            totalPage = list.Count / 7 + 1;
        }
        else
        {
            totalPage = list.Count / 7;
        }
        curPage = 1;
        currentCardList = list;
        curShowList = new List<string>();
      
        if (currentCardList.Count <= 7)
        {
            for (int i = 0; i < currentCardList.Count; i++)
            {
                string str = currentCardList[i];
                curShowList.Add(str);
            }
        }
        else
        {
            for (int i = 0; i < 7; i++)
            {
                string str = currentCardList[i];
                curShowList.Add(str);
            }
        }
       
        Show(curShowList);
        UpdateText();
    }

    /// <summary>
    /// 显示卡组
    /// </summary>
    /// <param name="cardList"></param>
    private void Show(List<string> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Card_SearchResult card_select = cardList[i];
            string str = list[i];
            card_select.SetActive(true);
            card_select.SetTexture(str, false);
            card_select.SetText(str);
        }
        for (int i = list.Count; i < 7; i++)
        {
            Card_SearchResult card_select = cardList[i];
            card_select.SetActive(false);
        }
      
    }

    private void UpdateText()
    {
        text.text = curPage.ToString() + "/" + totalPage.ToString();
    }
}
