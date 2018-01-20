using UnityEngine;
using System.Collections;
using Protocol;
using UnityEngine.UI;
using DG.Tweening;

public delegate void GuessDele(bool isMy);

public class GuessFirst : MonoBehaviour
{
    GuessFirstCard scissors;
    GuessFirstCard stone;
    GuessFirstCard paper;

    RawImage myValue;
    RawImage otherValue;

    Texture tex_scissors;
    Texture tex_stone;
    Texture tex_paper;

    GuessDele theDele;

    int myVal;
    int otherVal;

    Vector3 myValueLocation;
    Vector3 otherValueLoaction;
    void Awake()
    {
        scissors = transform.FindChild("scissors").GetComponent<GuessFirstCard>();
        stone = transform.FindChild("stone").GetComponent<GuessFirstCard>();
        paper = transform.FindChild("paper").GetComponent<GuessFirstCard>();
        myValue = transform.FindChild("myValue").GetComponent<RawImage>();
        otherValue = transform.FindChild("otherValue").GetComponent<RawImage>();

        tex_scissors = Resources.Load("Texture/f1") as Texture;
        tex_stone = Resources.Load("Texture/f2") as Texture;
        tex_paper = Resources.Load("Texture/f3") as Texture;

        myValueLocation = myValue.rectTransform.localPosition;
        otherValueLoaction = otherValue.rectTransform.localPosition;

        scissors.Init(ClickCard, 1);
        stone.Init(ClickCard, 2);
        paper.Init(ClickCard, 3);
    }

    public void Show(GuessDele dele)
    {
        gameObject.SetActive(true);
        theDele = dele;
    }

    void ClickCard(int value)
    {
        NetWorkScript.Instance.write(TypeProtocol.TYPE_DUEL_BRQ, 0, DuelProtocol.GUESS_BRQ, value);
        WaitTip.GetInstance().ShowWaitTip();
        HideCard();
    }

    public void ReciveMes(DuelGuessMesDTO dto)
    {
        string account1 = dto.account1;
        int val1 = dto.account1Val;
        string account2 = dto.account2;
        int val2 = dto.account2Val;

        if (account1 == ComVal.account)
        {
            myValue.texture = GetTexture(val1);
            otherValue.texture = GetTexture(val2);

            myVal = val1;
            otherVal = val2;
        }
        else if (account2 == ComVal.account)
        {
            myValue.texture = GetTexture(val2);
            otherValue.texture = GetTexture(val1);

            myVal = val2;
            otherVal = val1;
        }

        HideCard();
        ShowResultCard();

        Tweener myTweener = myValue.rectTransform.DOLocalMove(new Vector3(0, myValue.rectTransform.localPosition.y + 160, 0),1f);
        Tweener otherTweener = otherValue.rectTransform.DOLocalMove(new Vector3(0, otherValue.rectTransform.localPosition.y - 160, 0), 1f);

        myTweener.SetUpdate(true);
        otherTweener.SetUpdate(true);

        myTweener.SetEase(Ease.InOutQuart);
        otherTweener.SetEase(Ease.InOutQuart);

        myTweener.onComplete = delegate 
        {
            StartCoroutine(CaculateResult());
            
        };

        WaitTip.GetInstance().HideWaitTip();
       
    }

    void ShowResultCard()
    {
        myValue.gameObject.SetActive(true);
        otherValue.gameObject.SetActive(true);
    }

    void HideResultCard()
    {
        myValue.gameObject.SetActive(false);
        otherValue.gameObject.SetActive(false);
    }

    void HideCard()
    {
        scissors.gameObject.SetActive(false);
        stone.gameObject.SetActive(false);
        paper.gameObject.SetActive(false);
    }

    void ShowCard()
    {
        scissors.SetToStartPos();
        stone.SetToStartPos();
        paper.SetToStartPos();

        scissors.gameObject.SetActive(true);
        stone.gameObject.SetActive(true);
        paper.gameObject.SetActive(true);
    }

    IEnumerator CaculateResult()
    {
        yield return new WaitForSeconds(1);
        if (myVal == otherVal)
        {
            ShowCard();
        }
        else if (myVal == 1)
        {
            if (otherVal == 2)
            {
                theDele(false);
            }
            else
            {
                theDele(true);
            }
        }
        else if (myVal == 2)
        {
            if (otherVal == 3)
            {
                theDele(false);
            }
            else
            {
                theDele(true);
            }
        }
        else if (myVal == 3)
        {
            if (otherVal == 1)
            {
                theDele(false);
            }
            else
            {
                theDele(true);
            }
        }
        HideResultCard();
        myValue.rectTransform.localPosition = myValueLocation;
        otherValue.rectTransform.localPosition = otherValueLoaction;
    }

    Texture GetTexture(int val)
    {
        if(val==0)
        {
            Debug.Log("error");
        }
        else if (val == 1)
        {
            return tex_scissors;
        }
        else if (val == 2)
        {
            return tex_stone;
        }
        else if (val == 3)
        {
            return tex_paper;
        }
        else
        {
            Debug.Log("error");
        }
        return null;
    }
}