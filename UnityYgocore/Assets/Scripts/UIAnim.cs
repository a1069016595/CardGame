using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAnim : MonoBehaviour
{
    protected void SetRotateAnimVal(RawImage image)
    {
        rotateImage = image;
        rotateTimer = new Timer(0.02f);
    }

    RawImage rotateImage;

    Timer rotateTimer;


    protected void RotateLoop()
    {
        if (rotateTimer.Update())
        {
            float v = rotateImage.rectTransform.localEulerAngles.z;
            rotateImage.rectTransform.localEulerAngles = new Vector3(0, 0, v + 2f);
            rotateTimer.Reset();
        }
    }

    List<RawImage> flashImageList;
    Timer flashIntervalTimer;
    Timer flashTimer;
    normalDele mDele;
    bool isStartFlash;

    protected void SetFlashAnimVal(List<RawImage> list, normalDele dele)
    {
        flashImageList = list;
        flashIntervalTimer = new Timer(0.2f);
        flashTimer = new Timer(1);
        mDele = dele;
        isStartFlash = true;
    }


    protected void FlashObj()
    {
        if(!isStartFlash)
        {
            return;
        }
        if (flashTimer.Update())
        {
            mDele();
            isStartFlash = false;
            mDele = null;
        }
        else
        {
            if (flashIntervalTimer.Update())
            {
                for (int i = 0; i < flashImageList.Count; i++)
                {
                    GameObject obj = flashImageList[i].gameObject;
                    obj.SetActive(!obj.activeSelf);
                }
                flashIntervalTimer.Reset();
            }
        }
    }
}
