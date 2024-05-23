using FiveRound;
using Michsky.UI.Shift;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller_Phases : MonoBehaviour
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
        SignInUpFinished,
        InitComponentsFinished, InitDataManagerFinished,
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField] public CanvasUI curtainUI_Cp;
    [SerializeField] public UI_Phases uiManager_Cp;
    [SerializeField] public UI_GameCanvas ui_gameCanvas_Cp;
    [SerializeField] public UI_PanelCanvas ui_panelCanvas_Cp;
    [SerializeField] public UI_TopCanvas ui_topCanvas_Cp;
    [SerializeField] public List<PlayerFaction> player_Cps = new List<PlayerFaction>();
    [SerializeField] public Data_Phases data_Cp;
    [SerializeField] public ProgressHandler progHandler_Cp;
    [SerializeField] public NetworkHandler netHandler_Cp;

    [SerializeField]
    public Controller_StartPhase startController_Cp; // depreciated

    [SerializeField]
    public Controller_StrPhase strController_Cp; // depreciated

    [SerializeField]
    public Controller_BattlePhase btlController_Cp; // depreciated

    [SerializeField]
    public StatusManager statusManager_Cp; // depreciated

    [SerializeField]
    public Transform cam_Tf; // depreciated

    [SerializeField]
    public List<Player_Phases> player_Cps_de = new List<Player_Phases>(); // depreciated

    [SerializeField]
    public DiceHandler_de diceHandler_Cp;

    [SerializeField]
    List<int> p1UnitIndexes = new List<int>();

    [SerializeField]
    List<int> p2UnitIndexes = new List<int>();

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    [ReadOnly]
    public DataManager_Gameplay dataManager_Cp;

    [ReadOnly]
    public int localPlayerID_de, otherPlayerID_de, comPlayerID_de;

    [ReadOnly]
    public Player_Phases localPlayer_Cp_de, otherPlayer_Cp_de, comPlayer_Cp_de; // depreciated

    [ReadOnly] public PlayerFaction localPlayer_Cp, otherPlayer_Cp, comPlayer_Cp;
    [ReadOnly] public int localPlayerId, otherPlayerId, comPlayerId;

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

    bool isOnline { get { return PhotonNetwork.IsConnectedAndReady; } }

    //-------------------------------------------------- private properties

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
        curtainUI_Cp.gameObject.SetActive(true);
        AddMainGameState(GameState_En.Nothing);
    }

    //-------------------------------------------------- Start is called before the first frame update
    async void Start()
    {
        Init();
        await InitDataManager();
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
        // top canvas ui
        ui_topCanvas_Cp.Init();
        yield return new WaitUntil(() => ui_topCanvas_Cp.mainGameState == UI_TopCanvas.GameState_En.Inited);
        ui_topCanvas_Cp.Play_Load();
        yield return new WaitUntil(() => ui_topCanvas_Cp.mainGameState == UI_TopCanvas.GameState_En.LoadStarted);

        // curtain
        curtainUI_Cp.Hide();

        // get data
        yield return new WaitUntil(() => ExistGameStates(GameState_En.InitDataManagerFinished));
        RemoveGameStates(GameState_En.InitDataManagerFinished);
        GetDataFromDataManager();

        // net handler
        if (netHandler_Cp.isOnline)
        {
            netHandler_Cp.Init();
            yield return new WaitUntil(() => netHandler_Cp.mainGameState == NetworkHandler.GameState_En.Inited
                || netHandler_Cp.mainGameState == NetworkHandler.GameState_En.InitFailed);
            if (netHandler_Cp.mainGameState == NetworkHandler.GameState_En.InitFailed)
            {
                ReturnToMainScene();
                yield break;
            }
        }

        //
        SetPlayerComponents();

        //
        InitComponents();
        yield return new WaitUntil(() => mainGameState == GameState_En.InitComponentsFinished);

        mainGameState = GameState_En.Inited;

        ReadyToPlay();
    }

    //--------------------------------------------------
    async Task InitDataManager()
    {
        dataManager_Cp = GameObject.FindObjectOfType<DataManager_Gameplay>();
        if (dataManager_Cp == null)
        {
            dataManager_Cp = new GameObject("DataManager").AddComponent<DataManager_Gameplay>();
            dataManager_Cp.Init();
            await dataManager_Cp.LoadGamePlayRealData();
        }

        //
        AddGameStates(GameState_En.InitDataManagerFinished);
    }

    //--------------------------------------------------
    void GetDataFromDataManager()
    {
        dataManager_Cp.GenPlayerUnitCardsDataByIndex(0, p1UnitIndexes.ToArray());
        dataManager_Cp.GenPlayerUnitCardsDataByIndex(1, p2UnitIndexes.ToArray());
        for (int i = 0; i < 2; i++)
        {
            dataManager_Cp.GenerateRandomPlayerBattleUnitCardsData(i);
            dataManager_Cp.GenRandPlMUnitsData(i);
        }

        dataManager_Cp.SupplyTakaraCardsData_Phases();
    }

    //--------------------------------------------------
    void SetPlayerComponents()
    {
        if (netHandler_Cp.isOnline)
        {
            if (player_Cps[0].hasAuthority)
            {
                localPlayer_Cp = player_Cps[0];
                localPlayer_Cp.isLocalPlayer = true;
                otherPlayer_Cp = player_Cps[1];
                otherPlayer_Cp.isLocalPlayer = false;
            }
            else
            {
                localPlayer_Cp = player_Cps[1];
                localPlayer_Cp.isLocalPlayer = true;
                otherPlayer_Cp = player_Cps[0];
                otherPlayer_Cp.isLocalPlayer = false;
            }
            if (PhotonNetwork.PlayerList.Length == 2)
            {
                localPlayer_Cp.isCom = false;
                otherPlayer_Cp.isCom = false;
                comPlayerId = -1;
            }
            else
            {
                otherPlayer_Cp.isCom = true;
                comPlayer_Cp = otherPlayer_Cp;
                comPlayerId = comPlayer_Cp.playerId;
            }
        }
        else
        {
            for (int i = 0; i < player_Cps.Count; i++)
            {
                if (player_Cps[i].isLocalPlayer)
                {
                    localPlayer_Cp = player_Cps[i];
                }
                else
                {
                    otherPlayer_Cp = player_Cps[i];
                }
                if (player_Cps[i].isCom)
                {
                    comPlayer_Cp = player_Cps[i];
                    comPlayerId = comPlayer_Cp.playerId;
                }
            }
        }

        localPlayerId = localPlayer_Cp.playerId;
        otherPlayerId = otherPlayer_Cp.playerId;
    }

    //--------------------------------------------------
    void InitComponents()
    {
        StartCoroutine(Corou_InitComponents());
    }

    IEnumerator Corou_InitComponents()
    {
        //
        ui_panelCanvas_Cp.Init();
        yield return new WaitUntil(() => ui_panelCanvas_Cp.mainGameState == UI_PanelCanvas.GameState_En.Inited);

        //
        ui_gameCanvas_Cp.Init();

        //
        for (int i = 0; i < player_Cps.Count; i++)
        {
            player_Cps[i].Init();
            yield return new WaitUntil(() => player_Cps[i].mainGameState == PlayerFaction.GameState_En.Inited);
        }

        //
        progHandler_Cp.Init();

        //
        data_Cp.Init();
        yield return new WaitUntil(() => data_Cp.mainGameState == Data_Phases.GameState_En.Inited);

        //
        mainGameState = GameState_En.InitComponentsFinished;
    }

    #endregion

    //-------------------------------------------------- ReadyToPlay
    void ReadyToPlay()
    {
        StartCoroutine(Corou_ReadyToPlay());
    }

    IEnumerator Corou_ReadyToPlay()
    {
        if (netHandler_Cp.isOnline)
        {
            netHandler_Cp.ReadyToPlay();
            yield return new WaitUntil(() => netHandler_Cp.ExistGameStates(NetworkHandler.GameState_En.AllReadyToPlay));
            netHandler_Cp.RemoveGameStates(NetworkHandler.GameState_En.AllReadyToPlay);
        }

        //
        ui_topCanvas_Cp.Finish_Load();
        yield return new WaitUntil(() => ui_topCanvas_Cp.mainGameState
            == UI_TopCanvas.GameState_En.LoadFinished);

        //
        Play();
    }

    //-------------------------------------------------- Play
    public void Play()
    {
        mainGameState = GameState_En.Playing;

        progHandler_Cp.PlayProgress();
    }

    //-------------------------------------------------- ReturnToMain
    public void ReturnToMainScene()
    {
        Debug.LogError("ReturnToMainScene");
        curtainUI_Cp.Show(false, () => HandleReturnToMainScene());
    }

    void HandleReturnToMainScene()
    {
        StartCoroutine(Corou_HandleReturnToMainScene());
    }

    IEnumerator Corou_HandleReturnToMainScene()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
            while (PhotonNetwork.IsConnected)
            {
                yield return null;
            }
        }

        SceneManager.LoadScene("Main");
    }

}
