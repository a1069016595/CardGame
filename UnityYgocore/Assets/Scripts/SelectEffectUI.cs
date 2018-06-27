using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using Protocol;
public delegate void SelectEffectCallBack(LauchEffect  effect);

public class SelectEffectUI : DuelUIOpearate
{
    #region 单例
    private static SelectEffectUI instance;

    public SelectEffectUI()
    {
        instance = this;
    }

    public static SelectEffectUI GetInstance()
    {
        return instance;
    }
    #endregion

    public Button applyButton;
    public Button leftButton;
    public Button rightButton;
    public Text effectText;

    private int curRank;
    private int length;

    public IntDele callBack;

    private List<string> describeList;

    RectTransform rectTransform;
    public void Init()
    {
        describeList = new List<string>();

        rightButton = transform.FindChild("rightButton").GetComponent<Button>();
        leftButton = transform.FindChild("leftButton").GetComponent<Button>();
        effectText = transform.FindChild("effectText").GetComponent<Text>();
        applyButton = transform.FindChild("applyButton").GetComponent<Button>();
        rectTransform = transform.GetComponent<RectTransform>();
        rightButton.onClick.AddListener(OnRightButton);
        leftButton.onClick.AddListener(OnLeftButton); 
        applyButton.onClick.AddListener(OnApplyButton);

        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(false);

        gameObject.SetActive(false);

        AddHandler(DuelEvent.netEvent_ReciveChangeSelectEffect, ReciveChangeSelectEffect);
        AddHandler(DuelEvent.netEvent_ReciveApplySelectEffect, ReciveApplySelectEffect);

        AddHandler(DuelEvent.playBackEvent_SelectEffect, PlayBackSelectEffect);
    }

    private void PlayBackSelectEffect(params object[] args)
    {
        curRank = (int)args[0];
        OperateApplyButton();
    }

    private void ReciveApplySelectEffect(params object[] args)
    {
        OperateApplyButton();
    }

    private void ReciveChangeSelectEffect(params object[] args)
    {
        DuelSelectEffectButtonDTO mes = (DuelSelectEffectButtonDTO)args[0];
        if (mes.isRight)
        {
            OperateRightButton();
        }
        else
        {
            OperateLeftButton();
        }
    }

    public void ShowSelectEffectDialogBox(IntDele dele, List<string> effectList, bool isMy)
    {
        isMySelect = isMy;
        Duel.GetInstance().SetSelect();
        rectTransform.localScale = new Vector3(1, 0, 1);
        Tweener a = rectTransform.DOScaleY(1, 0.1f);
        a.SetEase(Ease.Linear);
        gameObject.SetActive(true);
        describeList = effectList;
        curRank = 0;
        length = effectList.Count;

        callBack = dele;

        leftButton.gameObject.SetActive(false);
        rightButton.gameObject.SetActive(true);

        ShowText();
    }

    private void OnRightButton()
    {
        if (CanNotControl())
        {
            return;
        }
        if (curRank == length - 1)
        {
            return;
        }
        if (IsSendMes())
        {
            DuelEventSys.GetInstance.SendEvent(DuelEvent.netEvent_SendChangeSelectEffect, true);
        }
        OperateRightButton();
    }

    private void OperateRightButton()
    {
        curRank++;
        if (curRank == length - 1)
        {
            rightButton.gameObject.SetActive(false);
        }
        leftButton.gameObject.SetActive(true);
        ShowText();
    }

    private void OnLeftButton()
    {
        if (CanNotControl())
        {
            return;
        }
        if (curRank == 0)
        {
            return;
        }
        if (IsSendMes())
        {
            DuelEventSys.GetInstance.SendEvent(DuelEvent.netEvent_SendChangeSelectEffect, false);
        }
        OperateLeftButton();
    }

    private void OperateLeftButton()
    {
        curRank--;
        if (curRank == 0)
        {
            leftButton.gameObject.SetActive(false);
        }
        rightButton.gameObject.SetActive(true);
        ShowText();
    }

    private void OnApplyButton()
    {
        if (CanNotControl())
        {
            return;
        }
        if (IsSendMes())
        {
           eventSys.SendEvent(DuelEvent.netEvent_SendApplySelectEffect);
        }
        eventSys.SendEvent(DuelEvent.duelEvent_RecordOperate, RecordEvent.recordEvent_SelectEffect, curRank);
        OperateApplyButton();
    }

    private void OperateApplyButton()
    {
        Tweener a = rectTransform.DOScaleY(0, 0.1f);
        a.SetEase(Ease.Linear);

        eventSys.SendEvent(DuelEvent.uiEvent_ShowOperateTip, describeList[curRank]);

        a.onComplete = delegate
        {
            StartCoroutine(CallBack());
        };
    }
    
    IEnumerator CallBack()
    {
        yield return new WaitForSeconds(2.5f);
        Duel.GetInstance().SetNotSelect();
        callBack(curRank);
        gameObject.SetActive(false);
    }

    private void ShowText()
    {
        effectText.text = describeList[curRank];
    }
}
