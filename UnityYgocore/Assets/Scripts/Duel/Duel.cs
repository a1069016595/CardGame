using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEngine.UI;
using Protocol;
using System.Linq;

/// <summary>
/// 
/// </summary>
/// <param name="card"></param>

public delegate void CardDele(Card card);
public delegate void normalDele();
public delegate void BoolDele(bool val);
public delegate bool Filter(Card card);
//public delegate bool Filter(Card card,Card effectCard);
public delegate void IntDele(int val);

public enum GameState
{
    netWork,
    playBack,
    single,
}

/// <summary>
/// 处理游戏的逻辑
/// </summary>
public class Duel : BaseMonoBehivour, IDuel
{
    #region 单例
    private static Duel instance;

    public Duel()
    {
        instance = this;
    }

    public static Duel GetInstance()
    {
        return instance;
    }
    #endregion

    public bool isShuffle;

    public string TestMyDeck;
    public string TestOtherDeck;

    #region 变量定义
    public Player player1;
    public Player player2;

    public bool isFinishGame = false;
    /// <summary>
    /// 当前阶段
    /// </summary>
    public int currentPhase = 0;

    /// <summary>
    /// 回合数
    /// </summary>
    public int roundCount = 0;

    public Code curCode = new Code(null);
    DuelUIManager duelUIManager;

    public bool isMyRound;

    public Player curPlayer;

    /// <summary>
    /// 效果操作的list
    /// </summary>
    public List<Operation> operateList;

    Chain curChain = new Chain();

    List<Code> waitToCode = new List<Code>();

    private bool isInAnim = false;

    public void SetIsAnim(bool val)
    {
        isInAnim = val;
    }

    public bool IsInAnim()
    {
        return isInAnim;
    }

    List<BaseEffect> resetEffectList;
    List<LimitPlayerEffect> limitEFfectList;

    float timer;

    AttackEvent attackEvent;
    SpSummonEvent spSummonEvent;

    public bool isInAttackAnim = false;

    Stack<DuelDele> delegateStack;


    #endregion

    public int deleCount;

    public bool IsNetWork
    {
        get { return curGameState == GameState.netWork; }
    }

    public bool IsPlayBack
    {
        get { return curGameState == GameState.playBack; }
    }

    public bool IsSingle
    {
        get { return curGameState == GameState.single; }
    }

    public GameState curGameState;

    Console console;

    List<DelayAction> duelActionList = new List<DelayAction>();

    void Awake()
    {
        delegateStack = new Stack<DuelDele>();
        duelUIManager = transform.GetComponent<DuelUIManager>();
        resetEffectList = new List<BaseEffect>();
        limitEFfectList = new List<LimitPlayerEffect>();


        AddHandler(DuelEvent.duelEvent_changePhase, ChangeToPhase);
        AddHandler(DuelEvent.duelEvent_operateCard, OperateCard);
        AddHandler(DuelEvent.duelEvent_Surrender, PlayerSurrender);

        AddHandler(DuelEvent.netEvent_ReciveOperateCard, ReciveOperateCard);
        AddHandler(DuelEvent.netEvent_ReciveChangePhase, ReciveChangePhase);
        AddHandler(DuelEvent.netEvent_ReciveGameEnd, ReciveGameEnd);
        AddHandler(DuelEvent.netEvent_ReciveSurrender, ReciveSurrender);

        AddHandler(DuelEvent.playBackEvent_StartGame, StartPlayBack);
        AddHandler(DuelEvent.playBackEvent_ChangePhase, PlayBackChangePhase);
        AddHandler(DuelEvent.playBackEvent_OperateCard, PlayBackOperateCard);
        AddHandler(DuelEvent.playBackEvent_Surrender, PlayBackSurrender);

        console = GetComponent<Console>();
    }




    void Start()
    {
        if (curGameState == GameState.single)
        {
            Deck deck1 = DeckLoad.LoadDeck(TestMyDeck);
            Deck deck2 = DeckLoad.LoadDeck(TestOtherDeck);
            StartGame(deck1, deck2, "player1", "player2",true);
        }
        // NetWorkScript.Instance.write(TypeProtocol.TYPE_DUEL_BRQ, 0, DuelProtocol.ChangePhase_BRQ, null);
    }

    void Update()
    {
        deleCount = delegateStack.Count;
    }

    private void ReciveOperateCard(params object[] args)
    {
        DuelOperateCardDTO dto = (DuelOperateCardDTO)args[0];
        OperateCard(dto.area, dto.rank, dto.str, false);

        SendEvent(DuelEvent.duelEvent_RecordOperate, RecordEvent.recordEvent_OperateCard, dto.area, dto.rank, dto.str, false);
    }

    private void ReciveChangePhase(params object[] args)
    {
        DuelChangePhaseDTO dto = (DuelChangePhaseDTO)args[0];
        ChangeToPhase(dto.phase);

    }

    private void PlayBackOperateCard(params object[] args)
    {
        OperateCard(args);
    }

    private void PlayBackChangePhase(params object[] args)
    {
        if((int)args[0]==roundCount)//确认是否为该回合
        {
            ChangeToPhase(args[1]);
        }
    }


    private void StartPlayBack(params object[] args)
    {
        curGameState = GameState.playBack;
        string player1Name = (string)args[0];
        string player2Name = (string)args[1];

        Deck deck1 = (Deck)args[2];
        Deck deck2 = (Deck)args[3];
        bool isMyRound = (bool)args[4];

        StartGame(deck1, deck2, player1Name, player2Name, isMyRound);
    }


    /// <summary>
    /// 单机或回放用
    /// </summary>
    /// <param name="_isSuffler"></param>
    public void StartGame(Deck deck1, Deck deck2, string p1, string p2, bool isMy)
    {
        player1 = new Player(true, p1);
        player2 = new Player(false, p2);
        isMyRound = isMy;
        if (isMyRound)
        {
            curPlayer = player1;
        }
        else
        {
            curPlayer = player2;
        }
        FloatTextDele b = delegate()
        {
            normalDele d1 = delegate
            {
                curChain = new Chain();
                ChangeToPhase(ComVal.Phase_Drawphase);
            };
            AddDelegate(d1, "转向抽卡阶段");
            Init(deck1, deck2, p1, p2);
        };
        duelUIManager.ShowFloatText("决斗开始", b);
        curCode.code = ComVal.code_NoCode;
        InvokeRepeating("CheckPhaseChange", 0, 0.1f);
        roundCount = 1;
        duelUIManager.UpdateRoundCount(roundCount);
    }


    /// <summary>
    /// 用于单机测试
    /// </summary>
    /// <param name="dto1"></param>
    /// <param name="dto2"></param>
    public void Init(Deck dto1, Deck dto2, string p1, string p2)
    {
        duelUIManager.InitBothLPUI(p1, p2);
        InitDeck(dto1, dto2);
        DrawCard(player1, 6);
        DrawCard(player2, 6);
    }

    /// <summary>
    /// 实际用
    /// </summary>
    /// <param name="dto1"></param>
    /// <param name="dto2"></param>
    public void StartGame()
    {
        //猜拳逻辑由客户端实现
        curGameState = GameState.netWork;

        GuessDele a = delegate(bool val)
        {
            if (val)
            {
                isMyRound = true;
                //猜完拳之后发送游戏开始信息
                NetWorkScript.Instance.write(TypeProtocol.TYPE_DUEL_BRQ, 0, DuelProtocol.STARTGAME_BRQ, null);
            }
            else
            {
                isMyRound = false;
            }
        };
        duelUIManager.StartGuessFirst(a);
    }

    /// <summary>
    /// 接收到服务器信息之后开始游戏
    /// </summary>
    /// <param name="dto1"></param>
    /// <param name="dto2"></param>
    public void StartDuel(StartGameDTO val)
    {
        FloatTextDele b = delegate()
        {
            curChain = new Chain();
            Init(val);
            ChangeToPhase(ComVal.Phase_Drawphase);
        };
        duelUIManager.ShowFloatText("决斗开始", b);
        curCode.code = ComVal.code_NoCode;
        InvokeRepeating("CheckPhaseChange", 0, 0.1f);
        roundCount = 1;
        duelUIManager.UpdateRoundCount(roundCount);
    }


    /// <summary>
    /// 用于联网测试
    /// </summary>
    /// <param name="dto1"></param>
    /// <param name="dto2"></param>
    public void Init(StartGameDTO dto)
    {
        Deck mDeck;
        Deck oDeck;

        string account1 = dto.deckMes1.accountName;
        string account2 = dto.deckMes2.accountName;

        player1 = new Player(true, account1);
        player2 = new Player(false, account2);

        if(isMyRound)
        {
            curPlayer = player1;
        }
        else
        {
            curPlayer = player2;
        }
        if (ComVal.account == account1)
        {
            duelUIManager.InitBothLPUI(account1, account2);
            mDeck = new Deck(dto.deckMes1.deck.mainDeck, dto.deckMes1.deck.extraDeck);
            oDeck = new Deck(dto.deckMes2.deck.mainDeck, dto.deckMes2.deck.extraDeck);
        }
        else
        {
            duelUIManager.InitBothLPUI(account2, account1);
            mDeck = new Deck(dto.deckMes2.deck.mainDeck, dto.deckMes2.deck.extraDeck);
            oDeck = new Deck(dto.deckMes1.deck.mainDeck, dto.deckMes1.deck.extraDeck);
        }
        InitDeck(mDeck, oDeck);
        DrawCard(player1, 6);
        DrawCard(player2, 6);
    }


    /// <summary>
    /// 检测阶段变化
    /// </summary>
    private void CheckPhaseChange()
    {
        if (currentPhase == ComVal.Phase_Drawphase || currentPhase == ComVal.Phase_Standbyphase)
        {
            if (curCode.code == ComVal.code_NoCode && isInAnim == false)
            {
                int val;
                if (currentPhase == ComVal.Phase_Drawphase)
                {
                    val = ComVal.Phase_Standbyphase;
                }
                else
                {
                    val = ComVal.Phase_Mainphase1;
                }
                ChangeToPhase(val);
            }
        }
    }

    /// <summary>
    /// 检测在墓地，额外，除外卡组的可发动效果
    /// <para>用于显示act动画</para>
    /// </summary>
    void CheckDeckAct()
    {
        duelUIManager.HideActAnim(ComVal.Area_Graveyard);
        duelUIManager.HideActAnim(ComVal.Area_Remove);
        duelUIManager.HideActAnim(ComVal.Area_Extra);
        foreach (var item in player1.group_Graveyard.cardList)
        {
            if (item.CanLauchEffect(curCode, curChain, ComVal.cardEffectType_normalLauch))
            {
                duelUIManager.ShowActAnim(ComVal.Area_Graveyard);
            }
        }
        foreach (var item in player1.group_Remove.cardList)
        {
            if (item.CanLauchEffect(curCode, curChain, ComVal.cardEffectType_normalLauch))
            {
                duelUIManager.ShowActAnim(ComVal.Area_Remove);
            }
        }

        ShowSynchro();
    }

    #region 阶段管理


    public void EnterDrawPhase()
    {
        duelUIManager.PhaseNotControl();
        FloatTextDele a = delegate
       {
           CreateCode(null, curPlayer, ComVal.code_EnterDrawPhase, null, 0, null);
           DrawCard(curPlayer, 1);
           duelUIManager.PhaseCanControl();
           isInAnim = false;
       };
        duelUIManager.ShowFloatText("抽卡阶段", a);
        isInAnim = true;
    }

    public void EnterStandbyphase()
    {
        duelUIManager.PhaseNotControl();
        FloatTextDele a = delegate
        {
            CreateCode(null, curPlayer, ComVal.code_EnterStandByPhase, null, 0, null);
            duelUIManager.PhaseCanControl();
            isInAnim = false;
        };
        duelUIManager.ShowFloatText("准备阶段", a);
        isInAnim = true;
    }

    public void EnterMainPhase()
    {
        duelUIManager.PhaseNotControl();
        FloatTextDele a = delegate
       {
           CreateCode(null, curPlayer, ComVal.code_EnterMainPhase1, null, 0, null);
           duelUIManager.PhaseCanControl();
           isInAnim = false;
       };
        duelUIManager.ShowFloatText("主阶段1", a);
        isInAnim = true;
    }

    public void EnterBattlePhase()
    {
        duelUIManager.PhaseNotControl();
        FloatTextDele a = delegate
       {
           CreateCode(null, curPlayer, ComVal.code_EnterBattlePhase, null, 0, null);
           duelUIManager.PhaseCanControl();
           isInAnim = false;
       };
        duelUIManager.ShowFloatText("战斗阶段", a);
        isInAnim = true;
    }

    public void EnterMainPhase2()
    {
        duelUIManager.PhaseNotControl();
        FloatTextDele a = delegate
       {
           CreateCode(null, curPlayer, ComVal.code_EnterMainPhase2, null, 0, null);
           duelUIManager.PhaseCanControl();
           isInAnim = false;
       };
        duelUIManager.ShowFloatText("主阶段2", a);
        isInAnim = true;
    }

    public void EnterEndPhase()
    {
        duelUIManager.PhaseNotControl();
        FloatTextDele a = delegate
       {
           normalDele b = delegate
           {
               ChangeRound();
           };
           AddDelegate(b, "改变回合");
           duelUIManager.PhaseCanControl();
           isInAnim = false;

           CreateCode(null, curPlayer, ComVal.code_EnterEndPhase, null, 0, null);
       };
        duelUIManager.ShowFloatText("结束阶段", a);
        isInAnim = true;
    }

    /// <summary>
    /// 转到下一阶段
    /// <para>由phaseButtonMgr调用</para>
    /// </summary>
    public void ChangeToPhase(params object[] args)
    {
        int phase = (int)args[0];
        if (isInAnim || !IsFree())
        {
            console.Log("切换阶段错误");
            return;
        }
        if(phase>ComVal.Phase_Mainphase1)
        {
            SendEvent(DuelEvent.duelEvent_RecordOperate, RecordEvent.recordEvent_ChangePhase, roundCount, phase);
        }

        currentPhase = phase;
        normalDele d = delegate
        {
            
            if (ComVal.isBind(currentPhase, ComVal.Phase_Drawphase))
            {
                EnterDrawPhase();
            }
            else if (ComVal.isBind(currentPhase, ComVal.Phase_Standbyphase))
            {
                EnterStandbyphase();
            }
            else if (ComVal.isBind(currentPhase, ComVal.Phase_Mainphase1))
            {
                EnterMainPhase();
            }
            else if (ComVal.isBind(currentPhase, ComVal.Phase_Battlephase))
            {
                EnterBattlePhase();
            }
            else if (ComVal.isBind(currentPhase, ComVal.Phase_Mainphase2))
            {
                EnterMainPhase2();
            }
            else if (ComVal.isBind(currentPhase, ComVal.Phase_Endphase))
            {
                EnterEndPhase();
            }
            else
            {
                console.Log("error");
            }
            duelUIManager.ChangeToPhase(phase);
            UpdateCardMesShow();
        };
        AddDelegate(d, true);

        int resetCode = ComVal.GetResetCode(currentPhase);
        HandleResetEffectList(resetCode);
        HandleDelayAction(resetCode);
    }

