using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class ChangeAreaAnim : MonoBehaviour
{
    public List<RectTransform> monsterGrid;
    public List<RectTransform> trapGrid;

    public RectTransform extraDeck;
    public RectTransform mainDeck;
    public RectTransform graveyardDeck;
    public RectTransform removeDeck;
    public RectTransform fieldSpellGrid;

    public bool isMy;

    RectTransform targetRect;

    DuelUIManager duelUIMgr;
    Duel duel;

    RectTransform rectTransform;

    Image image;

    public int fromArea;
    public int toArea;
    public int fromType;
    public int toType;

    public int fromRank;
    public int toRank;

    bool isInAnim;
    bool isChange;

    void Start()
    {
        gameObject.SetActive(false);
        duelUIMgr = DuelUIManager.GetInstance();
        rectTransform = GetComponent<RectTransform>();
        duel = Duel.GetInstance();
        image = GetComponent<Image>();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A) && isMy)
        //{
        //    PlayAnim("00213326", ComVal.Area_Monster, ComVal.Area_Hand, ComVal.CardPutType_UpRightFront, -1, 1, toRank);
        //}
        if (isInAnim&&!isChange)
        {
            if (toType == ComVal.CardPutType_layBack || toType == ComVal.CardPutType_UpRightBack)
            {
                if (rectTransform.localEulerAngles.y > 100)
                {
                    isChange = true;
                    image.sprite = StaticMethod.GetCardSprite("0", true);
                }
            }
        }
    }

    public void PlayAnim(string cardID, int fromArea, int toArea, int fromType, int toType, int fromRank = -1, int toRank = -1)
    {
        float speed = 0.2f;
        this.fromArea = fromArea;
        this.toArea = toArea;
        this.fromType = fromType;
        this.toType = toType;

        isInAnim = true;
        isChange = false;
        image.sprite = StaticMethod.GetCardSprite(cardID, true);
        gameObject.SetActive(true);
        targetRect = GetRect(fromArea, fromRank);
        rectTransform.anchoredPosition3D = targetRect.anchoredPosition3D;

        rectTransform.localEulerAngles = targetRect.localEulerAngles;
        rectTransform.localScale = Vector3.one;
        if (fromArea.IsBind(ComVal.Area_Field)||fromArea.IsBind(ComVal.Area_InSummon))
        {
            rectTransform.localEulerAngles.Set(rectTransform.localEulerAngles.x, GetRotationY(fromType), GetRotationZ(fromType));
        }
        if (fromArea.IsBind(ComVal.Area_Hand))
        {
            rectTransform.anchoredPosition3D = targetRect.anchoredPosition3D + targetRect.parent.GetComponent<RectTransform>().anchoredPosition3D;
            rectTransform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        }

        Vector3 targetPosition = Vector3.zero;
        Vector3 targetRotation = Vector3.zero;
        Vector3 targetScale = Vector3.one;

        if (toArea.IsBind(ComVal.Area_Field))
        {
            targetPosition = GetRect(toArea, toRank).anchoredPosition3D;
            targetRotation.Set(rectTransform.localEulerAngles.x, GetRotationY(toType), GetRotationZ(toType));
        }
        else if (toArea.IsBind(ComVal.Area_Hand))
        {
            targetPosition = GetReturnHandCardPos(toRank) + duelUIMgr.GetHandCardUI(isMy).anchoredPosition3D;
            targetRotation = new Vector3(-35, 0, 0);
            targetScale = new Vector3(1.5f, 1.5f, 1.5f);
        }
        else
        {
            targetPosition = GetRect(toArea, toRank).anchoredPosition3D;
            targetRotation = GetRect(toArea, toRank).localEulerAngles;
        }

        Tweener t = rectTransform.DOLocalMove(targetPosition, speed);
        Tweener t1 = rectTransform.DOLocalRotate(targetRotation, speed);
        Tweener t2 = rectTransform.DOScale(targetScale, speed);
        t.onComplete = FinishAnim;
    }

    private void FinishAnim()
    {
        isInAnim = false;
        gameObject.SetActive(false);
        duel.FinishHandle();
    }

    private RectTransform GetRect(int area, int rank)
    {
        if (area == ComVal.Area_Extra)
        {
            return extraDeck;
        }
        else if (area == ComVal.Area_Monster || area == ComVal.Area_InSummon)
        {
            return monsterGrid[rank];
        }
        else if (area == ComVal.Area_NormalTrap)
        {
            return trapGrid[rank];
        }
        else if (area == ComVal.Area_FieldSpell)
        {
            return fieldSpellGrid;
        }
        else if (area == ComVal.Area_Remove)
        {
            return removeDeck;
        }
        else if (area == ComVal.Area_Graveyard)
        {
            return graveyardDeck;
        }
        else if (area == ComVal.Area_Hand)
        {
            return duelUIMgr.GetHandCardRect(rank, isMy);
        }
        else if (area == ComVal.Area_MainDeck)
        {
            return mainDeck;
        }
        return null;
    }

    private Vector3 GetReturnHandCardPos(int rank)
    {
        return duelUIMgr.GetReturnHandCardPos(rank, isMy);
    }

    public float GetRotationY(int type)
    {
        if (type == ComVal.CardPutType_layBack || type == ComVal.CardPutType_UpRightBack)
        {
            return 180;
        }
        else
        {
            return 0;
        }
    }

    public float GetRotationZ(int type)
    {
        if (type == ComVal.CardPutType_layBack || type == ComVal.CardPutType_layFront)
        {
            return 90;
        }
        else
        {
            return 0;
        }
    }
}
