using cakeslice;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller_StartPrepare : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    // Types
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing,
        Inited, InitComponentFinished, PreparePlayFinished,
        Playing, Finished,
        ZoomInOut
    }

    [Serializable]
    public class Components
    {
        [Tooltip("Background component for the scene.")]
        public Background_StartPrepare bgd_Cp;

        [Tooltip("UI Manager component for handling start preparation.")]
        public UI_StartPrepare uiManager_Cp;

        public Transform camera_Tf;

        [Tooltip("DataManager")]
        public DataManager_Gameplay dataManager_Cp;

        [Tooltip("Data for StartPrepare scene")]
        public Data_StartPrepare data_Cp;

        public StatusManager statusManager_Cp;

        public OutlineEffect hlManager_Cp;
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
    public bool isComExist;

    [ReadOnly]
    public List<Cards_StartPrepare> cardsManager_Cps;

    [ReadOnly]
    public List<TakedCards_StartPrepare> takedCardsManager_Cps;

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

    public Background_StartPrepare bgd_Cp
    {
        get { return components.bgd_Cp; }
    }

    public UI_StartPrepare uiManager_Cp
    {
        get { return components.uiManager_Cp; }
    }

    public Transform camera_Tf
    {
        get { return components.camera_Tf; }
    }

    public DataManager_Gameplay dataManager_Cp
    {
        get { return components.dataManager_Cp; }
    }

    public Data_StartPrepare data_Cp
    {
        get { return components.data_Cp; }
    }

    public StatusManager statusManager_Cp
    {
        get { return components.statusManager_Cp; }
    }

    public Cards_StartPrepare localCardsManager_Cp
    {
        get { return cardsManager_Cps[localPlayerID]; }
    }

    public TakedCards_StartPrepare localTakedCardsManager_Cp
    {
        get { return takedCardsManager_Cps[localPlayerID]; }
    }

    public Cards_StartPrepare otherCardsManager_Cp
    {
        get { return cardsManager_Cps[otherPlayerID]; }
    }

    public TakedCards_StartPrepare otherTakedCardsManager_Cp
    {
        get { return takedCardsManager_Cps[otherPlayerID]; }
    }

    public Cards_StartPrepare personCardsManager_Cp
    {
        get { return cardsManager_Cps[personPlayerID]; }
    }

    public TakedCards_StartPrepare personTakedCardsManager_Cp
    {
        get { return takedCardsManager_Cps[personPlayerID]; }
    }

    public Cards_StartPrepare comCardsManager_Cp
    {
        get { return cardsManager_Cps[comPlayerID]; }
    }

    public TakedCards_StartPrepare comTakedCardsManager_Cp
    {
        get { return takedCardsManager_Cps[comPlayerID]; }
    }

    OutlineEffect hlManager_Cp
    {
        get { return components.hlManager_Cp; }
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

        //
        SetComponents();

        InitComponents();
        yield return new WaitUntil(() => mainGameState == GameState_En.InitComponentFinished);

        InitVariables();

        dataManager_Cp.LoadGameplayData();
        yield return new WaitUntil(() => dataManager_Cp.mainGameState ==
            DataManager_Gameplay.GameState_En.LoadDBFinished);

        data_Cp.LoadUnitCardsData();
        yield return new WaitUntil(() => data_Cp.mainGameState ==
            Data_StartPrepare.GameState_En.LoadUnitCardsDataFinished);

        for (int i = 0; i < cardsManager_Cps.Count; i++)
        {
            cardsManager_Cps[i].InitUnitCards();
            yield return new WaitUntil(() => cardsManager_Cps[i].mainGameState ==
                Cards_StartPrepare.GameState_En.InitUnitCardsFinished);
        }

        for (int i = 0; i < takedCardsManager_Cps.Count; i++)
        {
            takedCardsManager_Cps[i].InitTakedUnitCards();
            yield return new WaitUntil(() => takedCardsManager_Cps[i].mainGameState
                == TakedCards_StartPrepare.GameState_En.InitTakedUnitCardsFinished);
        }

        //
        mainGameState = GameState_En.Inited;

        PreparePlay();
    }

    //------------------------------
    void SetComponents()
    {
        SetCardsComponents();
    }

    //------------------------------
    void SetCardsComponents()
    {
        Cards_StartPrepare[] cardsManager_Cps_tp = FindObjectsOfType<Cards_StartPrepare>();

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

        //
        for (int i = 0; i < cardsManager_Cps.Count; i++)
        {
            takedCardsManager_Cps.Add(cardsManager_Cps[i].GetComponent<TakedCards_StartPrepare>());
        }
    }

    //------------------------------
    void InitComponents()
    {
        StartCoroutine(CorouInitComponents());
    }

    IEnumerator CorouInitComponents()
    {
        bgd_Cp.Init();
        yield return new WaitUntil(() => bgd_Cp.mainGameState == Background_StartPrepare.GameState_En.Inited);

        dataManager_Cp.Init();
        yield return new WaitUntil(() => dataManager_Cp.mainGameState == DataManager_Gameplay.GameState_En.Inited);

        data_Cp.Init();
        yield return new WaitUntil(() => data_Cp.mainGameState == Data_StartPrepare.GameState_En.Inited);

        uiManager_Cp.Init();
        yield return new WaitUntil(() => uiManager_Cp.mainGameState == UI_StartPrepare.GameState_En.Inited);

        statusManager_Cp.Init();

        for (int i = 0; i < cardsManager_Cps.Count; i++)
        {
            cardsManager_Cps[i].Init();
            yield return new WaitUntil(() => cardsManager_Cps[i].mainGameState
                == Cards_StartPrepare.GameState_En.Inited);
        }

        for (int i = 0; i < takedCardsManager_Cps.Count; i++)
        {
            takedCardsManager_Cps[i].Init();
            yield return new WaitUntil(() => takedCardsManager_Cps[i].mainGameState
                == TakedCards_StartPrepare.GameState_En.Inited);
        }

        mainGameState = GameState_En.InitComponentFinished;
    }

    //------------------------------
    void InitVariables()
    {
        
    }

    #endregion

    //------------------------------
    public void PreparePlay()
    {
        StartCoroutine(CorouPreparePlay());
    }

    IEnumerator CorouPreparePlay()
    {
        for (int i = 0; i < cardsManager_Cps.Count; i++)
        {
            cardsManager_Cps[i].ArrangeUnitCards();
            yield return new WaitUntil(() => cardsManager_Cps[i].mainGameState ==
                Cards_StartPrepare.GameState_En.ArrangeUnitCardsFinished);
        }

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
    }

    //------------------------------
    public void ZoomInOutStarted()
    {
        uiManager_Cp.OnZoomInOutStarted();

        hlManager_Cp.lineColor0 = new Color(hlManager_Cp.lineColor0.r, hlManager_Cp.lineColor0.g,
            hlManager_Cp.lineColor0.b, 0f);
    }

    public void ZoomInOutFinished()
    {
        uiManager_Cp.OnZoomInOutFinished();

        hlManager_Cp.lineColor0 = new Color(hlManager_Cp.lineColor0.r, hlManager_Cp.lineColor0.g,
            hlManager_Cp.lineColor0.b, 1f);
    }

    //------------------------------
    void OnUnitCardClicked(UnitCard unit_Cp_pr)
    {
        bool isHighlighted_tp = unit_Cp_pr.isHighlighted;

        if (isHighlighted_tp)
        {
            cardsManager_Cps[unit_Cp_pr.playerID].OnUnitSelected(unit_Cp_pr);
        }
        else
        {
            cardsManager_Cps[unit_Cp_pr.playerID].OnUnitUnSelected(unit_Cp_pr);
        }
    }

    //------------------------------
    public void OnClickDecisionBtn()
    {
        localCardsManager_Cp.OnDecision();

        if (isComExist)
        {
            comCardsManager_Cp.OnPerson_ClickDecisionBtn();
        }
    }

    //------------------------------
    public void CheckPlayersPartyIsReady()
    {
        bool allIsReady_tp = true;
        for (int i = 0; i < cardsManager_Cps.Count; i++)
        {
            if (!cardsManager_Cps[i].isPartyReady)
            {
                allIsReady_tp = false;
                break;
            }
        }

        if (!allIsReady_tp)
        {
            return;
        }
        else
        {
            PrepareToFinish();
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
        yield return new WaitUntil(() => data_Cp.mainGameState == Data_StartPrepare.GameState_En.FinishPrepared);

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
        //DontDestroyOnLoad(dataManager_Cp.gameObject);

        //
        bgd_Cp.curtain_Cp.CurtainDown();
        yield return new WaitUntil(() => bgd_Cp.curtain_Cp.mainGameState
            == CurtainHandler.GameState_En.CurtainDownFinished);

        //
        LoadSceneHandler.LoadNextScene();
    }

}
