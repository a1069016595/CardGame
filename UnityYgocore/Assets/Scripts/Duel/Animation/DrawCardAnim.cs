using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 抽卡动画
/// </summary>
public class DrawCardAnim : MonoBehaviour
{
    public RectTransform target;
    public bool val;

    public Sprite cardSprite;
    Sprite cover;
    Image image;

    RectTransform rectTransform;

    Vector3 localEulerAngles;
    Vector3 localPosition;
    bool isMove = false;

    Vector3 targetPosition;
    Vector3 targetEulerAngles;

    float x;

    public bool isMy;

    void Awake()
    {
        image = transform.GetComponent<Image>();
        rectTransform = transform.GetComponent<RectTransform>();
        cover = StaticMethod.GetCardSprite("0", true);
        localEulerAngles = rectTransform.localEulerAngles;
        localPosition = rectTransform.localPosition;
        targetPosition = target.localPosition;
        targetEulerAngles = target.localEulerAngles;
        gameObject.SetActive(false);
        x = target.sizeDelta.x / 2;
        if (this.name == "ODrawCardAnim")
            isMy = false;
        else
            isMy = true;
    }

    void Update()
    {
        if (isMove)
        {
            if (rectTransform.localEulerAngles.y < 120 && isMy)
            {
                image.overrideSprite = cardSprite;
            }
        }
    }

    public void Play(string id, normalDele dele, int cardCount)
    {
        rectTransform.localPosition = localPosition;
        rectTransform.localEulerAngles = localEulerAngles;
        rectTransform.localScale = Vector3.one;
        gameObject.SetActive(true);
        image.overrideSprite = cover;
        cardSprite = StaticMethod.GetCardSprite(id, true);
        isMove = true;
        Tweener t = image.rectTransform.DOLocalRotate(targetEulerAngles, 0.2f);

        float val = cardCount > 7 ? x * 8 : x * (cardCount + 1);
        if (!isMy)
            val = -val;
        Tweener t1 = image.rectTransform.DOLocalMove(new Vector3(val, targetPosition.y, targetPosition.z), 0.2f);
        Tweener t2 = image.rectTransform.DOScale(new Vector3(1.5f, 1.5f, 1), 0.3f);
        t1.onComplete = delegate
        {
            isMove = false;
            gameObject.SetActive(false);
            if (dele != null)
            {
                dele();
            }
        };
    }
}