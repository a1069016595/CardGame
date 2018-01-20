using UnityEngine;
using System.Collections;

public class ComMethod
{
    /// <summary>
    /// 生成obj 加入到父物体的下面
    /// </summary>
    /// <param name="obj">游戏物体</param>
    /// <param name="parent">父物体</param>
    /// <returns></returns>
    public static GameObject InitGameObject(GameObject theObj, Transform parent)
    {
        GameObject obj = GameObject.Instantiate(theObj);
        obj.transform.SetParent(parent);
        obj.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        //  prefeb.rectTransform.sizeDelta = new Vector2(cardSizeX, cardSizeY);
        obj.GetComponent<RectTransform>().localScale = Vector3.one;
        obj.GetComponent<RectTransform>().localRotation = new Quaternion(0, 0, 0, 1);
        return obj;
    }

}
