using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 卡牌的基类
/// <para>鼠标放在卡牌上时显示卡牌大图</para>
/// <para>给卡牌设置图片</para>
/// </summary>
public class BaseCard : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{

    public Texture coverTexture;

    public RawImage image;

    public bool isVisual = true;//对于玩家是否可见

    public string id;

    public RectTransform rectTransform;

    public FieldMgr fieldMgr;
    public HandCardUI handCardUI;
    public OptionListUI optionListUI;

    void Awake()
    {

        //gameFieldUI = GameFieldUI.GetInstance();
        rectTransform = this.GetComponent<RectTransform>();
        coverTexture = Resources.Load("Prefebs/cover") as Texture;


        optionListUI = OptionListUI.GetInstance();
    }

    /// <summary>
    /// 设置图片
    /// <para>同时设置id</para>
    /// </summary>
    /// <param name="id"></param>
    public void SetTexture(string _id, bool isSmall)
    {

        image.texture = StaticMethod.GetCardPics(_id, isSmall);
        id = _id;
    }

    public void SetOverTexture(string _id)
    {
        image.texture = StaticMethod.GetCardTexture("0", true) ;
        id = _id;
    }

   

    /// <summary>
    /// 设置为背面
    /// </summary>
    public void SetTexture()
    {
        image.texture = coverTexture;
    }

    /// <summary>
    /// 鼠标进入事件
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (isVisual)
        {
            DuelEventSys.GetInstance.SendEvent(DuelEvent.uiEvent_UpdateSelectCardShow,id);
        }
    }

    public virtual int GetCard()
    {
        return 0;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
       
    }

    public void SetActive(bool active)
    {
        this.gameObject.SetActive(active);
    }
}
