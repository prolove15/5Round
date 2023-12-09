using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        InitComponentsFinished, InitDataManagerFinished,
        StartPhaseStarted, StartPhaseFinished,
        StrPhaseStarted, StrPhaseFinished,
        BattlePhaseStarted, BattlePhaseFinished,
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
    Background bgd_Cp;

    [SerializeField]
    public Controller_StartPhase startController_Cp;

    [SerializeField]
    public Controller_StrPhase strController_Cp;

    [SerializeField]
    public Controller_BattlePhase btlController_Cp;

    [SerializeField]
    public StatusManager statusManager_Cp;

    [SerializeField]
    public Transform cam_Tf;

    [SerializeField]
    public DiceHandler diceHandler_Cp;

    [SerializeField]
    public Data_Phases data_Cp;

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
    public int localPlayerID, otherPlayerID, comPlayerID;

    [ReadOnly]
    public List<Player_Phases> player_Cps = new List<Player_Phases>();

    [ReadOnly]
    public Player_Phases localPlayer_Cp, otherPlayer_Cp, comPlayer_Cp;

    [ReadOnly]
    public List<GameEventsInfo> curGEventCollr = new List<GameEventsInfo>();

    [ReadOnly]
    public List<GameEventsInfo> waitGEventCollr = new List<GameEventsInfo>();

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
        Init();
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
        AddMainGameState(GameState_En.Nothing);

        //
        bgd_Cp.Init();

        //
        InitDataManager();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.InitDataManagerFinished));
        RemoveGameStates(GameState_En.InitDataManagerFinished);

        //
        InitPlayerComponentsAndIDs();

        //
        InitComponents();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.InitComponentsFinished));
        RemoveGameStates(GameState_En.InitComponentsFinished);

        //
        mainGameState = GameState_En.Inited;

        ReadyToPlay();
    }

    //--------------------------------------------------
    void InitDataManager()
    {
        StartCoroutine(Corou_InitDataManager());
    }

    IEnumerator Corou_InitDataManager()
    {
        dataManager_Cp = FindObjectOfType<DataManager_Gameplay>();

        if (!dataManager_Cp)
        {
            dataManager_Cp = new GameObject("DataManager").AddComponent<DataManager_Gameplay>();

            dataManager_Cp.Init();

            dataManager_Cp.LoadGameplayData();
            yield return new WaitUntil(() => dataManager_Cp.mainGameState
                == DataManager_Gameplay.GameState_En.LoadDBFinished);

            //dataManager_Cp.GenRandUnitCardsData_Phases();
            dataManager_Cp.GenPlayerUnitCardsDataByIndex(0, p1UnitIndexes.ToArray());
            dataManager_Cp.GenPlayerUnitCardsDataByIndex(1, p2UnitIndexes.ToArray());
            for (int i = 0; i < 2; i++)
            {
                dataManager_Cp.GenerateRandomPlayerBattleUnitCardsData(i);
                //dataManager_Cp.GenerateRandomPlayerMihariUnitCardsData(i);
                dataManager_Cp.GenRandPlMUnitsData(i);
            }

            dataManager_Cp.SupplyTakaraCardsData_Phases();
        }

        //
        AddGameStates(GameState_En.InitDataManagerFinished);
    }

    //--------------------------------------------------
    void InitPlayerComponentsAndIDs()
    {
        //
        Player_Phases[] player_Cps_tp = FindObjectsOfType<Player_Phases>();
        for (int i = 0; i < player_Cps_tp.Length; i++)
        {
            if (player_Cps_tp[i].isCreator)
            {
                player_Cps.Add(player_Cps_tp[i]);

                int otherPlayerID_tp = i == 0 ? 1 : 0;
                player_Cps.Add(player_Cps_tp[otherPlayerID_tp]);

                break;
            }
        }

        //
        for (int i = 0; i < player_Cps.Count; i++)
        {
            player_Cps[i].playerID = i;

            if (player_Cps[i].isLocalPlayer)
            {
                localPlayerID = i;
                localPlayer_Cp = player_Cps[i];
            }
            else
            {
                otherPlayerID = i;
                otherPlayer_Cp = player_Cps[i];
            }

            if (player_Cps[i].isCom)
            {
                comPlayerID = i;
                comPlayer_Cp = player_Cps[i];
            }
        }
    }

    //--------------------------------------------------
    void InitComponents()
    {
        StartCoroutine(Corou_InitComponents());
    }

    IEnumerator Corou_InitComponents()
    {
        for (int i = 0; i < player_Cps.Count; i++)
        {
            player_Cps[i].Init();
            yield return new WaitUntil(() => player_Cps[i].mainGameState
                == Player_Phases.GameState_En.Inited);
        }

        startController_Cp.Init();
        yield return new WaitUntil(() => startController_Cp.mainGameState
            == Controller_StartPhase.GameState_En.Inited);

        strController_Cp.Init();
        yield return new WaitUntil(() => strController_Cp.mainGameState
            == Controller_StrPhase.GameState_En.Inited);

        btlController_Cp.Init();
        yield return new WaitUntil(() => btlController_Cp.mainGameState
            == Controller_BattlePhase.GameState_En.Inited);

        //
        AddGameStates(GameState_En.InitComponentsFinished);
    }

    #endregion

    //-------------------------------------------------- ReadyToPlay
    void ReadyToPlay()
    {
        StartCoroutine(Corou_ReadyToPlay());
    }

    IEnumerator Corou_ReadyToPlay()
    {
        //
        bgd_Cp.curtain_Cp.CurtainUp();
        yield return new WaitUntil(() => bgd_Cp.curtain_Cp.mainGameState
            == Curtain.GameState_En.CurtainUpFinished);

        //
        Play();
    }

    //-------------------------------------------------- Play
    public void Play()
    {
        StartCoroutine(Corou_Play());
    }

    IEnumerator Corou_Play()
    {
        mainGameState = GameState_En.Playing;

        //
        PlayStartPhase();
        yield return new WaitUntil(() => mainGameState == GameState_En.StartPhaseFinished);

        //
        PlayStrPhase();
        yield return new WaitUntil(() => mainGameState == GameState_En.StrPhaseFinished);

        //
        PlayBattlePhase();
        yield return new WaitUntil(() => mainGameState == GameState_En.BattlePhaseFinished);
    }

    //-------------------------------------------------- 
    void PlayStartPhase()
    {
        StartCoroutine(Corou_PlayStartPhase());
    }

    IEnumerator Corou_PlayStartPhase()
    {
        mainGameState = GameState_En.StartPhaseStarted;

        //
        startController_Cp.PlayPhase();
        yield return new WaitUntil(() => startController_Cp.mainGameState
            == Controller_StartPhase.GameState_En.PhaseFinished);

        //
        mainGameState = GameState_En.StartPhaseFinished;
    }

    //--------------------------------------------------
    void PlayStrPhase()
    {
        StartCoroutine(Corou_PlayStrPhase());
    }

    IEnumerator Corou_PlayStrPhase()
    {
        mainGameState = GameState_En.StrPhaseStarted;

        //
        strController_Cp.PlayPhase();
        yield return new WaitUntil(() => strController_Cp.mainGameState
            == Controller_StrPhase.GameState_En.PhaseFinished);

        //
        mainGameState = GameState_En.StrPhaseFinished;
    }

    //--------------------------------------------------
    void PlayBattlePhase()
    {
        StartCoroutine(Corou_PlayBattlePhase());
    }

    IEnumerator Corou_PlayBattlePhase()
    {
        mainGameState = GameState_En.BattlePhaseStarted;

        //
        btlController_Cp.Init_BtlPhase();
        yield return new WaitUntil(() => btlController_Cp.mainGameState
            == Controller_BattlePhase.GameState_En.Inited);

        //
        btlController_Cp.PlayPhase();
        yield return new WaitUntil(() => btlController_Cp.mainGameState
            == Controller_BattlePhase.GameState_En.PhaseFinished);

        //
        mainGameState = GameState_En.BattlePhaseFinished;
    }

    //--------------------------------------------------
    public void OnFinishLeftTimeCounting()
    {
        if (mainGameState == GameState_En.StrPhaseStarted)
        {
            strController_Cp.ForceMoveToBattlePhase();
        }
    }

}
