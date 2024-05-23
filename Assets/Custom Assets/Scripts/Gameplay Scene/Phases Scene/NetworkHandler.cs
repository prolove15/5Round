using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkHandler : MonoBehaviourPun
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Types
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, InitFailed, Playing,
        InitOnlineModeSucc, InitOnlineModeFailed,
        NetworkReadyToStart,
        AssignLocalPlayerFinished,
        ReadyToPlay, AllReadyToPlay
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

    public bool isOnline { get { return PhotonNetwork.IsConnectedAndReady; } }
    public bool isServer { get { return PhotonNetwork.IsMasterClient || !isOnline; } }

    //-------------------------------------------------- private properties
    Controller_Phases controller_Cp;
    List<PlayerFaction> player_Cps;

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
        AddMainGameState(GameState_En.Nothing);

        SetComponents();

        InitOnlineMode();
        yield return new WaitUntil(() => ExistAnyGameStates(GameState_En.InitOnlineModeSucc,
            GameState_En.InitOnlineModeFailed));
        if (ExistGameStates(GameState_En.InitOnlineModeSucc))
        {
            RemoveGameStates(GameState_En.InitOnlineModeSucc);
            mainGameState = GameState_En.Inited;
            yield break;
        }
        else if (ExistGameStates(GameState_En.InitOnlineModeFailed))
        {
            RemoveGameStates(GameState_En.InitOnlineModeFailed);
            mainGameState = GameState_En.InitFailed;
            yield break;
        }
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        player_Cps = controller_Cp.player_Cps;
    }

    //--------------------------------------------------
    void InitOnlineMode()
    {
        StartCoroutine(Corou_InitOnlineMode());
    }

    IEnumerator Corou_InitOnlineMode()
    {
        // cehck network is ready
        photonView.RPC("Rpc_NetworkReadyToStart", RpcTarget.MasterClient);

        if (!PhotonNetwork.IsMasterClient)
        {
            yield break;
        }

        // check network is ready
        float curTime = Time.time;
        float maxWaitTime = 15f;
        yield return new WaitUntil(() => GetExistGameStatesCount(GameState_En.NetworkReadyToStart)
            == PhotonNetwork.PlayerList.Length || (Time.time - curTime) > maxWaitTime);
        if (GetExistGameStatesCount(GameState_En.NetworkReadyToStart) < PhotonNetwork.PlayerList.Length)
        {
            RemoveGameStates(GameState_En.NetworkReadyToStart);
            photonView.RPC("Rpc_SetGameState", RpcTarget.All, GameState_En.InitOnlineModeFailed);
            yield break;
        }
        else
        {
            RemoveGameStates(GameState_En.NetworkReadyToStart);
        }

        // assign ownership
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            player_Cps[i].photonView.TransferOwnership(PhotonNetwork.PlayerList[i]);
            yield return new WaitUntil(() => player_Cps[i].photonView.Owner == PhotonNetwork.PlayerList[i]);
        }
        photonView.RPC("Rpc_AssignLocalPlayer", RpcTarget.All);

        // wait assign ownership is finished
        curTime = Time.time;
        maxWaitTime = 15f;
        yield return new WaitUntil(() => GetExistGameStatesCount(GameState_En.AssignLocalPlayerFinished)
            == PhotonNetwork.PlayerList.Length || (Time.time - curTime) > maxWaitTime);
        if (GetExistGameStatesCount(GameState_En.AssignLocalPlayerFinished) < PhotonNetwork.PlayerList.Length)
        {
            RemoveGameStates(GameState_En.AssignLocalPlayerFinished);
            photonView.RPC("Rpc_SetGameState", RpcTarget.All, GameState_En.InitOnlineModeFailed);
            yield break;
        }
        else
        {
            RemoveGameStates(GameState_En.AssignLocalPlayerFinished);
        }

        //
        photonView.RPC("Rpc_SetGameState", RpcTarget.All, GameState_En.InitOnlineModeSucc);
    }

    //--------------------------------------------------
    [PunRPC]
    void Rpc_NetworkReadyToStart()
    {
        AddGameStates(GameState_En.NetworkReadyToStart);
    }

    //--------------------------------------------------
    [PunRPC]
    void Rpc_AssignLocalPlayer()
    {
        if (player_Cps[0].photonView.IsMine)
        {
            player_Cps[0].isLocalPlayer = true;
            player_Cps[1].isLocalPlayer = false;
        }
        else if (player_Cps[1].photonView.IsMine)
        {
            player_Cps[0].isLocalPlayer = false;
            player_Cps[1].isLocalPlayer = true;
        }
        photonView.RPC("Rpc_AssignLocalPlayerFinished", RpcTarget.MasterClient);
    }

    //--------------------------------------------------
    [PunRPC]
    void Rpc_AssignLocalPlayerFinished()
    {
        AddGameStates(GameState_En.AssignLocalPlayerFinished);
    }

    //--------------------------------------------------
    [PunRPC]
    void Rpc_SetGameState(GameState_En gameState_tp)
    {
        AddGameStates(gameState_tp);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Ready to play
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Ready to play

    //--------------------------------------------------
    public void ReadyToPlay()
    {
        photonView.RPC("Master_ReadyToPlay", RpcTarget.MasterClient);
    }

    //--------------------------------------------------
    [PunRPC]
    void Master_ReadyToPlay()
    {
        AddGameStates(GameState_En.ReadyToPlay);

        if (GetExistGameStatesCount(GameState_En.ReadyToPlay) == PhotonNetwork.PlayerList.Length)
        {
            RemoveGameStates(GameState_En.ReadyToPlay);
            photonView.RPC("Rpc_ReadyToPlay", RpcTarget.All);
        }
    }

    //--------------------------------------------------
    [PunRPC]
    void Rpc_ReadyToPlay()
    {
        AddGameStates(GameState_En.AllReadyToPlay);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Network interface
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Network interface

    #endregion

}
