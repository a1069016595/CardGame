using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
public class Card_Hand : BaseCard, IPointerExitHandler
{
    ///public RectTransform rectTransform;

    bool isMy;

    float posY;

    bool isMove = false;

    DashedAnim HandCardAnim;

    private bool isInSelect;
    private bool isSelect;

    public void Init(HandCardUI _handCardUI, bool _isMy)
    {
        isMy = _isMy;
        handCardUI = _handCardUI;
        image = this.GetComponent<RawImage>();
        handCardUI = this.transform.parent.GetComponent<HandCardUI>();
        HandCardAnim = transform.FindChild("HandCardAnim").GetComponent<DashedAnim>();
        posY = rectTransform.localPosition.y;
    }

    public override int GetCard()
    {
        int i = handCardUI.GetCardRank(this.gameObject);
        return i;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (isInSelect)
        {
            if (!isSelect)
            {
                if (handCardUI.SelectCard(this))
                {
                    SetSelect();
                }
            }
            else
            {
                if (handCardUI.DisSelectCard(this))
                {
                    SetDisSelect();
                }
            }
        }
        //if (!isMy)
        //    return;
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            return;
        }
        int i = GetCard();
        if (i != -1)
        {
            optionListUI.ShowOptionList(ComVal.Area_Hand, i, rectTransform.position, isMy);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (isMove)
            return;
        isMove = true;

        DuelEventSys.GetInstance.OnOver_updateSelectCardShow(id);
        Tweener tw = rectTransform.DOLocalMoveY(posY + 15, 0.05f);
        tw.SetEase(Ease.Linear);
        tw.onComplete = delegate
        {
            isMove = false;
        };
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartCoroutine(a());
    }

    IEnumerator a()
    {
        while (isMove)
        {
            yield return null;
        }
        isMove = true;
        Tweener tw = rectTransform.DOLocalMoveY(posY, 0.05f);
        tw.SetEase(Ease.Linear);
        tw.onComplete = delegate
        {
            isMove = false;
        };
    }

    public void ShowDashAnim()
    {
        HandCardAnim.StartSelectState();
    }

    public void HideDashAnim()
    {
        HandCardAnim.EndSelectState();
    }


    /// <summary>
    /// 进入选择状态
    /// </summary>
    public void EnterSelectState()
    {
        isInSelect = true;
        HandCardAnim.StartSelectState();
    }
    /// <summary>
    /// 结束选择状态
    /// </summary>
    public void EndSelectState()
    {
        isSelect = false;
        isInSelect = false;
        HandCardAnim.EndSelectState();
    }


    public void SetSelect()
    {
        HandCardAnim.SetSelect();
        isSelect = true;
    }

    public void SetDisSelect()
    {
        HandCardAnim.SetNotSelect();
        isSelect = false;
    }

}
