using System;
using System.Collections.Generic;


/// <summary>
/// 时点信息
/// </summary>
public class Code
{
    public Int64 code;
    public Reason reason;


    public Group group;

    public LauchEffect effect;

    public int previousArea;

    public Group targetCard;

    public Player triggerPlayer;

    public bool isDelayCode;

    public Code(Player _player)
    {
        group = new Group();
        triggerPlayer = _player;
    }

    public Code(int _code)
    {
        code = _code;
    }

    public void SetEffect(LauchEffect _effect)
    {
        effect = _effect;
    }

    public void SetReason(Reason _reason)
    {
        reason = _reason;
    }
    /// <summary>
    /// 设置目标卡片 
    /// </summary>
    /// <param name="target"></param>
    public void SetTargetCard(Group target)
    {
        targetCard = target;
    }
}
