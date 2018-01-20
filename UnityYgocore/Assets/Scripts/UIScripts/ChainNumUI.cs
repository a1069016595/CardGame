using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ChainNumUI : UIAnim
{
    RectTransform rectTransform;
    RawImage numImage;
    RawImage ringImage;


    public void Init(int val, Vector3 pos, GameObject parent)
    {
        if (rectTransform == null)
        {
            GetVal();
        }
        gameObject.SetActive(true);

        transform.SetParent(parent.transform);
        rectTransform.anchoredPosition3D = pos;
        rectTransform.localEulerAngles = Vector3.zero;
        rectTransform.localScale = Vector3.one;
        SetRotateAnimVal(ringImage);

        SetNumVal(val);
    }

    void Update()
    {
        RotateLoop();
        FlashObj();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        if(ringImage!=null)
        {
            ringImage.gameObject.SetActive(true);
            numImage.gameObject.SetActive(true);
        }
    }

    public void ShowFlashAnim(normalDele dele)
    {
        SetFlashAnimVal(new List<RawImage> { ringImage, numImage }, dele);
    }

    private void GetVal()
    {
        rectTransform = GetComponent<RectTransform>();
        numImage = transform.GetChild<RawImage>("Num");
        ringImage = transform.GetChild<RawImage>("Ring");
    }

    private void SetNumVal(int num)
    {
        int row;
        int column = num % 5 == 0 ? 5 : num % 5;
        if (column == 5)
        {
            row = num / 5;
        }
        else
        {
            row = num / 5 + 1;
        }
        numImage.uvRect = new Rect((column - 1) * 0.2f, 1 - 0.25f * row, 0.2f, 0.25f);
    }
}
