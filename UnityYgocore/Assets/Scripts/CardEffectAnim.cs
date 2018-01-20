using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class CardEffectAnim : MonoBehaviour
{
    #region 单例
    private static CardEffectAnim instance;

    public CardEffectAnim()
    {
        instance = this;
    }

    public static CardEffectAnim GetInstance()
    {
        return instance;
    }
    #endregion

    Image image;
    RectTransform imageRect;
    public void Init()
    {
        image = this.GetComponent<Image>();
        imageRect = transform.FindChild("Image").GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 卡图由小变大
    /// </summary>
    /// <param name="card"></param>
    /// <param name="dele"></param>
    public void ShowAnim(Card card, normalDele dele)
    {
        image.rectTransform.localScale = Vector3.one;
        image.overrideSprite = StaticMethod.GetCardSprite(card.cardID, false);
        this.gameObject.SetActive(true);

        Tweener tweener = image.rectTransform.DOScale(Vector3.one * 3, 0.2f);
        tweener.SetEase(Ease.Linear);

        tweener.onComplete = delegate()
        {
            Tweener tweener1 = image.rectTransform.DOScale(Vector3.one * 3, 0.2f);
            tweener1.SetEase(Ease.Linear);

            tweener1.onComplete = delegate()
            {
                image.transform.localScale = Vector3.one;
                this.gameObject.SetActive(false);
                if (dele != null)
                {
                    dele();
                }

            };
        };


    }

    /// <summary>
    /// 发动效果时动画
    /// </summary>
    /// <param name="card"></param>
    /// <param name="dele"></param>
    /// <param name="effect"></param>
    public void ShowEffectAnim(Card card, normalDele dele)
    {
        image.rectTransform.localScale = Vector3.one * 3;
        image.overrideSprite = StaticMethod.GetCardSprite(card.cardID, false);
        gameObject.SetActive(true);

        Vector2 pos = image.rectTransform.sizeDelta;
        Vector3 startPos = new Vector3(-pos.x / 2, pos.y / 2, 0);

        Vector3 endPos = new Vector3(pos.x / 2, -pos.y / 2, 0);
        imageRect.localPosition = startPos;
        Tweener tweener = imageRect.DOLocalMove(endPos, 0.2f);
        tweener.SetEase(Ease.Linear);
        tweener.onComplete = delegate()
        {
            if (dele != null)
            {
                gameObject.SetActive(false);
                dele();
            }
        };
    }
}
