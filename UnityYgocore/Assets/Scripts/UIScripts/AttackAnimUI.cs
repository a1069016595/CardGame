using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class AttackAnimUI : MonoBehaviour
{
    private RectTransform rectTransform;
    public Vector3 localVector;
    bool isShow = true;
    bool isStart = false;
    Tweener tweener;

    public void Init(bool isMy)
    {
        rectTransform = this.GetComponent<RectTransform>();
        if (!isMy)
        {
            rectTransform.localEulerAngles = new Vector3(0, 0, 180);
        }
        localVector = rectTransform.localPosition;
        HideAnim();
    }

    public void ShowAnim()
    {
        if (isShow)
        {
            return;
        }
        rectTransform.localPosition = localVector;
        isShow = true;
        StartAnim();
        this.gameObject.SetActive(true);
    }

    public void HideAnim()
    {
        if (!isShow)
        {
            return;
        }
        tweener.Kill();
        tweener = null;
        isShow = false;
        rectTransform.localPosition = localVector;
        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 开始动画
    /// </summary>
    public void StartAnim()
    {
        tweener = rectTransform.DOLocalMove(new Vector3(0, 20, 0), 1f);
        tweener.SetUpdate(true);
        tweener.SetEase(Ease.Linear);
        tweener.onComplete = delegate()
        {
            EndAnim();
        };
    }
    void EndAnim()
    {
        tweener = rectTransform.DOLocalMove(localVector, 1f);
        tweener.SetUpdate(true);
        tweener.SetEase(Ease.Linear);
        tweener.onComplete = delegate()
        {
            StartAnim();
        };
    }
}