    private void ChangeRound()
    {
        roundCount++;
        duelUIManager.UpdateRoundCount(roundCount);
        curPlayer.EndRoundReset();
        player1.ResetCounter();
        player2.ResetCounter();
        if (curPlayer == player1)
        {
            curPlayer = player2;
            isMyRound = false;
        }
        else
        {
            curPlayer = player1;
            isMyRound = true;
        }
        ChangeToPhase(ComVal.Phase_Drawphase);
    }

    #endregion



    Player GetPlayer(bool isMy)
    {
        if (isMy)
            return player1;
        else
            return player2;
    }

    public Player GetOpsitePlayer(Player player)
    {
        if (player == player1)
        {
            return player2;
        }
        else
        {
            return player1;
        }
    }

    bool GetBool(Player player)
    {
        if (player == player1)
            return true;
        else
            return false;
    }

    /// <summary>
    /// 生成卡组 并且洗牌
    /// </summary>
    /// <param name="deck"></param>
    private void InitDeck(Deck mDeck, Deck oDeck)
    {
        player1.SetOtherPlayer(player2);
        player2.SetOtherPlayer(player1);
        player1.group_MainDeck = mDeck.mainDeck;
        player1.group_ExtraDeck = mDeck.extraDeck;

        for (int i = 0; i < player1.group_MainDeck.cardList.Count; i++)
        {
            Card card = player1.group_MainDeck.GetCard(i);
            card.SetArea(ComVal.Area_MainDeck, null);
            card.Init(true);
            card.SetPlayer(player1);
            card.SetOpponentPlayer(player2);
            ResignCard(card);
        }
        for (int i = 0; i < player1.group_ExtraDeck.GroupNum; i++)
        {
            Card card = player1.group_ExtraDeck.GetCard(i);
            card.SetArea(ComVal.Area_Extra, null);
            card.SetAreaRank(i);
            card.Init(true);
            card.SetPlayer(player1);
            card.SetOpponentPlayer(player2);
            ResignCard(card);
        }

        player2.group_MainDeck = oDeck.mainDeck;
        player2.group_ExtraDeck = oDeck.extraDeck;

        for (int i = 0; i < player2.group_MainDeck.cardList.Count; i++)
        {
            Card card = player2.group_MainDeck.GetCard(i);
            card.SetArea(ComVal.Area_MainDeck, null);
            card.Init(false);
            card.SetPlayer(player2);
            card.SetOpponentPlayer(player1);
            ResignCard(card);
        }
        for (int i = 0; i < player2.group_ExtraDeck.GroupNum; i++)
        {
            Card card = player2.group_ExtraDeck.GetCard(i);
            card.SetArea(ComVal.Area_Extra, null);
            card.SetAreaRank(i);
            card.Init(false);
            card.SetPlayer(player2);
            card.SetOpponentPlayer(player1);
            ResignCard(card);
        }
        duelUIManager.InitBothDeck(player1.group_MainDeck.GroupNum, player1.group_ExtraDeck.GroupNum,
        player2.group_MainDeck.GroupNum, player2.group_ExtraDeck.GroupNum);

        if (isShuffle)
        {
            ShuffleDeck(player1);
            ShuffleDeck(player2);
        }

        //for (int i = 0; i < player1.group_MainDeck.cardList.Count; i++)
        //{
        //    Debug.Log(player1.group_MainDeck.cardList[i].cardName);
        //}

        RecordStartGameMes mes = new RecordStartGameMes(player1.name, player2.name,
                                                 player1.group_MainDeck.ToArray(), player1.group_ExtraDeck.ToArray(),
                                                player2.group_MainDeck.ToArray(), player2.group_ExtraDeck.ToArray(), isMyRound);
        SendEvent(DuelEvent.duelEvent_RecordOperate, RecordEvent.recordEvent_StartGame, mes);
    }

    /// <summary>
    /// 洗主卡组
    /// </summary>
    public void ShuffleDeck(Player player)
    {
        if (!IsSingle || !isShuffle)
        {
            return;
        }
        player.group_MainDeck.Shuffle();
    }

    #region 注册

    /// <summary>
    /// 注册一张新卡片
    /// </summary>
    public void ResignCard(Card card)
    {
        string str;
        str = "C" + card.cardID;
        Type a = Type.GetType(str);
        if (a == null)
        {
            return;
        }
        try
        {
            MethodInfo info = a.GetMethod("InitialEffect");
            object obj = Activator.CreateInstance(a);
            info.Invoke(obj, new object[] { card, card.ownerPlayer, this });
        }
        catch (Exception)
        {
            Debug.Log(str);
            throw;
        }

    }


    /// <summary>
    /// 注册一个效果
    /// </summary>
    /// <param name="effect"></param>
    public void ResignEffect(BaseEffect effect, Card card, Player player)
    {
        effect.Init(card);
        card.AddEffect(effect);
        if (effect.resetCode != 0)
        {
            resetEffectList.Add(effect);
        }
        if (effect.IsBindType(EffectType.LimitPlayerEffect))
        {
            limitEFfectList.Add((LimitPlayerEffect)effect);
            return;
        }
        else if (effect.IsBindType(EffectType.StateEffect))
        {
            StateEffect stateEffect = (StateEffect)effect;
            if (stateEffect.IsBindCardEffectType(ComVal.cardEffectType_Single))
            {
                AddStateEffectToCard(stateEffect.targetCard, stateEffect);
                return;
            }
            else if (stateEffect.IsBindCardEffectType(ComVal.cardEffectType_Multiple))
            {
                foreach (Card item in stateEffect.targetGroup)
                {
                    AddStateEffectToCard(item, stateEffect);
                    return;
                }
            }
            else
            {
                console.Log("error");
            }
        }
        else if (effect.IsBindType(EffectType.EquipEffect))
        {

        }
    }

    public void ResignEffectLauchLimitCounter(Player p, string effectID, int maxLuachTime)
    {
        p.AddEffectCounter(effectID, maxLuachTime);
    }

    #endregion

    /// <summary>
    /// 处理效果
    /// </summary>
    /// <param name="eventCode"></param>
    private void HandleResetEffectList(int eventCode)
    {
        foreach (BaseEffect item in resetEffectList)
        {
            item.CheckReset(eventCode);
        }
    }

    private void AddStateEffectToCard(Card card, StateEffect effect)
    {
        if (effect.IsBindCardEffectType(ComVal.cardEffectType_equip))
        {
            effect.targetCard.AddEquipEffect(effect);
            effect.equipCard.AddEquipEffect(effect);
        }
        else if (effect.IsBindCardEffectType(ComVal.cardEffectType_materialSynchro))
        {
            card.AddMaterialSynchroEffect(effect);
        }
        else if (effect.IsBindCardEffectType(ComVal.cardEffectType_materialXYZ))
        {
            card.AddMaterialXYZEffect(effect);
        }
        else if (effect.IsBindCardEffectType(ComVal.cardEffectType_normalStateEffect))
        {
            card.AddSingleEffect(effect);
        }
        else
        {
            console.Log(card.cardID);
            console.Log("error");
        }
    }



    /// <summary>
    /// 获取连锁
    /// </summary>
    /// <returns></returns>
    public Chain GetCurChain()
    {
        return curChain;
    }

    /// <summary>
    /// 从卡组抽卡
    /// </summary>
    public void DrawCard(Player player, int num, Card reasonCard = null, BaseEffect reasonEffect = null)
    {
        if (player.group_MainDeck.GroupNum < num)
        {
            console.Log("主卡组没卡");
            return;
        }
        if (num == 0)
        {
            FinishHandle();
            return;
        }
        Card card = Duel_ChangeCardArea.RemoveCardFromArea(Card.Empty(), player, ComVal.Area_MainDeck);
        normalDele dele = delegate
        {
            Reason r;
            if (reasonEffect != null)
            {
                r = new Reason(ComVal.reason_Effect, reasonCard, reasonEffect);
            }
            else
            {
                r = new Reason(ComVal.reason_NormalDrawCard, null, null);
            }
            Duel_ChangeCardArea.AddCardToArea(ComVal.Area_Hand, card, player, r, 0, false);
            if (num - 1 == 0)
            {
                if (reasonCard != null)
                {
                    CreateCode(null, player, ComVal.code_DrawCard, reasonCard, 0, reasonEffect, true);
                }
                else
                {
                    FinishHandle();
                }
                return;
            }
            DrawCard(player, num - 1, reasonCard, reasonEffect);
        };
        duelUIManager.ShowDrawAnim(card.cardID, dele, player.group_HandCard.GroupNum, player.isMy);
    }

    /// <summary>
    /// 将卡片从卡组加入到手牌
    /// </summary>
    /// <param name="card"></param>
    /// <param name="player"></param>
    public void AddCardToHandFromMainDeck(Card card, Player player, Card reasonCard, BaseEffect reasonEffect)
    {
        if (ComVal.Area_MainDeck != card.curArea)
            return;
        Duel_ChangeCardArea.RemoveCardFromArea(card, player, ComVal.Area_MainDeck);
        Reason r = new Reason(ComVal.reason_Effect, reasonCard, reasonEffect);

        int returnArea = ComVal.Area_Hand;
        if (card.cardType.IsBind(ComVal.cardType_Extra))
        {
            returnArea = ComVal.Area_Extra;
        }
        normalDele dele = delegate
        {
            Group group = new Group();
            group.AddCard(card);
            AddFinishHandle();
            CreateCode(group, player, ComVal.code_AddCardToHand, reasonCard, ComVal.reason_Effect, reasonEffect, true);
        };
        AddDelegate(dele, "处理加入手牌时点");
        Duel_ChangeCardArea.AddCardToArea(returnArea, card, player, r);
    }


    public void AddCardToHandFromArea(int area, Group g, Player player, Card reasonCard, BaseEffect reasonEffect)
    {
        g = g.GetAreaGroup(area);
        if (g.GroupNum == 0)
        {
            FinishHandle();
            return;
        }
        Reason r = new Reason(ComVal.reason_Effect, reasonCard, reasonEffect);

        normalDele d1 = delegate
        {
            CreateCode(g, player, ComVal.code_AddCardToHand, reasonCard, ComVal.reason_Effect, reasonEffect);
            FinishHandle();
        };
        AddDelegate(d1);
        AddCardToHand(g.ToList(), r);
    }

    public void AddCardToHandFromArea(int area, Card c, Player player, Card card, LauchEffect effect)
    {
        if (area.IsBind(ComVal.Area_MainDeck))
        {
            AddCardToHandFromMainDeck(c, player, card, effect);
        }
        else
        {
            AddCardToHandFromArea(area, c.ToGroup(), player, card, effect);
        }
    }

    private void AddCardToHand(List<Card> cardList, Reason r)
    {
        Card c = cardList[0];
        cardList.RemoveAt(0);
        int returnArea = ComVal.Area_Hand;
        if (c.cardType.IsBind(ComVal.cardType_Extra))
        {
            returnArea = ComVal.Area_Extra;
        }
        normalDele d1 = delegate
        {
            Duel_ChangeCardArea.AddCardToArea(returnArea, c, c.controller, r);
            normalDele d2 = delegate
            {
                if (cardList.Count == 0)
                {
                    FinishHandle();
                }
                else
                {
                    AddCardToHand(cardList, r);
                }
            };
            AddDelegate(d2, true);
            CardLeaveAreaCheck(c);
        };
        AddDelegate(d1);
        duelUIManager.ShowChangeAreaAnim(c.cardID, c.curArea, returnArea, c.curPlaseState, -1, c.ownerPlayer.isMy, c.areaRank, c.controller.group_HandCard.GroupNum);
        Duel_ChangeCardArea.RemoveCardFromArea(c, c.controller, c.curArea);
    }

    /// <summary>
    /// 改变怪兽表示形式
    /// </summary>
    /// <param name="val"></param>
    void ChangeMonsterType(Card card, Player player)
    {
        player.group_MonsterCard.GetCard(card.areaRank);
        if (!card.IsMonster())
        {
            console.Log("error");
        }
        if (card.curPlaseState == ComVal.CardPutType_layFront)
        {
            duelUIManager.ChangeMonsterPlaseType(card.areaRank, ComVal.CardPutType_UpRightFront, player.isMy, card.cardID);
            card.curPlaseState = ComVal.CardPutType_UpRightFront;
        }
        else if (card.curPlaseState == ComVal.CardPutType_UpRightFront)
        {
            duelUIManager.ChangeMonsterPlaseType(card.areaRank, ComVal.CardPutType_layFront, player.isMy, card.cardID);
            card.curPlaseState = ComVal.CardPutType_layFront;
        }
        else if (card.curPlaseState == ComVal.CardPutType_layBack)
        {
            duelUIManager.ChangeMonsterPlaseType(card.areaRank, ComVal.CardPutType_UpRightFront, player.isMy, card.cardID);
            card.curPlaseState = ComVal.CardPutType_UpRightFront;
            CreateCode(card.ToGroup(), card.controller, ComVal.code_TurnBackSummon, card, -1, null);
        }
        else
        {
            console.Log("error");
        }
        card.cardChangeTypeedTime++;
        card.cardChangeTypeTime--;
    }


    /// <summary>
    /// 改变卡片放置形式
    /// <para>卡的效果</para>
    /// </summary>
    /// <param name="putType"></param>
    /// <param name="card"></param>
    public void ChangeMonsterType(int putType, Card card)
    {
        if (card.curArea != ComVal.Area_Monster)
            return;
        duelUIManager.ChangeMonsterPlaseType(card.areaRank, putType, card.ownerPlayer.isMy, card.cardID);
        card.curPlaseState = putType;
    }

    /// <summary>
    /// 翻转怪兽
    /// <para>默认翻转后为正面竖放状态</para>
    /// </summary>
    public void FlipCard(Card card, Player player, Card reasonCard, BaseEffect reasonEffect)
    {
        if (!card.IsMonster())
        {
            console.Log("error");
        }
        duelUIManager.ChangeMonsterPlaseType(card.areaRank, ComVal.CardPutType_UpRightFront, player.isMy, card.cardID);
        card.curPlaseState = ComVal.CardPutType_UpRightFront;
        card.cardChangeTypeedTime++;
        card.cardChangeTypeTime--;
        Group group = new Group();
        group.AddAndSetCard(card);
        CreateCode(group, player, ComVal.code_TurnBackSummon, reasonCard, 0, reasonEffect);
    }

