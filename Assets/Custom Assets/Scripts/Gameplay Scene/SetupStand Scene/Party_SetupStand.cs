using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Party_SetupStand : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    /// Types
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing, WillFinish
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// Fields
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField]
    bool m_isLocalPlayer;

    [SerializeField]
    public ComPartyManager_SetupStand comPartyManager_Cp;

    [SerializeField]
    public Transform cameraLookPoint_Tf;

    [SerializeField]
    GameObject unit_Pf;

    [SerializeField]
    Transform candidateUnitsCenterPoint_Tf;

    [SerializeField]
    Transform standUnitsCenterPoint_Tf;

    [SerializeField]
    int candidateUnitsCountPerRow = 6;

    [SerializeField]
    float candidateUnitsRowInterval = 0.17f, candidateUnitsColumnInterval = 0.11f;

    [SerializeField]
    float standUnitsRowInterval = 0f, standUnitsColumnInterval = 0.1f;

    [SerializeField]
    float candidateUnitsPanelLandValidDist = 0.1f, standUnitsPanelLandValidDist = 0.15f;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    [ReadOnly]
    public int playerID;

    [ReadOnly]
    public bool isCom;

    [ReadOnly]
    public List<Transform> unit_Tfs = new List<Transform>();

    [ReadOnly]
    public List<UnitCard> unit_Cps = new List<UnitCard>();

    [ReadOnly]
    public List<Vector3> candidateUnitsArrangePoints = new List<Vector3>();

    [ReadOnly]
    public List<Vector3> standUnitsArrangePoints = new List<Vector3>();

    //-------------------------------------------------- private fields
    //
    Controller_SetupStand controller_Cp;

    UI_SetupStand uiManager_Cp;

    DataManager_Gameplay dataManager_Cp;

    StatusManager statusManager_Cp;

    HoldingHandler holdingHandler_Cp;

    //
    [SerializeField]
    [ReadOnly]
    bool m_decisionAble;

    [SerializeField]
    [ReadOnly]
    bool m_decided;

    UnitCardsData unitCardsData = new UnitCardsData();

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// Properties
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

    //
    int maxStandUnitsCount
    {
        get { return DataManager_Gameplay.maxStandUnitsCount; }
    }

    public bool decisionAble
    {
        get { return m_decisionAble; }
        set
        {
            m_decisionAble = value;

            if (playerID == controller_Cp.localPlayerID)
            {
                uiManager_Cp.decisionBtnInteract = value;
            }
            else if (playerID == controller_Cp.otherPlayerID)
            {
                statusManager_Cp.opponentReadyState = value;

                if (value)
                {
                    decided = true;
                }
            }
        }
    }

    public bool decided
    {
        get { return m_decided; }
        set
        {
            m_decided = value;

            if (value)
            {
                if (playerID == controller_Cp.localPlayerID)
                {
                    uiManager_Cp.decisionBtnInteract = value;
                }

                controller_Cp.HandlePlayersDecision();
            }
        }
    }

    //-------------------------------------------------- private properties

    #endregion

    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////
    /// Methods
    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////

    //-------------------------------------------------- Start is called before the first frame update
    void Start()
    {

    }

    //-------------------------------------------------- Update is called once per frame
    void Update()
    {

    }

    //////////////////////////////////////////////////////////////////////
    /// Manage gameStates
    //////////////////////////////////////////////////////////////////////
    #region ManageGameStates

    //--------------------------------------------------
    public int GetExistGameStatesCount(GameState_En gameState_pr)
    {
        int stateCount = 0;

        for (int i = 0; i < gameStates.Count; i++)
        {
            if (gameStates[i] == gameState_pr)
            {
                stateCount++;
            }
        }

        return stateCount;
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
    /// Initialize
    //////////////////////////////////////////////////////////////////////
    #region Initialize

    //--------------------------------------------------
    public void Init()
    {
        gameStates.Add(GameState_En.Nothing);

        InitComponents();

        InitVariables();

        InitArrangePoints();

        InitUnits();

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void InitComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_SetupStand>();

        uiManager_Cp = controller_Cp.uiManager_Cp;

        dataManager_Cp = controller_Cp.dataManager_Cp;

        statusManager_Cp = controller_Cp.statusManager_Cp;

        holdingHandler_Cp = controller_Cp.holdingHandler_Cp;
    }

    //--------------------------------------------------
    void InitVariables()
    {
        if (isCom)
        {
            comPartyManager_Cp.Init();
        }

        unitCardsData = dataManager_Cp.psUnitCardsData[playerID];

        decisionAble = false;

        decided = false;
    }

    //--------------------------------------------------
    void InitArrangePoints()
    {
        InitCandidateUnitsArrangePoints();

        InitStandUnitsArrangePoints();
    }

    //--------------------------------------------------
    void InitCandidateUnitsArrangePoints()
    {
        candidateUnitsArrangePoints = ArrangeObjects.GetArrangePoints(candidateUnitsCenterPoint_Tf.position,
            unitCardsData.unitCards.Count, candidateUnitsCountPerRow, candidateUnitsRowInterval,
            candidateUnitsColumnInterval);
    }

    //--------------------------------------------------
    void InitStandUnitsArrangePoints()
    {
        standUnitsArrangePoints = ArrangeObjects.GetArrangePoints(standUnitsCenterPoint_Tf.position,
            maxStandUnitsCount, maxStandUnitsCount, standUnitsRowInterval, standUnitsColumnInterval);
    }

    //--------------------------------------------------
    void InitUnits()
    {
        InstantUnits();

        InitUnitValues();

        ArrangeUnits();
    }

    #endregion

    //--------------------------------------------------
    public void InstantUnits()
    {
        for (int i = 0; i < unitCardsData.unitCards.Count; i++)
        {
            GameObject unit_GO_tp = Instantiate(unit_Pf, candidateUnitsCenterPoint_Tf);
            unit_Tfs.Add(unit_GO_tp.transform);
            unit_Cps.Add(unit_GO_tp.GetComponent<UnitCard>());
        }
    }

    //--------------------------------------------------
    public void InitUnitValues()
    {
        for (int i = 0; i < unit_Cps.Count; i++)
        {
            unit_Cps[i].playerID = playerID;

            unit_Cps[i].posType_SetupStand = UnitCard.UnitPositionType_SetupStand.Candidate;
            unit_Cps[i].posIndex_SetupStand = i;

            unit_Cps[i].enableZoom = false;

            //
            if (!dataManager_Cp.psBUnitCardsData[playerID].unitCards.Contains
                (unitCardsData.unitCards[i]))
            {
                unit_Cps[i].activate = true;
            }
            else
            {
                unit_Cps[i].activate = false;
            }
            
            //
            UnitCardData unitData_tp = unitCardsData.unitCards[i];
            unit_Cps[i].cost = unitData_tp.cost;
            unit_Cps[i].frontSide = unitData_tp.frontSide;
            unit_Cps[i].backSide = unitData_tp.backSide;
        }
    }

    //--------------------------------------------------
    public void ArrangeUnits()
    {
        for (int i = 0; i < unit_Tfs.Count; i++)
        {
            unit_Tfs[i].position = candidateUnitsArrangePoints[i];
        }
    }

    //--------------------------------------------------
    bool IsLandValid(UnitCard willLandUnit_Cp_pr,
        out UnitCard.UnitPositionType_SetupStand posType_pr, out int posIndex_pr)
    {
        bool result = false;
        posType_pr = UnitCard.UnitPositionType_SetupStand.Stand;
        posIndex_pr = 0;

        //
        int nearestValiePointIndex = HoldingHandler.GetNearestValidPointIndex(standUnitsArrangePoints.ToArray(),
            willLandUnit_Cp_pr.transform.position, standUnitsPanelLandValidDist);
        if (nearestValiePointIndex != -1)
        {
            posType_pr = UnitCard.UnitPositionType_SetupStand.Stand;
            posIndex_pr = nearestValiePointIndex;
            UnitCard targetUnit_Cp_tp = GetUnitCardFromPositionInfo(posType_pr, posIndex_pr);

            if (targetUnit_Cp_tp != null)
            {
                if (!targetUnit_Cp_tp.activate)
                {
                    result = false;
                    return result;
                }
            }

            result = true;
            return result;
        }

        //
        nearestValiePointIndex = HoldingHandler.GetNearestValidPointIndex(candidateUnitsArrangePoints.ToArray(),
            willLandUnit_Cp_pr.transform.position, candidateUnitsPanelLandValidDist);
        if (nearestValiePointIndex != -1)
        {
            posType_pr = UnitCard.UnitPositionType_SetupStand.Candidate;
            posIndex_pr = nearestValiePointIndex;
            UnitCard targetUnit_Cp_tp = GetUnitCardFromPositionInfo(posType_pr, posIndex_pr);

            if (targetUnit_Cp_tp != null)
            {
                if (!targetUnit_Cp_tp.activate)
                {
                    result = false;
                    return result;
                }
            }
            
            result = true;
        }

        return result;
    }

    //--------------------------------------------------
    void ExchangeUnitPositionInfo(UnitCard firstUnit_Cp_pr, UnitCard sndUnit_Cp_pr, UnityEvent unityEvent_pr)
    {
        UnitCard.UnitPositionType_SetupStand firstPosType_tp = firstUnit_Cp_pr.posType_SetupStand;
        int firstPosIndex_tp = firstUnit_Cp_pr.posIndex_SetupStand;

        firstUnit_Cp_pr.posType_SetupStand = sndUnit_Cp_pr.posType_SetupStand;
        firstUnit_Cp_pr.posIndex_SetupStand = sndUnit_Cp_pr.posIndex_SetupStand;

        sndUnit_Cp_pr.posType_SetupStand = firstPosType_tp;
        sndUnit_Cp_pr.posIndex_SetupStand = firstPosIndex_tp;

        unityEvent_pr.Invoke();
    }

    //--------------------------------------------------
    void MoveUnit(UnitCard movingUnit_Cp_pr, UnitCard targetUnit_Cp_pr, UnityEvent unityEvent_pr)
    {
        Transform targetUnitParent_Tf_tp = null;
        Vector3 targetInstantPos = GetUnitPosition(targetUnit_Cp_pr, out targetUnitParent_Tf_tp);

        movingUnit_Cp_pr.transform.SetParent(targetUnitParent_Tf_tp);
        movingUnit_Cp_pr.transform.DOKill();
        movingUnit_Cp_pr.transform.DOMove(targetInstantPos, 0.3f)
            .OnComplete(() => unityEvent_pr.Invoke());
        movingUnit_Cp_pr.transform.DORotate(targetUnitParent_Tf_tp.rotation.eulerAngles, 0.3f);
    }

    //--------------------------------------------------
    void MoveUnit(UnitCard movingUnit_Cp_pr, UnitCard.UnitPositionType_SetupStand targetPosType_pr,
        int targetPosIndex_pr, UnityEvent unityEvent_pr)
    {
        Transform targetUnitParent_Tf_tp = null;
        Vector3 targetPos = GetUnitPosition(targetPosType_pr, targetPosIndex_pr, out targetUnitParent_Tf_tp);

        movingUnit_Cp_pr.transform.SetParent(targetUnitParent_Tf_tp);
        movingUnit_Cp_pr.transform.DOKill();
        movingUnit_Cp_pr.transform.DOMove(targetPos, 0.3f)
            .OnComplete(() => unityEvent_pr.Invoke());
        movingUnit_Cp_pr.transform.DORotate(targetUnitParent_Tf_tp.rotation.eulerAngles, 0.3f);

        //
        movingUnit_Cp_pr.posType_SetupStand = targetPosType_pr;
        movingUnit_Cp_pr.posIndex_SetupStand = targetPosIndex_pr;
    }

    //--------------------------------------------------
    Vector3 GetUnitPosition(UnitCard unit_Cp_pr, out Transform unitCenter_Tf_pr)
    {
        Vector3 unitPos = new Vector3();
        unitCenter_Tf_pr = null;

        switch (unit_Cp_pr.posType_SetupStand)
        {
            case UnitCard.UnitPositionType_SetupStand.Stand:
                unitCenter_Tf_pr = standUnitsCenterPoint_Tf;
                unitPos = standUnitsArrangePoints[unit_Cp_pr.posIndex_SetupStand];
                break;
            case UnitCard.UnitPositionType_SetupStand.Candidate:
                unitCenter_Tf_pr = candidateUnitsCenterPoint_Tf;
                unitPos = candidateUnitsArrangePoints[unit_Cp_pr.posIndex_SetupStand];
                break;
        }

        return unitPos;
    }

    Vector3 GetUnitPosition(UnitCard.UnitPositionType_SetupStand posType_pr, int posIndex_pr,
        out Transform unitCenter_Tf_pr)
    {
        Vector3 unitPos = new Vector3();
        unitCenter_Tf_pr = null;

        switch (posType_pr)
        {
            case UnitCard.UnitPositionType_SetupStand.Stand:
                unitCenter_Tf_pr = standUnitsCenterPoint_Tf;
                unitPos = standUnitsArrangePoints[posIndex_pr];
                break;
            case UnitCard.UnitPositionType_SetupStand.Candidate:
                unitCenter_Tf_pr = candidateUnitsCenterPoint_Tf;
                unitPos = candidateUnitsArrangePoints[posIndex_pr];
                break;
        }

        return unitPos;
    }

    //--------------------------------------------------
    UnitCard GetUnitCardFromPositionInfo(UnitCard.UnitPositionType_SetupStand posType_pr, int posIndex_pr)
    {
        UnitCard result = null;

        for (int i = 0; i < unit_Cps.Count; i++)
        {
            if (unit_Cps[i].posType_SetupStand == posType_pr &&
                unit_Cps[i].posIndex_SetupStand == posIndex_pr)
            {
                result = unit_Cps[i];
            }
        }

        return result;
    }

    //--------------------------------------------------
    void HandleDecisionAble()
    {
        decisionAble = IsDecisionAble();
    }

    bool IsDecisionAble()
    {
        bool result = false;

        int standUnitsCount = 0;
        for (int i = 0; i < unit_Cps.Count; i++)
        {
            if (unit_Cps[i].posType_SetupStand == UnitCard.UnitPositionType_SetupStand.Stand)
            {
                standUnitsCount++;
            }
        }

        if (standUnitsCount == DataManager_Gameplay.maxStandUnitsCount)
        {
            result = true;
        }

        return result;
    }

    //////////////////////////////////////////////////////////////////////
    /// On events
    //////////////////////////////////////////////////////////////////////
    #region OnEvents

    //--------------------------------------------------
    public void OnUnitHolded()
    {
        
    }

    public void OnUnitUnHolded()
    {
        //
        if (!isCom)
        {
            controller_Cp.OnPerson_UnitUnHolded();
        }

        //
        Transform holdedUnit_Tf_tp = holdingHandler_Cp.holdedCard_Tf;
        UnitCard holdedUnit_Cp_tp = holdedUnit_Tf_tp.GetComponent<UnitCard>();

        UnitCard.UnitPositionType_SetupStand posType_tp = UnitCard.UnitPositionType_SetupStand.Stand;
        int posIndex_tp = 0;
        bool landValid_tp = IsLandValid(holdedUnit_Cp_tp, out posType_tp, out posIndex_tp);

        UnityEvent unityEvent = new UnityEvent();
        if (landValid_tp)
        {
            UnitCard targetUnit_Cp_tp = GetUnitCardFromPositionInfo(posType_tp, posIndex_tp);
            if (targetUnit_Cp_tp != null && targetUnit_Cp_tp != holdedUnit_Cp_tp)
            {
                ExchangeUnitPositionInfo(holdedUnit_Cp_tp, targetUnit_Cp_tp, unityEvent);

                MoveUnit(holdedUnit_Cp_tp, holdedUnit_Cp_tp, unityEvent);
                MoveUnit(targetUnit_Cp_tp, targetUnit_Cp_tp, unityEvent);
            }
            else
            {
                MoveUnit(holdedUnit_Cp_tp, posType_tp, posIndex_tp, unityEvent);
            }
        }
        else
        {
            MoveUnit(holdedUnit_Cp_tp, holdedUnit_Cp_tp, unityEvent);
        }

        //
        HandleDecisionAble();
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// Actions of computer party manager
    //////////////////////////////////////////////////////////////////////
    #region ComPlayerActions

    public void ExchangeUnits()
    {
        int willLandUnitIndex_tp = 0;
        int landTargetUnitIndex_tp = 0;
        bool landAble_tp = false;

        //
        for (int i = 0; i < unit_Cps.Count; i++)
        {
            if (unit_Cps[i].activate)
            {
                if (unit_Cps[i].posType_SetupStand == UnitCard.UnitPositionType_SetupStand.Candidate)
                {
                    willLandUnitIndex_tp = i;
                    landAble_tp = true;
                    break;
                }
            }
        }

        if (!landAble_tp)
        {
            return;
        }

        // 
        landAble_tp = false;
        for (int i = 0; i < standUnitsArrangePoints.Count; i++)
        {
            bool isEmptyStandPoint_tp = true;
            for (int j = 0; j < unit_Cps.Count; j++)
            {
                if (unit_Cps[j].activate)
                {
                    if (unit_Cps[j].posType_SetupStand == UnitCard.UnitPositionType_SetupStand.Stand
                        && unit_Cps[j].posIndex_SetupStand == i)
                    {
                        isEmptyStandPoint_tp = false;
                        break;
                    }
                }
            }

            if (isEmptyStandPoint_tp)
            {
                landTargetUnitIndex_tp = i;
                landAble_tp = true;
                break;
            }
        }

        if (!landAble_tp)
        {
            return;
        }

        //
        UnitCard willLandUnit_Cp_tp = unit_Cps[willLandUnitIndex_tp];
        UnitCard.UnitPositionType_SetupStand posType_tp = UnitCard.UnitPositionType_SetupStand.Stand;
        int posIndex_tp = landTargetUnitIndex_tp;
        UnityEvent unityEvent = new UnityEvent();

        MoveUnit(willLandUnit_Cp_tp, posType_tp, posIndex_tp, unityEvent);

        //
        HandleDecisionAble();
    }

    #endregion

}
