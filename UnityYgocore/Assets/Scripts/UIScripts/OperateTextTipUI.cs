using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class OperateTextTipUI : BaseMonoBehivour
{

    Text tipText;
    RectTransform rectTransform;

    void Awake()
    {
        AddHandler(DuelEvent.uiEvent_ShowOperateTip, ShowTipMes);
        tipText = transform.GetChild<Text>("Text");
        rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(0, 1,1);
    }

    private void ShowTipMes(params object[] args)
    {
        string mes = (string)args[0];
        tipText.text = mes;

        Tweener a = rectTransform.DOScaleX(1, 0.15f);
        a.SetEase(Ease.Linear);
        a.onComplete = delegate
        {
            StartCoroutine(HideText());
        };
    }

    IEnumerator HideText()
    {
        yield return new WaitForSeconds(2);
        Tweener a = rectTransform.DOScaleX(0, 0.15f);
        a.SetEase(Ease.Linear);
    }
}
