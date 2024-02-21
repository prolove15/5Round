using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Cards_PartyDecision : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    /// Types
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing, Finished,
        UnitAutoMoving
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// Fields
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField]
    public bool isLocalPlayer;

    [SerializeField]
    public bool isCom;

    [SerializeField]
    GameObject unit_Pf;

    [SerializeField]
    Transform vanUnitsInstantCenterPoint_Tf;

    [SerializeField]
    Transform rearUnitsInstantCenterPoint_Tf;

    [SerializeField]
    Transform candidateUnitsInstantCenterPoint_Tf;

    [SerializeField]
    public Transform unitsLookPoint_Tf;

    [SerializeField]
    float candidateUnitsRowInterval = 0.17f, candidateUnitsColumnInterval = 0.11f,
        vanUnitsRowInterval = 0f, vanUnitsColumnInterval = 0.2f,
        rearUnitsRowInterval = 0f, rearUnitsColumnInterval = 0.2f;

    [SerializeField]
    public static int candidateUnitsPerRow = 6, vanUnitsPerRow = 2, rearUnitsPerRow = 3;

    [SerializeField]
    float landValidDist = 0.05f;

    [SerializeField]
    int maxBattleCost = 14;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    [ReadOnly]
    public int playerID;

    [ReadOnly]
    public UnitCardsData unitCardsData = new UnitCardsData();

    //-------------------------------------------------- private fields
    Controller_PartyDecision controller_Cp;

    UI_PartyDecision uiManager_Cp;

    Data_PartyDecision data_Cp;

    StatusManager statusManager_Cp;

    HoldingHandler holdingHandler_Cp;

    Transform holdedUnit_Tf;

    //
    List<UnitCard> partyUnit_Cps = new List<UnitCard>();

    List<Vector3> candidateUnitPoints = new List<Vector3>();

    List<Vector3> vanUnitPoints = new List<Vector3>();

    List<Vector3> rearUnitPoints = new List<Vector3>();

    //
    [SerializeField]
    [ReadOnly]
    bool m_isPartyReady;

    int m_battleCost;

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
    UnitCard holdedUnit_Cp
    {
        get { return holdedUnit_Tf.GetComponent<UnitCard>(); }
    }

    int battleCost
    {
        get { return m_battleCost; }
        set
        {
            m_battleCost = value;

            int uiBattleCost_tp = Mathf.Clamp(value, 0, int.MaxValue);
            uiManager_Cp.battleCost = uiBattleCost_tp.ToString();
        }
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////
    /// Methods
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
    /// ManageGameState
    //////////////////////////////////////////////////////////////////////
    #region ManageGameState

    //------------------------------
    public int GetExistGameStatesNum(GameState_En gameState_pr)
    {
        int stateNum = 0;

        for (int i = 0; i < gameStates.Count; i++)
        {
            if (gameStates[i] == gameState_pr)
            {
                stateNum++;
            }
        }

        return stateNum;
    }

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

    //------------------------------
    public void Init()
    {
        //
        gameStates.Add(GameState_En.Nothing);

        //
        InitComponents();

        InitVariables();

        //
        CheckBattleStandDecisionValid();

        //
        mainGameState = GameState_En.Inited;
    }

    //------------------------------
    public void InitComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_PartyDecision>();

        uiManager_Cp = controller_Cp.uiManager_Cp;

        data_Cp = controller_Cp.data_Cp;

        statusManager_Cp = controller_Cp.statusManager_Cp;

        holdingHandler_Cp = controller_Cp.holdingHandler_Cp;
    }

    //------------------------------
    public void InitVariables()
    {
        //
        unitCardsData = data_Cp.playersUnitCardsData[playerID];

        //
        candidateUnitPoints = ArrangeObjects.GetArrangePoints(candidateUnitsInstantCenterPoint_Tf.position,
            unitCardsData.unitCards.Count, candidateUnitsPerRow, candidateUnitsRowInterval,
            candidateUnitsColumnInterval);

        vanUnitPoints = ArrangeObjects.GetArrangePoints(vanUnitsInstantCenterPoint_Tf.position,
            DataManager_Gameplay.maxVanUnitsCount, vanUnitsPerRow, vanUnitsRowInterval, vanUnitsColumnInterval);

        rearUnitPoints = ArrangeObjects.GetArrangePoints(rearUnitsInstantCenterPoint_Tf.position,
            DataManager_Gameplay.maxRearUnitsCount, rearUnitsPerRow, rearUnitsRowInterval, rearUnitsColumnInterval);

        //
        isPartyReady = false;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// Instant units
    //////////////////////////////////////////////////////////////////////
    #region Instant Units

    //------------------------------
    public void InstantAllUnits()
    {
        InstantCandidateUnits();

        InstantEmptyVanUnits();

        InstantEmptyRearUnits();
    }

    //------------------------------
    public void InstantCandidateUnits()
    {
        for (int i = 0; i < unitCardsData.unitCards.Count; i++)
        {
            GameObject candidateUnit_GO_tp = Instantiate(unit_Pf, candidateUnitsInstantCenterPoint_Tf);
            candidateUnit_GO_tp.transform.position = candidateUnitPoints[i];

            UnitCard candidateUnit_Cp_tp = candidateUnit_GO_tp.GetComponent<UnitCard>();
            partyUnit_Cps.Add(candidateUnit_Cp_tp);
            SetUnitCardFromUnitData(candidateUnit_Cp_tp, unitCardsData.unitCards[i]);

            candidateUnit_Cp_tp.posType_PartyDecision = UnitCard.UnitPositionType_PartyDecision.Candidate;
            candidateUnit_Cp_tp.posIndex_PartyDecision = i;
            candidateUnit_Cp_tp.enableZoom = false;
            candidateUnit_Cp_tp.enableClickDetect = false;
        }
    }

    //------------------------------
    public void InstantEmptyVanUnits()
    {
        for (int i = 0; i < vanUnitPoints.Count; i++)
        {
            GameObject vanUnit_GO_tp = Instantiate(unit_Pf, vanUnitsInstantCenterPoint_Tf);
            vanUnit_GO_tp.transform.position = vanUnitPoints[i];

            UnitCard vanUnit_Cp_tp = vanUnit_GO_tp.GetComponent<UnitCard>();
            partyUnit_Cps.Add(vanUnit_Cp_tp);
            vanUnit_Cp_tp.posType_PartyDecision = UnitCard.UnitPositionType_PartyDecision.Van;
            vanUnit_Cp_tp.posIndex_PartyDecision = i;
            vanUnit_Cp_tp.enableZoom = false;
            vanUnit_Cp_tp.SetAsEmptyState();
        }
    }

    //------------------------------
    public void InstantEmptyRearUnits()
    {
        for (int i = 0; i < rearUnitPoints.Count; i++)
        {
            GameObject rearUnit_GO_tp = Instantiate(unit_Pf, rearUnitsInstantCenterPoint_Tf);
            rearUnit_GO_tp.transform.position = rearUnitPoints[i];

            UnitCard rearUnit_Cp_tp = rearUnit_GO_tp.GetComponent<UnitCard>();
            partyUnit_Cps.Add(rearUnit_Cp_tp);
            rearUnit_Cp_tp.posType_PartyDecision = UnitCard.UnitPositionType_PartyDecision.Rear;
            rearUnit_Cp_tp.posIndex_PartyDecision = i;
            rearUnit_Cp_tp.enableZoom = false;
            rearUnit_Cp_tp.SetAsEmptyState();
        }
    }

    #endregion

    //------------------------------
    public List<UnitCard> GetVanUnits()
    {
        List<UnitCard> result = new List<UnitCard>();

        for (int i = 0; i<partyUnit_Cps.Count; i++)
        {
            if (partyUnit_Cps[i].posType_PartyDecision == UnitCard.UnitPositionType_PartyDecision.Van)
            {
                result.Add(partyUnit_Cps[i]);
            }
        }

        return result;
    }

    public List<UnitCard> GetRearUnits()
    {
        List<UnitCard> result = new List<UnitCard>();

        for (int i = 0; i < partyUnit_Cps.Count; i++)
        {
            if (partyUnit_Cps[i].posType_PartyDecision == UnitCard.UnitPositionType_PartyDecision.Rear)
            {
                result.Add(partyUnit_Cps[i]);
            }
        }

        return result;
    }

    //------------------------------
    void MoveUnit(UnitCard movingUnit_Cp_pr, UnitCard targetUnit_Cp_pr, UnityEvent unityEvent_pr)
    {
        Transform targetUnitsCenter_Tf_tp = null;
        Vector3 replacerInstantPos = GetUnitPosition(targetUnit_Cp_pr, out targetUnitsCenter_Tf_tp);

        movingUnit_Cp_pr.transform.SetParent(targetUnitsCenter_Tf_tp, true);
        movingUnit_Cp_pr.transform.DOMove(replacerInstantPos, 0.3f)
            .OnComplete(() => unityEvent_pr.Invoke());
        movingUnit_Cp_pr.transform.DORotate(targetUnitsCenter_Tf_tp.rotation.eulerAngles, 0.3f);
    }

    //------------------------------
    public UnitCard GetLandTargetUnit(List<UnitCard> partyUnit_Cps_pr, UnitCard willLandUnit_Cp_pr,
        float landValidDist_tp)
    {
        UnitCard landTargetUnit_Cp_tp = null;

        for (int i = 0; i < partyUnit_Cps_pr.Count; i++)
        {
            if (IsClosed(partyUnit_Cps_pr[i].transform.position, willLandUnit_Cp_pr.transform.position,
                landValidDist_tp))
            {
                if (partyUnit_Cps_pr[i] != willLandUnit_Cp_pr)
                {
                    landTargetUnit_Cp_tp = partyUnit_Cps_pr[i];
                    break;
                }
            }
        }

        return landTargetUnit_Cp_tp;
    }

    bool IsClosed(Vector3 firstPos_pr, Vector3 sndPos_pr, float criteriaDist)
    {
        return Vector3.Distance(firstPos_pr, sndPos_pr) <= criteriaDist;
    }

    //------------------------------
    Vector3 GetUnitPosition(UnitCard unit_Cp_pr, out Transform unitCenter_Tf_pr)
    {
        Vector3 unitPos = new Vector3();
        unitCenter_Tf_pr = null;

        switch (unit_Cp_pr.posType_PartyDecision)
        {
            case UnitCard.UnitPositionType_PartyDecision.Candidate:
                unitCenter_Tf_pr = candidateUnitsInstantCenterPoint_Tf;
                unitPos = candidateUnitPoints[unit_Cp_pr.posIndex_PartyDecision];
                break;
            case UnitCard.UnitPositionType_PartyDecision.Van:
                unitCenter_Tf_pr = vanUnitsInstantCenterPoint_Tf;
                unitPos = vanUnitPoints[unit_Cp_pr.posIndex_PartyDecision];
                break;
            case UnitCard.UnitPositionType_PartyDecision.Rear:
                unitCenter_Tf_pr = rearUnitsInstantCenterPoint_Tf;
                unitPos = rearUnitPoints[unit_Cp_pr.posIndex_PartyDecision];
                break;
        }

        return unitPos;
    }

    //------------------------------
    void ExchangeUnitPositionInfo(UnitCard firstUnit_Cp_pr, UnitCard sndUnit_Cp_pr, UnityEvent unityEvent_pr)
    {
        UnitCard.UnitPositionType_PartyDecision firstPosType_tp = firstUnit_Cp_pr.posType_PartyDecision;
        int sndPosIndex_tp = firstUnit_Cp_pr.posIndex_PartyDecision;

        firstUnit_Cp_pr.posType_PartyDecision = sndUnit_Cp_pr.posType_PartyDecision;
        firstUnit_Cp_pr.posIndex_PartyDecision = sndUnit_Cp_pr.posIndex_PartyDecision;

        sndUnit_Cp_pr.posType_PartyDecision = firstPosType_tp;
        sndUnit_Cp_pr.posIndex_PartyDecision = sndPosIndex_tp;
    }

    //------------------------------
    void SetUnitCardFromUnitData(UnitCard unit_Cp_pr, UnitCardData unitData_pr)
    {
        unit_Cp_pr.cardIndex = unitData_pr.id;
        unit_Cp_pr.cost = unitData_pr.cost;
        unit_Cp_pr.frontSide = unitData_pr.frontSide;
        unit_Cp_pr.backSide = unitData_pr.backSide;
    }

    //------------------------------
    void CheckBattleStandDecisionValid()
    {
        CalculateBattleCost();

        if (isLocalPlayer)
        {
            if (battleCost <= maxBattleCost && IsBattlePanelValid())
            {
                uiManager_Cp.SetActiveBattleStandDecisionBtn(true);
            }
            else
            {
                uiManager_Cp.SetActiveBattleStandDecisionBtn(false);
            }
        }
    }

    //------------------------------
    void CalculateBattleCost()
    {
        int battleCost_tp = 0;

        for (int i = 0; i < partyUnit_Cps.Count; i++)
        {
            if (partyUnit_Cps[i].posType_PartyDecision == UnitCard.UnitPositionType_PartyDecision.Van
                || partyUnit_Cps[i].posType_PartyDecision == UnitCard.UnitPositionType_PartyDecision.Rear)
            {
                int cardIndex_tp = partyUnit_Cps[i].cardIndex;
                if (cardIndex_tp != 0)
                {
                    battleCost_tp += GetUnitDataFromUnit(partyUnit_Cps[i]).cost;
                }
            }
        }

        battleCost = battleCost_tp;
    }

    //------------------------------
    bool IsBattlePanelValid()
    {
        bool result = true;

        List<UnitCard> vanUnit_Cps_tp = GetVanUnits();
        if (vanUnit_Cps_tp.Count != DataManager_Gameplay.maxVanUnitsCount)
        {
            result = false;
        }
        for (int i = 0; i < vanUnit_Cps_tp.Count; i++)
        {
            if (vanUnit_Cps_tp[i].cardIndex == 0)
            {
                result = false;
                break;
            }
        }

        List<UnitCard> rearUnit_Cps_tp = GetRearUnits();
        if (rearUnit_Cps_tp.Count != DataManager_Gameplay.maxRearUnitsCount)
        {
            result = false;
        }
        for (int i = 0; i < rearUnit_Cps_tp.Count; i++)
        {
            if (rearUnit_Cps_tp[i].cardIndex == 0)
            {
                result = false;
                break;
            }
        }

        return result;
    }

    //------------------------------
    public UnitCardData GetUnitDataFromUnit(UnitCard unitCard_pr)
    {
        UnitCardData unitData_tp = null;

        UnitCardsData pUnitCardsData = data_Cp.GetUnitCardsData(playerID);

        for (int i = 0; i < pUnitCardsData.unitCards.Count; i++)
        {
            if (pUnitCardsData.unitCards[i].id == unitCard_pr.cardIndex)
            {
                unitData_tp = pUnitCardsData.unitCards[i];
                break;
            }
        }

        return unitData_tp;
    }

    //------------------------------
    public void PartyDecided()
    {
        isPartyReady = true;
    }

    //////////////////////////////////////////////////////////////////////
    /// On events
    //////////////////////////////////////////////////////////////////////
    #region OnEvents

    //------------------------------
    public void OnUnitHolded()
    {
        holdedUnit_Tf = holdingHandler_Cp.holdedCard_Tf;
    }

    //------------------------------
    public void OnUnitUnHolded()
    {
        UnitCard landTargetUnit_Cp_tp = GetLandTargetUnit(partyUnit_Cps, holdedUnit_Cp, landValidDist);
        UnityEvent unityEvent_tp = new UnityEvent();
        if (landTargetUnit_Cp_tp == null)
        {
            MoveUnit(holdedUnit_Cp, holdedUnit_Cp, unityEvent_tp);
        }
        else
        {
            ExchangeUnitPositionInfo(holdedUnit_Cp, landTargetUnit_Cp_tp, unityEvent_tp);
            CheckBattleStandDecisionValid();

            MoveUnit(holdedUnit_Cp, holdedUnit_Cp, unityEvent_tp);
            MoveUnit(landTargetUnit_Cp_tp, landTargetUnit_Cp_tp, unityEvent_tp); 
        }
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle Computer
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region HandleComputer

    //--------------------------------------------------
    public void ActAsComputer()
    {
        SetBattleUnitsPanelForComPlayer();
    }

    void SetBattleUnitsPanelForComPlayer()
    {
        StartCoroutine(Corou_SetBattleUnitsPanelForComPlayer());
    }

    IEnumerator Corou_SetBattleUnitsPanelForComPlayer()
    {
        //
        List<UnitCard> willVanUnit_Cps_tp = new List<UnitCard>();
        List<UnitCard> willRearUnit_Cps_tp = new List<UnitCard>();
        GetAnyAvailableBattleUnits(out willVanUnit_Cps_tp, out willRearUnit_Cps_tp);

        //
        UnityEvent unityEvent = new UnityEvent();

        List<UnitCard> curVanUnitCard_Cps_tp = GetVanUnits();
        for (int i = 0; i < DataManager_Gameplay.maxVanUnitsCount; i++)
        {
            yield return new WaitForSeconds(1f);
            ExchangeUnitsForComPlayer(willVanUnit_Cps_tp[i], curVanUnitCard_Cps_tp[i], unityEvent);
        }

        List<UnitCard> curRearUnitCard_Cps_tp = GetRearUnits();
        for (int i = 0; i < DataManager_Gameplay.maxRearUnitsCount; i++)
        {
            yield return new WaitForSeconds(1f);
            ExchangeUnitsForComPlayer(willRearUnit_Cps_tp[i], curRearUnitCard_Cps_tp[i], unityEvent);
        }

        //
        isPartyReady = true;
    }

    void ExchangeUnitsForComPlayer(UnitCard firstUnit_Cp_pr, UnitCard sndUnit_Cp_pr, UnityEvent unityEvent)
    {
        ExchangeUnitPositionInfo(firstUnit_Cp_pr, sndUnit_Cp_pr, unityEvent);

        MoveUnit(firstUnit_Cp_pr, firstUnit_Cp_pr, unityEvent);
        MoveUnit(sndUnit_Cp_pr, sndUnit_Cp_pr, unityEvent);
    }

    //--------------------------------------------------
    void GetAnyAvailableBattleUnits(out List<UnitCard> vanUnit_Cps_pr, out List<UnitCard> rearUnit_Cps_pr)
    {
        vanUnit_Cps_pr = new List<UnitCard>();
        rearUnit_Cps_pr = new List<UnitCard>();

        //
        Dictionary<int, int> unitCosts_tp = new Dictionary<int, int>();
        for (int i = 0; i < partyUnit_Cps.Count; i++)
        {
            if (partyUnit_Cps[i].posType_PartyDecision == UnitCard.UnitPositionType_PartyDecision.Candidate)
            {
                int cost_tp = DataManager_Gameplay.GetUnitCardDataFromCardIndex(unitCardsData.unitCards,
                    partyUnit_Cps[i].cardIndex).cost;
                unitCosts_tp.Add(partyUnit_Cps[i].cardIndex, cost_tp);
            }
        }

        //
        Dictionary<int, int> battleUnitCosts_tp = new Dictionary<int, int>();
        while (battleUnitCosts_tp.Count < DataManager_Gameplay.maxBattleUnitsCount)
        {
            int unitCardCost_tp = 0;
            int unitCardIndex_tp = GetAndRemoveMinValueCardIndex(unitCosts_tp, out unitCardCost_tp);
            battleUnitCosts_tp.Add(unitCardIndex_tp, unitCardCost_tp);
        }

        //
        int battleUnitsTotalCost_tp = 0;
        foreach (int cost_tp in battleUnitCosts_tp.Values)
        {
            battleUnitsTotalCost_tp += cost_tp;
        }
        if (battleUnitsTotalCost_tp > maxBattleCost)
        {
            Debug.Log("Invalid battle cost has been detected");
            return;
        }

        //
        List<int> battleUnitIndexes_tp = battleUnitCosts_tp.Keys.ToList<int>();

        for (int i = 0; i < DataManager_Gameplay.maxVanUnitsCount; i++)
        {
            vanUnit_Cps_pr.Add(DataManager_Gameplay.GetUnitFromCardIndex(partyUnit_Cps,
                battleUnitIndexes_tp[i]));
        }
        for (int i = DataManager_Gameplay.maxVanUnitsCount; i < DataManager_Gameplay.maxBattleUnitsCount; i++)
        {
            rearUnit_Cps_pr.Add(DataManager_Gameplay.GetUnitFromCardIndex(partyUnit_Cps,
                battleUnitIndexes_tp[i]));
        }
    }

    int GetAndRemoveMinValueCardIndex(Dictionary<int, int> values_pr, out int cost_pr)
    {
        int minValueCardIndex_tp = 0;
        cost_pr = maxBattleCost;

        foreach (int index_tp in values_pr.Keys)
        {
            if (values_pr[index_tp] < cost_pr)
            {
                cost_pr = values_pr[index_tp];
                minValueCardIndex_tp = index_tp;
            }
        }
        values_pr.Remove(minValueCardIndex_tp);

        return minValueCardIndex_tp;
    }

    #endregion
    //////////////////////////////////////////////////////////////////////

}
