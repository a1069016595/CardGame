using UnityEngine;
using System.Collections;

public class EditShowCardUI : SelectCardShowUI
{
     #region 单例
    private static EditShowCardUI instance;

    public EditShowCardUI()
    {
        instance = this;
    }

    public static EditShowCardUI GetInstance()
    {
        return instance;
    }
    #endregion
}
