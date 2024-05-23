using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundHandler : MonoBehaviourPun, IPunObservable
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
        RoundsStarted, RoundsFinished,
        RoundStarted, RoundFinished,
        ReadyToNext,
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields

    //-------------------------------------------------- public fields
    [SerializeField][ReadOnly] List<GameState_En> gameStates = new List<GameState_En>();

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    ProgressHandler progHandler_Cp;
    UI_GameCanvas gameUI_Cp;
    List<PlayerFaction> player_Cps = new List<PlayerFaction>();
    PlayerFaction localPlayer_Cp, otherPlayer_Cp, comPlayer_Cp;
    DiceHandler diceHandler_Cp;
    [SerializeField][ReadOnly] int firstPlayerId, sndPlayerId;

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
    public bool isOnlineAndMine { get { return photonView.IsMine && PhotonNetwork.IsConnectedAndReady; } }

    //-------------------------------------------------- private properties
    GameInfo gameInfo { get { return progHandler_Cp.gameInfo; } }
    bool isServer { get { return PhotonNetwork.IsMasterClient || !isOnline; } }
    bool isOnline { get { return PhotonNetwork.IsConnectedAndReady && PhotonNetwork.PlayerListOthers.Length > 0; } }

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
        AddGameStates(GameState_En.Nothing);
    }

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
        SetComponents();

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        progHandler_Cp = controller_Cp.progHandler_Cp;
        gameUI_Cp = controller_Cp.ui_gameCanvas_Cp;
        player_Cps = controller_Cp.player_Cps;
        localPlayer_Cp = controller_Cp.localPlayer_Cp;
        otherPlayer_Cp = controller_Cp.otherPlayer_Cp;
        comPlayer_Cp = controller_Cp.comPlayer_Cp;
        diceHandler_Cp = controller_Cp.progHandler_Cp.diceHandler_Cp;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle rounds
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle rounds

    //--------------------------------------------------
    public void HandleRounds()
    {
        StartCoroutine(Corou_HandleRounds());
    }

    IEnumerator Corou_HandleRounds()
    {
        AddGameStates(GameState_En.RoundsStarted);

        for (int i = 0; i < 5; i++)
        {
            HandleRound(i);
            yield return new WaitUntil(() => ExistGameStates(GameState_En.RoundFinished));
            RemoveGameStates(GameState_En.RoundFinished);
        }

        RemoveGameStates(GameState_En.RoundsStarted);
        AddGameStates(GameState_En.RoundsFinished);
    }

    //-------------------------------------------------- handle round
    void HandleRound(int rndIndex_tp)
    {
        StartCoroutine(Corou_HandleRound(rndIndex_tp));
    }

    IEnumerator Corou_HandleRound(int rndIndex_tp)
    {
        AddGameStates(GameState_En.RoundStarted);

        // prepare round
        Hash128 hash_tp = HashHandler.RegRandHash();
        PrepareRound(rndIndex_tp, hash_tp);
        yield return new WaitUntil(() => !HashHandler.ContainsHash(hash_tp));

        // start round
        player_Cps[firstPlayerId].StartRound();
        yield return new WaitUntil(() => player_Cps[firstPlayerId].mainGameState
            == PlayerFaction.GameState_En.StartRoundFinished);
        player_Cps[sndPlayerId].StartRound();
        yield return new WaitUntil(() => player_Cps[sndPlayerId].mainGameState
            == PlayerFaction.GameState_En.StartRoundFinished);

        // play round
        Hash128 hash2_tp = HashHandler.RegRandHash();
        PlayRound(rndIndex_tp, hash2_tp);
        yield return new WaitUntil(() => !HashHandler.ContainsHash(hash2_tp));

        // finish round
        player_Cps[firstPlayerId].FinishRound();
        yield return new WaitUntil(() => player_Cps[firstPlayerId].mainGameState
            == PlayerFaction.GameState_En.FinishRoundFinished);
        player_Cps[sndPlayerId].FinishRound();
        yield return new WaitUntil(() => player_Cps[sndPlayerId].mainGameState
            == PlayerFaction.GameState_En.FinishRoundFinished);

        // ready for next phase
        Hash128 hash3_tp = HashHandler.RegRandHash();
        PrepareNextRound(hash3_tp);
        yield return new WaitUntil(() => !HashHandler.ContainsHash(hash3_tp));

        if (comPlayer_Cp != null) { comPlayer_Cp.isReadyForNextPhase = true; }
    }

    //-------------------------------------------------- prepare round
    void PrepareRound(int rndIndex_tp, Hash128 hash_tp)
    {
        StartCoroutine(Corou_PrepareRound(rndIndex_tp, hash_tp));
    }

    IEnumerator Corou_PrepareRound(int rndIndex_tp, Hash128 hash_tp)
    {
        // set isReadyForNextPhase
        for (int i = 0; i < player_Cps.Count; i++)
        {
            player_Cps[i].isReadyForNextPhase = false;
        }

        // set round index
        if (hasAuthority) { SetRoundIndex(rndIndex_tp); }

        // deter player priority
        if (hasAuthority)
        {
            Hash128 hash2_tp = HashHandler.RegRandHash();
            SetPlayerActionPriority(rndIndex_tp, hash2_tp);
            yield return new WaitUntil(() => !HashHandler.ContainsHash(hash2_tp));
        }

        // remove hash_tp
        HashHandler.RemoveHash(hash_tp);
    }

    //-------------------------------------------------- play round
    void PlayRound(int rndIndex_tp, Hash128 hash_tp)
    {
        StartCoroutine(Corou_PlayRound(rndIndex_tp, hash_tp));
    }

    IEnumerator Corou_PlayRound(int rndIndex_tp, Hash128 hash_tp)
    {
        // first player round
        player_Cps[firstPlayerId].isTurn = true;
        OnPlayer_PlayRound(firstPlayerId, rndIndex_tp);
        yield return new WaitUntil(() => player_Cps[firstPlayerId].rndDoneFlag);
        player_Cps[firstPlayerId].rndDoneFlag = false;
        player_Cps[firstPlayerId].isTurn = false;

        // wait
        yield return new WaitForSeconds(1f);

        // snd player round
        player_Cps[sndPlayerId].isTurn = true;
        OnPlayer_PlayRound(sndPlayerId, rndIndex_tp);
        yield return new WaitUntil(() => player_Cps[sndPlayerId].rndDoneFlag);
        player_Cps[sndPlayerId].rndDoneFlag = false;
        player_Cps[sndPlayerId].isTurn = false;

        //
        HashHandler.RemoveHash(hash_tp);
    }

    //--------------------------------------------------
    void OnPlayer_PlayRound(int playerId_tp, int rndIndex_tp)
    {
        if (isOnline) { photonView.RPC("Rpc_OnPlayer_PlayRound", RpcTarget.All, playerId_tp, rndIndex_tp); }
        else { Offline_OnPlayer_PlayRound(playerId_tp, rndIndex_tp); }
    }

    [PunRPC]
    void Rpc_OnPlayer_PlayRound(int playerId_tp, int rndIndex_tp)
    {
        if (player_Cps[playerId_tp].hasAuthority)
        {
            Offline_OnPlayer_PlayRound(playerId_tp, rndIndex_tp);
        }
    }

    void Offline_OnPlayer_PlayRound(int playerId_tp, int rndIndex_tp)
    {
        player_Cps[playerId_tp].OnLocal_PlayRound(rndIndex_tp);
    }

    //-------------------------------------------------- prepare next round
    void PrepareNextRound(Hash128 hash_tp)
    {
        if (isOnline) { photonView.RPC("Rpc_PrepareNextRound", RpcTarget.All, true); }
        else { Offline_PrepareNextRound(true); }

        HashHandler.RemoveHash(hash_tp);
    }

    [PunRPC]
    void Rpc_PrepareNextRound(bool flag)
    {
        Offline_PrepareNextRound(flag);
    }

    void Offline_PrepareNextRound(bool flag)
    {
        gameUI_Cp.SetActiveNextPhaseBtn(flag);
        Invoke("OnClickToNextRound", 5f);
    }

    //-------------------------------------------------- reset round
    void ResetRound(Hash128 hash_tp)
    {
        StartCoroutine(Corou_ResetRound(hash_tp));
    }

    IEnumerator Corou_ResetRound(Hash128 hash_tp)
    {
        //
        if (hasAuthority)
        {
            firstPlayerId = -1;
            sndPlayerId = -1;
        }

        // reset isReadyForNextPhase
        for (int i = 0; i < player_Cps.Count; i++)
        {
            player_Cps[i].isReadyForNextPhase = false; 
        }
        for (int i = 0; i < player_Cps.Count; i++)
        {
            yield return new WaitUntil(() => !player_Cps[i].isReadyForNextPhase);
        }

        //
        SetAllActiveFireImage(false);

        HashHandler.RemoveHash(hash_tp);
    }

    //--------------------------------------------------
    void SetAllActiveFireImage(bool flag)
    {
        if (!hasAuthority) { Debug.LogWarning("hasAuthority is false"); return; }

        if (isOnline) { photonView.RPC("Rpc_SetAllActiveFireImage", RpcTarget.All, flag); }
        else { Offline_SetAllActiveFireImage(flag); }
    }

    [PunRPC]
    void Rpc_SetAllActiveFireImage(bool flag)
    {
        Offline_SetAllActiveFireImage(flag);
    }

    void Offline_SetAllActiveFireImage(bool flag)
    {
        for (int i = 0; i < gameUI_Cp.playerUI_Cps.Count; i++)
        {
            gameUI_Cp.playerUI_Cps[i].SetActiveFireImage(false);
        }
    }

    //-------------------------------------------------- set round index
    void SetRoundIndex(int rndIndex_tp)
    {
        if (!progHandler_Cp.hasAuthority) { Debug.LogWarning("hasAuthority is false"); return; }

        // set value
        gameInfo.rndIndex = rndIndex_tp;

        // set ui
        if (isOnline) { photonView.RPC("Rpc_SetRoundIndex_UI", RpcTarget.All, rndIndex_tp); }
        else { Offline_SetRoundIndex_UI(rndIndex_tp); }
    }

    [PunRPC]
    void Rpc_SetRoundIndex_UI(int rndIndex_tp)
    {
        Offline_SetRoundIndex_UI(rndIndex_tp);
    }

    void Offline_SetRoundIndex_UI(int rndIndex_tp)
    {
        gameUI_Cp.SetRoundIndex(rndIndex_tp + 1);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Set player action priority
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Set player action priority

    //-------------------------------------------------- Set player action priority
    void SetPlayerActionPriority(int roundIndex, Hash128 hash_tp)
    {
        StartCoroutine(Corou_SetPlayerActionPriority(roundIndex, hash_tp));
    }

    IEnumerator Corou_SetPlayerActionPriority(int roundIndex, Hash128 hash_tp)
    {
        RoundValue localRoundValue = localPlayer_Cp.roundsData[roundIndex];
        RoundValue otherRoundValue = otherPlayer_Cp.roundsData[roundIndex];

        // evaulate priority using action token
        int localPlayerPriority = 0;
        switch (localRoundValue.actionType)
        {
            case ActionType.Guard:
                localPlayerPriority = 4;
                break;
            case ActionType.Shien:
                localPlayerPriority = 3;
                break;
            case ActionType.Move:
                localPlayerPriority = 2;
                break;
            case ActionType.Atk:
                localPlayerPriority = 1;
                break;
            default:
                localPlayerPriority = 0;
                break;
        }

        int otherPlayerPriority = 0;
        switch (otherRoundValue.actionType)
        {
            case ActionType.Guard:
                otherPlayerPriority = 4;
                break;
            case ActionType.Shien:
                otherPlayerPriority = 3;
                break;
            case ActionType.Move:
                otherPlayerPriority = 2;
                break;
            case ActionType.Atk:
                otherPlayerPriority = 1;
                break;
            default:
                otherPlayerPriority = 0;
                break;
        }

        if (localPlayerPriority > otherPlayerPriority)
        {
            firstPlayerId = 0;
            sndPlayerId = 1;
            HashHandler.RemoveHash(hash_tp);
            yield break;
        }
        else if (localPlayerPriority < otherPlayerPriority)
        {
            firstPlayerId = 1;
            sndPlayerId = 0;
            HashHandler.RemoveHash(hash_tp);
            yield break;
        }
        else if (localPlayerPriority != 1)
        {
            firstPlayerId = 0;
            sndPlayerId = 1;
            HashHandler.RemoveHash(hash_tp);
            yield break;
        }

        // evaulate priority using agi
        int localUnitAgi = localPlayer_Cp.bBoard_Cp.bbUnit_Cps[localRoundValue.oriUnitIndex].unitInfo.agi;
        int otherUnitAgi = otherPlayer_Cp.bBoard_Cp.bbUnit_Cps[otherRoundValue.oriUnitIndex].unitInfo.agi;
        if (localUnitAgi > otherUnitAgi)
        {
            firstPlayerId = 0;
            sndPlayerId = 1;
            HashHandler.RemoveHash(hash_tp);
            yield break;
        }
        else if (localUnitAgi < otherUnitAgi)
        {
            firstPlayerId = 1;
            sndPlayerId = 0;
            HashHandler.RemoveHash(hash_tp);
            yield break;
        }

        // evaluate priority using dice
        int localPlayerDice = -1, otherPlayerDice = -1;
        do
        {
            // local player cast a dice
            diceHandler_Cp.ThrowDice(localPlayer_Cp.playerId);
            yield return new WaitUntil(() => diceHandler_Cp.ExistGameStates(DiceHandler.GameState_En.DiceRollDone));
            diceHandler_Cp.RemoveGameStates(DiceHandler.GameState_En.DiceRollDone);
            localPlayerDice = diceHandler_Cp.diceTotalAmount;

            diceHandler_Cp.ResetDiceRoll();
            yield return new WaitUntil(() => diceHandler_Cp.ExistGameStates(DiceHandler.GameState_En.DiceRollFinished));
            diceHandler_Cp.RemoveGameStates(DiceHandler.GameState_En.DiceRollFinished);

            // other player cast a dice
            diceHandler_Cp.ThrowDice(otherPlayer_Cp.playerId);
            yield return new WaitUntil(() => diceHandler_Cp.ExistGameStates(DiceHandler.GameState_En.DiceRollDone));
            diceHandler_Cp.RemoveGameStates(DiceHandler.GameState_En.DiceRollDone);
            otherPlayerDice = diceHandler_Cp.diceTotalAmount;

            diceHandler_Cp.ResetDiceRoll();
            yield return new WaitUntil(() => diceHandler_Cp.ExistGameStates(DiceHandler.GameState_En.DiceRollFinished));
            diceHandler_Cp.RemoveGameStates(DiceHandler.GameState_En.DiceRollFinished);
        }
        while (localPlayerDice == otherPlayerDice);

        if (localPlayerDice > otherPlayerDice)
        {
            firstPlayerId = 0;
            sndPlayerId = 1;
            HashHandler.RemoveHash(hash_tp);
            yield break;
        }
        else if (localPlayerDice < otherPlayerDice)
        {
            firstPlayerId = 1;
            sndPlayerId = 0;
            HashHandler.RemoveHash(hash_tp);
            yield break;
        }
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle moving to next round
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle moving to next round

    //--------------------------------------------------
    public void OnClickToNextRound()
    {
        MoveToNextRound(localPlayer_Cp);
    }

    //--------------------------------------------------
    void MoveToNextRound(PlayerFaction player_Cp_tp)
    {
        DebugHandler.Log("MoveToNextRound started");
        if (isServer) { StartCoroutine(Corou_OnServer_MoveToNextRound(player_Cp_tp)); }
        else { StartCoroutine(Corou_OnClient_MoveToNextRound(player_Cp_tp)); }
    }

    IEnumerator Corou_OnServer_MoveToNextRound(PlayerFaction player_Cp_tp)
    {
        gameUI_Cp.SetActiveNextPhaseBtn(false);
        CancelInvoke("OnClickToNextRound");
        if (player_Cp_tp.hasAuthority) { player_Cp_tp.isReadyForNextPhase = true; }

        // wait all is ready
        for (int i = 0; i < player_Cps.Count; i++)
        {
            yield return new WaitUntil(() => player_Cps[i].isReadyForNextPhase);
        }

        // wait for a while
        yield return new WaitForSeconds(1f);

        // reset round data
        Hash128 hash_tp = HashHandler.RegRandHash();
        ResetRound(hash_tp);
        yield return new WaitUntil(() => !HashHandler.ContainsHash(hash_tp));

        RemoveGameStates(GameState_En.RoundStarted);
        AddGameStates(GameState_En.RoundFinished);
    }

    IEnumerator Corou_OnClient_MoveToNextRound(PlayerFaction player_Cp_tp)
    {
        gameUI_Cp.SetActiveNextPhaseBtn(false);
        CancelInvoke("OnClickToNextRound");
        if (player_Cp_tp.hasAuthority) { player_Cp_tp.isReadyForNextPhase = true; }

        yield return null;
    }

    #endregion

    //--------------------------------------------------
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gameStates[0]);
            stream.SendNext(firstPlayerId);
            stream.SendNext(sndPlayerId);
        }
        else
        {
            gameStates[0] = (GameState_En)stream.ReceiveNext();
            firstPlayerId = (int)stream.ReceiveNext();
            sndPlayerId = (int)stream.ReceiveNext();
        }
    }

}
