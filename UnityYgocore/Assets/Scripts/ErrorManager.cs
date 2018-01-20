using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ErrorModel
{
    public string text;
    public normalDele function;

    public ErrorModel(string s, normalDele e = null)
    {
        text = s;
        function = e;
    }
}

public class ErrorManager : MonoBehaviour
{
    #region 单例
    private static ErrorManager instance;

    public ErrorManager()
    {
        instance = this;
    }

    public static ErrorManager GetInstance()
    {
        return instance;
    }
    #endregion

    public ErrorPlane errorPlane;
    private List<ErrorModel> errorList;
    public void Init()
    {
        errorPlane = ErrorPlane.GetInstance();
        errorList = new List<ErrorModel>();
        errorPlane.Init();
    }

    void Update()
    {
        if (errorPlane.gameObject.activeSelf || errorList.Count == 0)
        {
            return;
        }
        ErrorModel e = errorList[0];
        errorList.RemoveAt(0);
        errorPlane.Show(e.text, e.function);
    }


    public void AddErrorModel(string mes, normalDele e = null)
    {
        errorList.Add(new ErrorModel(mes, e));
    }
}

