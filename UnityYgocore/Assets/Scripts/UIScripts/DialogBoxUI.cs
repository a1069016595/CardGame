using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Threading;
using DG.Tweening;
using Protocol;
public delegate void GetMes(bool mes);

public class DialogBoxUI : DuelUIOpearate
{
    #region 单例
    private static DialogBoxUI instance;

    public DialogBoxUI()
    {
        instance = this;

    }

    public static DialogBoxUI GetInstance()
    {
        return instance;
    }
    #endregion


    Button trueButton;
    Button falseButton;
    Text text;

    GetMes getMes;

    RectTransform rectTransform;


    public void Init()
    {

        trueButton = this.transform.FindChild("TrueButton").GetComponent<Button>();
        falseButton = this.transform.FindChild("FalseButton").GetComponent<Button>();
        text = this.transform.FindChild("Text").GetComponent<Text>();
        rectTransform = this.GetComponent<RectTransform>();
      
        trueButton.onClick.AddListener(SendTrueMes);
        falseButton.onClick.AddListener(SendFalseMes);
        this.gameObject.SetActive(false);

        DuelEventSys.GetInstance.AddHandler(DuelEvent.netEvent_ReciveDialogBoxSelect, ReciveDialogBoxSelect);
    }

    private void ReciveDialogBoxSelect(params object[] args)
    {
        DuelOperateDialogBoxDTO dto = (DuelOperateDialogBoxDTO)args[0];

        HandleDialogBox(dto.isTrue);
    }



    void SendTrueMes()
    {
        HideDialogBox(true);
    }

    void SendFalseMes()
    {
        HideDialogBox(false);
    }

    public void ShowDialogBox(string str, GetMes dele,bool isMy)
    {
        isMySelect = isMy;
        Duel.GetInstance().SetSelect();
        rectTransform.localScale = new Vector3(1, 0, 1);
        Tweener a = rectTransform.DOScaleY(1, 0.15f);
        a.SetEase(Ease.Linear);
        this.gameObject.SetActive(true);
        a.onComplete = delegate
        {
            trueButton.enabled = true;
            falseButton.enabled = true;
        };
        text.text = str;
        getMes = dele;
    }

    void HideDialogBox(bool mes)
    {
        if(CanNotControl())
        {
            return;
        }
        if(IsSendMes())
        {
            DuelEventSys.GetInstance.SendEvent(DuelEvent.netEvent_SendDialogBoxSelect, mes);
        }
        HandleDialogBox(mes);
    }

    private void HandleDialogBox(bool mes)
    {
        trueButton.enabled = false;
        falseButton.enabled = false;
        Tweener a = rectTransform.DOScaleY(0, 0.15f);
        a.SetEase(Ease.Linear);
        a.onComplete = delegate
        {
            this.gameObject.SetActive(false);
            Duel.GetInstance().SetNotSelect();
            getMes(mes);
        };
    }
}

