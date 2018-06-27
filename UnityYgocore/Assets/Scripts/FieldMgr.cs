using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum FieldUIType
{
    monster = ComVal.Area_Monster,
    trap = ComVal.Area_NormalTrap,
    areaSpell = ComVal.Area_FieldSpell,
}

public class FieldMgr : MonoBehaviour
{

    bool isMy;

    #region 变量
    public List<GameObject> monsterGridList;
    public List<GameObject> trapGridList;
    public GameObject fieldSpellGrid;

    private GameObject cardPrefeb;

    public FieldCardArray monsterArray;
    public FieldCardArray trapArray;
    private GameObject fieldSpellCard;

    SelectCardMgr selectCardMgr;

    #endregion


    public void Init(bool _isMy)
    {
        selectCardMgr = SelectCardMgr.GetInstance();
        isMy = _isMy;
        monsterArray = new FieldCardArray(5);
        trapArray = new FieldCardArray(5);
        cardPrefeb = Resources.Load("Prefebs/prefeb_fieldCard") as GameObject;
    }

    #region 选择场上卡片

    int GetCardRank(Card_Field card, FieldUIType type)
    {
        switch (type)
        {
            case FieldUIType.monster:
                return monsterArray.GetRank(card.gameObject);
            case FieldUIType.trap:
                return trapArray.GetRank(card.gameObject);
            case FieldUIType.areaSpell:
                return 0;
            default:
                return -1;
        }
    }

    /// <summary>
    /// 进入选择卡片状态
    /// </summary>
    /// <param name="cardType"></param>
    /// <param name="num"></param>
    /// <param name="dele"></param>
    public void SelectFieldCard(Group cardGroup,bool isMySelect)
    {
        if (cardGroup.GroupNum == 0)
        {
            return;
        }
        for (int i = 0; i < cardGroup.GroupNum; i++)
        {
            Card card = cardGroup.GetCard(i);
            GameObject obj = GetCardObj(card);
            if (obj == null)
            {
                Debug.Log("error");
                return;
            }
            Card_Field cardField = obj.GetComponent<Card_Field>();
            cardField.EnterSelectState(isMySelect);
        }
    }

    /// <summary>
    /// 结束卡片选择状态
    /// </summary>
    public void EndSelectCard()
    {

        foreach (var item in monsterArray.gameObjList)
        {
            if (item != null)
            {
                Card_Field card = item.GetComponent<Card_Field>();
                card.EndSelectState();
            }
        }
        foreach (var item in trapArray.gameObjList)
        {
            if (item != null)
            {
                Card_Field card = item.GetComponent<Card_Field>();
                card.EndSelectState();
            }
        }
    }

    /// <summary>
    /// 由card_field调用
    /// </summary>
    /// <param name="card"></param>
    public bool SelectCard(Card_Field card)
    {
        switch (card.GetUIAreaType())
        {
            case FieldUIType.monster:
                return selectCardMgr.SelectCard(ComVal.Area_Monster, GetCardRank(card, FieldUIType.monster), isMy);
            case FieldUIType.trap:
                return selectCardMgr.SelectCard(ComVal.Area_NormalTrap, GetCardRank(card, FieldUIType.trap), isMy);
            case FieldUIType.areaSpell:
                return selectCardMgr.SelectCard(ComVal.Area_FieldSpell, GetCardRank(card, FieldUIType.areaSpell), isMy);
            default:
                Debug.Log("error");
                return false;
        }
    }

    public bool DisSelectCard(Card_Field card)
    {
        switch (card.GetUIAreaType())
        {
            case FieldUIType.monster:
                return selectCardMgr.DisSelectCard(ComVal.Area_Monster, GetCardRank(card, FieldUIType.monster), isMy);
            case FieldUIType.trap:
                return selectCardMgr.DisSelectCard(ComVal.Area_NormalTrap, GetCardRank(card, FieldUIType.trap), isMy);
            case FieldUIType.areaSpell:
                return selectCardMgr.DisSelectCard(ComVal.Area_FieldSpell, GetCardRank(card, FieldUIType.areaSpell), isMy);
            default:
                Debug.Log("error");
                return false;
        }
    }

    #endregion

    /// <summary>
    /// 获取obj
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    public GameObject GetCardObj(Card card)
    {
        if (card.curArea == ComVal.Area_Monster)
        {
            return monsterArray.GetCard(card.areaRank);
        }
        else if (card.curArea == ComVal.Area_NormalTrap)
        {
            return trapArray.GetCard(card.areaRank);
        }
        else if (card.curArea == ComVal.Area_FieldSpell)
        {
            return fieldSpellCard;
        }
        else
        {
            Debug.Log("error");
            return null;
        }
    }

