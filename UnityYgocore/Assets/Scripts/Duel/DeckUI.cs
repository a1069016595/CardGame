using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
/// <summary>
/// 管理卡组的显示
/// </summary>
public class DeckUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Text deckNumText;

    public RawImage rawImage;

    public Texture coverTexture;

    public GameObject cardCube;

    List<GameObject> cardCubeList;

    public int cardNum;

    public Dictionary<GameObject, string> cardCubeDic;
    /// <summary>
    /// false表示为卡牌朝上
    /// </summary>
    public bool isCover;

    public bool isMine;

    public float cardCubeDepth;

    OptionListUI optionListUI;
    RectTransform rectTransform;

    public ActImage actImage;
    void Awake()
    {
        optionListUI = OptionListUI.GetInstance();
        rectTransform = GetComponent<RectTransform>();
        cardCubeList = new List<GameObject>();
        if (!isCover)
            cardCubeDic = new Dictionary<GameObject, string>();

        deckNumText = transform.FindChild("Text").GetComponent<Text>();
        rawImage = GetComponent<RawImage>();
        coverTexture = Resources.Load("Prefebs/cover") as Texture;
        if (isMine)
            cardCube = Resources.Load("Prefebs/cardCube") as GameObject;
        else
            cardCube = Resources.Load("Prefebs/cardCube_Small") as GameObject;
        cardNum = 0;
        Transform theAct = transform.FindChild("ActImage");
        if (theAct != null)
        {
            actImage = theAct.GetComponent<ActImage>();
            actImage.gameObject.SetActive(false);
        }
     
        UpdateText();
        
    }

    public void InitDeck(int num)
    {
        for (int i = 0; i < num; i++)
        {
            cardCubeList.Add(InitCardCube(-i * cardCubeDepth));
        }
        cardNum = num;
        if (cardNum == 0)
        {
            deckNumText.text = "";
        }
        UpdateText();
    }


    public void AddCard(Group group)
    {
        for (int i = 0; i < group.GroupNum; i++)
        {
            Card card = group.GetCard(i);
            AddCard(card, 0);
        }
        UpdateText();
    }


    public void RemoveCard(Card card, int num)
    {

        if (isCover)
        {
            for (int i = 0; i < num; i++)
            {
                GameObject obj = cardCubeList[cardCubeList.Count - 1];
                cardCubeList.RemoveAt(cardCubeList.Count - 1);
                Destroy(obj);
                cardNum--;
            }
        }
        else
        {
            cardNum--;
            GameObject obj = new GameObject();
            foreach (var item in cardCubeDic)
            {
                if (item.Value == card.cardID)
                {
                    obj = item.Key;
                    break;
                }
            }
            cardCubeDic.Remove(obj);
            cardCubeList.Remove(obj);
            Destroy(obj);
            //移除卡片后要重新排序
            for (int i = 0; i < cardCubeList.Count; i++)
            {
                cardCubeList[i].transform.localPosition = new Vector3(0, 0, -i * cardCubeDepth);
            }
        }
        UpdateText();
    }


    public void RemoveCard(Group group)
    {
        for (int i = 0; i < group.cardList.Count; i++)
        {
            Card card = group.cardList[i];
            RemoveCard(card, 0);
        }
        UpdateText();
    }

    GameObject InitCardCube(float Z)
    {
        GameObject obj = Instantiate(cardCube);

        obj.transform.SetParent(transform);
        obj.transform.localScale = cardCube.transform.localScale;
        obj.transform.localPosition = new Vector3(0, 0, Z);
        obj.transform.localRotation = cardCube.transform.localRotation;
        return obj;
    }



    public void AddCard(Card card, int num)
    {
        if (isCover)
        {
            for (int i = 0; i < num; i++)
            {
                if (cardCubeList.Count == 0)
                {
                    cardCubeList.Add(InitCardCube(0));
                }
                else
                    cardCubeList.Add(InitCardCube(cardCubeList[cardCubeList.Count - 1].transform.localPosition.z - cardCubeDepth));
                cardNum++;
            }
        }
        else
        {
            Texture tex = StaticMethod.GetCardPics(card.cardID, true);
            GameObject obj = new GameObject();
            if (cardCubeList.Count == 0)
                obj = InitCardCube(0);
            else
                obj = InitCardCube(cardCubeList[cardCubeList.Count - 1].transform.localPosition.z - cardCubeDepth);
            obj.GetComponent<Renderer>().material.mainTexture = tex;
            cardCubeList.Add(obj);
            cardCubeDic.Add(obj, card.cardID);
            cardNum++;
        }
        UpdateText();
    }

    void UpdateText()
    {
        deckNumText.text = cardNum.ToString();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rawImage.color = new Color(1, 1, 1, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rawImage.color = new Color(1, 1, 1, 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.name == "ExtraDeck")
            optionListUI.ShowOptionList(ComVal.Area_Extra, -1, rectTransform.position, isMine);
        else if (this.name == "MainDeck")
            optionListUI.ShowOptionList(ComVal.Area_MainDeck, -1, rectTransform.position, isMine);
        else if (this.name == "GraveyardDeck")
            optionListUI.ShowOptionList(ComVal.Area_Graveyard, -1, rectTransform.position, isMine);
        else if (this.name == "RemoveDeck")
            optionListUI.ShowOptionList(ComVal.Area_Remove, -1, rectTransform.position, isMine);
        else
            Debug.Log("error");
    }

    public Vector3 GetChainPos()
    {
        return new Vector3(rectTransform.anchoredPosition3D.x, rectTransform.anchoredPosition3D.y,rectTransform.anchoredPosition3D.z+( -cardCubeList.Count * cardCubeDepth-0.5f));
    }


    public void ShowAnim()
    {
        actImage.StartAnim();
    }

    public void HideAnim()
    {
        actImage.EndAnim();
    }
}

