
using UnityEngine;
using System.Collections;
using System;
using System.Text.RegularExpressions;

/// <summary>
/// 存放公用方法 
/// </summary>
public static class StaticMethod
{

    /// <summary>
    /// 获取卡图 是小卡图还是大卡图
    /// <para>如果找不到 则为空卡图</para>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isSmall"></param>
    /// <returns></returns>
    public static Texture GetCardPics(string id, bool isSmall)
    {
        Texture texture;

        if (isSmall)
        {

            texture = Resources.Load("SmallPics/" + id) as Texture;

        }
        else
        {
            texture = Resources.Load("Pics/" + id) as Texture;
        }
        if (texture == null)
        {
            texture = Resources.Load("unknown") as Texture;
        }
        return texture;
    }
    /// <summary>
    /// id为0时 返回卡片背面
    /// </summary>
    /// <param name="id"></param>
    /// <param name="isSmall"></param>
    /// <returns></returns>
    public static Sprite GetCardSprite(string id, bool isSmall)
    {
        Sprite texture;
        if (id == ComStr.CardId_Null)
        {
            return Resources.Load("Prefebs/cover", typeof(Sprite)) as Sprite;
        }
        if (isSmall)
        {

            texture = Resources.Load("SmallPics/" + id, typeof(Sprite)) as Sprite;

        }
        else
        {
            texture = Resources.Load("Pics/" + id, typeof(Sprite)) as Sprite;
        }
        if (texture == null)
        {
            texture = Resources.Load("unknown", typeof(Sprite)) as Sprite;
        }
        return texture;
    }

    public static Texture GetCardTexture(string id, bool isSmall)
    {
        Texture texture;
        if (id == ComStr.CardId_Null)
        {
            return Resources.Load("Prefebs/cover", typeof(Texture)) as Texture;
        }
        if (isSmall)
        {

            texture = Resources.Load("SmallPics/" + id, typeof(Texture)) as Texture;

        }
        else
        {
            texture = Resources.Load("Pics/" + id, typeof(Texture)) as Texture;
        }
        if (texture == null)
        {
            texture = Resources.Load("unknown", typeof(Texture)) as Texture;
        }
        return texture;
    }

