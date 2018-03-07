using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class Card_Field : BaseCard,IPointerExitHandler
{

    Text AfkText;
    Text LevelText;
    Text DefText;
    Text SpritText;

    bool isShowMes;


    public int myPutType;

    private AttackAnimUI attackAnimUI;

    public bool isInSelect;
    public bool isSelect;

    private DashedAnim selectAnim;

    private int curArea;
    private int curRank;

    public bool isMy;

    List<RawImage> xyzImageList = new List<RawImage>();

    private RawImage CreateImage(int i)
    {
        RawImage obj = Instantiate(image);
        obj.transform.SetParent(transform);
        obj.rectTransform.localEulerAngles = Vector3.zero;
        obj.rectTransform.anchoredPosition3D = new Vector3(-4+i * -4, 0, 0);
        obj.rectTransform.localScale = Vector3.one;
        obj.transform.SetAsFirstSibling();
        return obj;
    }


    public void Init(FieldMgr field, bool _isMy)
    {
        fieldMgr = field;
        isMy = _isMy;
        image = transform.FindChild("cardImage").GetComponent<RawImage>();

        attackAnimUI = transform.FindChild("attackAnimUI").GetComponent<AttackAnimUI>();
        selectAnim = transform.FindChild("selectAnim").GetComponent<DashedAnim>();

        foreach (Text t in this.GetComponentsInChildren<Text>())
        {
            if (t.name == "AfkText")
                AfkText = t;
            if (t.name == "DefText")
                DefText = t;
            if (t.name == "LevelText")
                LevelText = t;
            if (t.name == "SpritText")
                SpritText = t;
        }
        attackAnimUI.Init(isMy);
        HideAttackAnim();
        isInSelect = false;
        isSelect = false;
        AfkText.text = "";
        DefText.text = "";
        LevelText.text = "";
        SpritText.text = "";
    }

    public void ShowAttackAnim()
    {
        attackAnimUI.ShowAnim();
    }

    public void HideAttackAnim()
    {
        attackAnimUI.HideAnim();
    }

    public void ShowXYZMaterial(List<string> idList)
    {
        if (xyzImageList.Count < idList.Count)
        {
            for (int i = xyzImageList.Count; i < idList.Count; i++)
            {
               xyzImageList.Add( CreateImage(i));
            }
        }
        else if (xyzImageList.Count > idList.Count)
        {
            for (int i = xyzImageList.Count-1; i > idList.Count-1; i--)
            {
                GameObject obj = xyzImageList[i].gameObject;
                Destroy(obj);
                xyzImageList.RemoveAt(i);
            }
        }
        for (int i = 0; i < xyzImageList.Count; i++)
        {
            xyzImageList[i].texture = StaticMethod.GetCardPics(idList[i], true);
        }
    }

    /// <summary>
    /// 设置放置的方式
    /// </summary>
    /// <param name="cardPutType"></param>
    public void SetPutType(int cardPutType, string cardID)
    {
        switch (cardPutType)
        {
            case ComVal.CardPutType_UpRightFront:
                image.rectTransform.localEulerAngles = new Vector3(0, 0, 0);
                selectAnim.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 0, 0);
                image.texture = StaticMethod.GetCardPics(cardID, true);
                break;
            case ComVal.CardPutType_UpRightBack:
                image.rectTransform.localEulerAngles = new Vector3(0, 180, 0);
                selectAnim.GetComponent<RectTransform>().localEulerAngles = new Vector3(0, 180, 0);
                image.texture = coverTexture;
                break;
            case ComVal.CardPutType_layFront:
                image.rectTransform.localEulerAngles = new Vector3(180, 0, 90);
                selectAnim.GetComponent<RectTransform>().localEulerAngles = new Vector3(180, 0, 90);
                image.texture = StaticMethod.GetCardPics(cardID, true);
                break;
            case ComVal.CardPutType_layBack:
                image.rectTransform.localEulerAngles = new Vector3(180, 0, 90);
                selectAnim.GetComponent<RectTransform>().localEulerAngles = new Vector3(180, 0, 90);
                image.texture = coverTexture;
                break;
        }
        myPutType = cardPutType;
    }


    /// <summary>
    /// 用于怪兽卡显示自己的攻击力，防守与星阶
    /// </summary>
    public void UpdateCardMes(int curAfk, int curDef, int level, int afk, int def)
    {
        if (!isShowMes)
        {
            ShowCardMes();
        }
        ChangeTextColor(curAfk, afk, AfkText);
        ChangeTextColor(curDef, def, DefText);
        AfkText.text = curAfk.ToString();
        DefText.text = curDef.ToString();
        SpritText.text = "/";
        LevelText.text = "L" + level.ToString();
    }

    //private void UpdatePutType()
    //{
    //    if (myPutType == ComVal.CardPutType_layBack || myPutType == ComVal.CardPutType_layFront)
    //    {
    //        objRectTransform.localEulerAngles = new Vector3(0, 180, -90);
    //    }
    //    else
    //    {
    //        objRectTransform.localEulerAngles = new Vector3(0, 0, 0);
    //    }
    //}

    public void HideCardMes()
    {
        isShowMes = false;
        AfkText.gameObject.SetActive(false);
        DefText.gameObject.SetActive(false);
        SpritText.gameObject.SetActive(false);
        LevelText.gameObject.SetActive(false);
    }

    public void ShowCardMes()
    {
        isShowMes = true;
        AfkText.gameObject.SetActive(true);
        DefText.gameObject.SetActive(true);
        SpritText.gameObject.SetActive(true);
        LevelText.gameObject.SetActive(true);
    }

    void ChangeTextColor(int curval, int val, Text text)
    {
        if (curval > val)
            text.color = Color.green;
        else if (curval == val)
            text.color = Color.white;
        else
            text.color = Color.red;
    }

    /// <summary>
    /// 通过父节点的名字判断是否为怪兽区域
    /// </summary>
    /// <returns></returns>
    public FieldUIType GetUIAreaType()
    {
        if (this.transform.parent.name == "monsterGrid")
        {
            return FieldUIType.monster;
        }
        else if (this.transform.parent.name == "trapGrid")
        {
            return FieldUIType.trap;
        }
        else
        {
            return FieldUIType.areaSpell;
        }
    }

    public bool IsInTrapArea()
    {
        if (this.transform.parent.name == "trapGrid")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 获取当前的位置
    /// </summary>
    /// <returns></returns>
    public override int GetCard()
    {
        return curRank;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DuelEventSys.GetInstance.SendEvent(DuelEvent.uiEvent_HideFieldCardMes);
    }

    void OnMouseOver()
    {
        DuelEventSys.GetInstance.OnOver_updateSelectCardShow(id);
        DuelEventSys.GetInstance.SendEvent(DuelEvent.uiEvent_ShowFieldCardMes,isMy, curArea, curRank,  transform.parent.GetComponent<RectTransform>().anchoredPosition);
        if(!Input.GetMouseButtonDown(0))
        {
            return;
        }
        
        if (isInSelect)
        {
            if (isSelect)
            {
                if (fieldMgr.DisSelectCard(this))
                {
                    SetDisSelect();
                }
            }
            else
            {
                if (fieldMgr.SelectCard(this))
                {
                    SetSelect();
                }
            }
        }

        optionListUI.ShowOptionList(curArea, curRank, rectTransform.position, isMy);
    }

    /// <summary>
    /// 进入选择状态
    /// </summary>
    public void EnterSelectState()
    {
        isInSelect = true;
        isSelect = false;
        selectAnim.StartSelectState();
    }
    /// <summary>
    /// 结束选择状态
    /// </summary>
    public void EndSelectState()
    {
        isInSelect = false;
        selectAnim.EndSelectState();
    }

    public void SetSelect()
    {
        selectAnim.SetSelect();
        isSelect = true;
    }

    public void SetDisSelect()
    {
        selectAnim.SetNotSelect();
        isSelect = false;
    }

    public void SetArea(int area, int rank)
    {
        curArea = area;
        curRank = rank;
    }

    public void ShowDashAnim()
    {
        selectAnim.StartSelectState();
    }

    public void HideDashAnim()
    {
        selectAnim.EndSelectState();
    }

}
