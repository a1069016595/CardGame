using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;


public class DragCardUI : MonoBehaviour,IDragHandler
{

    #region 单例
    private static DragCardUI instance;

    public DragCardUI()
    {
        instance = this;
    }

    public static DragCardUI GetInstance()
    {
        return instance;
    }
    #endregion

    private RawImage rawImage;
    RectTransform rectTransform;

    private Vector2 originalLocalPointerPosition;

    public bool isDrap;

    public string id;

    private EditUI editUI;

    public void Init()
    {
        rawImage = this.GetComponent<RawImage>();
        rectTransform = this.GetComponent<RectTransform>();
        gameObject.SetActive(false);
        isDrap = false;
        editUI = EditUI.GetInstance();
    }

    public void StartDrap(string _id)
    {
        this.gameObject.SetActive(true);
        id = _id;
        SetTexture(id);

        isDrap = true;
    }

    private void SetTexture(string id)
    {
        Texture texture = StaticMethod.GetCardPics(id, true);
        rawImage.texture = texture;
    }

    public void EndDrag()
    {
        isDrap = false;
        this.gameObject.SetActive(false);
    }


    public void SetPosition(PointerEventData data)
    {
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rectTransform, data.position, data.pressEventCamera, out globalMousePos))
        {
            rectTransform.position = globalMousePos;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetPosition(eventData);
    }
}