    public Group SendToGraveyard(List<int> area, Group group, Card reasonCard, int theReason, BaseEffect effect = null)
    {
        group = group.SiftingGroupInArea(area);
        if (group.GroupNum == 0)
        {
            FinishHandle();
            return group;
        }

        Card mReasonCard = null;
        BaseEffect mReasonEffect = null;
        Card oReasonCard = null;
        BaseEffect oReasonEffect = null;

        Group sendToGraveyardGroup = new Group();

        Group msendToRemoveGroup = new Group();
        Group osendToRemoveGroup = new Group();

        for (int i = 0; i < group.cardList.Count; i++)
        {
            Card item = group.cardList[i];
            if (item.ownerPlayer.isMy)
            {
                if (item.ownerPlayer.CanSendToGraveyard())
                {
                    sendToGraveyardGroup.AddCard(item);
                }
                else
                {
                    mReasonCard = item.ownerPlayer.GetSendToRemoveReasonCard();
                    mReasonEffect = item.ownerPlayer.GetSendToRemoveReasonEffect();
                    msendToRemoveGroup.AddCard(item);
                }
            }
            else
            {
                if (item.ownerPlayer.CanSendToGraveyard())
                {
                    sendToGraveyardGroup.AddCard(item);
                }
                else
                {
                    oReasonCard = item.ownerPlayer.GetSendToRemoveReasonCard();
                    oReasonEffect = item.ownerPlayer.GetSendToRemoveReasonEffect();
                    osendToRemoveGroup.AddCard(item);
                }
            }
        }

        if (msendToRemoveGroup.GroupNum > 0)
        {
            Reason r = new Reason(ComVal.reason_Effect, mReasonCard, mReasonEffect);
            SendToRemove(msendToRemoveGroup.GetAreaList(), msendToRemoveGroup, mReasonCard, ComVal.reason_Effect, mReasonEffect);
        }
        if (osendToRemoveGroup.GroupNum > 0)
        {
            Reason r = new Reason(ComVal.reason_Effect, oReasonCard, oReasonEffect);
            SendToRemove(osendToRemoveGroup.GetAreaList(), osendToRemoveGroup, oReasonCard, ComVal.reason_Effect, oReasonEffect);
        }
        if (sendToGraveyardGroup.GroupNum > 0)
        {
            Reason r = new Reason(theReason, reasonCard, effect);

            normalDele d1 = delegate
            {
                CreateCode(sendToGraveyardGroup, reasonCard.controller, ComVal.code_ToGraveyard, reasonCard, theReason, effect);
                FinishHandle();
            };
            AddDelegate(d1);
            SendCardToGraveyard(sendToGraveyardGroup.ToList(), r);
            return sendToGraveyardGroup;
        }
        return new Group();
    }

    /// <summary>
    /// 送去墓地   
    /// <para>检测状态效果的清除</para>
    /// <para>当卡片不在指定区域时，不执行此操作</para>
    /// </summary>
    /// <param name="area">卡片当前区域</param>
    /// <param name="group"></param>
    /// <param name="player"></param>
    /// <param name="reasonCard">原因卡片</param>
    /// <param name="theReason">原因</param>
    /// <param name="effect">原因效果</param>
    public Group SendToGraveyard(int area, Group group, Card reasonCard, int theReason, BaseEffect effect = null)
    {
        List<int> val = new List<int>();
        for (int i = 0; i < group.cardList.Count; i++)
        {
            val.Add(area);
        }
        return SendToGraveyard(val, group, reasonCard, theReason, effect);
    }

    /// <summary>
    /// 从卡组最上方送去墓地
    /// TODO:实现动画效果
    /// </summary>
    public Group DiscardFromDeck(int cardNum, Card reasonCard, BaseEffect effect, Player player)
    {
        Group group = new Group();
        for (int i = 0; i < cardNum; i++)
        {
            if (player.group_MainDeck.GetFirstCard() == null)
            {
                break;
            }
            group.AddCard(player.group_MainDeck.GetFirstCard());
            player.group_MainDeck.RemoveFirstCard();
        }
        return SendToGraveyard(ComVal.Area_MainDeck, group, reasonCard, ComVal.reason_Effect, effect);
    }

    /// <summary>
    /// 送去除外区
    /// <para>检测状态效果的清除</para>
    /// </summary>
    /// <param name="area"></param>
    /// <param name="group"></param>
    /// <param name="player"></param>
    /// <param name="reasonCard"></param>
    /// <param name="theReason"></param>
    /// <param name="effect"></param>
    public void SendToRemove(List<int> area, Group group, Card reasonCard, int theReason, BaseEffect effect)
    {
        group = group.SiftingGroupInArea(area);
        if (group.GroupNum == 0)
            return;
        Reason r = new Reason(theReason, reasonCard, effect);

        normalDele d = delegate
        {
            CreateCode(group, reasonCard.ownerPlayer, ComVal.code_Remove, reasonCard, theReason, effect);
            FinishHandle();
        };
        AddDelegate(d);
        SendCardToRemove(group.ToList(), r);
    }

    /// <summary>
    /// 送去除外区
    /// <para>检测状态效果的清除</para>
    /// </summary>
    /// <param name="area"></param>
    /// <param name="group"></param>
    /// <param name="player"></param>
    /// <param name="reasonCard"></param>
    /// <param name="theReason"></param>
    /// <param name="effect"></param>
    public void SendToRemove(int area, Group group, Card reasonCard, int theReason, BaseEffect effect)
    {
        List<int> areaList = new List<int>();
        for (int i = 0; i < group.cardList.Count; i++)
        {
            areaList.Add(area);
        }
        SendToRemove(areaList, group, reasonCard, theReason, effect);
    }

    /// <summary>
    /// 将卡片返回主卡组
    /// <para></para>
    /// </summary>
    private void SendCardToMainDeck(List<Card> list, Reason r)
    {
        Card c = list[0];
        list.RemoveAt(0);

        int returnArea;
        if (c.cardType.IsBind(ComVal.cardType_Extra))
        {
            returnArea = ComVal.Area_Extra;
        }
        else
        {
            returnArea = ComVal.Area_MainDeck;
        }

        normalDele d = delegate
        {
            Duel_ChangeCardArea.AddCardToArea(returnArea, c, c.ownerPlayer, r);
            normalDele d1 = delegate
            {

                if (list.Count == 0)
                {
                    FinishHandle();
                }
                else
                {
                    SendCardToMainDeck(list, r);
                }
            };
            AddDelegate(d1, true);
            CardLeaveAreaCheck(c);
        };
        AddDelegate(d);
        duelUIManager.ShowChangeAreaAnim(c.cardID, c.curArea, returnArea, c.curPlaseState, -1, c.controller.isMy, c.areaRank);
        Duel_ChangeCardArea.RemoveCardFromArea(c, c.ownerPlayer, 0);
    }

    public void SendToMainDeck(int fromArea, Group g, Card reasonCard, int theReason, BaseEffect effect)
    {
        g = g.SiftingGroupInArea(fromArea);
        if (g.GroupNum == 0)
            return;
        Reason r = new Reason(theReason, reasonCard, effect);

        normalDele d = delegate
        {
            CreateCode(g, reasonCard.ownerPlayer, ComVal.code_ToMainDeck, reasonCard, theReason, effect);
            FinishHandle();
        };
        AddDelegate(d);
        SendCardToMainDeck(g.ToList(), r);
    }


    /// <summary>
    /// 将卡片送去墓地 
    /// <para></para>
    /// </summary>
    private void SendCardToGraveyard(List<Card> list, Reason r)
    {
        Card c = list[0];
        list.RemoveAt(0);
        normalDele d = delegate
        {
            Duel_ChangeCardArea.AddCardToArea(ComVal.Area_Graveyard, c, c.ownerPlayer, r);

            normalDele d2 = delegate
            {
                if (list.Count == 0)
                {
                    FinishHandle();
                }
                else
                {
                    SendCardToGraveyard(list, r);
                }
            };
            AddDelegate(d2, true);
            CardLeaveAreaCheck(c);

        };
        AddDelegate(d);
        console.Log(c.curArea);
        duelUIManager.ShowChangeAreaAnim(c.cardID, c.curArea, ComVal.Area_Graveyard, c.curPlaseState, -1, c.controller.isMy, c.areaRank);
        Duel_ChangeCardArea.RemoveCardFromArea(c, c.ownerPlayer, 0);
    }


    /// <summary>
    /// 将卡片送去除外
    /// <para></para>
    /// </summary>
    private void SendCardToRemove(List<Card> list, Reason r)
    {
        Card c = list[0];
        list.RemoveAt(0);
        normalDele d = delegate
        {
            Duel_ChangeCardArea.AddCardToArea(ComVal.Area_Remove, c, c.ownerPlayer, r);

            normalDele d1 = delegate
            {
                if (list.Count == 0)
                {
                    FinishHandle();
                }
                else
                {
                    SendCardToRemove(list, r);
                }
            };
            AddDelegate(d1, true);
            CardLeaveAreaCheck(c);

        };
        AddDelegate(d);
        duelUIManager.ShowChangeAreaAnim(c.cardID, c.curArea, ComVal.Area_Remove, c.curPlaseState, -1, c.controller.isMy, c.areaRank);
        Duel_ChangeCardArea.RemoveCardFromArea(c, c.ownerPlayer, 0);
    }


    /// <summary>
    /// 将卡片返回手牌
    /// </summary>
    /// <param name="area"></param>
    /// <param name="group"></param>
    /// <param name="player"></param>
    /// <param name="reason"></param>
    public void SendCardToHand(int area, Group group, Player player, Card reasonCard, BaseEffect reasonEffect)
    {
        if (group.GroupNum == 0)
        {
            console.Log("error");
        }
        for (int i = 0; i < group.GroupNum; i++)
        {
            Reason r = new Reason(ComVal.reason_Effect, reasonCard, reasonEffect);
            Card card = group.GetCard(i);
            Duel_ChangeCardArea.RemoveCardFromArea(card, player, 0);
        }
        CreateCode(group, player, ComVal.code_ReturnToHand, reasonCard, 0, reasonEffect);
    }

    /// <summary>
    /// 将卡片返回手牌
    /// <para></para>
    /// </summary>
    private void SendCardToHand(Group g, Reason r, Player p)
    {
        Card c = g.GetCard(0);
        g.RemoveCard(c);

        normalDele d = delegate
        {
            if (c.cardType == ComVal.CardType_Monster_Fusion || c.cardType == ComVal.CardType_Monster_Synchro || c.cardType == ComVal.CardType_Monster_XYZ)
            {
                Duel_ChangeCardArea.AddCardToArea(ComVal.Area_Extra, c, p, r);
            }
            else
            {
                Duel_ChangeCardArea.AddCardToArea(ComVal.Area_Hand, c, p, r);
            }
            if (g.GroupNum == 0)
            {
                FinishHandle();
            }
            else
            {
                SendCardToHand(g, r, p);
            }
        };
        AddDelegate(d);
        duelUIManager.ShowChangeAreaAnim(c.cardID, c.curArea, ComVal.Area_Hand, c.curPlaseState, -1, c.controller.isMy, c.areaRank, c.ownerPlayer.group_HandCard.GroupNum);
        Duel_ChangeCardArea.RemoveCardFromArea(c, c.controller, c.curArea);
    }

    //0为移除全部 
    public void RemoveXYZMaterial(Card c, int num, int reason, Card reasonCard, BaseEffect reasonEffect)
    {
        Group g = new Group(c.materialsXYZCardList);

        GroupCardSelectBack callBack = delegate(Group val)
        {
            for (int i = 0; i < val.GroupNum; i++)
            {
                c.materialsXYZCardList.Remove(val.GetCard(i));
            }
            Reason r = new Reason(reason, reasonCard, reasonEffect);
            SendXYZCardMaterialToGraveyard(val, r);
            FinishHandle();
        };
        if (g.GroupNum == num || num == 0)
        {
            callBack(g);
        }
        else
        {
            SelectCardFromGroup(g, callBack, num, c.controller);
        }
    }

    /// <summary>
    /// 装备卡
    /// </summary>
    public void EquipCardFromArea(int area, Card equipCard, Player targetPlayer, Card reasonCard, BaseEffect e)
    {
        if (!equipCard.curArea.IsBind(area) || targetPlayer.GetLeftTrapAreaNum() == 0)
        {
            FinishHandle();
            return;
        }
        normalDele d = delegate
        {
            Reason r = new Reason(ComVal.reason_Effect, reasonCard, e);
            Duel_ChangeCardArea.AddCardToArea(ComVal.Area_NormalTrap, equipCard, targetPlayer, r, ComVal.CardPutType_UpRightFront);
            FinishHandle();
        };
        AddDelegate(d);
        int rank = duelUIManager.GetAreaRank(false, targetPlayer.isMy);
        duelUIManager.ShowChangeAreaAnim(equipCard.cardID, equipCard.curArea, ComVal.Area_NormalTrap, equipCard.curPlaseState,
                                            ComVal.CardPutType_UpRightFront, targetPlayer.isMy, equipCard.areaRank, rank);
        Duel_ChangeCardArea.RemoveCardFromArea(equipCard, equipCard.controller, equipCard.curArea);
    }

    private void SendXYZCardMaterialToGraveyard(Group g, Reason toGraveyardReason)
    {
        for (int i = 0; i < g.GroupNum; i++)
        {
            Card c = g[i];

            c.SetArea(ComVal.Area_Graveyard, null);
        }

        Card mReasonCard = null;
        BaseEffect mReasonEffect = null;
        Card oReasonCard = null;
        BaseEffect oReasonEffect = null;

        Group sendToGraveyardGroup = new Group();

        Group msendToRemoveGroup = new Group();
        Group osendToRemoveGroup = new Group();

        foreach (var item in g.cardList)
        {
            if (item.ownerPlayer.isMy)
            {
                if (item.ownerPlayer.CanSendToGraveyard())
                {
                    sendToGraveyardGroup.AddCard(item);
                }
                else
                {
                    mReasonCard = item.ownerPlayer.GetSendToRemoveReasonCard();
                    mReasonEffect = item.ownerPlayer.GetSendToRemoveReasonEffect();
                    msendToRemoveGroup.AddCard(item);
                }
            }
            else
            {
                if (item.ownerPlayer.CanSendToGraveyard())
                {
                    sendToGraveyardGroup.AddCard(item);
                }
                else
                {
                    oReasonCard = item.ownerPlayer.GetSendToRemoveReasonCard();
                    oReasonEffect = item.ownerPlayer.GetSendToRemoveReasonEffect();
                    osendToRemoveGroup.AddCard(item);
                }
            }
        }
        if (msendToRemoveGroup.GroupNum > 0)
        {
            Reason r = new Reason(ComVal.reason_Effect, mReasonCard, mReasonEffect);

            for (int i = 0; i < msendToRemoveGroup.cardList.Count; i++)
            {
                Card c = msendToRemoveGroup.cardList[i];
                Duel_ChangeCardArea.AddCardToArea(ComVal.Area_Remove, c, c.ownerPlayer, r);
            }


        }
        if (osendToRemoveGroup.GroupNum > 0)
        {
            Reason r = new Reason(ComVal.reason_Effect, oReasonCard, oReasonEffect);

            for (int i = 0; i < osendToRemoveGroup.cardList.Count; i++)
            {
                Card c = osendToRemoveGroup.cardList[i];
                Duel_ChangeCardArea.AddCardToArea(ComVal.Area_Remove, c, c.ownerPlayer, r);
            }
        }

        if (sendToGraveyardGroup.GroupNum > 0)
        {
            for (int i = 0; i < sendToGraveyardGroup.cardList.Count; i++)
            {
                Card c = sendToGraveyardGroup.cardList[i];
                Duel_ChangeCardArea.AddCardToArea(ComVal.Area_Graveyard, c, c.ownerPlayer, toGraveyardReason);
            }
        }
    }

