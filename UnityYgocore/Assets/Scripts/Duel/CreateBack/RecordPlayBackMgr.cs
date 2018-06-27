using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public enum RecordEvent
{
    recordEvent_StartGame = 1,//游戏开始，初始化卡组
    recordEvent_OperateCard = 2,//操作卡片
    recordEvent_SelectFieldCard = 3,//选择场地上的卡片
    recordEvent_DialogBoxSelect = 4,//对话框，是否干什么...
    recordEvent_SelectCardGroup = 5,//选择卡片组
    recordEvent_ChangePhase = 6,//转换回合
    recordEvent_SelectPutType = 7,//选择卡片摆放方式
    recordEvent_SelectEffect = 8,//选择效果
    recordEvent_Surrender=9,//投降
}



public class RecordPlayBackMgr : BaseMonoBehivour
{

    List<string> recordMes;

    string S = " ";

    void Awake()
    {
        AddHandler(DuelEvent.duelEvent_RecordOperate, RecordOperate);
        AddHandler(DuelEvent.duelEvent_SavePlayBack, SavePlayBack);
    }

    private void RecordOperate(params object[] args)
    {
        RecordEvent val = (RecordEvent)args[0];
        string result = null;

        switch (val)
        {
            case RecordEvent.recordEvent_StartGame:
                RecordEvent_StartGame((RecordStartGameMes)args[1]);
                break;
            case RecordEvent.recordEvent_OperateCard:
                result = RecoreEventOperateCard(args);
                break;
            case RecordEvent.recordEvent_SelectFieldCard:
                result = RecordEventSelectFieldCard(args);
                break;
            case RecordEvent.recordEvent_DialogBoxSelect:
                result = RecordEventDialogBoxSelect(args);
                break;
            case RecordEvent.recordEvent_SelectCardGroup:
                result = RecordEventSelectCardGroup(args);
                break;
            case RecordEvent.recordEvent_ChangePhase:
                result = RecordEventChangePhase(args);
                break;
            case RecordEvent.recordEvent_SelectPutType:
                result = RecordEventSelectPutType(args);
                break;
            case RecordEvent.recordEvent_SelectEffect:
                result = RecordEventSelectEffect(args);
                break;
            case RecordEvent.recordEvent_Surrender:
                result = RecordEventSurrender(args);
                break;
            default:
                break;
        }
        if (result == null)
        {
            return;
        }
        else
        {
            recordMes.Add(val + S + result);
        }
    }

    private string RecordEventSurrender(object[] args)
    {
        bool val = (bool)args[1];
        return val.ToString();
    }

    private string RecordEventSelectEffect(object[] args)
    {
        int rank = (int)args[1];
        return rank.ToString();
    }

    private string RecordEventSelectPutType(object[] args)
    {
        bool isAttack = (bool)args[1];
        return isAttack.ToString();
    }

    private string RecordEventChangePhase(object[] args)
    {
        int roundCount = (int)args[1];
        int phase = (int)args[2];
        return roundCount+S+ phase.ToString();
    }

    private string RecordEventSelectCardGroup(object[] args)
    {
        List<int> list = (List<int>)args[1];

        string result = list.Count.ToString() + S;
        for (int i = 0; i < list.Count; i++)
        {
            result += list[i].ToString() + S;
        }
        return result;
    }

    private string RecordEventDialogBoxSelect(object[] args)
    {
        bool isTrue = (bool)args[1];
        return isTrue.ToString();
    }

    private string RecordEventSelectFieldCard(object[] args)
    {
        Group g = (Group)args[1];

        string result = g.GroupNum.ToString() + S;
        for (int i = 0; i < g.cardList.Count; i++)
        {
            Card c = g.cardList[i];
            result += c.curArea + S + c.areaRank + S + c.isMy + S;
        }
        return result;
    }

    private string RecoreEventOperateCard(object[] args)
    {
        int area = (int)args[1];
        int rank = (int)args[2];
        string operate = (string)args[3];
        bool isMy = (bool)args[4];
        return area + S + rank + S + operate + S + isMy;
    }


    private void RecordEvent_StartGame(RecordStartGameMes mes)
    {
        recordMes = new List<string>();

        recordMes.Add(mes.player1);
        recordMes.Add(mes.player2);

        AddDeckToMes(mes.player1MainDeck, mes.player2ExtraDeck);
        AddDeckToMes(mes.player2MainDeck, mes.player2ExtraDeck);

        recordMes.Add(mes.isPlayer1First.ToString());
    }

    private void AddDeckToMes(string[] mainDeck, string[] extraDeck)
    {
        recordMes.Add("#main");
        for (int i = 0; i < mainDeck.Length; i++)
        {
            recordMes.Add(mainDeck[i]);
        }
        recordMes.Add("#extra");
        for (int i = 0; i < extraDeck.Length; i++)
        {
            recordMes.Add(extraDeck[i]);
        }
        recordMes.Add("#end");
    }


    private void SavePlayBack(params object[] args)
    {
        string name = (string)args[0];
        FileStream fs = new FileStream(Application.streamingAssetsPath + "/" + name + ".PB", FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);

        recordMes.Insert(0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        for (int i = 0; i < recordMes.Count; i++)
        {
            sw.WriteLine(recordMes[i]);
        }
        sw.WriteLine("#PlayBackEnd");
        sw.Flush();
        sw.Close();
    }
}

public class RecordStartGameMes
{
    public string player1;
    public string player2;

    public string[] player1MainDeck;
    public string[] player1ExtraDeck;

    public string[] player2MainDeck;
    public string[] player2ExtraDeck;

    public bool isPlayer1First;

    public RecordStartGameMes()
    {

    }

    public RecordStartGameMes(string name1, string name2, string[] player1MainDeck, string[] player1ExtraDeck,
                                 string[] player2MainDeck, string[] player2ExtraDeck, bool isPlayer1First)
    {
        this.player1 = name1;
        this.player2 = name2;
        this.player1MainDeck = player1MainDeck;
        this.player1ExtraDeck = player1ExtraDeck;
        this.player2MainDeck = player2MainDeck;
        this.player2ExtraDeck = player2ExtraDeck;
        this.isPlayer1First = isPlayer1First;
    }
}