    /// <summary>
    /// 怪兽区域满了返回真
    /// </summary>
    /// <returns></returns>
    private bool MonsterAreaIsFull()
    {
        for (int i = 0; i < monsterGridList.Count; i++)
        {
            if (monsterGridList[i].transform.childCount == 0)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    ///  魔陷区域满了返回真
    /// </summary>
    /// <returns></returns>
    private bool TrapAreaIsFull()
    {
        for (int i = 0; i < trapGridList.Count; i++)
        {
            if (trapGridList[i].transform.childCount == 0)
            {
                return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 加入卡片到怪兽魔陷区
    /// <para>返回卡片的位置</para>
    /// <para>返回值为0到4</para>
    /// </summary>
    /// <param name="cardID"></param>
    /// <param name="putType"></param>
    /// <param name="gridID"></param>
    /// <param name="isInMonsterArea">真为放置在怪兽区域 否则为魔陷区域</param>
    public int AddCard(string cardID, int putType, FieldUIType fieldType)
    {
        int val = 5;//记录卡片的位置
        Card_Field prefeb = GameObject.Instantiate(cardPrefeb).GetComponent<Card_Field>();
        prefeb.Init(this, isMy);

        switch (fieldType)
        {
            case FieldUIType.monster:
                if (MonsterAreaIsFull())
                {
                    Debug.Log("MonsterIsFull");
                    return 0;
                }
                for (int i = 0; i < monsterGridList.Count; i++)
                {
                    if (monsterGridList[i].transform.childCount == 0)
                    {
                        prefeb.transform.SetParent(monsterGridList[i].transform);
                        monsterArray.AddCard(prefeb.gameObject, i);
                        val = i;
                        break;
                    }
                }
                break;
            case FieldUIType.trap:
                if (TrapAreaIsFull())
                {
                    Debug.Log("TrapIsFull");
                    return 0;
                }
                for (int i = 0; i < trapGridList.Count; i++)
                {
                    if (trapGridList[i].transform.childCount == 0)
                    {
                        prefeb.transform.SetParent(trapGridList[i].transform);
                        trapArray.AddCard(prefeb.gameObject, i);

                        val = i;
                        break;
                    }
                }
                break;
            case FieldUIType.areaSpell:
                prefeb.transform.SetParent(fieldSpellGrid.transform);
                fieldSpellCard = prefeb.gameObject;
                val = 0;
                break;
            default:
                break;
        }
        prefeb.rectTransform.anchoredPosition3D = Vector3.zero;
        //  prefeb.rectTransform.sizeDelta = new Vector2(cardSizeX, cardSizeY);
        prefeb.rectTransform.localScale = Vector3.one;
        prefeb.rectTransform.localRotation = new Quaternion(0, 0, 0, 1);
        prefeb.SetTexture(cardID, true);
        prefeb.SetPutType(putType, cardID);
        prefeb.SetArea((int)fieldType, val);
        return val;
    }

    public int GetCardRank(bool isInMonsterArea)
    {
        if (isInMonsterArea)
        {
            if (MonsterAreaIsFull())
            {
                Debug.Log("error");
                return 0;
            }
            for (int i = 0; i < monsterGridList.Count; i++)
            {
                if (monsterGridList[i].transform.childCount == 0)
                {
                    return i;
                }
            }
        }
        else
        {
            if (TrapAreaIsFull())
            {
                Debug.Log("error");
                return 0;
            }
            for (int i = 0; i < trapGridList.Count; i++)
            {
                if (trapGridList[i].transform.childCount == 0)
                {
                    return i;
                }
            }
        }
        Debug.Log("error");
        return 0;
    }

    /// <summary>
    /// 更新场地上怪兽卡信息的显示
    /// </summary>
    /// <param name="monsterGroup"></param>
    public void UpdateCardMesShow(FieldCardGroup monsterGroup)
    {
        Card[] cardList = monsterGroup.cardList;
        for (int i = 0; i < cardList.Length; i++)
        {
            Card card = cardList[i];
            if (card != null)
            {

                Card_Field card_field = monsterArray.GetCard(i).GetComponent<Card_Field>();
                if (card.IsMonster() && card.curPlaseState != ComVal.CardPutType_layBack)
                {
                    card_field.UpdateCardMes(card.GetCurAfk(), card.GetCurDef(), card.GetCurLevel(), card.afk, card.def);
                }
                else if (card.curPlaseState == ComVal.CardPutType_layBack)
                {
                    card_field.HideCardMes();
                }
            }
        }
    }

    public void UpdateXYZCard(List<string> materialList, int rank)
    {
        monsterArray.GetCard(rank).GetComponent<Card_Field>().ShowXYZMaterial(materialList);
    }

    /// <summary>
    /// 跟新卡片攻击动画的显示
    /// </summary>
    /// <param name="monsterGroup"></param>
    public void UpdateCardAttackAnimShow(FieldCardGroup monsterGroup)
    {
        Card[] cardList = monsterGroup.cardList;
        for (int i = 0; i < cardList.Length; i++)
        {
            Card card = cardList[i];
            if (card != null)
            {
                if (card.IsMonster())
                {
                    if (card.CanAttack())
                    {
                        monsterArray.GetCard(i).GetComponent<Card_Field>().ShowAttackAnim();
                    }
                    else
                    {
                        monsterArray.GetCard(i).GetComponent<Card_Field>().HideAttackAnim();
                    }
                }
            }
        }
    }
    /// <summary>
    /// 隐藏攻击动画
    /// </summary>
    /// <param name="monsterGroup"></param>
    public void HideCardAttackAnim(FieldCardGroup monsterGroup)
    {
        Card[] cardList = monsterGroup.cardList;
        for (int i = 0; i < cardList.Length; i++)
        {
            Card card = cardList[i];
            if (card != null)
            {
                if (card.IsMonster())
                {
                    monsterArray.GetCard(i).GetComponent<Card_Field>().HideAttackAnim();
                }
            }
        }
    }

    /// <summary>
    /// 改变怪兽卡摆放形式
    /// </summary>
    /// <param name="val"></param>
    /// <param name="type"></param>
    public void ChangeMonsterPlaseType(int val, int type, string cardID)
    {
        GameObject obj = monsterArray.GetCard(val);
        obj.GetComponent<Card_Field>().SetPutType(type, cardID);
    }

    public void ChangeTrapPlaseType(int val, int type, string cardID)
    {
        GameObject obj = trapArray.GetCard(val);
        obj.GetComponent<Card_Field>().SetPutType(type, cardID);
    }

    /// <summary>
    /// 移除卡片
    /// </summary>
    /// <param name="card"></param>
    public void RemoveCard(Card card)
    {
        GameObject obj = GetCardObj(card);
        if (card.curArea == ComVal.Area_Monster)
        {
            monsterArray.RemoveCard(card.areaRank);
        }
        else if (card.curArea == ComVal.Area_NormalTrap)
        {
            trapArray.RemoveCard(card.areaRank);
        }
        else
        {
            fieldSpellCard = null;
        }
        Destroy(obj);
    }

    public void ShowDashAnim(List<int> list, FieldUIType type,bool isMySelect)
    {
        switch (type)
        {
            case FieldUIType.monster:
                for (int i = 0; i < list.Count; i++)
                {
                    int val = list[i];
                    monsterArray.GetCard(val).GetComponent<Card_Field>().ShowDashAnim(isMySelect);
                }
                break;
            case FieldUIType.trap:
                for (int i = 0; i < list.Count; i++)
                {
                    int val = list[i];
                    trapArray.GetCard(val).GetComponent<Card_Field>().ShowDashAnim(isMySelect);
                }
                break;
            case FieldUIType.areaSpell:
                if (list.Count > 0)
                {
                    fieldSpellCard.GetComponent<Card_Field>().ShowDashAnim(isMySelect);
                }
                break;
            default:
                break;
        }
    }

    public void HideDashAnim()
    {
        foreach (var item in monsterArray.gameObjList)
        {
            if (item != null)
            {
                item.GetComponent<Card_Field>().HideDashAnim();
            }
        }
        foreach (var item in trapArray.gameObjList)
        {
            if (item != null)
            {
                item.GetComponent<Card_Field>().HideDashAnim();
            }
        }
        if (fieldSpellCard != null)
        {
            fieldSpellCard.GetComponent<Card_Field>().HideDashAnim();
        }
    }

    public Vector3 GetAreaPos(int area, int rank)
    {
        if (area == ComVal.Area_Monster)
        {
            return monsterGridList[rank].GetComponent<RectTransform>().anchoredPosition;
        }
        else if (area == ComVal.Area_NormalTrap)
        {
            return trapGridList[rank].GetComponent<RectTransform>().anchoredPosition;
        }
        else if (area == ComVal.Area_FieldSpell)
        {
            return fieldSpellGrid.GetComponent<RectTransform>().anchoredPosition;
        }
        else
        {
            Debug.Log("error");
            return Vector3.zero;
        }
    }
}

/// <summary>
/// 场地上的卡片集合
/// </summary>
public class FieldCardArray
{
    public GameObject[] gameObjList;

    public FieldCardArray(int length)
    {
        gameObjList = new GameObject[length];
    }

    public GameObject GetCard(int val)
    {
        GameObject obj = gameObjList[val];
        if (obj == null)
        {
            return null;
        }
        return obj;
    }

    public void AddCard(GameObject obj, int val)
    {
        gameObjList[val] = obj;
    }

    public GameObject RemoveCard(int val)
    {
        GameObject card = gameObjList[val];
        gameObjList[val] = null;
        return card;
    }

    public int GetRank(GameObject obj)
    {
        for (int i = 0; i < gameObjList.Length; i++)
        {
            if(gameObjList[i]!=null)
            {
                if(gameObjList[i]==obj)
                {
                    return i;
                }
            }
        }
        return -1;
    }

}
