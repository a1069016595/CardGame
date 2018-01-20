using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public delegate void SelectEffectCallBack(LauchEffect  effect);

public class SelectEffectUI : MonoBehaviour
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
    }

    public void ShowSelectEffectDialogBox(IntDele dele,List<string> effectList,bool isMy)
    {
        Duel.GetInstance().SetSelect();
        rectTransform.localScale = new Vector3(1, 0, 1);
        Tweener a = rectTransform.DOScaleY(1, 0.1f);
        a.SetEase(Ease.Linear);
        gameObject.SetActive(true);
        describeList = effectList;
        curRank = 0;
        length = effectList.Count;

        callBack = dele;

        if(length>1)
        {
            rightButton.gameObject.SetActive(true);
        }

        ShowText();
    }

    private void OnRightButton()
    {
        if(curRank==length-1)
        {
            return;
        }
        curRank++;
        if(curRank==length-1)
        {
            rightButton.gameObject.SetActive(false);
        }
        leftButton.gameObject.SetActive(true);
        ShowText();
    }
    private void OnLeftButton()
    {
        if(curRank==0)
        {
            return;
        }
        curRank --;
        if(curRank==0)
        {
            leftButton.gameObject.SetActive(false);
        }
        rightButton.gameObject.SetActive(true);
        ShowText();
    }

    private void OnApplyButton()
    {
       
        Tweener a = rectTransform.DOScaleY(0, 0.1f);
        a.SetEase(Ease.Linear);

        a.onComplete = delegate
        {
            Duel.GetInstance().SetNotSelect();
            callBack(curRank);
            gameObject.SetActive(false);
        };
    }

    private void ShowText()
    {
        effectText.text =describeList[curRank];
    }
}
