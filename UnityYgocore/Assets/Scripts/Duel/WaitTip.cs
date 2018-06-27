using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaitTip : MonoBehaviour
{
     #region 单例
    private static WaitTip instance;

    public WaitTip()
    {
        instance = this;
    }

    public static WaitTip GetInstance()
    {
        return instance;
    }
    #endregion

    Text text;
    float timer;

    bool isShow;
    int count;

    void Awake()
    {
        text = transform.FindChild("Text").GetComponent<Text>();
        timer = 0;
        isShow = false;
        text.text = "正在等待..";
        count = 0;
        HideWaitTip();
    }

    void Update()
    {
        if (!isShow)
            return;
        timer += Time.deltaTime;
        if (timer > 0.5f)
        {
            if (count > 2)
            {
                text.text = "正在等待..";
                count = 0;
            }
            else
            {
                text.text = text.text + ".";
                count++;
            }
            timer = 0;
        }
    }

    public void ShowWaitTip()
    {
        //Debug.Log("显示等待提示");
        isShow = true;
        gameObject.SetActive(true);
    }
    public void HideWaitTip()
    {
        //Debug.Log("隐藏等待提示");
        isShow = false;
        gameObject.SetActive(false);
    }
}
