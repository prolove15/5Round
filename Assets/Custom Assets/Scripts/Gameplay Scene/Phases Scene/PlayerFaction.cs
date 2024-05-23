using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerFaction : MonoBehaviourPunCallbacks, IPunObservable
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
        InitComponentFinished,
        StartPhaseStarted, StartPhaseFinished,
        StrPhaseStarted, StrPhaseFinished,
        StartRoundStarted, StartRoundFinished,
        PlayRoundStarted, PlayRoundDone, PlayRoundFinished,
        FinishRoundStarted, FinishRoundFinished,
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField] public int playerId;
    [SerializeField] public bool isLocalPlayer;
    [SerializeField] public bool isCom;
    [SerializeField] public Playerboard pBoard_Cp;
    [SerializeField] public Battleboard bBoard_Cp;
    [SerializeField] public Mihariboard mBoard_Cp;
    [SerializeField] RoundActionHandler rndAction_Cp;
    [SerializeField] public TokensData tokensData = new TokensData();
    [SerializeField] public MarkersData markersData = new MarkersData();

    //-------------------------------------------------- public fields
    [ReadOnly] public List<GameState_En> gameStates = new List<GameState_En>();

    [ReadOnly] public RoundData roundsData = new RoundData();
    [ReadOnly] public BattleInfo battleInfo = new BattleInfo();
    
    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    UI_GameCanvas gameUI_Cp;
    ProgressHandler progHandler_Cp;
    RoundHandler rndHandler_Cp;

    [SerializeField]
    [ReadOnly] int m_playerAp;

    [SerializeField]
    [ReadOnly] bool m_isReadyForNextPhase;

    [SerializeField]
    [ReadOnly] bool m_isTurn;

    [SerializeField]
    [ReadOnly] bool m_rndDoneFlag;

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
                if (photonView.IsMine) { result = true; }
            }
            else
            {
                result = true;
            }
            return result;
        }
    }
    public bool isMine { get { return photonView.IsMine && PhotonNetwork.IsConnectedAndReady; } }
    public int playerAp { get { return m_playerAp; } set { m_playerAp = value; } }
    public List<Unit_Bb> bbUnit_Cps { get { return bBoard_Cp.bbUnit_Cps; } }
    public List<UnitCardData> mUnitsData { get { return mBoard_Cp.mUnitsData; } }
    public bool isReadyForNextPhase { get { return m_isReadyForNextPhase; } set { SetIsReadyForNextPhase(value); } }
    public bool isTurn { get { return m_isTurn; } set { SetIsTurn(value); } }
    public bool rndDoneFlag { get { return m_rndDoneFlag; } set { SetRoundDoneFlag(value); } }

    //-------------------------------------------------- private properties
    bool isOnline { get { return PhotonNetwork.IsConnectedAndReady && PhotonNetwork.PlayerListOthers.Length > 0; } }
    bool isServer { get { return PhotonNetwork.IsMasterClient || !isOnline; } }

    #endregion

    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Methods
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////

    private void Awake()
    {
        AddMainGameState(GameState_En.Nothing);
        for (int i = 0; i < 5; i++)
        {
            roundsData.rndValues.Add(new RoundValue());
        }
    }

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
        StartCoroutine(Corou_Init());
    }

    IEnumerator Corou_Init()
    {
        //
        SetComponents();
        InitUI();
        InitTokensData();
        InitMarkersData();

        InitComponents();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.InitComponentFinished));
        RemoveGameStates(GameState_En.InitComponentFinished);

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        gameUI_Cp = controller_Cp.ui_gameCanvas_Cp;
        progHandler_Cp = controller_Cp.progHandler_Cp;
        rndHandler_Cp = controller_Cp.progHandler_Cp.rndHandler_Cp;
    }

    //--------------------------------------------------
    void InitUI()
    {
        if (PhotonNetwork.PlayerList.Length > playerId)
        {
            gameUI_Cp.SetPlayerName(playerId, PhotonNetwork.PlayerList[playerId].NickName);
        }
    }

    //--------------------------------------------------
    void InitTokensData()
    {
        
    }

    //--------------------------------------------------
    void InitMarkersData()
    {
        
    }

    //--------------------------------------------------
    void InitComponents()
    {
        StartCoroutine(Corou_InitComponents());
    }

    IEnumerator Corou_InitComponents()
    {
        pBoard_Cp.Init(this);
        yield return new WaitUntil(() => pBoard_Cp.mainGameState == Playerboard.GameState_En.Inited);

        bBoard_Cp.Init(this);
        yield return new WaitUntil(() => bBoard_Cp.mainGameState == Battleboard.GameState_En.Inited);

        mBoard_Cp.Init(this);
        rndAction_Cp.Init(this);

        AddGameStates(GameState_En.InitComponentFinished);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play

    //--------------------------------------------------
    public void Play()
    {
        mainGameState = GameState_En.Playing;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play start phase
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play start phase

    //--------------------------------------------------
    public void PlayStartPhase()
    {
        StartCoroutine(Corou_PlayStartPhase());
    }

    IEnumerator Corou_PlayStartPhase()
    {
        AddGameStates(GameState_En.StartPhaseStarted);

        if (hasAuthority) { SetAp(playerAp + 1); }
        
        yield return new WaitForSeconds(2f);

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
    public void PlayStrPhase()
    {
        StartCoroutine(Corou_PlayStrPhase());
    }

    IEnumerator Corou_PlayStrPhase()
    {
        AddGameStates(GameState_En.StrPhaseStarted);

        if (hasAuthority) { isReadyForNextPhase = false; }
        if (isLocalPlayer) { pBoard_Cp.SetActiveClickable(true); }

        yield return null;
    }

    //--------------------------------------------------
    public void EndStrPhase()
    {
        if (hasAuthority) { SetIsReadyForNextPhase(true); }

        RemoveGameStates(PlayerFaction.GameState_En.StrPhaseStarted);
        AddGameStates(PlayerFaction.GameState_En.StrPhaseFinished);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle round
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle round

    //-------------------------------------------------- start round
    public void StartRound()
    {
        StartCoroutine(Corou_StartRound());
    }

    IEnumerator Corou_StartRound()
    {
        mainGameState = GameState_En.StartRoundStarted;

        mainGameState = GameState_En.StartRoundFinished;
        yield return null;
    }

    //--------------------------------------------------
    public void OnLocal_PlayRound(int rndIndex_tp)
    {
        StartCoroutine(Corou_OnLocal_PlayRound(rndIndex_tp));
    }

    IEnumerator Corou_OnLocal_PlayRound(int rndIndex_tp)
    {
        mainGameState = GameState_En.PlayRoundStarted;

        rndAction_Cp.PlayRoundAction(rndIndex_tp);
        yield return new WaitUntil(() => rndAction_Cp.mainGameState
            == RoundActionHandler.GameState_En.RoundActionFinished);

        mainGameState = GameState_En.PlayRoundDone;
        m_rndDoneFlag = true;
    }

    //--------------------------------------------------
    public void FinishRound()
    {
        mainGameState = GameState_En.FinishRoundStarted;

        mainGameState = GameState_En.FinishRoundFinished;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Set Ap
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Set Ap

    //--------------------------------------------------
    public void SetAp(int ap_tp)
    {
        playerAp = ap_tp;

        if (isMine)
        {
            photonView.RPC("Rpc_SetAp_UI", RpcTarget.All, ap_tp);
        }
        else
        {
            Offline_SetAp_UI(ap_tp);
        }
    }

    [PunRPC]
    void Rpc_SetAp_UI(int ap_tp)
    {
        Offline_SetAp_UI(ap_tp);
    }

    void Offline_SetAp_UI(int ap_tp)
    {
        gameUI_Cp.SetAp(playerId, ap_tp);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Set isReadyForNextPhase
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Set isReadyForNextPhase

    //-------------------------------------------------- isReadyForNextPhase
    void SetIsReadyForNextPhase(bool flag)
    {
        if (isOnline) { photonView.RPC("Rpc_SetIsReadyForNextPhase", RpcTarget.All, flag); }
        else { Offline_SetIsReadyForNextPhase(flag); }

        if (isOnline) { photonView.RPC("Rpc_SetIsReadyForNextPhase_UI", RpcTarget.All, flag); }
        else { Offline_Rpc_SetIsReadyForNextPhase_UI(flag); }
    }

    [PunRPC]
    void Rpc_SetIsReadyForNextPhase(bool flag)
    {
        if (hasAuthority) { Offline_SetIsReadyForNextPhase(flag); }        
    }

    void Offline_SetIsReadyForNextPhase(bool flag)
    {
        m_isReadyForNextPhase = flag;
    }

    [PunRPC]
    void Rpc_SetIsReadyForNextPhase_UI(bool flag)
    {
        Offline_Rpc_SetIsReadyForNextPhase_UI(flag);
    }

    void Offline_Rpc_SetIsReadyForNextPhase_UI(bool flag)
    {
        gameUI_Cp.SetIsReadyForNextPhase(playerId, flag);
    }

    //-------------------------------------------------- isTurn
    void SetIsTurn(bool value)
    {
        if (isOnline) { photonView.RPC("Rpc_SetIsTurn", RpcTarget.All, value); }
        else { Offline_SetIsTurn(value); }
    }

    [PunRPC]
    void Rpc_SetIsTurn(bool value)
    {
        Offline_SetIsTurn(value);
    }

    void Offline_SetIsTurn(bool value)
    {
        if (!hasAuthority) { return; }

        m_isTurn = value;
        if (isOnline)
        {
            photonView.RPC("Rpc_SetActiveFireImage", RpcTarget.All, value);
        }
        else
        {
            Offline_Rpc_SetActiveFireImage(value);
        }
    }

    [PunRPC]
    void Rpc_SetActiveFireImage(bool flag)
    {
        Offline_Rpc_SetActiveFireImage(flag);
    }

    void Offline_Rpc_SetActiveFireImage(bool flag)
    {
        gameUI_Cp.playerUI_Cps[playerId].SetActiveFireImage(flag);
    }

    //-------------------------------------------------- set mainGameState
    public void SetRoundDoneFlag(bool rndDoneFlag_tp)
    {
        if (isOnline) { photonView.RPC("Rpc_SetRoundDoneFlag", RpcTarget.All, rndDoneFlag_tp); }
        else { Offline_SetRoundDoneFlag(rndDoneFlag_tp); }
    }

    [PunRPC]
    void Rpc_SetRoundDoneFlag(bool rndDoneFlag_tp)
    {
        if (hasAuthority) { Offline_SetRoundDoneFlag(rndDoneFlag_tp); }        
    }

    void Offline_SetRoundDoneFlag(bool rndDoneFlag_tp)
    {
        m_rndDoneFlag = rndDoneFlag_tp;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// External interface
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region External interface

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Callback from photon
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Callback from photon

    //--------------------------------------------------
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // playerAp
            stream.SendNext(m_playerAp);

            // roundsData
            for (int i = 0; i < roundsData.rndValues.Count; i++)
            {
                stream.SendNext(roundsData.rndValues[i].index);
                stream.SendNext(roundsData.rndValues[i].minAgi);
                stream.SendNext(roundsData.rndValues[i].actionType);
                stream.SendNext(roundsData.rndValues[i].spCount);
                stream.SendNext(roundsData.rndValues[i].tokenType);
                stream.SendNext(roundsData.rndValues[i].oriUnitIndex);
                stream.SendNext(roundsData.rndValues[i].tarUnitIndex);
                stream.SendNext(roundsData.rndValues[i].shienUnitId);
                stream.SendNext(roundsData.rndValues[i].atkType);
            }

            // tokensData
            stream.SendNext(tokensData.shien);
            stream.SendNext(tokensData.move1);
            stream.SendNext(tokensData.move2);
            stream.SendNext(tokensData.move3);
            stream.SendNext(tokensData.atk1);
            stream.SendNext(tokensData.atk2);
            stream.SendNext(tokensData.useShien);
            stream.SendNext(tokensData.useMove1);
            stream.SendNext(tokensData.useMove2);
            stream.SendNext(tokensData.useMove3);
            stream.SendNext(tokensData.useAtk1);
            stream.SendNext(tokensData.useAtk2);

            // markersData
            stream.SendNext(markersData.sp);
            stream.SendNext(markersData.useSp);
            stream.SendNext(markersData.gold);
            stream.SendNext(markersData.useGold);

            //
            stream.SendNext(m_isReadyForNextPhase);
            stream.SendNext(m_isTurn);
            stream.SendNext(m_rndDoneFlag);
        }
        else
        {
            // 
            m_playerAp = (int)stream.ReceiveNext();

            // roundsData
            for (int i = 0; i < roundsData.rndValues.Count; i++)
            {
                roundsData.rndValues[i].index = (int)stream.ReceiveNext();
                roundsData.rndValues[i].minAgi = (int)stream.ReceiveNext();
                roundsData.rndValues[i].actionType = (ActionType)stream.ReceiveNext();
                roundsData.rndValues[i].spCount = (int)stream.ReceiveNext();
                roundsData.rndValues[i].tokenType = (TokenType)stream.ReceiveNext();
                roundsData.rndValues[i].oriUnitIndex = (int)stream.ReceiveNext();
                roundsData.rndValues[i].tarUnitIndex = (int)stream.ReceiveNext();
                roundsData.rndValues[i].shienUnitId = (int)stream.ReceiveNext();
                roundsData.rndValues[i].atkType = (AttackType)stream.ReceiveNext();
            }

            // tokensData
            tokensData.shien = (int)stream.ReceiveNext();
            tokensData.move1 = (int)stream.ReceiveNext();
            tokensData.move2 = (int)stream.ReceiveNext();
            tokensData.move3 = (int)stream.ReceiveNext();
            tokensData.atk1 = (int)stream.ReceiveNext();
            tokensData.atk2 = (int)stream.ReceiveNext();
            tokensData.useShien = (int)stream.ReceiveNext();
            tokensData.useMove1 = (int)stream.ReceiveNext();
            tokensData.useMove2 = (int)stream.ReceiveNext();
            tokensData.useMove3 = (int)stream.ReceiveNext();
            tokensData.useAtk1 = (int)stream.ReceiveNext();
            tokensData.useAtk2 = (int)stream.ReceiveNext();

            // markersData
            markersData.sp = (int)stream.ReceiveNext();
            markersData.useSp = (int)stream.ReceiveNext();
            markersData.gold = (int)stream.ReceiveNext();
            markersData.useGold = (int)stream.ReceiveNext();

            //
            m_isReadyForNextPhase = (bool)stream.ReceiveNext();
            m_isTurn = (bool)stream.ReceiveNext();
            m_rndDoneFlag = (bool)stream.ReceiveNext();
        }
    }

    #endregion

}
