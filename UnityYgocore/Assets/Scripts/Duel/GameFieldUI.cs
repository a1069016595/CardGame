using UnityEngine;
using System.Collections;
using System.Reflection;
using System;
using UnityEngine.EventSystems;

/// <summary>
/// 管理场地卡片组的显示
/// </summary>
public class GameFieldUI : MonoBehaviour
{


    public DeckUI ui_ExtraDeck;
    public DeckUI ui_MainDeck;
    public DeckUI ui_GraveyardDeck;
    public DeckUI ui_RemoveDeck;



    public void Awake()
    {

        ui_ExtraDeck = transform.FindChild("ExtraDeck").GetComponent<DeckUI>();
        ui_MainDeck = transform.FindChild("MainDeck").GetComponent<DeckUI>();
        ui_GraveyardDeck = transform.FindChild("GraveyardDeck").GetComponent<DeckUI>();
        ui_RemoveDeck = transform.FindChild("RemoveDeck").GetComponent<DeckUI>();

    }
    /// <summary>
    /// 生成卡组
    /// </summary>
    /// <param name="mainNum"></param>
    /// <param name="extraNum"></param>
    public void InitDeck(int mainNum, int extraNum)
    {
        ui_MainDeck.InitDeck(mainNum);
        ui_ExtraDeck.InitDeck(extraNum);
    }

    public void AddCardToDeck(int deckArea,Group group)
    {
        GetDeckUI(deckArea).AddCard(group);
    }

    public void RemoveCardFromDeck(int deckArea, Group group)
    {
        GetDeckUI(deckArea).RemoveCard(group);
    }

    public void RemoveCardFromDeck(int deckArea, Card card,int num)
    {
        GetDeckUI(deckArea).RemoveCard(card,num);
    }

    public void ShowDeckAct(int deckArea)
    {
        GetDeckUI(deckArea).ShowAnim();
    }

    public void HideDeckAct(int deckArea)
    {
        GetDeckUI(deckArea).HideAnim();
    }

    DeckUI GetDeckUI(int i)
    {
        DeckUI result = null;
        switch (i)
        {
            case ComVal.Area_Extra:
                result= ui_ExtraDeck;
                break;
            case ComVal.Area_Graveyard:
                result = ui_GraveyardDeck;
                break;
            case ComVal.Area_MainDeck:
                result = ui_MainDeck;
                break;
            case ComVal.Area_Remove:
                result = ui_RemoveDeck;
                break;
            default:
                Debug.Log("error:"+i);
                break;
        }
        return result;
    }


    public void AddCardToDeck(int deckArea, Card card, int num)
    {
        GetDeckUI(deckArea).AddCard(card,num);
    }

    public Vector3 GetAreaPos(int area)
    {
        switch (area)
        {
            case ComVal.Area_Graveyard:
                return ui_GraveyardDeck.GetChainPos();
            case ComVal.Area_Remove:
                return ui_RemoveDeck.GetChainPos();
            default:
               
                break;
        }
        Debug.Log("error");
        return Vector3.zero;
    }
}



 