    private void XYZCardLeaveArea(Card c)
    {
        if (c.cardType.IsBind(ComVal.CardType_Monster_XYZ))
        {
            if (c.materialsXYZCardList.Count > 0)
            {
                SendXYZCardMaterialToGraveyard(new Group(c.materialsXYZCardList), null);
            }
        }
    }


    private void CardLeaveAreaCheck(Card c)
    {
        XYZCardLeaveArea(c);
        Group g = c.GetEquipGroup();
        Reason r = new Reason(0, null, null);
        c.ClearEquipCard();
        SendToGraveyard(ComVal.Area_NormalTrap, g, c, 0, null);
    }

    #region 召唤

    public void SpeicalSummon(int area, Group g, Player player, Card reasonCard, int reason, BaseEffect reasonEffect, int putType, GroupCardSelectBack theDele = null)
    {
        Group result = new Group();
        normalDele d = delegate
        {
            normalDele d1 = delegate
            {
                if (spSummonEvent.IsInvalid)
                {
                    if (theDele != null)
                    {
                        normalDele d3 = delegate
                        {
                            theDele(new Group());
                        };
                        AddDelegate(d3);
                    }
                    Group theGroup = spSummonEvent.spSummonGroup;
                    SendToGraveyard(ComVal.Area_Monster, theGroup, spSummonEvent.invalidReasonCard, ComVal.reason_InviadSpSummon, spSummonEvent.invalidReasonEffect);
                    return;
                }
                else
                {
                    if (theDele != null)
                        theDele(result);
                    CreateCode(g, player, ComVal.code_SpecialSummon, reasonCard, reason, reasonEffect);
                }
            };
            AddDelegate(d1);
            CreateCode(g, player, ComVal.code_SpDeclaration, reasonCard, 0, reasonEffect, true);
        };
        AddDelegate(d);
        Reason r = new Reason(reason, reasonCard, reasonEffect);
        SpeicalSummon(area, g.ToList(), player, putType, r,result);
    }

    private void SpeicalSummon(int area, List<Card> cardList, Player player, int putType,Reason r,Group resultGroup)
    {
        if (cardList.Count == 0)
        {
            FinishHandle();
            return ;
        }
        else
        {
            Card c = cardList[0];
            cardList.RemoveAt(0);
            if (area != c.curArea || player.GetLeftMonsterAreaNum() == 0)
            {
                SpeicalSummon(area, cardList, player, putType,r,resultGroup);
            }
            else
            {
                resultGroup.AddCard(c);
                IntDele spSummon = delegate(int mPutType)
                {
                    normalDele d3 = delegate
                    {
                        Duel_ChangeCardArea.AddCardToArea(ComVal.Area_Monster, c, player, r, mPutType);
                        SpeicalSummon(area, cardList, player, putType, r,resultGroup);
                    };
                    AddDelegate(d3, "jj");
                    int toRank = duelUIManager.GetAreaRank(true, player.isMy);
                    duelUIManager.ShowChangeAreaAnim(c.cardID, c.curArea, ComVal.Area_Monster, -1, mPutType, player.isMy, c.areaRank, toRank);
                    Duel_ChangeCardArea.RemoveCardFromArea(c, player, 0);
                };
                if (putType == 0)
                {
                    BoolDele dele = delegate(bool val)
                    {
                        int cardPutType;
                        if (val)
                        {
                            cardPutType = ComVal.CardPutType_UpRightFront;
                        }
                        else
                        {
                            cardPutType = ComVal.CardPutType_layFront;
                        }
                        spSummon(cardPutType);
                    };
                    duelUIManager.ShowSelectPutType(c.cardID, dele, player.isMy);
                }
                else
                {
                    spSummon(putType);
                }
            }
        }
    }

    /// <summary>
    /// 特殊召唤 TODO:召唤宣言时点
    /// <para>当卡片不在指定区域时，不会特殊召唤</para>
    /// <para>会将卡片从原来的区域移除</para>
    /// <para>需要选择摆放形式时 putType需为0</para>
    /// </summary>
    public void SpeicalSummon(int area, Card card, Player player, Card reasonCard, int reason, BaseEffect reasonEffect, int putType, normalDele theDele = null)
    {
        if (area != card.curArea || player.GetLeftMonsterAreaNum() == 0)
        {
            if (theDele != null)
                theDele();
            return;
        }
        Reason r = new Reason(reason, reasonCard, reasonEffect);
        Group a = new Group();
        a.AddCard(card);

        IntDele spSummon = delegate(int mPutType)
        {
            normalDele d2 = delegate
            {
                if (spSummonEvent.IsInvalid)
                {
                    if (theDele != null)
                    {
                        AddDelegate(theDele);
                    }
                    Group theGroup = spSummonEvent.spSummonGroup;
                    SendToGraveyard(ComVal.Area_Monster, theGroup, spSummonEvent.invalidReasonCard, ComVal.reason_InviadSpSummon, spSummonEvent.invalidReasonEffect);
                    return;
                }
                else
                {
                    if (theDele != null)
                    {
                        AddDelegate(theDele);
                        CreateCode(a, player, ComVal.code_SpecialSummon, reasonCard, reason, reasonEffect, true,true);
                    }
                    else
                    {
                        CreateCode(a, player, ComVal.code_SpecialSummon, reasonCard, reason, reasonEffect);
                    }
                }
            };
            AddDelegate(d2, "产生特殊召唤成功时点");
            spSummonEvent = new SpSummonEvent(a);
            player.AddSpSummonEvent(spSummonEvent);
            normalDele d3 = delegate
            {
                Duel_ChangeCardArea.AddCardToArea(ComVal.Area_Monster, card, player, r, mPutType);
                CreateCode(a, player, ComVal.code_SpDeclaration, reasonCard, 0, reasonEffect, true);
            };
            AddDelegate(d3, "jj");
            int toRank = duelUIManager.GetAreaRank(true, player.isMy);
            duelUIManager.ShowChangeAreaAnim(card.cardID, card.curArea, ComVal.Area_Monster, -1, mPutType, player.isMy, card.areaRank, toRank);
            Duel_ChangeCardArea.RemoveCardFromArea(card, player, 0);

        };

        if (putType == 0)
        {
            BoolDele dele = delegate(bool val)
            {
                int cardPutType;
                if (val)
                {
                    cardPutType = ComVal.CardPutType_UpRightFront;
                }
                else
                {
                    cardPutType = ComVal.CardPutType_layFront;
                }
                spSummon(cardPutType);
            };
            duelUIManager.ShowSelectPutType(card.cardID, dele, player.isMy);
        }
        else
        {
            spSummon(putType);
        }
    }

    /// <summary>
    /// 无效召唤
    /// </summary>
    public void NegateSummon(Card c, BaseEffect e)
    {
        if (spSummonEvent != null)
        {
            spSummonEvent.SetInvalid(c, e);
        }
    }

    /// <summary>
    /// 祭品召唤  TODO 处理动画效果和时点问题
    /// <para>祭品数为0时通常召唤</para>
    /// </summary>
    /// <param name="card"></param>
    /// <param name="player"></param>
    /// <param name="num">祭品数</param>
    void SacrificeSummon(Card card, Player player, bool isSet, int num)
    {
        if (num == 0)
        {
            NormalSummon(card, player, false, isSet);
            return;
        }
        GroupCardSelectBack SpSummon = delegate(Group group)
        {
            SendToGraveyard(ComVal.Area_Monster, group, card, ComVal.reason_SacrificeSummon);
            NormalSummon(card, player, true, isSet);
        };
        SelectCardFromGroup(player.group_MonsterCard.GetGroup(), SpSummon, num, player);
    }


    /// <summary>
    /// 从手牌通常召唤
    /// 产生时点
    /// </summary>
    /// <param name="val"></param>
    public void NormalSummon(Card card, Player player, bool isHighSummon, bool isSet, Card reasonCard = null, BaseEffect reasonEffect = null)
    {
        int putType;
        if (isSet)
        {
            putType = ComVal.CardPutType_layBack;
        }
        else
        {
            putType = ComVal.CardPutType_UpRightFront;
        }

        Reason r;
        if (reasonCard != null)
        {
            r = new Reason(ComVal.reason_Effect, reasonCard, reasonEffect);
        }
        else
        {
            r = null;
        }
        player.AddNormalSummontime();
        normalDele dele = delegate
        {
            Int64 val;
            if (isHighSummon)
            {
                val = ComVal.code_HSDeclaration;
            }
            else
            {
                val = ComVal.code_NSDeclaration;
            }

            Group a = new Group();
            a.AddCard(card);

            normalDele createSummonCode = delegate
            {
                a = a.GetAreaGroup(ComVal.Area_Monster);
                if (a.GroupNum != 0)
                {
                    Int64 code;
                    if (isHighSummon)
                    {
                        code = ComVal.code_HighSummon;
                    }
                    else
                    {
                        code = ComVal.code_NormalSummon;
                    }
                    CreateCode(a, player, code, reasonCard, 0, reasonEffect);
                }
            };
            AddDelegate(createSummonCode, "产生召唤成功时点");
            CreateCode(a, player, val, reasonCard, 0, reasonEffect);
            isInAnim = false;
        };

        normalDele d1 = delegate
        {
            Duel_ChangeCardArea.AddCardToArea(ComVal.Area_Monster, card, player, r, putType);
            if (isSet)
            {
                isInAnim = false;
                return;
            }
            AddDelegate(dele, "召唤怪兽");
            duelUIManager.ShowSummonAnim(card, FinishHandle);
        };
        isInAnim = true;
        AddDelegate(d1);
        int rank = duelUIManager.GetAreaRank(true, player.isMy);
        duelUIManager.ShowChangeAreaAnim(card.cardID, ComVal.Area_Hand, ComVal.Area_Monster, -1, putType, player.isMy, card.areaRank, rank);
        Duel_ChangeCardArea.RemoveCardFromArea(card, player, 0);
    }

    #endregion

    /// <summary>
    /// 从手牌设置魔法或陷阱卡
    /// </summary>
    public void SetTrapAndSpell(Card card, Player player)
    {
        normalDele d = delegate
        {
            Duel_ChangeCardArea.AddCardToArea(ComVal.Area_NormalTrap, card, player, null, ComVal.CardPutType_UpRightBack);
            card.SetCardSetRound();
        };
        AddDelegate(d);
        int rank = duelUIManager.GetAreaRank(false, player.isMy);
        duelUIManager.ShowChangeAreaAnim(card.cardID, ComVal.Area_Hand, ComVal.Area_NormalTrap, -1, ComVal.CardPutType_UpRightBack, player.isMy, card.areaRank, rank);
        Duel_ChangeCardArea.RemoveCardFromArea(card, player, 0);
    }


    private void UI_LauchSpell(Card card, Player player, int area)
    {
        if (area == ComVal.Area_NormalTrap)
        {
            if (!card.IsFaceUp())
            {
                duelUIManager.ChangeTrapPhaseType(card.areaRank, ComVal.CardPutType_UpRightFront, GetBool(player), card.cardID);
            }
            FinishHandle();
        }
        else if (area == ComVal.Area_Hand)
        {
            int toArea;
            if (card.cardType.IsBind(ComVal.CardType_Spell_Field))
            {
                toArea = ComVal.Area_FieldSpell;
            }
            else
            {
                toArea = ComVal.Area_NormalTrap;
            }

            normalDele d = delegate
            {
                console.Log("kk");
                Duel_ChangeCardArea.AddCardToArea(toArea, card, player, null, ComVal.CardPutType_UpRightFront);
                FinishHandle();
            };
            AddDelegate(d);
            int rank = duelUIManager.GetAreaRank(false, player.isMy);
            duelUIManager.ShowChangeAreaAnim(card.cardID, ComVal.Area_Hand, toArea, -1, ComVal.CardPutType_UpRightFront, player.isMy, card.areaRank, rank);
            Duel_ChangeCardArea.RemoveCardFromArea(card, player, 0);
        }
        else if (area == ComVal.Area_Graveyard)
        {
            console.Log("发动在墓地的魔法卡");
            FinishHandle();
        }
        else if (area == ComVal.Area_FieldSpell)
        {
            console.Log("发动地形卡效果");
            FinishHandle();
        }
        else
        {
            console.Log("error");

        }
    }

    /// <summary>
    /// 无效效果
    /// </summary>
    public void NegateEffect()
    {

    }

    ////////////--------------------------------------------------------------------------------------//////////

    /// <summary>
    /// 获取卡牌的选项
    /// </summary>
    public List<string> GetCardOption(int area, int rank, bool isMy)
    {
        Player player = GetPlayer(isMy);
        List<string> optionList = new List<string>();
        if (isInAnim || !IsFree())
        {
            return optionList;
        }

        if (player != curPlayer)
        {
            return optionList;
        }

        if (area == ComVal.Area_Hand)//为手牌
        {
            Card card = player.GetCard(area, rank);
            return GetHandCardOption(card, isMy);
        }
        else if (area == ComVal.Area_Monster)
        {
            Card card = player.GetCard(area, rank);
            return GetMonsterOption(card);
        }
        else if (area == ComVal.Area_NormalTrap)
        {
            Card card = player.GetCard(area, rank);
            return GetTrapOption(card);
        }
        else if (area == ComVal.Area_FieldSpell)
        {
            Card card = player.GetCard(area, rank);
            return GetTrapOption(card);
        }
        else if (area == ComVal.Area_MainDeck)
        {
            return new List<string>();
        }
        else if (area == ComVal.Area_Extra)
        {
            return GetExtraDeckOption(isMy);
        }
        else if (area == ComVal.Area_Remove)
        {
            return GetRemoveDeckOption(isMy);
        }
        else if (area == ComVal.Area_Graveyard)
        {
            return GetGraveyardDeckOption(isMy);
        }
        return optionList;
    }

    public Card GetCard(bool isMy, int area, int rank)
    {
        Player player = GetPlayer(isMy);
        Card card = player.GetCard(area, rank);
        return card;
    }

    #region 获取选项

    /// <summary>
    /// 获取场地上的怪兽卡选项
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    List<string> GetMonsterOption(Card card)
    {
        List<string> list = new List<string>();
        if (card.IsMonster())
        {
            foreach (LauchEffect effect in card.lauchEffectList)
            {
                if (effect.ownerCard == card)
                {
                    if (card.IsFaceUp() && effect.CanBeLaunch(curCode))
                    {
                        list.Add(ComStr.Operate_launch);
                    }
                }
            }
            if (card.cardChangeTypeTime != 0 && card.CanChangeType() && currentPhase != ComVal.Phase_Battlephase)
            {
                list.Add(ComStr.Operate_ChangeType);
            }
            if (currentPhase == ComVal.Phase_Battlephase && card.CanAttack() && card.controller == curPlayer)
            {
                list.Add(ComStr.Operate_Attack);
            }
            if (card.cardType.IsBind(ComVal.CardType_Monster_XYZ) && card.materialsXYZCardList.Count > 0)
            {
                list.Add(ComStr.Operate_CheckList);
            }
        }
        return list;
    }

