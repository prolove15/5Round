using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Controller_BattlePhase : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Types
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing, WillFinish,
        InitReplacementFinished,
        PhaseFinished,
        TurnStarted, TurnFinished,
        RoundStarted, RoundFinished,
        DiceThrown,
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField]
    public UI_BattlePhase btlUI_Cp;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    // variables
    [ReadOnly]
    public int curRoundIndex;

    [ReadOnly]
    public int playerActionPriority;

    [ReadOnly]
    public int dice;

    //-------------------------------------------------- private fields
    // components
    Controller_Phases controller_Cp;

    UI_Phases uiManager_Cp;

    StatusManager statusManager_Cp;

    List<Player_Phases> player_Cps = new List<Player_Phases>();

    Player_Phases localPlayer_Cp, otherPlayer_Cp, comPlayer_Cp;

    Transform cam_Tf;

    DiceHandler_de diceHandler_Cp;

    int localPlayerID;

    // variables
    List<RoundValue_de> localRoundsData = new List<RoundValue_de>();

    List<RoundValue_de> otherRoundsData = new List<RoundValue_de>();

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Properties
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Properties

    //-------------------------------------------------- public properties
    public GameState_En mainGameState
    {
        get { return gameStates[0]; }
        set { gameStates[0] = value; }
    }

    public List<Hash128> hashStates
    {
        get { return HashHandler.instance.hashes; }
    }

    //-------------------------------------------------- private properties

    #endregion

    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Methods
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////

    //-------------------------------------------------- Start is called before the first frame update
    void Start()
    {

    }

    //-------------------------------------------------- Update is called once per frame
    void Update()
    {

    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Manage gameStates
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region ManageGameStates

    //--------------------------------------------------
    public void AddMainGameState(GameState_En value)
    {
        if (gameStates.Count == 0)
        {
            gameStates.Add(value);
        }
    }

    //--------------------------------------------------
    public void AddGameStates(params GameState_En[] values)
    {
        foreach (GameState_En value_tp in values)
        {
            gameStates.Add(value_tp);
        }
    }

    //--------------------------------------------------
    public bool ExistGameStates(params GameState_En[] values)
    {
        bool result = true;
        foreach (GameState_En value in values)
        {
            if (!gameStates.Contains(value))
            {
                result = false;
                break;
            }
        }

        return result;
    }

    //--------------------------------------------------
    public bool ExistAnyGameStates(params GameState_En[] values)
    {
        bool result = false;
        foreach (GameState_En value in values)
        {
            if (gameStates.Contains(value))
            {
                result = true;
                break;
            }
        }

        return result;
    }

    //--------------------------------------------------
    public int GetExistGameStatesCount(GameState_En value)
    {
        int result = 0;

        for (int i = 0; i < gameStates.Count; i++)
        {
            if (gameStates[i] == value)
            {
                result++;
            }
        }

        return result;
    }

    //--------------------------------------------------
    public void RemoveGameStates(params GameState_En[] values)
    {
        foreach (GameState_En value in values)
        {
            gameStates.RemoveAll(gameState_tp => gameState_tp == value);
        }
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle hash states
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle hash states

    //--------------------------------------------------
    Hash128 RegRandHashValue()
    {
        return HashHandler.RegRandHash();
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Initialize
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Initialize

    //--------------------------------------------------
    public void Init()
    {
        StartCoroutine(Corou_Init());
    }

    IEnumerator Corou_Init()
    {
        AddMainGameState(GameState_En.Nothing);

        //
        SetComponents();

        //
        InitComponents();

        //
        InitVariables();

        //
        mainGameState = GameState_En.Inited;

        //
        yield return null;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();

        statusManager_Cp = controller_Cp.statusManager_Cp;

        cam_Tf = controller_Cp.cam_Tf;

        player_Cps = controller_Cp.player_Cps_de;

        localPlayer_Cp = controller_Cp.localPlayer_Cp_de;
        otherPlayer_Cp = controller_Cp.otherPlayer_Cp_de;
        comPlayer_Cp = controller_Cp.comPlayer_Cp_de;

        localPlayerID = controller_Cp.localPlayerID_de;

        diceHandler_Cp = controller_Cp.diceHandler_Cp;
    }

    //--------------------------------------------------
    void InitComponents()
    {
        btlUI_Cp.Init();
    }

    //--------------------------------------------------
    void InitVariables()
    {
        localRoundsData = localPlayer_Cp.roundsData;

        otherRoundsData = otherPlayer_Cp.roundsData;

        playerActionPriority = -1;

        dice = -1;
    }

    //--------------------------------------------------
    void InitReplacement()
    {
        StartCoroutine(Corou_InitReplacement());
    }

    IEnumerator Corou_InitReplacement()
    {
        //
        UnityEvent unityEvent = new UnityEvent();

        TargetTweening.TranslateGameObject(cam_Tf, localPlayer_Cp.camBpPoint_Tf, unityEvent);
        yield return new WaitForSeconds(1f);

        //
        for (int i = 0; i < player_Cps.Count; i++)
        {
            TargetTweening.TranslateGameObject(player_Cps[i].playerB_Tf, player_Cps[i].pbBpPoint_Tf, unityEvent);
        }
        yield return new WaitForSeconds(1f);

        //
        for (int i = 0; i < player_Cps.Count; i++)
        {
            TargetTweening.TranslateGameObject(player_Cps[i].miharidai_Tf, player_Cps[i].mdBpPoint_Tf, unityEvent);
        }
        yield return new WaitForSeconds(1f);

        //
        AddGameStates(GameState_En.InitReplacementFinished);
    }

    //--------------------------------------------------
    public void Init_BtlPhase()
    {
        btlUI_Cp.Init_BtlPhase();
    }

    #endregion

    //-------------------------------------------------- Play phase
    public void PlayPhase()
    {
        StartCoroutine(Corou_PlayPhase());
    }

    IEnumerator Corou_PlayPhase()
    {
        mainGameState = GameState_En.Playing;

        //
        statusManager_Cp.DisableInstructionPanel();

        //
        InitReplacement();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.InitReplacementFinished));
        RemoveGameStates(GameState_En.InitReplacementFinished);

        //
        PlayTurn();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.TurnFinished));

        //****************************** game events 10th. when battle phase end
        for (int i = 0; i < player_Cps.Count; i++)
        {
            Hash128 hash_tp = RegRandHashValue();
            player_Cps[i].unitAbil_Cp.HandleGameEventsTiming(GameEventsTiming.WhenBtlPhaseEnd, hash_tp);
            yield return new WaitUntil(() => !hashStates.Contains(hash_tp));
        }
    }

    //--------------------------------------------------
    void PlayTurn()
    {
        StartCoroutine(Corou_PlayTurn());
    }

    IEnumerator Corou_PlayTurn()
    {
        AddGameStates(GameState_En.TurnStarted);

        //****************************** game events 8th. when turn start
        for (int i = 0; i < player_Cps.Count; i++)
        {
            Hash128 hash_tp = RegRandHashValue();
            player_Cps[i].unitAbil_Cp.HandleGameEventsTiming(GameEventsTiming.WhenTurnStart, hash_tp);
            yield return new WaitUntil(() => !hashStates.Contains(hash_tp));
        }

        //
        for (int i = 0; i < 5; i++)
        {
            PlayRound(i);

            yield return new WaitUntil(() => ExistGameStates(GameState_En.RoundFinished));
            RemoveGameStates(GameState_En.RoundFinished);
        }

        //****************************** game events 9th. when turn end
        for (int i = 0; i < player_Cps.Count; i++)
        {
            Hash128 hash_tp = RegRandHashValue();
            player_Cps[i].unitAbil_Cp.HandleGameEventsTiming(GameEventsTiming.WhenTurnEnd, hash_tp);
            yield return new WaitUntil(() => !hashStates.Contains(hash_tp));
        }

        //
        RemoveGameStates(GameState_En.TurnStarted);
        AddGameStates(GameState_En.TurnFinished);
    }

    //-------------------------------------------------- Play round
    void PlayRound(int rndId_pr)
    {
        StartCoroutine(CorouPlayRound(rndId_pr));
    }

    IEnumerator CorouPlayRound(int rndId_pr)
    {
        // set current round index
        curRoundIndex = rndId_pr;

        // show round starting
        btlUI_Cp.SetNoticeEffText("ーー「ROUND" + (curRoundIndex + 1) + "」ーー");
        yield return new WaitUntil(() => btlUI_Cp.ExistGameStates(
            UI_BattlePhase.GameState_En.Done_NoticeTxtShow));
        btlUI_Cp.FinishNoticeEffText();

        // start new round
        SetPlayerActionPriority(rndId_pr);
        yield return new WaitUntil(() => playerActionPriority != -1);

        //
        Player_Phases firstPlayer_Cp = null;
        Player_Phases sndPlayer_Cp = null;
        if (playerActionPriority == 0)
        {
            firstPlayer_Cp = localPlayer_Cp;
            sndPlayer_Cp = otherPlayer_Cp;
        }
        else if (playerActionPriority == 1)
        {
            firstPlayer_Cp = otherPlayer_Cp;
            sndPlayer_Cp = localPlayer_Cp;
        }

        playerActionPriority = -1;

        // start round
        Hash128 hash_tp = RegRandHashValue();
        firstPlayer_Cp.StartRound(rndId_pr, hash_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash_tp));

        Hash128 hash2_tp = RegRandHashValue();
        sndPlayer_Cp.StartRound(rndId_pr, hash2_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash2_tp));

        // first player round play
        firstPlayer_Cp.HighlightRoundPanel(true, rndId_pr);

        firstPlayer_Cp.PlayRound(rndId_pr);
        yield return new WaitUntil(() => firstPlayer_Cp.ExistGameStates(
            Player_Phases.GameState_En.RoundActionDone));
        firstPlayer_Cp.RemoveGameStates(Player_Phases.GameState_En.RoundActionDone);

        firstPlayer_Cp.HighlightRoundPanel(false, rndId_pr);

        // wait
        yield return new WaitForSeconds(1f);

        // snd player round play
        sndPlayer_Cp.HighlightRoundPanel(true, rndId_pr);

        sndPlayer_Cp.PlayRound(rndId_pr);
        yield return new WaitUntil(() => sndPlayer_Cp.ExistGameStates(
            Player_Phases.GameState_En.RoundActionDone));
        sndPlayer_Cp.RemoveGameStates(Player_Phases.GameState_En.RoundActionDone);

        sndPlayer_Cp.HighlightRoundPanel(false, rndId_pr);

        // finish round
        firstPlayer_Cp.FinishRound(rndId_pr);
        yield return new WaitUntil(() => firstPlayer_Cp.ExistGameStates(
            Player_Phases.GameState_En.RoundFinished));
        firstPlayer_Cp.RemoveGameStates(Player_Phases.GameState_En.RoundFinished);

        sndPlayer_Cp.FinishRound(rndId_pr);
        yield return new WaitUntil(() => sndPlayer_Cp.ExistGameStates(
            Player_Phases.GameState_En.RoundFinished));
        sndPlayer_Cp.RemoveGameStates(Player_Phases.GameState_En.RoundFinished);

        yield return new WaitForSeconds(1f);

        //
        AddGameStates(GameState_En.RoundFinished);
    }

    //-------------------------------------------------- Set player action priority
    void SetPlayerActionPriority(int roundIndex)
    {
        StartCoroutine(Corou_SetPlayerActionPriority(roundIndex));
    }

    IEnumerator Corou_SetPlayerActionPriority(int roundIndex)
    {
        RoundValue_de localRoundValue = localRoundsData[roundIndex];
        RoundValue_de otherRoundValue = otherRoundsData[roundIndex];

        //
        int localPlayerPriority = 0;
        switch (localRoundsData[roundIndex].token.type)
        {
            case TokenType.Shien:
                localPlayerPriority = 3;
                break;
            case TokenType.Move:
                localPlayerPriority = 2;
                break;
            case TokenType.Attack:
                localPlayerPriority = 1;
                break;
            case TokenType.Null:
                if (localRoundValue.spMarkerCount > 0)
                {
                    localPlayerPriority = 4;
                }
                else
                {
                    localPlayerPriority = 0;
                }
                break;
        }

        //
        int otherPlayerPriority = 0;
        switch (otherRoundsData[roundIndex].token.type)
        {
            case TokenType.Shien:
                otherPlayerPriority = 3;
                break;
            case TokenType.Move:
                otherPlayerPriority = 2;
                break;
            case TokenType.Attack:
                otherPlayerPriority = 1;
                break;
            case TokenType.Null:
                if (otherRoundValue.spMarkerCount > 0)
                {
                    otherPlayerPriority = 4;
                }
                else
                {
                    otherPlayerPriority = 0;
                }
                break;
        }

        // evaulate priority using action token
        if (localPlayerPriority > otherPlayerPriority)
        {
            playerActionPriority = 0;
            yield break;
        }
        else if (localPlayerPriority < otherPlayerPriority)
        {
            playerActionPriority = 1;
            yield break;
        }
        else if (localPlayerPriority != 1 && localPlayerPriority != 1)
        {
            playerActionPriority = 0;
            yield break;
        }

        // evaulate priority using agi
        int localUnitAgi = localPlayer_Cp.bUnit_Cps[localRoundValue.originUnitIndex].unitCardData.agi;
        int otherUnitAgi = otherPlayer_Cp.bUnit_Cps[otherRoundValue.originUnitIndex].unitCardData.agi;
        if (localUnitAgi > otherUnitAgi)
        {
            playerActionPriority = 0;
            yield break;
        }
        else if (localUnitAgi < otherUnitAgi)
        {
            playerActionPriority = 1;
            yield break;
        }

        // show message
        btlUI_Cp.SetActionEffText("AGIが同じのため、ダイスを振り先行を決めます。");
        yield return new WaitUntil(() => btlUI_Cp.ExistGameStates(UI_BattlePhase.GameState_En.Done_ActionTxtShow));
        btlUI_Cp.FinishActionEffText();

        // evaluate priority using dice
        int localPlayerDice = -1, otherPlayerDice = -1;

        do
        {
            //
            ThrowDice(localPlayerID);
            yield return new WaitUntil(() => ExistGameStates(GameState_En.DiceThrown));
            RemoveGameStates(GameState_En.DiceThrown);

            localPlayerDice = dice;
            dice = -1;
            
            //
            ThrowDice(otherPlayer_Cp.playerID);
            yield return new WaitUntil(() => ExistGameStates(GameState_En.DiceThrown));
            RemoveGameStates(GameState_En.DiceThrown);

            otherPlayerDice = dice;
            dice = -1;

            // show message
            if (localPlayerDice > otherPlayerDice)
            {
                btlUI_Cp.SetActionEffText("Player1の先制攻撃　Player1：" + localPlayerDice + "　Player2："
                    + otherPlayerDice);
            }
            else if (localPlayerDice < otherPlayerDice)
            {
                btlUI_Cp.SetActionEffText("Player2の先制攻撃　Player1：" + localPlayerDice + "　Player2："
                    + otherPlayerDice);
            }
            else
            {
                btlUI_Cp.SetActionEffText("ダイスの目が同じなので再試行。　Player1：" + localPlayerDice + "　Player2："
                    + otherPlayerDice);
            }

            yield return new WaitUntil(() => btlUI_Cp.ExistGameStates(UI_BattlePhase.GameState_En.Done_ActionTxtShow));
            
            btlUI_Cp.FinishActionEffText();
        }
        while (localPlayerDice == otherPlayerDice);

        if (localPlayerDice > otherPlayerDice)
        {
            playerActionPriority = 0;
        }
        else if (localPlayerDice < otherPlayerDice)
        {
            playerActionPriority = 1;
        }
    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle dice
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle dice

    //-------------------------------------------------- Throw dice
    public void ThrowDice(int playerID_pr)
    {
        StartCoroutine(Corou_ThrowDice(playerID_pr));
    }

    IEnumerator Corou_ThrowDice(int playerID_pr)
    {
        // popup action effect text
        string actionEffText_tp = string.Empty;
        if (playerID_pr == 0)
        {
            actionEffText_tp = "Player1がサイコロを投げます。";
        }
        else if (playerID_pr == 1)
        {
            actionEffText_tp = "Player2がサイコロを投げます。";
        }

        btlUI_Cp.SetActionEffText(actionEffText_tp);
        yield return new WaitForSeconds(0.5f);

        //
        diceHandler_Cp.ThrowDice();
        yield return new WaitUntil(() => diceHandler_Cp.ExistGameStates(
            DiceHandler_de.GameState_En.DiceRollDone));
        diceHandler_Cp.RemoveGameStates(DiceHandler_de.GameState_En.DiceRollDone);

        //
        dice = diceHandler_Cp.diceTotalAmount;

        //
        diceHandler_Cp.ResetDiceRoll();
        yield return new WaitUntil(() => diceHandler_Cp.ExistGameStates(
            DiceHandler_de.GameState_En.DiceRollFinished));
        diceHandler_Cp.RemoveGameStates(DiceHandler_de.GameState_En.DiceRollFinished);

        // remove action effect text
        btlUI_Cp.FinishActionEffText();

        //****************************** game events 28th. after dice
        Hash128 hash_tp = RegRandHashValue();
        player_Cps[playerID_pr].unitAbil_Cp.HandleGameEventsTiming(GameEventsTiming.AftDice, hash_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash_tp));

        //
        AddGameStates(GameState_En.DiceThrown);
    }

    #endregion
}
