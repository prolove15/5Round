using FiveRound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ProgressHandler : MonoBehaviourPunCallbacks, IPunObservable
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Types
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing,
        TurnStarted, TurnFinished,
        RoundStarted, RoundFinished,
        StartPhaseStarted, StartPhaseFinished,
        StrPhaseStarted, StrPhaseFinished,
        BattlePhaseStarted, BattlePhaseFinished,
        SupplyPhaseStarted, SupplyPhaseFinished,
        EndPhaseStarted, EndPhaseFinished,
        NextPhaseClicked,
        ReadyForNextPhase,
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField] public GameInfo gameInfo;
    [SerializeField] public RoundHandler rndHandler_Cp;
    [SerializeField] public StrPhaseHandler strPhase_Cp;
    [SerializeField] public BattlePhaseHandler btlPhase_Cp;
    [SerializeField] public DiceHandler diceHandler_Cp;

    //-------------------------------------------------- public fields
    [SerializeField][ReadOnly] List<GameState_En> gameStates = new List<GameState_En>();

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    PlayerFaction player1_Cp, player2_Cp, localPlayer_Cp, otherPlayer_Cp;
    List<PlayerFaction> player_Cps = new List<PlayerFaction>();
    UI_GameCanvas gameUI_Cp;
    UI_PanelCanvas panelUI_Cp;
    NoticePanel modalNotice_Cp;

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

    public bool hasAuthority
    {
        get
        {
            bool result = false;
            if (isOnline)
            {
                if (photonView.IsMine)
                {
                    result = true;
                }
            }
            else
            {
                result = true;
            }
            return result;
        }
    }
    public bool isMine { get { return photonView.IsMine && PhotonNetwork.IsConnectedAndReady; } }

    //-------------------------------------------------- private properties
    bool isServer { get { return PhotonNetwork.IsMasterClient || !isOnline; } }
    bool isOnline { get { return PhotonNetwork.IsConnectedAndReady; } }

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

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Manage gameStates
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region ManageGameStates

    //--------------------------------------------------
    public void AddMainGameState(GameState_En value = GameState_En.Nothing)
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
    /// Initialize
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Initialize

    //--------------------------------------------------
    public void Init()
    {
        AddMainGameState(GameState_En.Nothing);

        SetComponents();
        InitComponents();
        InitVariables();

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        player1_Cp = controller_Cp.player_Cps[0];
        player2_Cp = controller_Cp.player_Cps[1];
        player_Cps = controller_Cp.player_Cps;
        localPlayer_Cp = controller_Cp.localPlayer_Cp;
        otherPlayer_Cp = controller_Cp.otherPlayer_Cp;
        gameUI_Cp = controller_Cp.ui_gameCanvas_Cp;
        panelUI_Cp = controller_Cp.ui_panelCanvas_Cp;
        modalNotice_Cp = panelUI_Cp.modalNotice_Cp;
    }

    //--------------------------------------------------
    void InitComponents()
    {
        rndHandler_Cp.Init();
        strPhase_Cp.Init();
        btlPhase_Cp.Init();
        diceHandler_Cp.Init();
    }

    //--------------------------------------------------
    void InitVariables()
    {
        
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play progress
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play progress

    //--------------------------------------------------
    public void PlayProgress()
    {
        StartCoroutine(Corou_PlayProgress());
    }

    IEnumerator Corou_PlayProgress()
    {
        for (int i = 0; i < 5; i++)
        {
            PlayTurn(i);
            yield return new WaitUntil(() => ExistGameStates(GameState_En.TurnFinished));
            RemoveGameStates(GameState_En.TurnFinished);
        }
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play turn
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play turn

    //-------------------------------------------------- Play Turn
    void PlayTurn(int turnIndex_tp)
    {
        StartCoroutine(Corou_PlayTurn(turnIndex_tp));
    }

    IEnumerator Corou_PlayTurn(int turnIndex_tp)
    {
        AddGameStates(GameState_En.TurnStarted);

        // play phases
        PlayStartPhase(turnIndex_tp);
        yield return new WaitUntil(() => ExistGameStates(GameState_En.StartPhaseFinished));
        RemoveGameStates(GameState_En.StartPhaseFinished);

        PlayStrPhase();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.StrPhaseFinished));
        RemoveGameStates(GameState_En.StrPhaseFinished);

        PlayBattlePhase();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.BattlePhaseFinished));
        RemoveGameStates(GameState_En.BattlePhaseFinished);

        PlaySupplyPhase();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.SupplyPhaseFinished));
        RemoveGameStates(GameState_En.SupplyPhaseFinished);

        PlayEndPhase();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.EndPhaseFinished));
        RemoveGameStates(GameState_En.EndPhaseFinished);

        //
        RemoveGameStates(GameState_En.TurnStarted);
        AddGameStates(GameState_En.TurnFinished);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play start phase
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play start phase

    //-------------------------------------------------- 
    void PlayStartPhase(int turnIndex_tp)
    {
        StartCoroutine(Corou_PlayStartPhase(turnIndex_tp));
    }

    IEnumerator Corou_PlayStartPhase(int turnIndex_tp)
    {
        AddGameStates(GameState_En.StartPhaseStarted);

        // set turn index
        SetTurnIndex(turnIndex_tp);
        yield return new WaitForSeconds(1f);

        // set phase name
        SetPhaseName(PhaseNames.startPhase);
        yield return new WaitForSeconds(1f);

        // play start phase
        for (int i = 0; i < player_Cps.Count; i++)
        {
            player_Cps[i].PlayStartPhase();
        }
        for (int i = 0; i < player_Cps.Count; i++)
        {
            yield return new WaitUntil(() => player_Cps[i].ExistGameStates(
                    PlayerFaction.GameState_En.StartPhaseFinished));
            player_Cps[i].RemoveGameStates(PlayerFaction.GameState_En.StartPhaseFinished);
        }

        // wait next phase clicked
        SetActiveNextPhaseBtn(true);
        yield return new WaitUntil(() => ExistGameStates(GameState_En.ReadyForNextPhase));
        RemoveGameStates(GameState_En.ReadyForNextPhase);

        //
        RemoveGameStates(GameState_En.StartPhaseStarted);
        AddGameStates(GameState_En.StartPhaseFinished);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play str phase
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play str phase

    //--------------------------------------------------
    void PlayStrPhase()
    {
        StartCoroutine(Corou_PlayStrPhase());
    }

    IEnumerator Corou_PlayStrPhase()
    {
        AddGameStates(GameState_En.StrPhaseStarted);

        // play str phase
        strPhase_Cp.PlayPhase();
        yield return new WaitUntil(() => strPhase_Cp.mainGameState == StrPhaseHandler.GameState_En.PhaseFinished);

        //
        RemoveGameStates(GameState_En.StrPhaseStarted);
        AddGameStates(GameState_En.StrPhaseFinished);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play battle phase
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play battle phase

    //--------------------------------------------------
    void PlayBattlePhase()
    {
        StartCoroutine(Corou_PlayBattlePhase());
    }

    IEnumerator Corou_PlayBattlePhase()
    {
        AddGameStates(GameState_En.BattlePhaseStarted);

        btlPhase_Cp.PlayPhase();
        yield return new WaitUntil(() => btlPhase_Cp.mainGameState == BattlePhaseHandler.GameState_En.PhaseFinished);
        
        //
        RemoveGameStates(GameState_En.BattlePhaseStarted);
        AddGameStates(GameState_En.BattlePhaseFinished);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play supply phase
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play supply phase

    //--------------------------------------------------
    void PlaySupplyPhase()
    {
        StartCoroutine(Corou_PlaySupplyPhase());
    }

    IEnumerator Corou_PlaySupplyPhase()
    {
        AddGameStates(GameState_En.SupplyPhaseStarted);

        // set phase name
        SetPhaseName(PhaseNames.supplyPhase);
        yield return new WaitForSeconds(3f);

        // play supply phase

        // wait next phase click
        yield return new WaitUntil(() => ExistGameStates(GameState_En.NextPhaseClicked));
        RemoveGameStates(GameState_En.NextPhaseClicked);

        //
        RemoveGameStates(GameState_En.SupplyPhaseStarted);
        AddGameStates(GameState_En.SupplyPhaseFinished);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play end phase
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play end phase

    //--------------------------------------------------
    void PlayEndPhase()
    {
        StartCoroutine(Corou_PlayEndPhase());
    }

    IEnumerator Corou_PlayEndPhase()
    {
        AddGameStates(GameState_En.EndPhaseStarted);

        // set phase name
        SetPhaseName(PhaseNames.endPhase);
        yield return new WaitForSeconds(3f);

        // play end phase

        // wait next phase click
        yield return new WaitUntil(() => ExistGameStates(GameState_En.NextPhaseClicked));
        RemoveGameStates(GameState_En.NextPhaseClicked);

        //
        RemoveGameStates(GameState_En.EndPhaseStarted);
        AddGameStates(GameState_En.EndPhaseFinished);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Set phase name
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Set phase name

    //--------------------------------------------------
    public void SetPhaseName(string phaseName_tp)
    {
        if (hasAuthority)
        {
            gameInfo.phaseName = phaseName_tp;
        }
        if (hasAuthority)
        {
            if (isOnline) { photonView.RPC("Rpc_SetPhaseName", RpcTarget.All, phaseName_tp); }
            else { Offline_SetPhaseName(phaseName_tp); }
        }
    }

    //--------------------------------------------------
    [PunRPC]
    void Rpc_SetPhaseName(string phaseName_tp)
    {
        Offline_SetPhaseName(phaseName_tp);
    }

    void Offline_SetPhaseName(string phaseName_tp)
    {
        gameUI_Cp.SetPhaseIndex(phaseName_tp);
        modalNotice_Cp.SetContent("第" + (gameInfo.turnIndex + 1).ToString() + "ターン", phaseName_tp, 2f);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Set turn index
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Set turn index

    //--------------------------------------------------
    void SetTurnIndex(int turnIndex_tp)
    {
        if (hasAuthority)
        {
            gameInfo.turnIndex = turnIndex_tp;
        }
        if (isMine)
        {
            photonView.RPC("Rpc_SetTurnIndex", RpcTarget.All, gameInfo.turnIndex);
        }
        else
        {
            Offline_SetTurnInde(turnIndex_tp);
        }
    }

    [PunRPC]
    void Rpc_SetTurnIndex(int turnIndex_tp)
    {
        Offline_SetTurnInde(turnIndex_tp);
    }

    void Offline_SetTurnInde(int turnIndex_tp)
    {
        gameUI_Cp.SetTurnIndex(turnIndex_tp + 1);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle next phase button
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle next phase button 

    //--------------------------------------------------
    public void OnClickNextPhase()
    {
        if (gameInfo.phaseName == PhaseNames.startPhase)
        {
            ReadyForNextPhase_StartPhase();
        }
        else if (gameInfo.phaseName == PhaseNames.strPhase)
        {
            strPhase_Cp.OnClickNextPhaseBtn();
        }
        else if (gameInfo.phaseName == PhaseNames.btlPhase)
        {
            btlPhase_Cp.OnClickToNextPhase();
        }
        else if (gameInfo.phaseName == PhaseNames.supplyPhase)
        {

        }
        else if (gameInfo.phaseName == PhaseNames.endPhase)
        {

        }
    }

    //--------------------------------------------------
    public void SetActiveNextPhaseBtn(bool flag)
    {
        gameUI_Cp.SetActiveNextPhaseBtn(flag);
    }

    //--------------------------------------------------
    public void ReadyForNextPhase_StartPhase()
    {
        StartCoroutine(Corou_ReadyForNextPhase_StartPhase());
    }

    IEnumerator Corou_ReadyForNextPhase_StartPhase()
    {
        SetActiveNextPhaseBtn(false);

        if (player1_Cp.hasAuthority) { player1_Cp.isReadyForNextPhase = true; }
        if (player2_Cp.hasAuthority) { player2_Cp.isReadyForNextPhase = true; }

        // check all players are ready
        yield return new WaitUntil(() => player1_Cp.isReadyForNextPhase && player2_Cp.isReadyForNextPhase);
        yield return new WaitForSeconds(1f);

        if (player1_Cp.hasAuthority) { player1_Cp.isReadyForNextPhase = false; }
        if (player2_Cp.hasAuthority) { player2_Cp.isReadyForNextPhase = false; }
        for (int i = 0; i < player_Cps.Count; i++)
        {
            yield return new WaitUntil(() => !player_Cps[i].isReadyForNextPhase);
        }

        AddGameStates(GameState_En.ReadyForNextPhase);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Network callback
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Network callback

    //--------------------------------------------------
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gameInfo.turnIndex);
            stream.SendNext(gameInfo.phaseName);
            stream.SendNext(gameInfo.rndIndex);
            stream.SendNext(gameInfo.cycleDur);
        }
        else
        {
            gameInfo.turnIndex = (int)stream.ReceiveNext();
            gameInfo.phaseName = (string)stream.ReceiveNext();
            gameInfo.rndIndex = (int)stream.ReceiveNext();
            gameInfo.cycleDur = (int)stream.ReceiveNext();
        }
    }

    #endregion
}
