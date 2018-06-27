using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class AttactAnim : MonoBehaviour
{

    RectTransform rectTransform;

    List<RectTransform> mList;
    List<RectTransform> oList;

    public FieldMgr mfieldMgr;
    public FieldMgr ofieldMgr;

    RectTransform mPos;
    RectTransform oPos;

    public void Init()
    {
        rectTransform = transform.FindChild("AttackAnim").GetComponent<RectTransform>();
        mPos = transform.FindChild("Mpos").GetComponent<RectTransform>();
        oPos = transform.FindChild("Opos").GetComponent<RectTransform>();
        mList = new List<RectTransform>();
        oList = new List<RectTransform>();
        foreach (var item in mfieldMgr.monsterGridList)
        {
            mList.Add(item.GetComponent<RectTransform>());
        }
        foreach (var item in ofieldMgr.monsterGridList)
        {
            oList.Add(item.GetComponent<RectTransform>());
        }
    }
    /// <summary>
    /// Attackeder_areaRank为-1则直接攻击
    /// </summary>
    /// <param name="Attacker_areaRank"></param>
    /// <param name="Attackeder_areaRank"></param>
    /// <param name="isMy"></param>
    /// <param name="dele"></param>
    public void Show(int Attacker_areaRank, int Attackeder_areaRank, bool isMy, normalDele dele)
    {
        RectTransform target = null;
        if (isMy)
        {
            rectTransform.anchoredPosition = mList[Attacker_areaRank].anchoredPosition;
            if (Attackeder_areaRank == -1)
            {
                target = oPos;
            }
            else
            {
                target = oList[Attackeder_areaRank];
            }
        }
        else
        {
            rectTransform.localEulerAngles = new Vector3(0, 0, 180);
            rectTransform.anchoredPosition = oList[Attacker_areaRank].anchoredPosition;
            if (Attackeder_areaRank == -1)
            {
                target = mPos;
            }
            else
            {
                target = mList[Attackeder_areaRank];
            }
        }
        rectTransform.gameObject.SetActive(true);
        gameObject.SetActive(true);

        bool isOppsite = false;
        if (target.anchoredPosition.x < rectTransform.anchoredPosition.x)
        {
            isOppsite = true;
        }
        float x = Mathf.Abs(target.anchoredPosition.x - rectTransform.anchoredPosition.x);
        float y = Mathf.Abs(target.anchoredPosition.y - rectTransform.anchoredPosition.y);
        float val = Mathf.Atan2(x, y) / Mathf.PI * 180;
        Tweener t;
        if (isMy)
        {
            if (isOppsite)
            {
                t = rectTransform.DOLocalRotate(new Vector3(0, 0, val), 0.3f);
            }
            else
            {
                t = rectTransform.DOLocalRotate(new Vector3(0, 0, -val), 0.3f);
            }
        }
        else
        {
            if (isOppsite)
            {
                t = rectTransform.DOLocalRotate(new Vector3(0, 0, 180 - val), 0.3f);
            }
            else
            {
                t = rectTransform.DOLocalRotate(new Vector3(0, 0, 180 + val), 0.3f);
            }
        }
        t.onComplete = delegate
        {
            Tweener t1 = rectTransform.DOLocalMove(new Vector3(target.anchoredPosition.x, target.anchoredPosition.y, 0), 0.4f);
            t1.SetEase(Ease.InBack);
            t1.onComplete = delegate
            {
                rectTransform.gameObject.SetActive(false);
                rectTransform.localEulerAngles = Vector3.zero;
                StartCoroutine(Wait(dele));
               // Debug.
            };
        };
    }


    IEnumerator Wait(normalDele dele)
    {
        yield return new WaitForSeconds(0.2f);
        Duel.GetInstance().SetIsAnim(false);
        dele();
        gameObject.SetActive(false);
    }
}