    /// <summary>
    /// 获取场地上魔陷卡的选项
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    List<string> GetTrapOption(Card card)
    {
        List<string> list = new List<string>();
        if (card == null && currentPhase == ComVal.Phase_Battlephase)
            return list;
        if (card.CanLauchEffect(curCode, curChain))
        {
            list.Add(ComStr.Operate_launch);
        }
        return list;
    }

    /// <summary>
    /// 获取手牌的选项
    /// </summary>
    /// <param name="card"></param>
    /// <returns></returns>
    List<string> GetHandCardOption(Card card, bool isMy)
    {
        List<string> list = new List<string>();
        if (card.IsMonster())
        {
            if (card.CanLauchEffect(curCode, curChain))
            {
                list.Add(ComStr.Operate_launch);
            }

            if (currentPhase == ComVal.Phase_Mainphase1 || currentPhase == ComVal.Phase_Mainphase2)
            {
                if (DuelRule.CheckCanNormalSummon(card, card.ownerPlayer) && !MonsterAreaIsFull(card.ownerPlayer) && GetPlayer(isMy).CanNormalSummon())
                {
                    list.Add(ComStr.Operate_NormalSummon);
                    list.Add(ComStr.Operate_Set);
                }
                if (GetPlayer(isMy).CanSpSummon(card))
                {
                    if (card.CanSpSummon(curCode) && !MonsterAreaIsFull(card.ownerPlayer))
                    {
                        list.Add(ComStr.Operate_SpecialSummon);
                    }
                }
            }
        }
        else if (card.IsTrap() && (!SpellAreaIsFull(card.ownerPlayer)) && currentPhase != ComVal.Phase_Battlephase)
        {
            list.Add(ComStr.Operate_Set);
        }
        else if (card.isSpell() && (!SpellAreaIsFull(card.ownerPlayer)) && currentPhase != ComVal.Phase_Battlephase)
        {
            if (!card.cardType.IsBind(ComVal.CardType_Spell_Field))
            {
                list.Add(ComStr.Operate_Set);
            }
            if (card.CanLauchEffect(curCode, curChain))
            {
                list.Add(ComStr.Operate_launch);
            }
        }
        return list;
    }


    /// <summary>
    /// 获取额外卡组的选项
    /// <para>卡组数目不为0即可查看列表</para>
    /// <para>有待添加：特殊召唤额外卡组的怪兽</para>
    /// </summary>
    /// <param name="isMy"></param>
    /// <returns></returns>
    List<string> GetExtraDeckOption(bool isMy)
    {
        List<string> result = new List<string>();
        if (isMy)
        {
            if (player1.group_ExtraDeck.GroupNum != 0)
            {
                result.Add(ComStr.Operate_CheckList);
            }
            if (curPlayer == player1 && GetCurPhase().IsBind(ComVal.Phase_Mainphase))
            {
                if (GetSynchroGroup(player1).GroupNum > 0 || GetXYZGroup(player1).GroupNum > 0)
                {
                    result.Add(ComStr.Operate_SpecialSummon);
                }
            }
        }
        else
        {
            if (player2.group_ExtraDeck.GroupNum != 0)
            {
                result.Add(ComStr.Operate_CheckList);
            }
            if (curPlayer == player2 && GetCurPhase().IsBind(ComVal.Phase_Mainphase))
            {
                if (GetSynchroGroup(player2).GroupNum > 0 || GetXYZGroup(player1).GroupNum > 0)
                {
                    result.Add(ComStr.Operate_SpecialSummon);
                }
            }
        }
        return result;
    }
    /// <summary>
    /// 获取除外卡组的选项
    /// <para>有待添加：发动效果</para>
    /// </summary>
    /// <param name="isMy"></param>
    /// <returns></returns>
    List<string> GetRemoveDeckOption(bool isMy)
    {
        List<string> result = new List<string>();
        if (isMy)
        {
            if (player1.group_Remove.GroupNum != 0)
                result.Add(ComStr.Operate_CheckList);
        }
        else
        {
            if (player2.group_Remove.GroupNum != 0)
                result.Add(ComStr.Operate_CheckList);
        }
        return result;
    }

    /// <summary>
    /// 获取墓地卡组的选项
    /// <para>有待添加：发动效果</para>
    /// </summary>
    /// <param name="isMy"></param>
    /// <returns></returns>
    List<string> GetGraveyardDeckOption(bool isMy)
    {
        List<string> result = new List<string>();
        if (isMy)
        {
            if (player1.group_Graveyard.GroupNum != 0)
                result.Add(ComStr.Operate_CheckList);
            foreach (var item in player1.group_Graveyard.cardList)
            {
                if (item.CanLauchEffect(curCode, curChain))
                {
                    result.Add(ComStr.Operate_launch);
                    break;
                }
            }
        }
        else
        {
            if (player2.group_Graveyard.GroupNum != 0)
                result.Add(ComStr.Operate_CheckList);
        }
        return result;
    }
    #endregion


    /// <summary>
    /// 操作卡片
    /// </summary>
    /// <param name="area"></param>
    /// <param name="rank"></param>
    /// <param name="operate"></param>
    /// <param name="isMy"></param>
    public void OperateCard(params object[] args)
    {
        int area = (int)args[0];
        int rank = (int)args[1];
        string operate = (string)args[2];
        bool isMy = (bool)args[3];
        if (!IsFree() || isInAnim)
        {
            console.Log("??");
            return;
        }
        //Debug.Log(operate);
        switch (operate)
        {
            case ComStr.Operate_NormalSummon:
                Operate_NormalSummon(area, rank, isMy);
                break;
            case ComStr.Operate_Set:
                Operate_Set(area, rank, isMy);
                break;
            case ComStr.Operate_launch:
                Operate_LauchEffect(area, rank, isMy);
                break;
            case ComStr.Operate_CheckList:
                Operate_CheckList(area, rank, isMy);
                break;
            case ComStr.Operate_SpecialSummon:
                if (area.IsBind(ComVal.Area_Extra))
                {
                    Operate_SpSummonFromExtra(GetPlayer(isMy));
                }
                else
                {
                    Operate_Spsummon(area, rank, isMy);
                }
                break;
            case ComStr.Operate_Attack:
                Operate_Attack(area, rank, isMy);
                break;
            case ComStr.Operate_ChangeType:
                Operate_ChangeType(area, rank, isMy);
                break;
        }
    }

    /// <summary>
    /// 处理攻击事件
    /// </summary>
    void HandleAttackEvent()
    {
        Group fightDestroyGroup = null;

        console.Log("处理攻击事件");
        if (attackEvent == null)
        {
            console.Log("error");
            return;
        }

        Card attacker = attackEvent.Attacker;
        Card attackeder = attackEvent.Attackeder;

        attacker.AddAttackTime();
        if (attackEvent.IsInvalid())
        {
            console.Log("无效攻击");
            attackEvent = null;
            console.Log("攻击事件处理完成");
            return;
        }

        if (attackEvent.IsDirectAttack() == true)//直接攻击玩家
        {
            attackEvent.SetDemage(attacker.GetCurAfk(), attacker.opponentPlayer);
            ReduceLP(attacker.GetCurAfk(), attacker.opponentPlayer, ComVal.reason_AttackDestroy, attacker);
        }
        else
        {
            int type = attackeder.curPlaseState;
            if (type == ComVal.CardPutType_UpRightFront)//攻击攻击显示的怪兽
            {
                int mAfk = attacker.GetCurAfk();
                int oAfk = attackeder.GetCurAfk();
                if (mAfk > oAfk)
                {
                    fightDestroyGroup = attackeder.ToGroup();
                    attackEvent.SetDemage(mAfk - oAfk, attackeder.controller);
                    ReduceLP(mAfk - oAfk, attackeder.controller, ComVal.reason_AttackDestroy, attacker);
                }
                else if (mAfk == oAfk)
                {
                    Group g = new Group();
                    g.AddCard(attacker);
                    g.AddCard(attackeder);
                    fightDestroyGroup = attacker.ToGroup();
                    fightDestroyGroup.AddCard(attackeder);
                }
                else
                {
                    fightDestroyGroup = attacker.ToGroup();
                    attackEvent.SetDemage(oAfk - mAfk, attackeder.controller);
                    ReduceLP(oAfk - mAfk, attacker.controller, ComVal.reason_AttackDestroy, attacker);
                }
            }
            else if (type == ComVal.CardPutType_layFront || type == ComVal.CardPutType_layBack)//攻击防守显示的怪兽
            {
                int mAfk = attacker.GetCurAfk();
                int odef = attackeder.GetCurDef();
                console.Log(mAfk + "  " + odef);
                if (mAfk > odef)
                {
                    fightDestroyGroup = attackeder.ToGroup();
                    if (attacker.CanPierce())
                    {
                        attackEvent.SetDemage(mAfk - odef, attackeder.controller);
                        ReduceLP(mAfk - odef, attackeder.controller, ComVal.reason_AttackDestroy, attacker);
                    }
                }
                else if (mAfk == odef)
                {

                }
                else
                {
                    attackEvent.SetDemage(odef - mAfk, attacker.controller);
                    ReduceLP(odef - mAfk, attacker.controller, ComVal.reason_AttackDestroy, attacker);
                }
            }
            else
            {
                console.Log("error");
            }
        }
        normalDele d1 = delegate
        {
            normalDele d2 = delegate
            {
                if (fightDestroyGroup != null)
                {
                    normalDele d3 = delegate
                    {
                        attackEvent = null;
                        console.Log("攻击事件处理完成");
                    };
                    AddDelegate(d3);
                    console.Log(fightDestroyGroup.GroupNum);
                    SendToGraveyard(ComVal.Area_Monster, fightDestroyGroup, attacker, ComVal.reason_AttackDestroy);
                }
            };
            if (attackEvent.IsTurnBack)
            {
                AddDelegate(d2, "处理战破送墓");
                CreateCode(attackEvent.Attackeder.ToGroup(), attacker.controller, ComVal.code_TurnBack, attacker, ComVal.reason_AttackDestroy, null, true);
            }
            else
            {
                d2();
            }
        };
        AddDelegate(d1, "伤害结算后处理");
        if (attackEvent.IsDirectAttack())
        {
            CreateCode(null, attackEvent.Attacker.controller, ComVal.code_AfterReckonAttack | ComVal.code_DirectAttack, attackEvent.Attacker, ComVal.reason_AttackDestroy, null);
        }
        else
        {
            CreateCode(attackeder.ToGroup(), attacker.controller, ComVal.code_AfterReckonAttack, attackEvent.Attacker, ComVal.reason_AttackDestroy, null);
        }
    }

    /// <summary>
    /// 翻转卡片
    /// </summary>
    void TurnBackCard(Card card, bool isAttack)
    {
        int type;
        if (isAttack)
            type = ComVal.CardPutType_UpRightFront;
        else
            type = ComVal.CardPutType_layFront;
        duelUIManager.ChangeMonsterPlaseType(card.areaRank, type, card.ownerPlayer.isMy, card.cardID);
        card.SetPlaseState(type);
    }

    #region 对卡片的操作 发动效果 召唤怪兽、、、

    /// <summary>
    /// 攻击操作 
    /// </summary>
    /// <param name="area"></param>
    /// <param name="rank"></param>
    /// <param name="isMy"></param>
    void Operate_Attack(int area, int rank, bool isMy)
    {
        Card card = GetPlayer(isMy).GetCard(area, rank);
        GroupCardSelectBack dele = delegate(Group group)
        {
            Card c = group.GetCard(0);
            attackEvent = new AttackEvent(card, c);
            normalDele d1 = delegate
            {
                normalDele d2 = delegate
                {
                    normalDele d3 = delegate
                    {
                        isInAttackAnim = false;
                        HandleAttackEvent();
                    };
                    Card attackeder = attackEvent.Attackeder;
                    int type = attackeder.curPlaseState;
                    if (type == ComVal.CardPutType_layFront || type == ComVal.CardPutType_layBack)//攻击防守显示的怪兽
                    {
                        if (attackeder.curPlaseState == ComVal.CardPutType_layBack)
                        {
                            TurnBackCard(attackeder, false);
                            attackEvent.SetIsTurnBack();
                        }
                    }
                    AddDelegate(d3, "伤害处理阶段");
                    CreateCode(c.ToGroup(), GetPlayer(isMy), ComVal.code_BeforeReckonAttack, card, ComVal.reason_AttackDestroy, null);
                };
                AddDelegate(d2, "伤害处理前");
                CreateCode(c.ToGroup(), GetPlayer(isMy), ComVal.code_AttackDeclaration, card, ComVal.reason_AttackDestroy, null);
            };
            AddDelegate(d1, "1");
            duelUIManager.ShowAttackAnim(card.areaRank, c.areaRank, isMy, FinishHandle);
            isInAttackAnim = true;
            UpdateCardMesShow();
        };
        Group g = card.controller.GetCanAttackMonster();
        if (g.GroupNum == 0)
        {
            attackEvent = new AttackEvent(card, GetPlayer(!isMy));
            console.Log("直接攻击");
            normalDele d1 = delegate
            {
                normalDele d2 = delegate
                {
                    normalDele d3 = delegate
                    {
                        isInAttackAnim = false;
                        HandleAttackEvent();
                    };
                    AddDelegate(d3, "伤害处理阶段");
                    CreateCode(null, GetPlayer(isMy), ComVal.code_BeforeReckonAttack, card, ComVal.reason_AttackDestroy, null);
                };
                AddDelegate(d2, "伤害处理前");
                CreateCode(null, card.ownerPlayer, ComVal.code_DirectAttack | ComVal.code_AttackDeclaration, card, ComVal.reason_AttackDestroy, null);
            };
            AddDelegate(d1, "1");
            duelUIManager.ShowAttackAnim(card.areaRank, -1, isMy, FinishHandle);
            isInAttackAnim = true;
            UpdateCardMesShow();
        }
        else
        {
            SelectCardFromGroup(g, dele, 1, GetPlayer(isMy));
        }
    }

    void Operate_LauchEffect(int area, int rank, bool isMy)
    {
       
        if (area == ComVal.Area_Graveyard || area == ComVal.Area_Remove)
        {
            LuachAreaEffect(area);
            return;
        }
        Card card = GetPlayer(isMy).GetCard(area, rank);

        if (card.isSpell())
        {
            LauchEffect(card, card.ownerPlayer);
        }
        else if (card.IsMonster())
        {
            LauchEffect(card, card.ownerPlayer);
        }
        else if(card.IsTrap())
        {
            LauchEffect(card, card.ownerPlayer);
        }
    }

