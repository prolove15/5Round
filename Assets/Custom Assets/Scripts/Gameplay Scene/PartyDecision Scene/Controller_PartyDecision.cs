using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Controller_PartyDecision : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    // Types
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing,
        Inited, ExtraInitFinished, InitVariableFinished, PreparePlayFinished,
        Playing, WillFinish
    }

    [Serializable]
    public class Components
    {
        [Tooltip("Background component")]
        public Background_PartyDecision bgd_Cp;

        [Tooltip("UI Manager component")]
        public UI_PartyDecision uiManager_Cp;

        [Tooltip("Data component")]
        public Data_PartyDecision data_Cp;

        public StatusManager statusManager_Cp;

        [Tooltip("Main camera")]
        public Transform camera_Tf;

        public HoldingHandler holdingHandler_Cp;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    // Fields
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField]
    public Components components;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    [ReadOnly]
    public int localPlayerID, otherPlayerID, personPlayerID, comPlayerID;

    [ReadOnly]
    public DataManager_Gameplay dataManager_Cp;

    [ReadOnly]
    public List<Cards_PartyDecision> cardsManager_Cps;

    [ReadOnly]
    public bool isComExist;

    //-------------------------------------------------- private fields

    #endregion

    //////////////////////////////////////////////////////////////////////
    // Properties
    //////////////////////////////////////////////////////////////////////
    #region Properties

    //-------------------------------------------------- public properties
    public GameState_En mainGameState
    {
        get { return gameStates[0]; }
        set { gameStates[0] = value; }
    }

    public Background_PartyDecision bgd_Cp
    {
        get { return components.bgd_Cp; }
    }

    public UI_PartyDecision uiManager_Cp
    {
        get { return components.uiManager_Cp; }
    }

    public Data_PartyDecision data_Cp
    {
        get { return components.data_Cp; }
    }

    public StatusManager statusManager_Cp
    {
        get { return components.statusManager_Cp; }
    }

    public Cards_PartyDecision LocalCardsManager_Cp
    {
        get { return cardsManager_Cps[localPlayerID]; }
    }

    public Cards_PartyDecision otherCardsManager_Cp
    {
        get { return cardsManager_Cps[otherPlayerID]; }
    }

    public Cards_PartyDecision personCardsManager_Cp
    {
        get { return cardsManager_Cps[personPlayerID]; }
    }

    public Cards_PartyDecision comCardsManager_Cp
    {
        get { return cardsManager_Cps[comPlayerID]; }
    }

    public Transform camera_Tf
    {
        get { return components.camera_Tf; }
    }

    public HoldingHandler holdingHandler_Cp
    {
        get { return components.holdingHandler_Cp; }
    }

    //-------------------------------------------------- private properties

    #endregion

    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////
    // Methods
    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////

    //------------------------------ Start is called before the first frame update
    void Start()
    {
        Init();
    }


    //------------------------------ Update is called once per frame
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
    // Initialize
    //////////////////////////////////////////////////////////////////////
    #region Initialize

    //------------------------------
    public void Init()
    {
        StartCoroutine(CorouInit());
    }

    IEnumerator CorouInit()
    {
        //
        gameStates.Add(GameState_En.Nothing);

        if (!FindObjectOfType<DataManager_Gameplay>())
        {
            ExtraInit_TestMode();
            yield return new WaitUntil(() => IsExistGameState(GameState_En.ExtraInitFinished));
            RemoveGameStates(GameState_En.ExtraInitFinished);
        }

        //
        SetComponents();

        InitSceneUnitCardsData();

        InitVariables();
        yield return new WaitUntil(() => mainGameState == GameState_En.InitVariableFinished);

        InitCameraPosition();

        //
        mainGameState = GameState_En.Inited;

        PreparePlay();
    }

    //------------------------------
    void SetComponents()
    {
        if (!dataManager_Cp)
        {
            dataManager_Cp = FindObjectOfType<DataManager_Gameplay>();
        }

        SetCardsComponents();
    }

    //------------------------------
    void SetCardsComponents()
    {
        Cards_PartyDecision[] cardsManager_Cps_tp = FindObjectsOfType<Cards_PartyDecision>();

        //
        for (int i = 0; i < cardsManager_Cps_tp.Length; i++)
        {
            if (cardsManager_Cps_tp[i].isLocalPlayer)
            {
                //
                localPlayerID = 0;
                cardsManager_Cps_tp[i].playerID = 0;
                cardsManager_Cps.Add(cardsManager_Cps_tp[i]);

                //
                otherPlayerID = 1;
                int j = (i == 0 ? 1 : 0);
                cardsManager_Cps_tp[j].playerID = 1;
                cardsManager_Cps.Add(cardsManager_Cps_tp[j]);
                break;
            }
        }

        //
        for (int i = 0; i < cardsManager_Cps.Count; i++)
        {
            if (cardsManager_Cps_tp[i].isCom)
            {
                comPlayerID = i;
                isComExist = true;

                personPlayerID = i == 0 ? 1 : 0;
            }
        }
    }

    //------------------------------
    void InitVariables()
    {
        StartCoroutine(CorouInitVariables());
    }

    IEnumerator CorouInitVariables()
    {
        //
        bgd_Cp.Init();

        //
        LocalCardsManager_Cp.Init();
        otherCardsManager_Cp.Init();

        //
        InitHoldingHandler();

        //
        uiManager_Cp.Init();

        statusManager_Cp.localFactionID = localPlayerID;
        statusManager_Cp.Init();

        //
        mainGameState = GameState_En.InitVariableFinished;

        yield return null;
    }

    //------------------------------
    void InitSceneUnitCardsData()
    {
        //
        for (int i = 0; i < dataManager_Cp.psUnitCardsData.Count; i++)
        {
            data_Cp.SetUnitCardsData(i, dataManager_Cp.psUnitCardsData[i]);
        }
    }

    //------------------------------
    void InitCameraPosition()
    {
        camera_Tf.SetPositionAndRotation(LocalCardsManager_Cp.unitsLookPoint_Tf.position,
            LocalCardsManager_Cp.unitsLookPoint_Tf.rotation);
    }

    //------------------------------
    void InitHoldingHandler()
    {
        holdingHandler_Cp.enableHoldDetect = true;

        UnityEvent onHoldEvent = new UnityEvent();
        onHoldEvent.AddListener(OnHoldedObject);
        holdingHandler_Cp.onHolded = onHoldEvent;

        UnityEvent onHoldEndedEvent = new UnityEvent();
        onHoldEndedEvent.AddListener(OnHoldEndedObject);
        holdingHandler_Cp.onHoldEnded = onHoldEndedEvent;
    }

    //------------------------------------------------------------
    void ExtraInit_TestMode()
    {
        StartCoroutine(Corou_ExtraInit_TestMode());
    }

    IEnumerator Corou_ExtraInit_TestMode()
    {
        dataManager_Cp = new GameObject("Data Manager").AddComponent<DataManager_Gameplay>();

        dataManager_Cp.Init();
        dataManager_Cp.LoadGameplayData();
        yield return new WaitUntil(() => dataManager_Cp.mainGameState
            == DataManager_Gameplay.GameState_En.LoadDBFinished);

        dataManager_Cp.GenerateRandomUnitCardsData_PartyDecision();

        //
        gameStates.Add(GameState_En.ExtraInitFinished);
    }

    #endregion

    //------------------------------
    public void PreparePlay()
    {
        StartCoroutine(CorouPreparePlay());
    }

    IEnumerator CorouPreparePlay()
    {
        //
        LocalCardsManager_Cp.InstantAllUnits();
        otherCardsManager_Cp.InstantAllUnits();

        //
        bgd_Cp.curtain_Cp.CurtainUp();
        yield return new WaitUntil(() => bgd_Cp.curtain_Cp.mainGameState
            == CurtainHandler.GameState_En.CurtainUpFinished);

        //
        mainGameState = GameState_En.PreparePlayFinished;

        Play();
    }

    //------------------------------
    public void Play()
    {
        mainGameState = GameState_En.Playing;

        statusManager_Cp.StartAllCounting();

        if (isComExist)
        {
            comCardsManager_Cp.ActAsComputer();
        }
    }

    //////////////////////////////////////////////////////////////////////
    // OnEvent
    //////////////////////////////////////////////////////////////////////
    #region OnEvent

    //------------------------------
    void OnHoldedObject()
    {
        LocalCardsManager_Cp.OnUnitHolded();
    }

    void OnHoldEndedObject()
    {
        LocalCardsManager_Cp.OnUnitUnHolded();
    }

    //------------------------------
    public void OnPartyDecided()
    {
        LocalCardsManager_Cp.PartyDecided();
    }

    #endregion
    //////////////////////////////////////////////////////////////////////

    //------------------------------
    public void CheckPlayersPartyIsReady()
    {
        bool isAllReady_tp = true;
        for (int i = 0; i < cardsManager_Cps.Count; i++)
        {
            if (!cardsManager_Cps[i].isPartyReady)
            {
                isAllReady_tp = false;
                break;
            }
        }

        if (!isAllReady_tp)
        {
            return;
        }
        else
        {
            LoadNextScene();
        }
    }

    //------------------------------
    public void PrepareToFinish()
    {
        StartCoroutine(Corou_PrepareToFinish());
    }

    IEnumerator Corou_PrepareToFinish()
    {
        data_Cp.PrepareToFinish();
        yield return new WaitUntil(() => data_Cp.mainGameState
            == Data_PartyDecision.GameState_En.NextScenePrepareFinished);

        LoadNextScene();
    }

    //------------------------------
    public void LoadNextScene()
    {
        StartCoroutine(CorouLoadNextScene());
    }

    IEnumerator CorouLoadNextScene()
    {
        //
        DontDestroyOnLoad(dataManager_Cp.gameObject);

        //
        bgd_Cp.curtain_Cp.CurtainDown();
        yield return new WaitUntil(() => bgd_Cp.curtain_Cp.mainGameState
            == CurtainHandler.GameState_En.CurtainDownFinished);

        //
        LoadSceneHandler.LoadNextScene();
    }

}
