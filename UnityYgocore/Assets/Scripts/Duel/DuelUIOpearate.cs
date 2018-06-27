using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelUIOpearate : BaseMonoBehivour
{
    protected bool isMySelect;

    protected bool IsSendMes()
    {
        return Duel.GetInstance().IsNetWork && isMySelect;
    }

    protected bool CanNotControl()
    {
        return (Duel.GetInstance().IsNetWork && !isMySelect) || Duel.GetInstance().IsPlayBack||Duel.GetInstance().isFinishGame;
    }
}