    /// <summary>
    /// 特殊召唤
    /// </summary>
    /// <param name="area"></param>
    /// <param name="rank"></param>
    /// <param name="isMy"></param>
    void Operate_Spsummon(int area, int rank, bool isMy)
    {
        Card card = GetPlayer(isMy).GetCard(area, rank);

        if (!card.IsMonster())
        {
            Debug.Log("error");
            return;
        }
        normalDele dele = delegate
        {
            SpSummonEffect e = card.spSummonEffect;
            SpeicalSummon(card.curArea, card, card.controller, card, ComVal.reason_Effect, e, e.PutType, null);
        };

        AddDelegate(dele);
        card.spSummonEffect.Cost(this, card, null);
    }

    private void LuachAreaEffect(int area)
    {
        Group g = new Group();
        if (area == ComVal.Area_Graveyard)
        {
            foreach (var item in player1.group_Graveyard.cardList)
            {
                if (item.CanLauchEffect(curCode, curChain))
                {
                    g.AddCard(item);
                }
            }
        }
        else if (area == ComVal.Area_Remove)
        {
            foreach (var item in player1.group_Remove.cardList)
            {
                if (item.CanLauchEffect(curCode, curChain))
                {
                    g.AddCard(item);
                }
            }
        }
        else
        {
            console.Log("error");
        }
        GroupCardSelectBack dele = delegate(Group g1)
        {
            Card c = g1.GetCard(0);
            LauchEffect(c, c.ownerPlayer);
        };
        SelectCardFromGroup(g, dele, 1, curPlayer);
    }

    void Operate_Set(int area, int rank, bool isMy)
    {
        Card card = GetPlayer(isMy).GetCard(area, rank);
        if (card.IsMonster())
        {
            NormalSummon(card, card.ownerPlayer, false, true);
        }
        else
        {
            SetTrapAndSpell(card, card.ownerPlayer);
        }
    }

    void Operate_NormalSummon(int area, int rank, bool isMy)
    {
        Card card = GetPlayer(isMy).GetCard(area, rank);
        //   SacrificeSummon(card, card.ownerPlayer, false, card.GetSacrificeNum());
        NormalSummon(card, card.ownerPlayer, false, false);
    }

    void Operate_CheckList(int area, int rank, bool isMy)
    {
        Group group = new Group();
        switch (area)
        {
            case ComVal.Area_Remove:
                group = GetPlayer(isMy).group_Remove;
                break;
            case ComVal.Area_Graveyard:
                group = GetPlayer(isMy).group_Graveyard;
                break;
            case ComVal.Area_Extra:
                group = GetPlayer(isMy).group_ExtraDeck;
                break;
            case ComVal.Area_Monster:
                Card c = GetPlayer(isMy).GetCard(area, rank);
                if (!c.cardType.IsBind(ComVal.CardType_Monster_XYZ) || c.materialsXYZCardList.Count == 0)
                {
                    Debug.Log("error  " + c.cardID);
                    return;
                }
                else
                {
                    group = new Group(c.materialsXYZCardList);
                }
                break;
        }

        GroupCardSelectBack dele = delegate
        {
            IsSelect = false;
        };
        duelUIManager.ShowSelectCardUI(group, dele, 0, true, false);
        IsSelect = true;
    }

    void Operate_ChangeType(int area, int rank, bool isMy)
    {
        Card c = GetPlayer(isMy).GetCard(area, rank);
        ChangeMonsterType(c, GetPlayer(isMy));
    }

    void Operate_SpSummonFromExtra(Player p)
    {
        Group g = GetSynchroGroup(p);
        Group g1 = GetXYZGroup(p);
        g.AddList(g1.cardList);
        GroupCardSelectBack callBack = delegate(Group val)
        {
            Card spSummonCard = val.GetFirstCard();
            if (SynchroFitler(spSummonCard))
            {
                GroupCardSelectBack selectMaterial = delegate(Group m)
                {
                    normalDele d = delegate
                    {
                        SpeicalSummon(ComVal.Area_Extra, spSummonCard, p, spSummonCard, ComVal.reason_SynchroSummon, null, 0);
                    };
                    AddDelegate(d, true);
                    SendToGraveyard(ComVal.Area_Monster, m, spSummonCard, ComVal.reason_SynchroSummon);
                };
                SelectSynchroMaterial(selectMaterial, spSummonCard, p);
            }
            else if (XYZFitler(spSummonCard))
            {
                GroupCardSelectBack selectMaterial = delegate(Group m)
                {
                    normalDele d = delegate
                    {
                        if (spSummonCard.curArea.IsBind(ComVal.Area_Field))
                        {
                            spSummonCard.SetXYZMaterial(m);
                            duelUIManager.UpdateXYZMaterial(spSummonCard.materialsXYZCardList.GetCardIDList(), spSummonCard.areaRank, p.isMy);
                        }
                    };
                    for (int i = 0; i < m.cardList.Count; i++)
                    {
                        duelUIManager.RemoveCardFromField(m.cardList[i], p.isMy);
                        p.group_MonsterCard.RemoveCard(m.cardList[i].areaRank);
                        m.cardList[i].SetArea(ComVal.Area_XYZMaterial, null);
                    }
                    SpeicalSummon(ComVal.Area_Extra, spSummonCard, p, spSummonCard, ComVal.reason_SynchroSummon, null, 0, d);
                };
                SelectXYZMaterial(selectMaterial, spSummonCard, p);
            }
            else
            {
                Debug.Log("error");
            }
        };
        SelectCardFromGroup(g, callBack, 1, p);
    }
    #endregion

