using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class LPChangeUI : MonoBehaviour
{
    #region 单例
    private static LPChangeUI instance;

    public LPChangeUI()
    {
        instance = this;
    }

    public static LPChangeUI GetInstance()
    {
        return instance;
    }
    #endregion

    private Text text;

    public float time;

    public void Init()
    {
        text = this.GetComponent<Text>();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 显示lp的变化
    /// </summary>
    /// <param name="val"></param>
    /// <param name="isAdd"></param>
    public void ShowLPChange(float val, bool isAdd)
    {
        
        gameObject.SetActive(true);
        if (isAdd)
        {
            text.text = "+" + val.ToString();
            text.color = Color.green;
            StartCoroutine(HideObj());
        }
        else
        {
            text.text = "-" + val.ToString();
            text.color = Color.red;
            StartCoroutine(HideObj());
        }
    }

    IEnumerator HideObj()
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
