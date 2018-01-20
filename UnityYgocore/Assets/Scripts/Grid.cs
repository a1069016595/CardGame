using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Grid : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RawImage rawImage;
    void Awake()
    {
        rawImage = this.GetComponent<RawImage>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rawImage.color = new Color(1, 1, 1, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rawImage.color = new Color(1, 1, 1, 0f);
    }
}