    /// <summary>
    /// 选择融合素材
    /// <para>此方法只适用于2个素材的融合怪兽</para>
    /// </summary>
    /// <param name="group"></param>
    /// <param name="dele"></param>
    /// <param name="fusionCard"></param>
    /// <param name="targetGroup"></param>
    public void SelectFusionMaterialFromGroup(Group group, GroupCardSelectBack dele, Card fusionCard, Group targetGroup = null)
    {
        GroupCardSelectBack callBack = delegate(Group theGroup)
        {
            Card card = theGroup.GetCard(0);
            if (targetGroup != null)
            {
                targetGroup.AddCard(card);
                if (targetGroup.GroupNum == fusionCard.fusionCount)
                {
                    dele(targetGroup);
                    return;
                }
            }
            else
            {
                targetGroup = new Group();
                targetGroup.AddCard(card);
            }
            group.RemoveCard(card);
            Group g = fusionCard.cardMaterialFitler.GetGroupList(group, card);
            SelectFusionMaterialFromGroup(g, dele, fusionCard, targetGroup);
        };
        if (MonsterAreaIsFull(fusionCard.ownerPlayer))//怪兽区满了
        {
            Group a = group.GetAreaGroup(ComVal.Area_Monster);
            SelectCardFromGroup(a, callBack, 1, fusionCard.ownerPlayer);
        }
        else
        {
            SelectCardFromGroup(group, callBack, 1, fusionCard.ownerPlayer);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="group"></param>
    /// <param name="dele"></param>
    /// <param name="synchroCard"></param>
    public void SelectSynchroMaterial(GroupCardSelectBack dele, Card synchroCard, Player p)
    {
        Group selectGroup = new Group();
        Group resultGroup = new Group();
        CardMaterialFitler fiter = synchroCard.cardMaterialFitler;
        List<Card> list1 = p.group_MonsterCard.GetGroup().GetFitlerList(fiter.cardFilter1);
        List<Card> list2 = p.group_MonsterCard.GetGroup().GetFitlerList(fiter.cardFilter2);
        for (int i = 0; i < list1.Count; i++)
        {
            for (int j = 0; j < list2.Count; j++)
            {
                if (list1[i] != list2[j])
                {
                    if ((list1[i].GetCurLevel() + list2[j].GetCurLevel()) == synchroCard.GetCurLevel())
                    {
                        selectGroup.AddCard(list1[i]);
                        break;
                    }
                }
            }
        }
        GroupCardSelectBack callBack = delegate(Group theGroup)
        {
            GroupCardSelectBack callBack1 = delegate(Group val)
            {
                resultGroup.AddCard(val.GetFirstCard());
                selectGroup.AddCard(val.GetFirstCard());
                dele(resultGroup);
            };
            selectGroup = new Group();
            Card selectCard = theGroup.GetFirstCard();
            resultGroup.AddCard(theGroup.GetFirstCard());
            selectCard.GetCurLevel();
            for (int i = 0; i < list2.Count; i++)
            {
                if (list2[i] != selectCard)
                {
                    if ((list2[i].GetCurLevel() + selectCard.GetCurLevel()) == synchroCard.GetCurLevel())
                    {
                        selectGroup.AddCard(list2[i]);
                    }
                }
            }
            SelectCardFromGroup(selectGroup, callBack1, 1, p);
        };
        SelectCardFromGroup(selectGroup, callBack, 1, p);
    }

    private void SelectXYZMaterial(GroupCardSelectBack dele, Card xyzCard, Player p)
    {
        Group g = p.group_MonsterCard.GetGroup().GetFitlerGroup(xyzCard.xyzFilter);
        Group result = new Group();
        SelectXYZMaterial(result, g, dele, p, xyzCard.xyzMaterialNum);
    }

    private void SelectXYZMaterial(Group result, Group selectGroup, GroupCardSelectBack dele, Player p, int selectNum)
    {
        GroupCardSelectBack callBack = delegate(Group g)
        {
            Card c = g.GetFirstCard();
            result.AddCard(c);
            if (result.GroupNum == selectNum)
            {
                dele(result);
            }
            else
            {
                selectGroup.RemoveCard(c);
                SelectXYZMaterial(result, selectGroup, dele, p, selectNum);
            }
        };
        SelectCardFromGroup(selectGroup, callBack, 1, p);
    }

    /// <summary>
    /// 跟新卡片的场上信息和攻击动画 TODO:显示对方场上的攻击动画
    /// </summary>
    public void UpdateCardMesShow()
    {
        for (int i = 0; i < 5; i++)
        {
            Card c = player1.group_MonsterCard.GetCard(i);
            if (c != null && c.cardType.IsBind(ComVal.CardType_Monster_XYZ))
            {
                duelUIManager.UpdateXYZMaterial(c.materialsXYZCardList.GetCardIDList(), c.areaRank, c.isMy);
            }
        }
        for (int i = 0; i < 5; i++)
        {
            Card c = player2.group_MonsterCard.GetCard(i);
            if (c != null && c.cardType.IsBind(ComVal.CardType_Monster_XYZ))
            {
                duelUIManager.UpdateXYZMaterial(c.materialsXYZCardList.GetCardIDList(), c.areaRank, c.isMy);
            }
        }
        duelUIManager.UpdateCardMesShow(player1.group_MonsterCard, player2.group_MonsterCard);
        if (currentPhase == ComVal.Phase_Battlephase && !isInAttackAnim)
        {
            duelUIManager.UpdateCardAttackAnimShow(curPlayer.group_MonsterCard, curPlayer.isMy);
        }
        else
        {
            duelUIManager.HideCardAttackAnimShow(curPlayer.group_MonsterCard, curPlayer.isMy);
        }
    }


    /// <summary>
    /// 生成时点 连锁的效果不会改变时点的group
    /// <para>生成攻击时点时reasoncard为攻击者</para>
    /// </summary>
    /// <param name="group">卡片组</param>
    /// <param name="player"></param>
    /// <param name="reasonCard">原因卡片</param>
    /// <param name="theReason">原因</param>
    /// <param name="reasonEffect">原因效果</param>
    void CreateCode(Group group, Player player, Int64 codeVal, Card reasonCard, int theReason, BaseEffect reasonEffect, bool isFinishHandle = false, bool isDebug = false)
    {
        Reason reason = new Reason(theReason, reasonCard, reasonEffect);
        Code code = new Code(player);
        code.code = codeVal;
        if (group != null)
        {
            code.group = group;
        }
        code.SetReason(reason);
        if (curCode.code == ComVal.code_NoCode && !IsHandleCost() && !IsHandleAction())
        {
            CreateNewCode(code);
        }
        else
        {
            List<Card> list;
            if (curChain.chainNum == 0 && !IsHandleAction())//发动效果中的时点,后面无处理行为
            {
                list = CheckCanLauchEffect(player, code, false);//检测
                list.AddRange(CheckCanLauchEffect(GetOpsitePlayer(player), code, false));//检测
                console.Log("处理延迟效果: " + ComStr.GetCodeMes(code.code) + "选发效果数量：  " + list.Count);
            }
            else
            {
                list = CheckCanLauchEffect(player, code, true);//检测
                list.AddRange(CheckCanLauchEffect(GetOpsitePlayer(player), code, true));//检测
                console.Log("处理延迟效果: " + ComStr.GetCodeMes(code.code) + "必发效果数量：  " + list.Count);
            }
            if (list.Count != 0 || GetNotInChainEffect(player, code).Count > 0)
            {
                //if (isDebug)
                //    Debug.Log("mj");
                console.Log("加入到等待处理的时点列表");
                waitToCode.Add(code);
            }
            if (isFinishHandle)
            {
                FinishHandle();
            }
        }
    }

    public void CreateNewCode(Code code)
    {
        console.Log("生成时点:" + "   " + ComStr.GetCodeMes(code.code) + "    " + code.code);
        curCode = code;
        Player targetPlayer;
        if (curCode.code == ComVal.code_EffectLanch)
        {
            targetPlayer = GetOpsitePlayer(code.triggerPlayer);
        }
        else
        {
            targetPlayer = code.triggerPlayer;
        }
        normalDele d1 = delegate
        {
            LauchEffect e = curChain.GetLastEffect();
            if (e == null)
            {
                CheckCode(targetPlayer);
            }
            else
            {
                CheckCode(GetOpsitePlayer(e.ownerCard.ownerPlayer));
            }
        };
        AddDelegate(d1, "检测连锁");
        normalDele d = delegate
        {
            HandleMustLauchEffect(targetPlayer);
        };
        AddDelegate(d);
        List<LauchEffect> list = GetNotInChainEffect(targetPlayer,curCode);
        //Debug.Log(list.Count);
        HandleNotInChainEffect(list);
    }

    private void HandleNotInChainEffect(List<LauchEffect> list)
    {
        normalDele d = delegate
        {
            HandleNotInChainEffect(list);
        };
        if (list.Count != 0)
        {
            LauchEffect effect = list[0];
            list.RemoveAt(0);

            GetMes callBack = delegate(bool val)
            {
                if (val)
                {
                    normalDele d1 = delegate
                    {
                        AddDelegate(d);
                        console.Log("发动不入连锁效果");
                        effect.Operate();
                    };
                    AddDelegate(d1);
                    effect.Cost();
                }
                else
                {
                    d();
                }
            };
            if (effect.cardEffectType.IsBind(ComVal.cardEffectType_mustNotInChain))
            {
                callBack(true);
            }
            else
            {
                ChooseToLauchEffect(curCode, callBack, effect.ownerCard.controller);
            }
        }
        else
        {
            FinishHandle();
        }
    }

    private List<LauchEffect> GetNotInChainEffect(Player player,Code code)
    {
        List<Card> canLauchEffectList = new List<Card>();
        canLauchEffectList = CheckCanLauchEffect(player, code, false, true);
        canLauchEffectList.AddRange(CheckCanLauchEffect(GetOpsitePlayer(player), code, false, true));

        List<LauchEffect> list = new List<LauchEffect>();
        foreach (var item in canLauchEffectList)
        {
            list.AddRange(item.GetNotInChainEffectList(code, curChain));
            console.Log("不进入连锁效果数量：" + list.Count);
        }
        return list;
    }

    /// <summary>
    /// TODO：处理必发效果
    /// </summary>
    void HandleMustLauchEffect(Player player)
    {
        List<Card> canLauchEffectList = new List<Card>();
        canLauchEffectList = CheckCanLauchEffect(player, curCode, false);

        if (canLauchEffectList.Count == 0)
        {
            canLauchEffectList = CheckCanLauchEffect(GetOpsitePlayer(player), curCode, false);
            player = GetOpsitePlayer(player);
        }

        List<LauchEffect> list = new List<LauchEffect>();
        foreach (var item in canLauchEffectList)
        {
            list.AddRange(item.GetMustLauchEffectList(curCode, curChain));
            console.Log("必发效果数量：" + list.Count);
        }
        if (list.Count != 0)
        {
            LauchEffect effect = list[0];
            list.RemoveAt(0);

            normalDele afterCost = delegate
            {
                normalDele afterSelectTarget = delegate
                {
                    curCode.code = curCode.code | ComVal.code_EffectLanch;
                    console.Log("发动效果");
                    AddToChain(effect);
                    HandleMustLauchEffect(player);
                };
                SelectTarget(effect, afterSelectTarget);
            };
            AddDelegate(afterCost, "Cost回调");
            CostTheEffect(effect);
        }
        else
        {
            FinishHandle();
        }
    }

    /// <summary>
    /// 检测时点
    /// <para>会显示卡片的虚线框动画</para>
    /// </summary>
    /// <param name="isLauch">是否延迟执行的时点</param>
    /// <param name="dele"></param>
    void CheckCode(Player player, bool isRefuse = false)
    {
        console.Log("检测时点:   " + "玩家：" + player.isMy);
        List<Card> canLauchEffectList = new List<Card>();

        Player targetPlayer;

        if (curChain.isDelayCode)
        {
            canLauchEffectList = CheckCanLauchEffect(player, curCode, true);
            targetPlayer = player;
            console.Log(targetPlayer.name + "延迟发动的效果：" + canLauchEffectList.Count);
        }
        else
        {
            canLauchEffectList = CheckCanLauchEffect(player, curCode, false);
            targetPlayer = player;
        }

        if (canLauchEffectList.Count != 0)
        {
            GetMes getMes = delegate(bool isLauch)
            {
                HideCanLauchEffectDash(targetPlayer.isMy);
                if (!isLauch)
                {
                    if (isRefuse)
                    {
                        OperateChain();
                    }
                    else
                    {
                        CheckCode(GetOpsitePlayer(targetPlayer), true);
                    }
                    return;
                }
                Group a = new Group();
                a.AddList(canLauchEffectList);
                GroupCardSelectBack theDele = delegate(Group g)
                {
                    normalDele d1 = delegate
                    {
                        CheckCode(GetOpsitePlayer(targetPlayer));
                    };
                    AddDelegate(d1, "检测连锁");
                    LauchEffect(g.GetCard(0), targetPlayer, true, false);
                };
                SelectCardFromGroup(a, theDele, 1, targetPlayer);
            };
            ChooseToLauchEffect(curCode, getMes, targetPlayer);
            ShowCanLauchEffectDash(canLauchEffectList, targetPlayer);
        }
        else//如果没有可以发动的效果，则处理连锁
        {
            if (isRefuse)
            {
                OperateChain();
            }
            else
            {
                CheckCode(GetOpsitePlayer(targetPlayer), true);
            }
        }
    }

    /// <summary>
    /// 提示可以发动效果的卡片的虚线框动画
    /// 网络对战时，不会显示对方的提示
    /// </summary>
    /// <param name="group"></param>
    void ShowCanLauchEffectDash(List<Card> list, Player player)
    {
        List<int> handCardList = new List<int>();
        List<int> monsterCardList = new List<int>();
        List<int> trapCardList = new List<int>();
        List<int> fieldSpellList = new List<int>();
        foreach (var item in list)
        {
            if (ComVal.isBind(item.curArea, ComVal.Area_Hand))
            {
                handCardList.Add(item.areaRank);
            }
            else if (ComVal.isBind(item.curArea, ComVal.Area_Monster))
            {
                monsterCardList.Add(item.areaRank);
            }
            else if (ComVal.isBind(item.curArea, ComVal.Area_NormalTrap))
            {
                trapCardList.Add(item.areaRank);
            }
            else if (item.curArea.IsBind(ComVal.Area_FieldSpell))
            {
                fieldSpellList.Add(0);
            }
        }

        duelUIManager.ShowHandCardDashAnim(handCardList, player.isMy);
        duelUIManager.ShowFieldDashAnim(monsterCardList, ComVal.Area_Monster, player.isMy);
        duelUIManager.ShowFieldDashAnim(trapCardList, ComVal.Area_NormalTrap, player.isMy);
        duelUIManager.ShowFieldDashAnim(fieldSpellList, ComVal.Area_FieldSpell, player.isMy);
    }

    /// <summary>
    /// 隐藏可以发动效果的卡片的虚线框动画
    /// </summary>
    /// <param name="group"></param>
    void HideCanLauchEffectDash(bool isMy)
    {
        duelUIManager.HideFieldDashAnim(isMy);
        duelUIManager.HideHandCardDashAnim(isMy);
    }

    /// <summary>
    /// 检测当前时点可发动的效果,并将其加入列表之中
    /// </summary>
    List<Card> CheckCanLauchEffect(Player player, Code code, bool isTrigger, bool isNotInChain = false)
    {
        List<Card> result = new List<Card>();
        foreach (var item in player.group_TrapCard.cardList)
        {
            if (item != null)
            {
                if (item.CanLauchTriggerEffect(code, curChain, isTrigger, isNotInChain))
                {
                    result.Add(item);
                }
            }
        }
        foreach (var item in player.group_MonsterCard.cardList)
        {
            if (item != null)
            {
                if (item.CanLauchTriggerEffect(code, curChain, isTrigger, isNotInChain))
                {
                    result.Add(item);
                }
            }
        }
        foreach (var item in player.group_HandCard.cardList)
        {
            if (item != null)
            {
                if (item.CanLauchTriggerEffect(code, curChain, isTrigger, isNotInChain))
                {
                    result.Add(item);
                }
            }
        }
        foreach (var item in player.group_Graveyard.cardList)
        {
            if (item != null)
            {
                if (item.CanLauchTriggerEffect(code, curChain, isTrigger, isNotInChain))
                {
                    result.Add(item);
                }
            }
        }
        foreach (var item in player.group_Remove.cardList)
        {
            if (item != null)
            {
                if (item.CanLauchTriggerEffect(code, curChain, isTrigger, isNotInChain))
                {
                    result.Add(item);
                }
            }
        }
        foreach (var item in player.group_ExtraDeck.cardList)
        {
            if (item != null)
            {
                if (item.CanLauchTriggerEffect(code, curChain, isTrigger, isNotInChain))
                {
                    result.Add(item);
                }
            }
        }
        if (player.fieldSpell != null)
        {
            if (player.fieldSpell.CanLauchTriggerEffect(code, curChain, isTrigger, isNotInChain))
            {
                result.Add(player.fieldSpell);
            }
        }
        string playerName;
        if (player.isMy)
        {
            playerName = "玩家1";
        }
        else
        {
            playerName = "玩家2";
        }
        console.Log(playerName + "  可以发动的效果数目:" + result.Count);
        return result;
    }


    /// <summary>
    /// 发动效果
    /// </summary>
    /// <param name="card"></param>
    /// <param name="player"></param>
    /// <param name="afterChoose"></param>
    void LauchEffect(Card card, Player player, bool isFinishHandle = false, bool createCode = true)
    {
        List<LauchEffect> list = card.GetCanLauchEffectList(curCode, curChain);
        IntDele handleEffect = delegate(int val)
        {
            LauchEffect effect = list[val];
            normalDele afterCost = delegate
            {
                normalDele afterSelectTarget = delegate
                {
                    console.Log("发动效果");
                    AddToChain(effect);
                    if (createCode)
                    {
                        CreateCode(card.ToGroup(), player, ComVal.code_EffectLanch, card, 0, effect);
                    }
                    else
                    {
                        curCode.code = curCode.code | ComVal.code_EffectLanch;
                    }
                    if (isFinishHandle)
                    {
                        FinishHandle();
                    }

                };
                SelectTarget(effect, afterSelectTarget);
            };

            normalDele cost = delegate
            {
                AddDelegate(afterCost, "Cost回调");
                CostTheEffect(effect);
            };

            if (card.isSpell())
            {
                normalDele d = delegate
                {
                    AddDelegate(cost);
                    UI_LauchSpell(card, player, card.curArea);
                };
                if (card.cardType.IsBind(ComVal.CardType_Spell_Field) && card.controller.fieldSpell != null && card.curArea.IsBind(ComVal.Area_Hand))
                {
                    Card oldFieldSpellCard = card.controller.fieldSpell;
                    AddDelegate(d, true);
                    SendToGraveyard(ComVal.Area_FieldSpell, oldFieldSpellCard.ToGroup(), card, ComVal.reason_LauchFinish);
                }
                else
                {
                    d();
                }
            }
            else if (card.IsTrap())
            {
                duelUIManager.ChangeTrapPhaseType(card.areaRank, ComVal.CardPutType_UpRightFront, GetBool(player), card.cardID);
                cost();
            }
            else
            {
                cost();
            }
        };

        if (list.Count > 1)
        {
            List<string> describeList = new List<string>();
            foreach (var item in list)
            {
                describeList.Add(item.describe);
            }
            duelUIManager.ShowSelectEffectUI(handleEffect, describeList, player.isMy);
        }
        else if (list.Count == 0)
        {
            console.Log("error");
        }
        else
        {
            handleEffect(0);
        }
    }

    /// <summary>
    /// 如果在场上和手牌外，则为选择框选择 -1为任意卡片
    /// </summary>
    /// <param name="group"></param>
    /// <param name="dele"></param>
    /// <param name="num"></param>
    public void SelectCardFromGroup(Group group, GroupCardSelectBack dele, int num, Player player, bool isMax = true)
    {
        if (num == 0)
        {
            console.Log("error");
            return;
        }
        if (group.GroupNum == num)
        {
            dele(group);
            return;
        }
        if (group.IsHandAndField())
        {
            duelUIManager.SelectFieldCard(group, num, dele, player.isMy, isMax);
        }
        else
        {
            duelUIManager.ShowSelectCardUI(group, dele, num, player.isMy, isMax);
        }
    }

    /// <summary>
    /// 选择是否发动效果
    /// </summary>
    /// <param name="code"></param>
    /// <param name="dele"></param>
    void ChooseToLauchEffect(Code code, GetMes dele, Player player)
    {
        string mes;
        string playerName;
        if (player.isMy)
        {
            playerName = "玩家1";
        }
        else
        {
            playerName = "玩家2";
        }
        if (curChain.chainNum == 0)
        {
            if (code.group.GroupNum == 1)
            {
                mes = playerName + "   " + "「" + code.group.GetCard(0).cardName + "」" + ComStr.GetCodeMes(code.code) + ",是否发动效果?";
            }
            else
            {
                mes = playerName + "   " + "卡片" + ComStr.GetCodeMes(code.code) + ",是否发动效果?";
            }
        }
        else
        {
            if (code.group.GroupNum == 1)
            {
                mes = playerName + "   " + "「" + code.group.GetCard(0).cardName + "」" + ComStr.GetCodeMes(code.code) + ",是否进行连锁?";
            }
            else
            {
                mes = playerName + "   " + "卡片" + ComStr.GetCodeMes(code.code) + ",是否进行连锁?";
            }
        }
        duelUIManager.ShowDialogBoxUI(mes, dele, player.isMy);//显示是否发动效果选项
    }

    /// <summary>
    /// 在确定没有连锁加入时,处理整个连锁
    /// </summary>
    void OperateChain()
    {
        curChain.SetHandle(true);
        if (curChain.chainNum != 0)
        {
            normalDele d1 = delegate
            {
                normalDele a = delegate
              {
                  OperateChain();
              };
                AddDelegate(a, "处理连锁");

                LauchEffect effect = curChain.GetEffect();
                if (curChain.IsEffectDisable(effect))
                {
                    curChain.RemoveEffect();
                    duelUIManager.UpdateChainUI(curChain);
                    FinishHandle();
                }
                else
                {
                    curChain.RemoveEffect();
                    duelUIManager.UpdateChainUI(curChain);
                    OperateEffect(effect);
                }
            };
            if (curChain.GetMaxChainNum() > 1)
            {
                AddDelegate(d1, "处理动画");
                duelUIManager.PlayChainAnim(FinishHandle);
            }
            else
            {
                d1();
            }
        }
        else
        {
            console.Log("处理当前连锁完成");
            Group sendGraveryGroup = new Group();
            while (curChain.GetCardNum() > 0)
            {
                Card card = curChain.RemoveCard();
                if (!card.IsForverSpellTrapCard())//魔陷卡发动完后送去墓地
                {
                    sendGraveryGroup.AddCard(card);
                }
            }
            normalDele d = delegate
            {
                curChain.SetHandle(false);
                if (waitToCode.Count > 0)
                {
                    console.Log("延迟处理时点");
                    Code code = waitToCode[0];
                    waitToCode.RemoveAt(0);
                    curChain = new Chain(code.isDelayCode);
                    curChain.SetCodeVal(code.code);
                    CreateNewCode(code);
                }
                else
                {
                    console.Log("设置新时点");
                    if (curChain.chainNum != 0)
                    {
                        console.Log("error");
                    }
                    curChain = new Chain();
                    curCode = new Code(null);
                    UpdateCardMesShow();
                    curCode.code = ComVal.code_NoCode;
                    FinishHandle();
                    CheckDeckAct();
                }
            };
            if (sendGraveryGroup.GroupNum > 0)
            {
                //TODO 原因卡片
                AddDelegate(d);
                SendToGraveyard(ComVal.Area_NormalTrap, sendGraveryGroup, sendGraveryGroup.GetFirstCard(), ComVal.reason_LauchFinish);
            }
            else
            {
                d();
            }
        }
    }

    void OperateEffect(LauchEffect effect)
    {
        normalDele a = delegate
        {
            effect.Operate();
        };
        AddDelegate(a, "处理效果");
        duelUIManager.ShowEffectAnim(effect.ownerCard, FinishHandle);
    }

    /// <summary>
    /// 执行cost
    /// </summary>
    void CostTheEffect(LauchEffect effect)
    {
        console.Log("执行Cost");
        effect.ownerCard.controller.AddEffectLauchTime(effect.effectID);
        effect.Cost();
    }
    /// <summary>
    /// 取对象
    /// </summary>
    /// <param name="effect"></param>
    /// <param name="dele"></param>
    void SelectTarget(LauchEffect effect, normalDele dele)
    {
        console.Log("取对象");
        if (effect.getTarget != null)
        {
            effect.GetTarget(dele);
        }
        else
        {
            dele();
        }
    }

    public void ReduceLP(int val, Player player, int reason, Card reasonCard, BaseEffect reasonEffect = null)
    {
        player.ReduceLP(val);
        duelUIManager.ShowLpChange(val, false);
        duelUIManager.ReduceLp(val, player.isMy);
        CreateCode(null, player, ComVal.code_AddLp, reasonCard, reason, reasonEffect);
    }

    public void AddLP(int val, Player player, int reason, Card reasonCard, BaseEffect reasonEffect = null)
    {
        player.AddLP(val);
        duelUIManager.ShowLpChange(val, true);
        duelUIManager.AddLp(val, player.isMy);
        CreateCode(null, player, ComVal.code_ReduceLp, reasonCard, reason, reasonEffect);
    }

    /// <summary>
    /// 怪兽区域是否满了
    /// </summary>
    /// <returns></returns>
    public bool MonsterAreaIsFull(Player player)
    {
        if (player.group_MonsterCard.GroupNum > 5)
        {
            console.Log("error");
        }
        if (player.group_MonsterCard.GroupNum == 5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 魔陷区是否满了
    /// </summary>
    /// <returns></returns>
    public bool SpellAreaIsFull(Player player)
    {
        if (player.group_TrapCard.GroupNum > 5)
        {
            console.Log("error");
        }
        if (player.group_TrapCard.GroupNum == 5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public Code GetCurCode()
    {
        return curCode;
    }

    public bool IsPlayerRound(Player p)
    {
        return curPlayer == p;
    }

    public int GetCurPhase()
    {
        return currentPhase;
    }

    /// <summary>
    /// 获取区域内符合条件的卡片组
    /// </summary>
    /// <param name="keyWord"></param>
    /// <param name="isBoth"></param>
    /// <param name="player"></param>
    /// <param name="cardType"></param>
    /// <param name="area"></param>
    /// <param name="filter"></param>
    /// <param name="isExcept"></param>
    /// <param name="exceptCard"></param>
    /// <param name="cardID"></param>
    /// <returns></returns>
    public Group GetIncludeNameCardFromArea(string keyWord, bool isBoth, Player player, int cardType, int area, Filter filter = null, bool isExcept = false, Card exceptCard = null, string cardID = null)
    {
        Group result = new Group();
        List<Card> list = GetList(isBoth, player, area);

        for (int i = 0; i < list.Count; i++)
        {
            Card card = list[i];
            if ((!StaticMethod.isCardID(keyWord) && card.cardName.Contains(keyWord)) || (StaticMethod.isCardID(keyWord) && card.cardID == keyWord))
            {
                if (cardType != 0)
                {
                    if (!ComVal.isBind(card.cardType, cardType))
                    {
                        continue;
                    }
                }
                if (filter != null)
                {
                    if (!filter(card))
                        continue;
                }
                if (isExcept)
                {
                    if (exceptCard != null)
                    {
                        if (exceptCard != card)
                            result.AddCard(card);
                    }
                    else
                    {
                        if (cardID != card.cardID)
                            result.AddCard(card);
                    }
                }
                else
                    result.AddCard(card);
            }
        }
        return result;
    }

    /// <summary>
    /// 获取区域符合条件的卡片数目
    /// </summary>
    /// <param name="keyWord"></param>
    /// <param name="isBoth"></param>
    /// <param name="player"></param>
    /// <param name="cardType"></param>
    /// <param name="area"></param>
    /// <param name="filter"></param>
    /// <param name="isExcept"></param>
    /// <param name="exceptCard"></param>
    /// <param name="cardID"></param>
    /// <returns></returns>
    public int GetIncludeNameCardNumFromArea(string keyWord, bool isBoth, Player player, int cardType, int area, Filter filter = null, bool isExcept = false, Card exceptCard = null, string cardID = null)
    {
        List<Card> list = GetList(isBoth, player, area);
        int result = 0;
        for (int i = 0; i < list.Count; i++)
        {
            Card card = list[i];
            if ((!StaticMethod.isCardID(keyWord) && card.cardName.Contains(keyWord)) || (StaticMethod.isCardID(keyWord) && card.cardID == keyWord))
            {
                if (cardType != 0)
                {
                    if (!ComVal.isBind(card.cardType, cardType))
                    {
                        continue;
                    }
                }
                if (filter != null)
                {
                    if (!filter(card))
                        continue;
                }
                if (isExcept)
                {
                    if (exceptCard != null)
                    {
                        if (exceptCard != card)
                            result++;
                    }
                    else
                    {
                        if (cardID != card.cardID)
                            result++;
                    }
                }
                else
                    result++;
            }

        }
        return result;
    }

    ///// <summary>
    ///// 获取生效的场地效果
    ///// </summary>
    ///// <returns></returns>
    //public List<Effect> GetFieldEffect()
    //{
    //    List<Effect> result = new List<Effect>();
    //    for (int i = 0; i < fieldEffectList.Count; i++)
    //    {
    //        if (fieldEffectList[i].IsValid(this, null))
    //        {
    //            result.Add(fieldEffectList[i]);
    //        }
    //        else
    //        {
    //            fieldEffectList.RemoveAt(i);
    //            i--;
    //        }
    //    }
    //    return result;
    //}

    public List<LimitPlayerEffect> GetLimitEffectList(Player p)
    {
        List<LimitPlayerEffect> result = new List<LimitPlayerEffect>();
        for (int i = 0; i < limitEFfectList.Count; i++)
        {
            LimitPlayerEffect e = limitEFfectList[i];

            switch (e.targetType)
            {
                case TargetPlayerType.my:
                    if (e.ownerCard.controller == p)
                    {
                        result.Add(e);
                    }
                    break;
                case TargetPlayerType.other:
                    if (GetOpsitePlayer(e.ownerCard.controller) == p)
                    {
                        result.Add(e);
                    }
                    break;
                case TargetPlayerType.both:
                    result.Add(e);
                    break;
                default:
                    break;
            }
        }
        return result;
    }

    List<Card> GetList(bool isBoth, Player player, int area)
    {
        List<Card> list = new List<Card>();
        if (isBoth)
        {
            list = DuelRule.GetCardListFromPlayer(area, player1);
            list.AddRange(DuelRule.GetCardListFromPlayer(area, player2));
        }
        else
        {
            list = DuelRule.GetCardListFromPlayer(area, player);
        }
        return list;
    }

    public void AddDelegate(normalDele dele, bool isAction = false)
    {
        AddDelegate(dele, null, isAction);
    }

    public void AddDelegate(normalDele dele, string mes, bool isAction = false)
    {
        console.Log("增加回调   " + mes);
        DuelDele duelDele = new DuelDele(dele, mes, isAction);
        delegateStack.Push(duelDele);
    }

    public void AddFinishHandle()
    {
        normalDele dele = delegate
        {
            FinishHandle();
        };
        AddDelegate(dele, null);
    }

    public bool IsHandleAction()
    {
        foreach (var item in delegateStack)
        {
            if (item.IsAction)
            {
                return true;
            }
        }
        return false;
    }

    public void FinishHandle()
    {
        if (delegateStack.Count != 0)
        {
            DuelDele val = delegateStack.Pop();
            if (val.Mes != "" || val.Mes != null)
            {
                console.Log("处理回调   " + val.Mes);
            }
            normalDele dele = val.Dele;
            dele();
            val = null;
        }
    }

    public bool IsFree()
    {
        return delegateStack.Count == 0 && !IsSelect;
    }

    /// <summary>
    /// 处理因cost中产生的时点
    /// </summary>
    /// <returns></returns>
    public bool IsHandleCost()
    {
        //      Cost回调
        foreach (var item in delegateStack)
        {
            if (item.Mes == "Cost回调")
            {
                return true;
            }
        }
        return false;
    }

    public void AddToChain(LauchEffect e)
    {
        //  curCode.code = curCode.code | ComVal.code_EffectLanch;
        console.Log("加入连锁");
        curChain.AddToChain(e);
        if (curChain.chainNum > 0)
        {
            duelUIManager.UpdateChainUI(curChain);
        }
    }

    public void ShowSynchro()
    {
        if (GetSynchroGroup(curPlayer).GroupNum > 0 || GetXYZGroup(player1).GroupNum > 0)
        {
            duelUIManager.ShowActAnim(ComVal.Area_Extra);
        }
        else
        {
            duelUIManager.HideActAnim(ComVal.Area_Extra);
        }
    }

    private Group GetSynchroGroup(Player p)
    {
        List<Card> synchroList = p.group_ExtraDeck.GetFitlerList(SynchroFitler);
        Group g = new Group();
        for (int i = 0; i < synchroList.Count; i++)
        {
            Card synchroCard = synchroList[i];
            if (p.CanSpSummon(synchroCard))
            {
                if (synchroCard.cardMaterialFitler != null)
                {
                    CardMaterialFitler fiter = synchroCard.cardMaterialFitler;
                    List<Card> list1 = p.group_MonsterCard.GetGroup().GetFitlerList(fiter.cardFilter1);
                    List<Card> list2 = p.group_MonsterCard.GetGroup().GetFitlerList(fiter.cardFilter2);


                    for (int j = 0; j < list1.Count; j++)
                    {

                        Card c = list1[j];
                        for (int k = 0; k < list2.Count; k++)
                        {
                            if (c != list2[k])
                            {
                                if ((c.GetCurLevel() + list2[k].GetCurLevel()) == synchroCard.GetCurLevel())
                                {
                                    if (!g.ContainCard(synchroCard))
                                    {
                                        g.AddCard(synchroCard);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        return g;
    }

    public Group GetXYZGroup(Player p)
    {
        List<Card> xyzList = p.group_ExtraDeck.GetFitlerList(XYZFitler);
        Group g = new Group();
        for (int i = 0; i < xyzList.Count; i++)
        {
            Card xyzCard = xyzList[i];
            if (p.CanSpSummon(xyzCard))
            {
                if (xyzCard.xyzFilter != null)
                {
                    List<Card> list = p.group_MonsterCard.GetGroup().GetFitlerList(xyzCard.xyzFilter);
                    if (list.Count >= xyzCard.xyzMaterialNum)
                    {
                        if (!g.ContainCard(xyzCard))
                        {
                            g.AddCard(xyzCard);
                        }
                    }
                }
            }
        }
        return g;
    }

    public void ShowDialogBox(Card c, GetMes dele, bool isMy)
    {
        string val = "是否发动[" + c.cardName + "]的效果";
        duelUIManager.ShowDialogBoxUI(val, dele, isMy);
    }


    public bool SynchroFitler(Card card)
    {
        return card.cardType.IsBind(ComVal.CardType_Monster_Synchro);
    }

    public bool XYZFitler(Card card)
    {
        return card.cardType.IsBind(ComVal.CardType_Monster_XYZ);
    }

    #region 游戏结束

    public void ReduceAllPlayerLP(float mVal, float oVal)
    {
        player1.ReduceLP(mVal, false);
        player2.ReduceLP(oVal, false);

        if (player1.LP == 0 && player2.LP == 0)
        {
            isFinishGame = true;
            duelUIManager.ShowMes("结束", "平局", ShowSavePlayBackPanel);
        }
        else
        {
            player1.CheckLost();
            player2.CheckLost();
        }
    }

    private void PlayBackSurrender(params object[] args)
    {
        bool val = (bool)args[0];

        isFinishGame = true;
        if (val)
        {
            duelUIManager.ShowMes("结束", "玩家[" + player1.name + "]投降", ShowSavePlayBackPanel);
        }
        else
        {
            duelUIManager.ShowMes("结束", "玩家[" + player2.name + "]投降", ShowSavePlayBackPanel);
        }
    }

    private void ReciveSurrender(params object[] args)//对方投降
    {
        if (isFinishGame)
        {
            return;
        }
        SendEvent(DuelEvent.duelEvent_RecordOperate, RecordEvent.recordEvent_Surrender, false);
        isFinishGame = true;
        duelUIManager.ShowMes("结束", "对方投降了", ShowSavePlayBackPanel);
    }
 

    private void PlayerSurrender(params object[] args)//己方投降
    {
        if(isFinishGame)
        {
            return;
        }
        if(IsNetWork)
        {
            SendEvent(DuelEvent.netEvent_SendSurrender);
            SendEvent(DuelEvent.netEvent_SendGameEnd);
        }
        SendEvent(DuelEvent.duelEvent_RecordOperate, RecordEvent.recordEvent_Surrender, true);
        isFinishGame = true;
        duelUIManager.ShowMes("结束", "你投降了", ShowSavePlayBackPanel);
    }


    public void LostMatch(Player player)
    {
        if(player.isMy)
        {
            SendEvent(DuelEvent.netEvent_SendGameEnd);
        }
        isFinishGame = true;
        duelUIManager.ShowMes("结束","玩家["+ GetOpsitePlayer(player).name + "]胜利", ShowSavePlayBackPanel);
    }

    private void ReciveGameEnd(params object[] args)
    {
        isFinishGame = true;
        duelUIManager.ShowMes("结束", "对方玩家失去连接", ShowSavePlayBackPanel);
    }

    private void ShowSavePlayBackPanel()
    {
        if(IsNetWork||IsSingle)
        {
            SendEvent(DuelEvent.duelEvent_ShowSavePlayBackPanel);
        }
    }

    #endregion

    public bool IsSelect;

    public void SetSelect()
    {
        IsSelect = true;
    }

    public void SetNotSelect()
    {
        IsSelect = false;
    }

    public AttackEvent GetCurAttackEvent()
    {
        return attackEvent;
    }


    #region 处理延迟动作

    public void AddDelayAction(normalDele dele, int code, int lauchPhaseCount)
    {
        DelayAction delayAction = new DelayAction(dele, code, lauchPhaseCount);
        delayAction.startRound = roundCount;
        duelActionList.Add(delayAction);
    }

    public void HandleDelayAction(int resetCode)
    {
        DelayAction result = null;
        for (int i = 0; i < duelActionList.Count; i++)
        {
            DelayAction item = duelActionList[i];
            int lauchRoundCount = item.startRound + item.lauchRoundCount;
            if (lauchRoundCount == roundCount)
            {
                if (item.lauchCode == resetCode)
                {
                   
                    result = item;
                    duelActionList.RemoveAt(i);
                    break;
                }
            }
            else if (lauchRoundCount > roundCount)
            {
                duelActionList.RemoveAt(i);
                i--;
            }
        }
        if (result == null)
        {
            FinishHandle();
        }
        else
        {
            normalDele d = delegate
            {
                HandleDelayAction(resetCode);
            };
            AddDelegate(d, true);
            result.action();
        }
    }

    #endregion

}