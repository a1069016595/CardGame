using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class WatchPlayBackMgr : BaseMonoBehivour
{
    StreamReader sr;

    int i = 0;

    Timer nextOperateTimer;
    bool isFinish = false;
    bool isStartPlay = false;
    Duel duel;

    void Awake()
    {
        AddHandler(DuelEvent.gameEvent_EnterPlayBack, StartGame);
        AddHandler(DuelEvent.playBackEvent_StartPlay, StartPlay);
        AddHandler(DuelEvent.playBackEvent_StopPlay, StopPlay);
        AddHandler(DuelEvent.gameEvent_ExitPlayBack, ExitPlay);
    }

    void Start()
    {
        duel = Duel.GetInstance();
        nextOperateTimer = new Timer(3);
    }

    void Update()
    {
        if (!isFinish)
        {
            if (isStartPlay && !duel.IsInAnim())
            {
                if (nextOperateTimer.Update())
                {
                    LoadNextOperate();
                    nextOperateTimer = new Timer(1);
                }
            }
        }
    }
    private void ExitPlay(params object[] args)
    {
        UIMgr.Instance().LoadUI(ComStr.UI_LoginUI);
    }

    private void StopPlay(params object[] args)
    {
        isStartPlay = false;
    }

    private void StartPlay(params object[] args)
    {
        isStartPlay = true;
    }

    private void StartGame(params object[] args)
    {
        isStartPlay = true;
        string name = (string)args[0];
        sr = new StreamReader(Application.streamingAssetsPath + "/" + name + ".PB");
        sr.ReadLine();
        string player1Name = sr.ReadLine();
        string player2Name = sr.ReadLine();
        Deck player1Deck = LoadStartGameDeck();
        Deck player2Deck = LoadStartGameDeck();

        bool isMyRound = sr.ReadLine().ToBool();
        SendEvent(DuelEvent.playBackEvent_StartGame, player1Name, player2Name,
                            player1Deck, player2Deck, isMyRound);
    }

    private Deck LoadStartGameDeck()
    {
        bool readMain = false;
        bool readExtra = false;

        List<string> mainDeck = new List<string>();
        List<string> extraDeck = new List<string>();
        string val = "";
        while (true)
        {
            val = sr.ReadLine();

            if (val == "#end")
            {
                break;
            }
            if (val == "#main")
            {
                readMain = true;
                continue;
            }
            if (val == "#extra")
            {
                readMain = false;
                readExtra = true;
                continue;
            }

            if (readMain)
            {
                mainDeck.Add(val);
            }
            else if (readExtra)
            {
                extraDeck.Add(val);
            }
        }
        Deck result = new Deck(mainDeck.ToArray(), extraDeck.ToArray());
        return result;
    }

    private void LoadNextOperate()
    {
        string val = sr.ReadLine();

        if (val == "#PlayBackEnd")
        {
            isFinish = true;
            StartCoroutine(ShowFinish());
            return;
        }
        string[] operates = Regex.Split(val, " ");


        RecordEvent recordEvent = (RecordEvent)Enum.Parse(typeof(RecordEvent), operates[0]);

        switch (recordEvent)
        {
            case RecordEvent.recordEvent_OperateCard:
                OperateCard(operates);
                break;
            case RecordEvent.recordEvent_SelectFieldCard:
                SelectFieldCard(operates);
                break;
            case RecordEvent.recordEvent_DialogBoxSelect:
                DialogBoxSelect(operates);
                break;
            case RecordEvent.recordEvent_SelectCardGroup:
                SelectCardGroup(operates);
                break;
            case RecordEvent.recordEvent_ChangePhase:
                ChangePhase(operates);
                break;
            case RecordEvent.recordEvent_SelectPutType:
                SelectPutType(operates);
                break;
            case RecordEvent.recordEvent_SelectEffect:
                SelectEffect(operates);
                break;
            case RecordEvent.recordEvent_Surrender:
                Surrender(operates);
                break;
            default:
                break;
        }
    }

    IEnumerator ShowFinish()
    {
        yield return new WaitForSeconds(3);
        normalDele d = delegate
        {
            UIMgr.Instance().LoadUI(ComStr.UI_LoginUI);
        };
        ErrorPlane.GetInstance().Show("录像结束", d);
        sr.Dispose();
        sr.Close();
    }

    private void Surrender(string[] val)
    {
        SendEvent(DuelEvent.playBackEvent_Surrender, val[1].ToBool());
    }

    private void OperateCard(string[] val)
    {
        SendEvent(DuelEvent.playBackEvent_OperateCard, val[1].ToInt(), val[2].ToInt(), val[3], val[4].ToBool());
    }

    private void SelectFieldCard(string[] val)
    {
        int num = val[1].ToInt();

        for (int i = 0; i < num; i++)
        {
            SendEvent(DuelEvent.playBackEvent_SelectFieldCard, val[i * 3 + 2].ToInt(), val[i * 3 + 3].ToInt(), val[i * 3 + 4].ToBool());
        }
    }

    private void DialogBoxSelect(string[] val)
    {
        SendEvent(DuelEvent.playBackEvent_DialogBoxSelect, val[1].ToBool());
    }

    private void SelectCardGroup(string[] val)
    {
        int num = val[1].ToInt();

        for (int i = 0; i < num; i++)
        {
            SendEvent(DuelEvent.playBackEvent_SelectCardGroup, val[i + 2].ToInt());
        }
    }

    private void ChangePhase(string[] val)
    {
        SendEvent(DuelEvent.playBackEvent_ChangePhase,val[1].ToInt(), val[2].ToInt());
    }

    private void SelectPutType(string[] val)
    {
        SendEvent(DuelEvent.playBackEvent_SelectPutType, val[1].ToBool());
    }

    private void SelectEffect(string[] val)
    {
        SendEvent(DuelEvent.playBackEvent_SelectEffect, val[1].ToInt());
    }
}
