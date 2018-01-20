using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Protocol;

public class SelectPutType : DuelUIOpearate
{
    Text title;
    Button attackType;
    Button defenceType;


    BoolDele myDele;

    RectTransform rectTransform;

    RawImage attackCardImage;
    RawImage defenceCardImage;

    public void Init()
    {
        title = transform.FindChild("Title").GetComponent<Text>();
        attackType = transform.FindChild("AttackType").GetComponent<Button>();
        defenceType = transform.FindChild("DefenceType").GetComponent<Button>();
        rectTransform = this.GetComponent<RectTransform>();
        attackCardImage = attackType.transform.FindChild("RawImage").GetComponent<RawImage>();
        defenceCardImage = defenceType.transform.FindChild("RawImage").GetComponent<RawImage>();

        title.text = ComStr.Text_SelectPutType;
        attackType.GetComponent<RawImage>().color = new Color(1, 1, 1, 0.5f);
        defenceType.GetComponent<RawImage>().color = new Color(1, 1, 1, 0.5f);
        attackType.onClick.AddListener(OnAttackTypeClick);
        defenceType.onClick.AddListener(OnDefenceTypeClick);

        gameObject.SetActive(false);

        DuelEventSys.GetInstance.AddHandler(DuelEvent.netEvent_ReciveSelectPutType, ReiveSelectPutType);
    }

    private void ReiveSelectPutType(params object[] args)
    {
        DuelSelectPutTypeDto dto = (DuelSelectPutTypeDto)args[0];
        HandleSelectPutType(dto.isAttack);
    }

    public void ShowSelectPutType(string cardID, BoolDele dele, bool isMy)
    {
        isMySelect = isMy;
        Duel.GetInstance().SetSelect();
        Texture tex = StaticMethod.GetCardPics(cardID, false);
        attackCardImage.texture = tex;
        defenceCardImage.texture = tex;
        rectTransform.localScale = new Vector3(1, 0, 1);
        Tweener tw = rectTransform.DOScaleY(1, 0.15f);
        tw.SetEase(Ease.Linear);
        gameObject.SetActive(true);
        tw.onComplete = delegate
        {
            attackType.enabled = true;
            defenceType.enabled = true;
        };
        myDele = dele;
    }

    void OnDefenceTypeClick()
    {
        if(CanNotControl())
        {
            return;
        }
        if(IsSendMes())
        {
            DuelEventSys.GetInstance.SendEvent(DuelEvent.netEvent_SendSelectPutType, false);
        }
        HandleSelectPutType(false);
    }

    void OnAttackTypeClick()
    {
        if (CanNotControl())
        {
            return;
        }
        if (IsSendMes())
        {
            DuelEventSys.GetInstance.SendEvent(DuelEvent.netEvent_SendSelectPutType, true);
        }
        HandleSelectPutType(true);
    }

    private void HandleSelectPutType(bool val)
    {
        attackType.enabled = false;
        defenceType.enabled = false;
        attackType.GetComponent<RawImage>().color = Color.white;
        Tweener tw = rectTransform.DOScaleY(0, 0.15f);
        tw.SetEase(Ease.Linear);
        tw.onComplete = delegate
        {
            attackType.GetComponent<RawImage>().color = new Color(1, 1, 1, 0.5f);
            gameObject.SetActive(false);
            Duel.GetInstance().SetNotSelect();
            myDele(val);
        };
    }
}
