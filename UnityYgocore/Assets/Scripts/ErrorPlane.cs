using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class ErrorPlane : MonoBehaviour
{
    #region 单例
    private static ErrorPlane instance;

    public ErrorPlane()
    {
        instance = this;
    }

    public static ErrorPlane GetInstance()
    {
        return instance;
    }
    #endregion

    public Button button;
    public Text text;
    public Text title;

    public normalDele dele;
    RectTransform rectTransform;

    public void Init()
    {
        button = transform.FindChild("Button").GetComponent<Button>();
        text = transform.FindChild("Mes").GetComponent<Text>();
        title = transform.FindChild("Title").GetComponent<Text>();
        rectTransform = this.GetComponent<RectTransform>();

        button.onClick.AddListener(OnButtonClick);

        gameObject.SetActive(false);
    }

    public void Show(string mes, normalDele e = null,string _title=null)
    {
        rectTransform.localScale = new Vector3(1, 0, 1);
        Tweener a = rectTransform.DOScaleY(1, 0.1f);
        a.SetEase(Ease.Linear);
        a.onComplete = delegate
        {
            button.enabled = true;
        };
        gameObject.SetActive(true);
        dele = e;
        text.text = mes;
        if(_title!=null)
        {
            title.text = _title;
        }
        else
        {
            title.text = "提示";
        }
    }

    private void OnButtonClick()
    {
        button.enabled = false;
        Tweener a = rectTransform.DOScaleY(0, 0.1f);
        a.SetEase(Ease.Linear);

        a.onComplete = delegate
        {
            gameObject.SetActive(false);

            text.text = "";
            if (dele != null)
            {
                dele();
            }
        };
    }
}
