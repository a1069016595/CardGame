using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;

public delegate void GuessFirstCard_Dele(int val);

/// <summary>
/// 游戏开始前的猜先卡片
/// </summary>
public class GuessFirstCard : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler
{
    public int value;

    RectTransform rectTransform;
    Vector3 localVector;

    GuessFirstCard_Dele dele;

    public void Init(GuessFirstCard_Dele _dele,int val)
    {
        dele = _dele;
        value = val;
        rectTransform = this.GetComponent<RectTransform>();
        localVector = rectTransform.localPosition;

    }

    void  ShowUpAnim()
    {
        Tweener tweener = rectTransform.DOLocalMove(new Vector3(localVector.x,localVector.y+20,localVector.z), 0.15f);

        tweener.SetUpdate(true);
        tweener.SetEase(Ease.Linear);
    }

    void ShowDropAnim()
    {
        Tweener tweener = rectTransform.DOLocalMove(localVector, 0.15f);

        tweener.SetUpdate(true);
        tweener.SetEase(Ease.Linear);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowUpAnim();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ShowDropAnim();
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        dele(value);
    }

    public void SetToStartPos()
    {
        rectTransform.localPosition = localVector;
    }
}
