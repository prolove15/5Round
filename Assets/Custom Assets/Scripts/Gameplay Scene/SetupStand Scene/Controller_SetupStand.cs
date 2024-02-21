using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Controller_SetupStand : MonoBehaviour
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
        ExtraInitFinished, InitComponentsFinished
    }

    [Serializable]
    public class Components
    {
        public Background background_Cp;

        public Transform camera_Tf;

        public UI_SetupStand uiManager_Cp;

        public Data_SetupStand data_Cp;

        public StatusManager statusManager_Cp;

        public HoldingHandler holdingHandler_Cp;
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
    // components fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    public Components components_Cp;

    [ReadOnly]
    public List<Party_SetupStand> party_Cps = new List<Party_SetupStand>();

    [ReadOnly]
    public ComPartyManager_SetupStand comPartyManager_Cp;

    [ReadOnly]
    public DataManager_Gameplay dataManager_Cp;

    // main fields
    [ReadOnly]
    public int localPlayerID, otherPlayerID, personPlayerID, comPlayerID;

    // normal fields

    //-------------------------------------------------- private fields
    [SerializeField]
    [ReadOnly]
    bool m_playersSetupStandReady;

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Properties
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Properties

    //-------------------------------------------------- public properties
    // components properties
    public GameState_En mainGameState
    {
        get { return gameStates[0]; }
        set { gameStates[0] = value; }
    }

    public Background background_Cp
    {
        get { return components_Cp.background_Cp; }
    }

    public Transform camera_Tf
    {
        get { return components_Cp.camera_Tf; }
    }

    public Party_SetupStand localParty_Cp
    {
        get
        {
            return party_Cps[localPlayerID];
        }
    }

    public Party_SetupStand otherParty_Cp
    {
        get
        {
            return party_Cps[otherPlayerID];
        }
    }

    public Party_SetupStand personParty_Cp
    {
        get { return party_Cps[personPlayerID]; }
    }

    public Party_SetupStand comParty_Cp
    {
        get { return party_Cps[comPlayerID]; }
    }

    public UI_SetupStand uiManager_Cp
    {
        get { return components_Cp.uiManager_Cp; }
    }

    public Data_SetupStand data_Cp
    {
        get { return components_Cp.data_Cp; }
    }

    public StatusManager statusManager_Cp
    {
        get { return components_Cp.statusManager_Cp; }
    }

    public HoldingHandler holdingHandler_Cp
    {
        get
        {
            return components_Cp.holdingHandler_Cp;
        }
    }

    // main properties
    public bool playersDecided
    {
        get { return m_playersSetupStandReady; }
        set
        {
            m_playersSetupStandReady = value;

            if (value)
            {
                LoadNextScene();
            }
        }
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
    public int GetExistGameStatesCount(GameState_En gameState_pr)
    {
        int result = 0;

        for (int i = 0; i < gameStates.Count; i++)
        {
            if (gameStates[i] == gameState_pr)
            {
                result++;
            }
        }

        return result;
    }

    //--------------------------------------------------
    public bool IsExistGameState(GameState_En gameState_pr)
    {
        return GetExistGameStatesCount(gameState_pr) > 0;
    }

    //--------------------------------------------------
    public void RemoveGameStates(GameState_En gameState_pr)
    {
        while (gameStates.Contains(gameState_pr))
        {
            gameStates.Remove(gameState_pr);
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
        StartCoroutine(CorouInit());
    }

    IEnumerator CorouInit()
    {
        gameStates.Add(GameState_En.Nothing);

        //
        if (!FindObjectOfType<Data_PartyDecision>())
        {
            ExtraInit_TestMode();
            yield return new WaitUntil(() => IsExistGameState(GameState_En.ExtraInitFinished));
            RemoveGameStates(GameState_En.ExtraInitFinished);
        }

        //
        SetComponents();

        InitComponents();
        yield return new WaitUntil(() => mainGameState == GameState_En.InitComponentsFinished);

        mainGameState = GameState_En.Inited;

        //
        PreparePlay();
    }

    //--------------------------------------------------
    void SetComponents()
    {
        SetPartyComponents();

        SetPlayersID();

        dataManager_Cp = FindObjectOfType<DataManager_Gameplay>();

        comPartyManager_Cp = comParty_Cp.comPartyManager_Cp;
    }

    //--------------------------------------------------
    void SetPlayersID()
    {
        // Set localPlayerID and otherPlayerID
        for (int i = 0; i < party_Cps.Count; i++)
        {
            if (party_Cps[i].isLocalPlayer)
            {
                localPlayerID = party_Cps[i].playerID;
                otherPlayerID = localPlayerID == 0 ? 1 : 0;
            }
        }

        // Set personPlayerID and comPlayerID
        personPlayerID = localPlayerID;
        comPlayerID = otherPlayerID;
    }

    //--------------------------------------------------
    void InitComponents()
    {
        StartCoroutine(CorouInitComponents());
    }

    IEnumerator CorouInitComponents()
    {
        //
        background_Cp.Init();

        //
        camera_Tf.SetPositionAndRotation(localParty_Cp.cameraLookPoint_Tf.position,
            localParty_Cp.cameraLookPoint_Tf.rotation);

        //
        uiManager_Cp.Init();

        //
        statusManager_Cp.localFactionID = localPlayerID;
        statusManager_Cp.Init();

        //
        personParty_Cp.isCom = false;
        comParty_Cp.isCom = true;
        
        //
        localParty_Cp.playerID = localPlayerID;
        localParty_Cp.Init();

        otherParty_Cp.playerID = otherPlayerID;
        otherParty_Cp.Init();

        //
        InitHoldingHandler();

        //
        mainGameState = GameState_En.InitComponentsFinished;

        yield return null;
    }

    //--------------------------------------------------
    void InitHoldingHandler()
    {
        holdingHandler_Cp.enableHoldDetect = true;

        UnityEvent onHoldEvent = new UnityEvent();
        onHoldEvent.AddListener(OnUnitHolded);
        holdingHandler_Cp.onHolded = onHoldEvent;

        UnityEvent onHoldEndedEvent = new UnityEvent();
        onHoldEndedEvent.AddListener(OnUnitUnHolded);
        holdingHandler_Cp.onHoldEnded = onHoldEndedEvent;
    }

    //--------------------------------------------------
    void SetPartyComponents()
    {
        // Set party components
        Party_SetupStand[] party_Cps_tp = FindObjectsOfType<Party_SetupStand>();

        for (int i = 0; i < party_Cps_tp.Length; i++)
        {
            if (party_Cps_tp[i].isLocalPlayer)
            {
                party_Cps.Add(party_Cps_tp[i]);

                party_Cps[0].playerID = 0;

                break;
            }
        }

        for (int i = 0; i < party_Cps_tp.Length; i++)
        {
            if (!party_Cps_tp[i].isLocalPlayer)
            {
                party_Cps.Add(party_Cps_tp[i]);

                party_Cps[1].playerID = 1;

                break;
            }
        }
    }

    //--------------------------------------------------
    void ExtraInit_TestMode()
    {
        StartCoroutine(CorouExtraInit_TestMode());
    }

    IEnumerator CorouExtraInit_TestMode()
    {
        //
        dataManager_Cp = new GameObject("DataManager").AddComponent<DataManager_Gameplay>();

        //
        dataManager_Cp.Init();
        dataManager_Cp.LoadGameplayData();
        yield return new WaitUntil(() => dataManager_Cp.mainGameState
            == DataManager_Gameplay.GameState_En.LoadDBFinished);
        dataManager_Cp.GenerateRandomUnitCardsData_SetupStand();

        //
        gameStates.Add(GameState_En.ExtraInitFinished);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Prepare play
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region PreparePlay

    //--------------------------------------------------
    public void PreparePlay()
    {
        StartCoroutine(CorouPreparePlay());
    }

    IEnumerator CorouPreparePlay()
    {
        background_Cp.curtain_Cp.CurtainUp();
        yield return new WaitUntil(() => background_Cp.curtain_Cp.mainGameState
            == CurtainHandler.GameState_En.CurtainUpFinished);

        //
        Play();
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

        //
        statusManager_Cp.StartAllCounting();
    }

    //--------------------------------------------------
    public void HandlePlayersDecision()
    {
        bool playersDecided_tp = true;

        for (int i = 0; i < party_Cps.Count; i++)
        {
            if (!party_Cps[i].decided)
            {
                playersDecided_tp = false;
                break;
            }
        }

        playersDecided = playersDecided_tp;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Transition scene
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region TransitionScene

    //--------------------------------------------------
    public void LoadNextScene()
    {
        StartCoroutine(CorouLoadNextScene());
    }

    IEnumerator CorouLoadNextScene()
    {
        //
        DontDestroyOnLoad(dataManager_Cp.gameObject);
        DontDestroyOnLoad(data_Cp.gameObject);

        //
        background_Cp.curtain_Cp.CurtainDown();
        yield return new WaitUntil(() => background_Cp.curtain_Cp.mainGameState
            == CurtainHandler.GameState_En.CurtainDownFinished);

        //
        LoadSceneHandler.LoadNextScene();
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// OnEvent
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region OnEvent

    //------------------------------
    void OnUnitHolded()
    {
        party_Cps[holdingHandler_Cp.holdedCard_Tf.GetComponent<UnitCard>().playerID].OnUnitHolded();
    }

    void OnUnitUnHolded()
    {
        party_Cps[holdingHandler_Cp.holdedCard_Tf.GetComponent<UnitCard>().playerID].OnUnitUnHolded();
    }

    //--------------------------------------------------
    public void OnClickDecisionBtn()
    {
        localParty_Cp.decided = true;
    }

    //------------------------------
    public void OnPerson_UnitUnHolded()
    {
        comPartyManager_Cp.OnPerson_UnitUnHolded();
    }

    #endregion

}
