using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMonoBehivour : MonoBehaviour
{

   
    protected Dictionary<DuelEvent, DuelEventHandler> dic = new Dictionary<DuelEvent, DuelEventHandler>();

    protected DuelEventSys eventSys
    {
        get { return DuelEventSys.GetInstance; }
    }

    protected void AddHandler(DuelEvent val, DuelEventHandler handler)
    {
        eventSys.AddHandler(val, handler,gameObject);
        dic.Add(val, handler);
    }

    protected void SendEvent(DuelEvent e,params object[] val)
    {
        eventSys.SendEvent(e, val);
    }

    protected void OnDestroy()
    {
        foreach (var item in dic)
        {
            eventSys.DeleteHandler(item.Key, item.Value);
        }
        dic.Clear();
    }
}
