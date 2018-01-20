using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelDele
{
    normalDele mDele;
    string mMes;
    bool isAction;


    public DuelDele(normalDele dele,string mes,bool isAction)
    {
        mDele = dele;
        mMes = mes;
        this.isAction = isAction;
    }

    public normalDele Dele
    {
        get{ return mDele;}
    }

    public string Mes
    {
        get { return mMes; }
    }

    public bool IsAction
    {
        get { return isAction; }
    }
}