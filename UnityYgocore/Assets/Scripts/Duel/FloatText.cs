using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
public delegate void FloatTextDele();
public class FloatText : MonoBehaviour
{
    FloatTextDele dele;

    Vector3 startPos;

    RectTransform rectTransform;

    Text text;

    void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
        text = this.GetComponent<Text>();
        startPos = rectTransform.localPosition;
    }

    public void Show(string mes, FloatTextDele _dele)
    {
        this.gameObject.SetActive(true);
        text.text = mes;
        Tweener tweener = transform.GetComponent<RectTransform>().DOLocalMove(Vector3.zero, 0.2f);
        tweener.SetEase(Ease.InOutQuart);
        tweener.onComplete = delegate
        {
            StartCoroutine(Anim());
        };
        dele = _dele;
    }

    IEnumerator Anim()
    {
        yield return new WaitForSeconds(0.2f);
        Tweener tweener = transform.GetComponent<RectTransform>().DOLocalMove(new Vector3(800, 0, 0), 0.2f);
        tweener.SetEase(Ease.Linear);
        tweener.onComplete = delegate
        {
            rectTransform.localPosition = startPos;
            this.gameObject.SetActive(false);
            dele();
        };
    }
}
