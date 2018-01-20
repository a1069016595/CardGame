

public class AttackEvent
{
    /// <summary>
    /// 是否无效
    /// </summary>
    bool isInvalid;
    bool isAvoidHarm;
    bool isDirectAttack;
    bool isTurnBack;
    float damage;

    Card attacker;
    Card attackeder;

    Player attackedPlayer;
    Player damageTarget;

    public Card Attacker
    {
        get { return attacker; }
    }

    public Card Attackeder
    {
        get { return attackeder; }
    }

    public Player AttackedPlayer
    {
        get { return attackedPlayer; }
    }

    public bool IsTurnBack
    {
        get { return isTurnBack; }
    }

    public float Damage
    {
        get { return damage; }
    }

    public Player DamageTarget
    {
        get { return damageTarget; }
    }

    /// <summary>
    /// 攻击事件
    /// </summary>
    /// <param name="attacker">攻击者</param>
    /// <param name="attackeder">被攻击者</param>
    public AttackEvent(Card theAttacker, Card theAttackeder)
    {
        isAvoidHarm = false;
        isInvalid = false;
        isDirectAttack = false;
        isTurnBack = false;
        attackeder = theAttackeder;
        attacker = theAttacker;
    }

    /// <summary>
    /// 攻击事件
    /// </summary>
    /// <param name="attacker">攻击者</param>
    /// <param name="attackeder">被攻击者</param>
    public AttackEvent(Card theAttacker, Player theAttackeder)
    {
        isAvoidHarm = false;
        isInvalid = false;
        isDirectAttack = true;
        isTurnBack = false;
        attackedPlayer = theAttackeder;
        attacker = theAttacker;
    }

    /// <summary>
    /// 无效攻击
    /// </summary>
    public void SetInvalid()
    {
        isInvalid = true;
    }

    public void SetAvoidHarm()
    {
        isAvoidHarm = true;
    }

    /// <summary>
    /// 是否无效
    /// </summary>
    /// <returns></returns>
    public bool IsInvalid()
    {
        bool val = isInvalid || attacker.curArea != ComVal.Area_Monster || !attacker.IsViliadAttack() || attacker.curPlaseState != ComVal.CardPutType_UpRightFront;
        if (isDirectAttack)
        {
            return val;
        }
        else
        {
            return val || attackeder.curArea != ComVal.Area_Monster;
        }
    }

    /// <summary>
    /// 无效伤害
    /// </summary>
    /// <returns></returns>
    public bool IsAvoidHarm()
    {
        return isAvoidHarm;
    }

    public bool IsDirectAttack()
    {
        return isDirectAttack;
    }

    public void SetIsTurnBack()
    {
        isTurnBack = true;
    }

    public void SetDemage(float val,Player target)
    {
        damage = val;
        damageTarget = target;
    }
}
