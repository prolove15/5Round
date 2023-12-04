using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Cards_StartPrepare : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    // Types
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing, Finished,
        InitUnitCardsFinished, InstantUnitCardsFinished, ArrangeUnitCardsFinished,
        TakingUnitCardsStarted, TakingUnitCardsFinished,
        OnMovingTakedUnitCardsFinished,
        ReturnToSelectionStarted, ReturnToSelectionFinished
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    // Fields
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField]
    bool m_isLocalPlayer;

    [SerializeField]
    bool m_isCom;

    [SerializeField]
    Transform unitCardsInstantPoint_Tf;

    [SerializeField]
    Transform unitCardsCenterPoint_Tf;

    [SerializeField]
    Transform unitCardLookPoint_Tf;

    [SerializeField]
    GameObject unitCard_Pf;

    [SerializeField]
    int maxHledCardsNum = 3;

    [SerializeField]
    int perRowUnitCardsNum = 4;

    [SerializeField]
    float rowInterval = 0.17f, columnInterval = 0.11f;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    [ReadOnly]
    public int playerID;

    [ReadOnly]
    public List<UnitCard> unit_Cps = new List<UnitCard>();

    [ReadOnly]
    public List<UnitCard> hledUnitCards_Cps = new List<UnitCard>();

    //-------------------------------------------------- private fields
    Controller_StartPrepare controller_Cp;

    UI_StartPrepare uiManager_Cp;

    Data_StartPrepare data_Cp;

    StatusManager statusManager_Cp;

    TakedCards_StartPrepare takedCardManager_Cp;

    Transform camera_Tf;

    int takedUnitCardsCount;

    [SerializeField]
    [ReadOnly]
    bool m_isPartyReady;

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

    public bool isLocalPlayer
    {
        get { return m_isLocalPlayer; }
    }

    public bool isCom
    {
        get { return m_isCom; }
    }

    public bool isPartyReady
    {
        get { return m_isPartyReady; }
        set
        {
            m_isPartyReady = value;

            if (isCom)
            {
                statusManager_Cp.opponentReadyState = value;
            }

            controller_Cp.CheckPlayersPartyIsReady();
        }
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
        gameStates.Add(GameState_En.Nothing);

        InitComponents();

        InitVariables();

        InitUI();

        if (isLocalPlayer)
        {
            InitCameraPosition();
        }

        mainGameState = GameState_En.Inited;
    }

    //------------------------------
    void InitComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_StartPrepare>();

        uiManager_Cp = controller_Cp.uiManager_Cp;

        if (isLocalPlayer)
        {
            camera_Tf = controller_Cp.components.camera_Tf;
        }

        data_Cp = controller_Cp.data_Cp;

        statusManager_Cp = controller_Cp.statusManager_Cp;

        takedCardManager_Cp = controller_Cp.takedCardsManager_Cps[playerID];
    }

    //------------------------------
    void InitVariables()
    {
        isPartyReady = false;

        takedCardManager_Cp.playerID = playerID;
    }

    //------------------------------
    void InitUI()
    {
        uiManager_Cp.SetActiveDecisionBtn(false);
        uiManager_Cp.returnToSelectionBtn_RT.gameObject.SetActive(false);
    }

    //------------------------------
    void InitCameraPosition()
    {
        camera_Tf.SetPositionAndRotation(unitCardLookPoint_Tf.position, unitCardLookPoint_Tf.rotation);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    // ManageGameState
    //////////////////////////////////////////////////////////////////////

    #region ManageGameState

    //------------------------------
    int ExistGameStatesNum(GameState_En gameState_pr)
    {
        int stateNum = 0;

        for(int i = 0; i < gameStates.Count; i++)
        {
            if (gameStates[i] == gameState_pr)
            {
                stateNum++;
            }
        }

        return stateNum;
    }

    void RemoveGameStates(GameState_En gameState_pr)
    {
        while (gameStates.Contains(gameState_pr))
        {
            gameStates.Remove(gameState_pr);
        }
    }

    #endregion

    //------------------------------
    public void InitUnitCards()
    {
        StartCoroutine(CorouInitUnitCards());
    }

    IEnumerator CorouInitUnitCards()
    {
        //
        InstantUnitCards();
        yield return new WaitUntil(() => mainGameState == GameState_En.InstantUnitCardsFinished);

        SetUnitCardsValues();

        InitUnitCardsValues();

        //
        mainGameState = GameState_En.InitUnitCardsFinished;
    }

    //------------------------------
    public void InstantUnitCards()
    {
        int unitCardsNum = data_Cp.maxPartyUnitsCount;
        List<GameObject> unit_GOs_tp = new List<GameObject>();

        for(int i = 0; i < unitCardsNum; i++)
        {
            unit_GOs_tp.Add(Instantiate(unitCard_Pf, unitCardsInstantPoint_Tf));
        }

        for(int i = 0; i < unit_GOs_tp.Count; i++)
        {
            unit_Cps.Add(unit_GOs_tp[i].GetComponent<UnitCard>());
        }

        mainGameState = GameState_En.InstantUnitCardsFinished;
    }

    //------------------------------
    public void SetUnitCardsValues()
    {
        UnitCardsData unitCardsData_tp = data_Cp.playersUnitCardsData[playerID];

        for(int i = 0; i < unitCardsData_tp.unitCards.Count; i++)
        {
            unit_Cps[i].playerID = playerID;

            //
            unit_Cps[i].cardIndex = unitCardsData_tp.unitCards[i].id;
            unit_Cps[i].cost = unitCardsData_tp.unitCards[i].cost;
            unit_Cps[i].frontSide = unitCardsData_tp.unitCards[i].frontSide;
            unit_Cps[i].backSide = unitCardsData_tp.unitCards[i].backSide;

            //
            unit_Cps[i].enableLongPress = true;
            unit_Cps[i].enableClickDetect = true;
        }
    }

    //------------------------------
    void InitUnitCardsValues()
    {
        for (int i = 0; i < unit_Cps.Count; i++)
        {
            unit_Cps[i].Init();
        }
    }

    //------------------------------
    public void ArrangeUnitCards()
    {
        StartCoroutine(CorouArrangeUnitCards());
    }

    IEnumerator CorouArrangeUnitCards()
    {
        List<GameObject> unit_GOs_tp = new List<GameObject>();
        int unitCardsNum = unit_GOs_tp.Count;

        for (int i = 0; i < unit_Cps.Count; i++)
        {
            unit_GOs_tp.Add(unit_Cps[i].gameObject);
        }

        //
        for (int i = 0; i < unitCardsNum; i++)
        {
            unit_GOs_tp[i].transform.SetParent(unitCardsCenterPoint_Tf, true);
        }

        //
        List<Vector3> arrangePoints = ArrangeObjects.GetArrangePoints(unitCardsCenterPoint_Tf.position,
            unit_GOs_tp.Count, perRowUnitCardsNum, rowInterval, columnInterval);
        
        for(int i = 0; i < unit_GOs_tp.Count; i++)
        {
            UnityEvent unityEvent = new UnityEvent();
            unityEvent.AddListener(OnCompleteUnitCardTranslate);

            TargetTweening.TranslateGameObject(unit_GOs_tp[i].transform, unit_GOs_tp[i].transform.position,
                arrangePoints[i], unit_GOs_tp[i].transform.rotation, unit_GOs_tp[i].transform.rotation,
                unityEvent);
        }

        //
        yield return null;
    }

    void OnCompleteUnitCardTranslate()
    {
        mainGameState = GameState_En.ArrangeUnitCardsFinished;
    }

    //////////////////////////////////////////////////////////////////////
    // Handle UnitCards
    //////////////////////////////////////////////////////////////////////
    
    #region HandleUnitCards

    //------------------------------
    void HandleUnitCardHighlight(UnitCard unitCard_Cp_pr, bool flag)
    {
        //
        if (flag)
        {
            hledUnitCards_Cps.Add(unitCard_Cp_pr);

            if (hledUnitCards_Cps.Count == maxHledCardsNum)
            {
                for (int i = 0; i < unit_Cps.Count; i++)
                {
                    if (!hledUnitCards_Cps.Contains(unit_Cps[i]))
                    {
                        unit_Cps[i].activate = false;
                    }
                }
            }
        }
        else
        {
            hledUnitCards_Cps.Remove(unitCard_Cp_pr);

            if(hledUnitCards_Cps.Count < maxHledCardsNum)
            {
                for (int i = 0; i < unit_Cps.Count; i++)
                {
                    if (!hledUnitCards_Cps.Contains(unit_Cps[i]))
                    {
                        unit_Cps[i].activate = true;
                    }
                }
            }
        }
    }

    //------------------------------
    void CheckDecisionBtnByHledUnits()
    {
        //
        if (isLocalPlayer)
        {
            if (hledUnitCards_Cps.Count == maxHledCardsNum)
            {
                uiManager_Cp.SetActiveDecisionBtn(true);
            }
            else
            {
                uiManager_Cp.SetActiveDecisionBtn(false);
            }
        }
    }

    //------------------------------
    void HandleTakeAndExchangeUnitCards(List<UnitCard> unit_Cps_pr)
    {
        StartCoroutine(CorouHandleTakeAndExchangeUnitCards(unit_Cps_pr));
    }

    IEnumerator CorouHandleTakeAndExchangeUnitCards(List<UnitCard> unit_Cps_pr)
    {
        //
        gameStates.Add(GameState_En.TakingUnitCardsStarted);

        //
        for (int i = unit_Cps_pr.Count - 1; i >= 0; i--)
        {
            unit_Cps.Remove(unit_Cps_pr[i]);
        }

        //
        for (int i = 0; i < unit_Cps_pr.Count; i++)
        {
            Transform unitCard_Tf_tp = unit_Cps_pr[i].transform;

            UnityEvent unityEvent = new UnityEvent();
            unityEvent.AddListener(OnMovingTakedUnitCardsFinished);

            TargetTweening.TranslateGameObject(unitCard_Tf_tp, unitCard_Tf_tp.position,
                takedCardManager_Cp.arrangePoints[takedUnitCardsCount], unitCard_Tf_tp.rotation,
                Quaternion.identity, unityEvent);

            //
            takedUnitCardsCount++;
        }
        yield return new WaitUntil(() => ExistGameStatesNum(GameState_En.OnMovingTakedUnitCardsFinished)
            == unit_Cps_pr.Count);
        RemoveGameStates(GameState_En.OnMovingTakedUnitCardsFinished);

        //
        takedCardManager_Cp.SetTakedUnitCards(unit_Cps_pr);

        // Check the party is ready
        if (takedUnitCardsCount == data_Cp.maxPartyUnitsCount)
        {
            isPartyReady = true;
            yield break;
        }

        //
        for (int i = 0; i < unit_Cps.Count; i++)
        {
            unit_Cps[i].activate = true;
        }

        // exchange between player1 and player2
        data_Cp.ExchangeUnitsRequest(playerID, unit_Cps);

        yield return new WaitUntil(() => data_Cp.ExistGameStatesNum(
            Data_StartPrepare.GameState_En.ExchUnitsReqProc) == 0);

        //
        unit_Cps_pr.Clear();

        //
        SetUnitCardsValues();

        //
        ArrangeUnitCards();

        //
        gameStates.Add(GameState_En.TakingUnitCardsFinished);
        gameStates.Remove(GameState_En.TakingUnitCardsStarted);
    }

    //------------------------------
    public void OnMovingTakedUnitCardsFinished()
    {
        gameStates.Add(GameState_En.OnMovingTakedUnitCardsFinished);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    // OnEvent
    //////////////////////////////////////////////////////////////////////
    #region OnEvent

    //------------------------------
    public void OnUnitSelected(UnitCard unitCard_Cp_pr)
    {
        HandleUnitCardHighlight(unitCard_Cp_pr, true);

        CheckDecisionBtnByHledUnits();
    }

    public void OnUnitUnSelected(UnitCard unitCard_Cp_pr)
    {
        HandleUnitCardHighlight(unitCard_Cp_pr, false);

        CheckDecisionBtnByHledUnits();
    }

    //------------------------------
    public void OnDecision()
    {
        uiManager_Cp.SetActiveDecisionBtn(false);

        HandleTakeAndExchangeUnitCards(hledUnitCards_Cps);
    }

    //------------------------------
    public void OnReturnToSelection()
    {
        //
        gameStates.Add(GameState_En.ReturnToSelectionStarted);

        //
        uiManager_Cp.returnToSelectionBtn_RT.gameObject.SetActive(false);

        //
        UnityEvent unityEvent = new UnityEvent();
        unityEvent.AddListener(OnCompletedReturnToSelection);

        if (isLocalPlayer)
        {
            TargetTweening.TranslateGameObject(camera_Tf, camera_Tf.position, unitCardLookPoint_Tf.position,
                camera_Tf.rotation, unitCardLookPoint_Tf.rotation, unityEvent);
        }
    }

    void OnCompletedReturnToSelection()
    {
        //
        uiManager_Cp.lookHandBtn_RT.gameObject.SetActive(true);
        uiManager_Cp.SetActiveDecisionBtn(true, "CameraMoving");

        //
        gameStates.Add(GameState_En.ReturnToSelectionFinished);
        RemoveGameStates(GameState_En.ReturnToSelectionStarted);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Actios on computer player
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region ActionsOnComPlayer

    //--------------------------------------------------
    public void OnPerson_ClickDecisionBtn()
    {
        if (!isCom)
        {
            return;
        }
        
        //
        while (hledUnitCards_Cps.Count < maxHledCardsNum)
        {
            int randIndex = Random.Range(0, unit_Cps.Count);
            if (!hledUnitCards_Cps.Contains(unit_Cps[randIndex]))
            {
                unit_Cps[randIndex].OnClicked();
            }
        }
        
        //
        HandleTakeAndExchangeUnitCards(hledUnitCards_Cps);
    }

    #endregion
}