    /// <summary>
    /// xml中的cardtype为中文字符串 需要转为int类型
    /// <para>错误时返回0</para>
    /// </summary>
    /// <returns></returns>
    public static int ChangeCardType(string str)
    {
        if (str == ComStr.CardType_Monster_Adjust)
        {
            return ComVal.CardType_Monster_Adjust;
        }
        if (str == ComStr.CardType_Monster_Effect)
        {
            return ComVal.CardType_Monster_Effect;
        }
        if (str == ComStr.CardType_Monster_Fusion)
        {
            return ComVal.CardType_Monster_Fusion;
        }
        if (str == ComStr.CardType_Monster_Normal)
        {
            return ComVal.CardType_Monster_Normal;
        }
        if (str == ComStr.CardType_Monster_Synchro)
        {
            return ComVal.CardType_Monster_Synchro;
        }
        if (str == ComStr.CardType_Monster_XYZ)
        {
            return ComVal.CardType_Monster_XYZ;
        }
        if (str == ComStr.CardType_Spell_Continuous)
        {
            return ComVal.CardType_Spell_Continuous;
        }
        if(str==ComStr.CardType_Spell_Field)
        {
            return ComVal.CardType_Spell_Field;
        }
        if (str == ComStr.CardType_Spell_Equit)
        {
            return ComVal.CardType_Spell_Equit;
        }
        if (str == ComStr.CardType_Spell_Normal)
        {
            return ComVal.CardType_Spell_Normal;
        }
        if (str == ComStr.CardType_Spell_Quick)
        {
            return ComVal.CardType_Spell_Quick;
        }
        if (str == ComStr.CardType_Trap_Continuous)
        {
            return ComVal.CardType_Trap_Continuous;
        }
        if (str == ComStr.CardType_Trap_Normal)
        {
            return ComVal.CardType_Trap_Normal;
        }
        if (str == ComStr.CardType_Trap_StrikeBack)
        {
            return ComVal.CardType_Trap_StrikeBack;
        }
        if(str==ComStr.CardType_Monster_Double)
        {
            return ComVal.CardType_Monster_Double;
        }
        else
        {
            return 0;
        }
    }
    /// <summary>
    /// xml中的CardRace为中文字符串 需要转为int类型
    /// <para>错误时返回0</para>
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int ChangeCardRace(string str)
    {
        if (str == ComStr.CardRace_Dragon)
        {
            return ComVal.CardRace_Dragon;
        }
        if (str == ComStr.CardRace_Zombie)
        {
            return ComVal.CardRace_Zombie;
        }
        if (str == ComStr.CardRace_Fiend)
        {
            return ComVal.CardRace_Fiend;
        }
        if (str == ComStr.CardRace_Pyro)
        {
            return ComVal.CardRace_Pyro;
        }
        if (str == ComStr.CardRace_SeaSerpent)
        {
            return ComVal.CardRace_SeaSerpent;
        }
        if (str == ComStr.CardRace_Rock)
        {
            return ComVal.CardRace_Rock;
        }
        if (str == ComStr.CardRace_Machine)
        {
            return ComVal.CardRace_Machine;
        }
        if (str == ComStr.CardRace_Fish)
        {
            return ComVal.CardRace_Fish;
        }
        if (str == ComStr.CardRace_Dinosaur)
        {
            return ComVal.CardRace_Dinosaur;
        }
        if (str == ComStr.CardRace_Insect)
        {
            return ComVal.CardRace_Insect;
        }
        if (str == ComStr.CardRace_Beast)
        {
            return ComVal.CardRace_Beast;
        }
        if (str == ComStr.CardRace_BeastWarrior)
        {
            return ComVal.CardRace_BeastWarrior;
        }
        if (str == ComStr.CardRace_Plant)
        {
            return ComVal.CardRace_Plant;
        }
        if (str == ComStr.CardRace_Aqua)
        {
            return ComVal.CardRace_Aqua;
        }
        if (str == ComStr.CardRace_Warrior)
        {
            return ComVal.CardRace_Warrior;
        }
        if (str == ComStr.CardRace_WingedBeast)
        {
            return ComVal.CardRace_WingedBeast;
        }
        if (str == ComStr.CardRace_Fairy)
        {
            return ComVal.CardRace_Fairy;
        }
        if (str == ComStr.CardRace_Spellcaster)
        {
            return ComVal.CardRace_Spellcaster;
        }
        if (str == ComStr.CardRace_Thunder)
        {
            return ComVal.CardRace_Thunder;
        }
        if (str == ComStr.CardRace_Reptile)
        {
            return ComVal.CardRace_Reptile;
        }
        else
        {
            return 0;
        }
    }
    /// <summary>
    /// xml中的CardAttr为中文字符串 需要转为int类型
    /// <para>错误时返回0</para>
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int ChangeCardAttr(string str,string cardID=null )
    {
        if (str.Contains( ComStr.CardAttr_Dark))
        {
            return ComVal.CardAttr_Dark;
        }
        if (str.Contains( ComStr.CardAttr_Earth))
        {
            return ComVal.CardAttr_Earth;
        }
        if (str.Contains( ComStr.CardAttr_Fire))
        {
            return ComVal.CardAttr_Fire;
        }
        if (str.Contains( ComStr.CardAttr_Light))
        {
            return ComVal.CardAttr_Light;
        }
        if (str.Contains( ComStr.CardAttr_Water))
        {
            return ComVal.CardAttr_Water;
        }
        if (str.Contains(ComStr.CardAttr_Wind))
        {
            return ComVal.CardAttr_Wind;
        }
        else
        {
            Debug.Log(cardID);
            return 0;
        }
    }


    public static void Log(object obj)
    {
        Debug.Log(obj);
    }

    public static bool isCardID(string str)
    {
        if (str == "")
        {
            return false;
        }
        return Regex.IsMatch(str, @"^[+-]?\d*$");
    }

    public static Group ToGroup(this Card card)
    {
        Group g = new Group();
        g.AddCard(card);
        return g;
    }
}
