using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player_Phases : MonoBehaviour
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
        RoundStarted, RoundActionDone, RoundFinished,
        Done_RearrangeTakara,
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
    public bool isCreator, isLocalPlayer, isCom;

    // strategy phase
    [SerializeField]
    GameObject bUnit_Pf, mUnit_Pf;

    [SerializeField]
    GameObject takara_Pf;

    [SerializeField]
    Transform bUnitPointsGroup_Tf;

    [SerializeField]
    Transform mUnitPointsGroup_Tf;

    [SerializeField]
    Transform takaraInstPoint_Tf;

    [SerializeField]
    Transform roundsGroup_Tf;

    [SerializeField]
    public Transform playerBLookPoint_Tf, enemyPbLookPoint_Tf, miharidaiLookPoint_Tf, battleBLookPoint_Tf;

    [SerializeField]
    GameObject spMarker_Pf, goldMarker_Pf;

    [SerializeField]
    GameObject shienToken_Pf, move1Token_Pf, move2Token_Pf, move3Token_Pf, atk1Token_Pf, atk2Token_Pf;

    [SerializeField]
    Transform shienUnitPointsGroup_Tf;

    [SerializeField]
    public Transform playerB_Tf;

    [SerializeField]
    public Transform miharidai_Tf;

    [SerializeField]
    public Transform pbBpPoint_Tf;

    [SerializeField]
    public Transform mdBpPoint_Tf;

    [SerializeField]
    public Transform camBpPoint_Tf;

    // battle phase
    [SerializeField]
    public UnitsAbilityHandler unitAbil_Cp;

    [SerializeField]
    Transform takaraPointsGroup_Tf;

    [SerializeField]
    float takaraInterval = 0.03f;

    [SerializeField]
    float excavTakaraArrangeDur = 0.5f;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    //
    [ReadOnly]
    public int playerID;

    [ReadOnly]
    public List<UnitCardData> bUnitCardsData = new List<UnitCardData>();

    [ReadOnly]
    public List<UnitCardData> mUnitCardsData = new List<UnitCardData>();

    [ReadOnly]
    public List<UnitCard> bUnit_Cps = new List<UnitCard>();

    [ReadOnly]
    public List<UnitCard> mUnit_Cps = new List<UnitCard>();

    [ReadOnly]
    public List<TakaraCard> takara_Cps = new List<TakaraCard>();

    [ReadOnly]
    public List<Transform> bUnitPoint_Tfs = new List<Transform>();

    [ReadOnly]
    public List<Transform> mUnitPoint_Tfs = new List<Transform>();

    [ReadOnly]
    public List<Vector3> takaraPoints = new List<Vector3>();

    public List<RoundValue_de> roundsData = new List<RoundValue_de>();

    public TokensData_de tokensData = new TokensData_de();

    public MarkersData_de markersData = new MarkersData_de();

    public BattleInfo battleInfo = new BattleInfo();

    public GameEventsData curGEventsData = new GameEventsData();

    [ReadOnly]
    [SerializeField]
    public GameEventsData waitGEventsData = new GameEventsData();

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;

    DataManager_Gameplay dataManager_Cp;

    List<Transform> round_Tfs = new List<Transform>();

    List<Transform> shienUnitPoint_Tfs = new List<Transform>();

    Transform cam_Tf;

    StatusManager statusManager_Cp;

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

    public List<Hash128> hashStates
    {
        get { return HashHandler.hashes; }
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
    /// Handle hash states
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle hash states

    //--------------------------------------------------
    Hash128 RegRandHashValue()
    {
        return HashHandler.RegRandHash();
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
        AddMainGameState(GameState_En.Nothing);

        //
        SetComponents();

        InitVariables();

        //
        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();

        dataManager_Cp = controller_Cp.dataManager_Cp;

        //statusManager_Cp = controller_Cp.statusManager_Cp;
    }

    //--------------------------------------------------
    void InitVariables()
    {
        InitUnitCardDatas();

        InitMarkersData();

        InitTokensData();

        //InitBattleUnits();

        //InitMihariUnits();

        //InitRounds();

        //InitBattleInfo();

        //InitShienUnitPoints();

        //InitUnitAbilityHandler();

        //InitHighlightRoundPanel();

        //InitTakaraCards();
    }

    //--------------------------------------------------
    void InitUnitCardDatas()
    {
        bUnitCardsData = dataManager_Cp.psBUnitCardsData[playerID].unitCards;

        mUnitCardsData = dataManager_Cp.psMihariUnitCardsData[playerID].unitCards;
    }

    //--------------------------------------------------
    void InitPlayerboard()
    {

    }

    //--------------------------------------------------
    void InitBattleUnits()
    {
        //
        for (int i = 0; i < bUnitPointsGroup_Tf.childCount; i++)
        {
            bUnitPoint_Tfs.Add(bUnitPointsGroup_Tf.GetChild(i));
        }

        //
        for (int i = 0; i < bUnitPoint_Tfs.Count; i++)
        {
            UnitCard bUnit_Cp_tp = Instantiate(bUnit_Pf, bUnitPoint_Tfs[i]).GetComponent<UnitCard>();
            bUnit_Cps.Add(bUnit_Cp_tp);
        }

        //
        for (int i = 0; i < bUnit_Cps.Count; i++)
        {
            bUnit_Cps[i].Init_Phases();

            bUnit_Cps[i].SetStatus_Phases(playerID, bUnitCardsData[i], false, true);

            bUnit_Cps[i].InitUnitInfo();

            bUnit_Cps[i].InitHpPanel();
            bUnit_Cps[i].SetHp(bUnitCardsData[i].hp);
            if (playerID == controller_Cp.localPlayerID)
            {
                bUnit_Cps[i].SetHpVisible(true);
            }
            else
            {
                bUnit_Cps[i].SetHpVisible(false);
            }

            if (i >= 0 && i <= 1)
            {
                bUnit_Cps[i].posType_Phases = UnitCard.UnitPositionType_Phases.Van;
                bUnit_Cps[i].posIndex_Phases = i;
            }
            else if (i >= 2 && i <= 4)
            {
                bUnit_Cps[i].posType_Phases = UnitCard.UnitPositionType_Phases.Rear;
                bUnit_Cps[i].posIndex_Phases = (i - 2);
            }

            bUnit_Cps[i].SetVisible(false);

            bUnit_Cps[i].InitEquipItems();

            bUnit_Cps[i].isHighlighted = false;
        }
    }

    //--------------------------------------------------
    void InitMihariUnits()
    {
        //
        for (int i = 0; i < mUnitPointsGroup_Tf.childCount; i++)
        {
            mUnitPoint_Tfs.Add(mUnitPointsGroup_Tf.GetChild(i));
        }

        //
        for (int i = 0; i < mUnitPoint_Tfs.Count; i++)
        {
            UnitCard mUnit_Cp_tp = Instantiate(mUnit_Pf, mUnitPoint_Tfs[i]).GetComponent<UnitCard>();
            mUnit_Cps.Add(mUnit_Cp_tp);
        }

        //
        for (int i = 0; i < mUnit_Cps.Count; i++)
        {
            mUnit_Cps[i].Init_Phases();

            mUnit_Cps[i].SetStatus_Phases(playerID, mUnitCardsData[i], true, true);

            mUnit_Cps[i].InitUnitInfo();

            mUnit_Cps[i].InitHpPanel();
            mUnit_Cps[i].SetHp(mUnitCardsData[i].hp);
            mUnit_Cps[i].SetHpVisible(false);

            mUnit_Cps[i].SetVisible(true);

            mUnit_Cps[i].InitEquipItems();

            mUnit_Cps[i].isHighlighted = false;
        }
    }

    //--------------------------------------------------
    void InitRounds()
    {
        //
        for (int i = 0; i < roundsGroup_Tf.childCount; i++)
        {
            round_Tfs.Add(roundsGroup_Tf.GetChild(i));
        }

        //
        for (int i = 0; i < round_Tfs.Count; i++)
        {
            RoundValue_de rndValue_tp = new RoundValue_de();

            rndValue_tp.roundPanel_Tf = round_Tfs[i];
            rndValue_tp.allyVan1_Tf = round_Tfs[i].GetChild(0);
            rndValue_tp.allyVan2_Tf = round_Tfs[i].GetChild(1);
            rndValue_tp.enemyVan1_Tf = round_Tfs[i].GetChild(2);
            rndValue_tp.enemyVan2_Tf = round_Tfs[i].GetChild(3);
            rndValue_tp.markersGroup_Tf = round_Tfs[i].GetChild(4);
            rndValue_tp.hlTarget_GO = round_Tfs[i].GetChild(5).gameObject;

            rndValue_tp.index = i;

            roundsData.Add(rndValue_tp);
        }

        // set min ap
        roundsData[0].minAgi = 6;
        roundsData[1].minAgi = 4;
        roundsData[2].minAgi = 0;
        roundsData[3].minAgi = 0;
        roundsData[4].minAgi = 0;

        //
        for (int i = 0; i < round_Tfs.Count; i++)
        {
            LongPressDetector clickDetector_Cp_tp = round_Tfs[i].gameObject
                .AddComponent<LongPressDetector>();

            clickDetector_Cp_tp.targetObject_Tf = clickDetector_Cp_tp.transform;
            clickDetector_Cp_tp.enableClickDetect = true;

            int index = i;
            UnityEvent unityEvent = new UnityEvent();
            //unityEvent.AddListener(() => OnClickRoundOnPlayerboard(index));
            clickDetector_Cp_tp.onClicked = unityEvent;
        }
    }

    //--------------------------------------------------
    void InitMarkersData()
    {
        MarkerValue usedSpMarkers_tp = new MarkerValue();
        usedSpMarkers_tp.type = MarkerType.SP;
        usedSpMarkers_tp.count = 0;
        markersData.usedSpMarkers = usedSpMarkers_tp;

        MarkerValue totalSpMarkers_tp = new MarkerValue();
        totalSpMarkers_tp.type = MarkerType.SP;
        totalSpMarkers_tp.count = 1;
        markersData.totalSpMarkers = totalSpMarkers_tp;

        MarkerValue apMarkers_tp = new MarkerValue();
        apMarkers_tp.type = MarkerType.AP;
        apMarkers_tp.count = 1;
        markersData.apMarkers = apMarkers_tp;

        MarkerValue turnMarkers_tp = new MarkerValue();
        turnMarkers_tp.type = MarkerType.Turn;
        turnMarkers_tp.count = 1;
        markersData.turnMarkers = turnMarkers_tp;

        SetTotalGoldMarkers(100);
        SetUsedGoldMarkers(0);
    }

    //--------------------------------------------------
    void InitTokensData()
    {
        //
        tokensData.usedShienToken.type = TokenType.Shien;
        tokensData.usedShienToken.count = 0;
        tokensData.totalShienToken.type = TokenType.Shien;
        tokensData.totalShienToken.count = 1;

        //
        tokensData.usedMove1Token.type = TokenType.Move1;
        tokensData.usedMove1Token.count = 0;
        tokensData.totalMove1Token.type = TokenType.Move1;
        tokensData.totalMove1Token.count = 1;

        tokensData.usedMove2Token.type = TokenType.Move2;
        tokensData.usedMove2Token.count = 0;
        tokensData.totalMove2Token.type = TokenType.Move2;
        tokensData.totalMove2Token.count = 1;

        tokensData.usedMove3Token.type = TokenType.Move3;
        tokensData.usedMove3Token.count = 0;
        tokensData.totalMove3Token.type = TokenType.Move3;
        tokensData.totalMove3Token.count = 1;

        //
        tokensData.usedAtk1Token.type = TokenType.Attack;
        tokensData.usedAtk1Token.count = 0;
        tokensData.totalAtk1Token.type = TokenType.Attack;
        tokensData.totalAtk1Token.count = 1;

        tokensData.usedAtk2Token.type = TokenType.Attack;
        tokensData.usedAtk2Token.count = 0;
        tokensData.totalAtk2Token.type = TokenType.Attack;
        tokensData.totalAtk2Token.count = 1;
    }

    //--------------------------------------------------
    void InitBattleInfo()
    {
        RefreshBattleInfo();
    }

    //--------------------------------------------------
    void InitShienUnitPoints()
    {
        for (int i = 0; i < shienUnitPointsGroup_Tf.childCount; i++)
        {
            shienUnitPoint_Tfs.Add(shienUnitPointsGroup_Tf.GetChild(i));
        }
    }

    //--------------------------------------------------
    void InitUnitAbilityHandler()
    {
        unitAbil_Cp.Init();
    }

    //--------------------------------------------------
    void InitHighlightRoundPanel()
    {
        for (int i = 0; i < roundsData.Count; i++)
        {
            HighlightRoundPanel(false, i);
        }
    }

    //--------------------------------------------------
    void InitTakaraCards()
    {
        
    }

    #endregion

    //-------------------------------------------------- Handle sp markers
    public void IncSpMarker(int roundIndex_pr, int incCount_pr = 1)
    {
        SetSpMarker(roundIndex_pr, incCount_pr);
    }

    public void DecSpMarker(int roundIndex_pr, int decCount_pr = 1)
    {
        SetSpMarker(roundIndex_pr, -1 * decCount_pr);
    }

    public void SetSpMarker(int roundIndex_pr, int changeAmount_pr)
    {
        //
        roundsData[roundIndex_pr].spMarkerCount = Mathf.Clamp(
            roundsData[roundIndex_pr].spMarkerCount + changeAmount_pr,
            0, markersData.totalSpMarkers.count);

        SetSpMarkersOnPlayerboard(roundIndex_pr, roundsData[roundIndex_pr].spMarkerCount);

        //
        markersData.usedSpMarkers.count += changeAmount_pr;
    }

    public void ResetSpMarker(int roundIndex_pr)
    {
        int changeAmount = roundsData[roundIndex_pr].spMarkerCount;

        SetSpMarker(roundIndex_pr, -1 * changeAmount);
    }

    void SetSpMarkersOnPlayerboard(int roundIndex_pr, int markerCount_pr)
    {
        // destroy markers
        RoundValue_de roundValue = roundsData[roundIndex_pr];
        for (int i = 0; i < roundValue.marker_Tfs.Count; i++)
        {
            Destroy(roundValue.marker_Tfs[i].gameObject);
        }
        roundValue.marker_Tfs.Clear();

        // instant markers
        for (int i = 0; i < markerCount_pr; i++)
        {
            Transform marker_Tf_tp = Instantiate(spMarker_Pf, roundValue.markersGroup_Tf).transform;
            roundValue.marker_Tfs.Add(marker_Tf_tp);
        }

        // arrange markers
        List<Vector3> markersPos = ArrangeObjects.GetArrangePoints(roundValue.markersGroup_Tf.position,
            markerCount_pr, 3, 0.04f, 0.04f);
        for (int i = 0; i < roundValue.marker_Tfs.Count; i++)
        {
            roundValue.marker_Tfs[i].position = markersPos[i];
        }
    }

    public void RefreshBattleInfo()
    {
        //
        battleInfo.mihariUnitCount = 0;
        for (int i = 0; i < mUnit_Cps.Count; i++)
        {
            if (mUnit_Cps[i] != null)
            {
                battleInfo.mihariUnitCount++;
            }
        }

        //
        battleInfo.discardUnitCount = 0;

        //
        battleInfo.ken = 0;
        battleInfo.ma = 0;
        battleInfo.yumi = 0;
        battleInfo.fushi = 0;
        battleInfo.ryu = 0;
        for (int i = 0; i < bUnitCardsData.Count; i++)
        {
            if (bUnitCardsData[i].attrib.Contains(UnitAttribute.Ken))
            {
                battleInfo.ken++;
            }
            if (bUnitCardsData[i].attrib.Contains(UnitAttribute.Ma))
            {
                battleInfo.ma++;
            }
            if (bUnitCardsData[i].attrib.Contains(UnitAttribute.Yumi))
            {
                battleInfo.yumi++;
            }
            if (bUnitCardsData[i].attrib.Contains(UnitAttribute.Fushi))
            {
                battleInfo.fushi++;
            }
            if (bUnitCardsData[i].attrib.Contains(UnitAttribute.Ryu))
            {
                battleInfo.ryu++;
            }
        }
    }

    //-------------------------------------------------- Set tokens
    public void SetShienToken(int roundIndex_pr, int shienUnitIndex_pr, int shienTargetUnitIndex_pr)
    {
        RoundValue_de roundValue = roundsData[roundIndex_pr];
        Transform tokenPoint_Tf_tp = null;

        if (shienTargetUnitIndex_pr == 0)
        {
            tokenPoint_Tf_tp = roundValue.allyVan1_Tf;
        }
        else if (shienTargetUnitIndex_pr == 1)
        {
            tokenPoint_Tf_tp = roundValue.allyVan2_Tf;
        }

        // set token variables
        roundValue.token.type = TokenType.Shien;
        roundValue.token.count = 1;
        roundValue.originUnitIndex = shienUnitIndex_pr;
        roundValue.tarUnitIndex = shienTargetUnitIndex_pr;

        // instant token
        roundValue.token_Tf = Instantiate(shienToken_Pf, tokenPoint_Tf_tp).transform;

        // move mihari unit to shien unit point
        roundValue.shienUnit_Cp = mUnit_Cps[shienUnitIndex_pr];
        roundValue.shienUnit_Cp.transform.parent = shienUnitPoint_Tfs[roundIndex_pr].transform;

        UnityEvent unityEvent = new UnityEvent();
        TargetTweening.TranslateGameObject(roundValue.shienUnit_Cp.transform, shienUnitPoint_Tfs[roundIndex_pr],
            unityEvent);
        mUnit_Cps[shienUnitIndex_pr] = null;

        // set tokens data
        tokensData.usedShienToken.count++;
    }

    public void SetMoveToken(int roundIndex_pr, int originIndex_pr, int targetIndex_pr)
    {
        RoundValue_de roundValue = roundsData[roundIndex_pr];
        Transform tokenPoint_Tf_tp = null;

        if (targetIndex_pr == 0)
        {
            tokenPoint_Tf_tp = roundValue.allyVan1_Tf;
        }
        else if (targetIndex_pr == 1)
        {
            tokenPoint_Tf_tp = roundValue.allyVan2_Tf;
        }

        // set token variables
        roundValue.token.type = TokenType.Move;
        roundValue.token.count = 1;
        roundValue.originUnitIndex = originIndex_pr;
        roundValue.tarUnitIndex = targetIndex_pr;

        // instant token
        GameObject token_Pf_tp = null;
        if (originIndex_pr == 0)
        {
            token_Pf_tp = move1Token_Pf;
        }
        else if (originIndex_pr == 1)
        {
            token_Pf_tp = move2Token_Pf;
        }
        else if (originIndex_pr == 2)
        {
            token_Pf_tp = move3Token_Pf;
        }
        roundValue.token_Tf = Instantiate(token_Pf_tp, tokenPoint_Tf_tp).transform;

        // set tokens data
        if (originIndex_pr == 0)
        {
            tokensData.usedMove1Token.count++;
        }
        else if (originIndex_pr == 1)
        {
            tokensData.usedMove2Token.count++;
        }
        else if (originIndex_pr == 2)
        {
            tokensData.usedMove3Token.count++;
        }
    }

    public void SetAtkToken(int roundIndex_pr, int originIndex_pr, int targetIndex_pr, AttackType atkType_pr)
    {
        RoundValue_de roundValue = roundsData[roundIndex_pr];
        Transform tokenPoint_Tf_tp = null;

        if (targetIndex_pr == 0)
        {
            tokenPoint_Tf_tp = roundValue.enemyVan1_Tf;
        }
        else if (targetIndex_pr == 1)
        {
            tokenPoint_Tf_tp = roundValue.enemyVan2_Tf;
        }

        // set token
        roundValue.token.type = TokenType.Attack;
        roundValue.token.count = 1;
        roundValue.originUnitIndex = originIndex_pr;
        roundValue.tarUnitIndex = targetIndex_pr;
        roundValue.atkType = atkType_pr;

        // instant token
        GameObject token_Pf_tp = null;
        if (originIndex_pr == 0)
        {
            token_Pf_tp = atk1Token_Pf;
        }
        else if (originIndex_pr == 1)
        {
            token_Pf_tp = atk2Token_Pf;
        }
        roundValue.token_Tf = Instantiate(token_Pf_tp, tokenPoint_Tf_tp).transform;

        // set tokens data
        if (originIndex_pr == 0)
        {
            tokensData.usedAtk1Token.count++;
        }
        else if (originIndex_pr == 1)
        {
            tokensData.usedAtk2Token.count++;
        }

        //
        //StartCoroutine(Corou_SetAtkToken());
    }

    IEnumerator Corou_SetAtkToken()
    {
        yield return null;
    }

    //-------------------------------------------------- Set markers
    public void SetTotalGoldMarkers(int count)
    {
        markersData.totalGoldMarkers.type = MarkerType.Gold;
        markersData.totalGoldMarkers.count = count;

        //
        statusManager_Cp.gold = markersData.totalGoldMarkers.count;
    }

    public void SetUsedGoldMarkers(int count)
    {
        markersData.usedGoldMarkers.type = MarkerType.Gold;
        markersData.usedGoldMarkers.count = count;
    }

    //-------------------------------------------------- Reset round tokens
    public void ResetRoundToken(int roundIndex_pr, UnityEvent unityEvent)
    {
        RoundValue_de roundValue = roundsData[roundIndex_pr];

        if (roundValue.token.type == TokenType.Null)
        {
            unityEvent.Invoke();
            return;
        }

        // destroy token
        Destroy(roundValue.token_Tf.gameObject);

        // reset shien token in case of shien
        if (roundValue.token.type == TokenType.Shien)
        {
            int mihariUnitIndex_pr = roundValue.originUnitIndex;
            roundValue.shienUnit_Cp.transform.SetParent(mUnitPoint_Tfs[mihariUnitIndex_pr]);

            TargetTweening.TranslateGameObject(roundValue.shienUnit_Cp.transform,
                mUnitPoint_Tfs[mihariUnitIndex_pr], unityEvent);

            mUnit_Cps[mihariUnitIndex_pr] = roundValue.shienUnit_Cp;

            roundValue.shienUnit_Cp = null;

            // set shien token count
            tokensData.usedShienToken.count--;
        }
        else if (roundValue.token.type == TokenType.Move)
        {
            int moveTokenIndex_pr = roundValue.originUnitIndex;
            if (moveTokenIndex_pr == 0)
            {
                tokensData.usedMove1Token.count--;
            }
            else if (moveTokenIndex_pr == 1)
            {
                tokensData.usedMove2Token.count--;
            }
            else if (moveTokenIndex_pr == 2)
            {
                tokensData.usedMove3Token.count--;
            }
        }
        else if (roundValue.token.type == TokenType.Attack)
        {
            int atkTokenIndex_pr = roundValue.originUnitIndex;
            if (atkTokenIndex_pr == 0)
            {
                tokensData.usedAtk1Token.count--;
            }
            else if (atkTokenIndex_pr == 1)
            {
                tokensData.usedAtk2Token.count--;
            }
        }

        // reset token variables
        roundValue.token.type = TokenType.Null;
        roundValue.token.count = 0;
        roundValue.originUnitIndex = 0;
        roundValue.tarUnitIndex = 0;
        roundValue.atkType = AttackType.Null;
    }

    //-------------------------------------------------- Highlight round
    public void HighlightRoundPanel(bool flag, int roundIndex_pr)
    {
        GameObject hlTarget_GO_tp = roundsData[roundIndex_pr].hlTarget_GO;
        Outline hlOutline_Cp_tp = hlTarget_GO_tp.GetComponent<Outline>();

        if (flag)
        {
            hlOutline_Cp_tp.color = 0;
        }
        else
        {
            hlOutline_Cp_tp.color = 1;
        }
    }

    //-------------------------------------------------- handle takara card
    public void MoveTakaraToExcavArea(TakaraCardData takaraData)
    {
        StartCoroutine(Corou_MoveTakaraToExcavArea(takaraData));
    }

    IEnumerator Corou_MoveTakaraToExcavArea(TakaraCardData takaraData)
    {
        //
        InstantAndInitTakaraCard(takaraData);

        //
        RearrangeTakarasOnExcavArea(takara_Cps.Count);

        //
        yield return null;
    }

    void InstantAndInitTakaraCard(TakaraCardData takaraData)
    {
        TakaraCard movingTakara_Cp_tp = Instantiate(takara_Pf, takaraInstPoint_Tf).GetComponent<TakaraCard>();

        //
        movingTakara_Cp_tp.takaraData = takaraData;
        takara_Cps.Add(movingTakara_Cp_tp);
    }

    void RearrangeTakarasOnExcavArea(int takaraCount)
    {
        StartCoroutine(Corou_RearrangeTakarasOnExcavArea(takaraCount));
    }

    IEnumerator Corou_RearrangeTakarasOnExcavArea(int takaraCount)
    {
        //
        takaraPoints.Clear();
        takaraPoints = ArrangeObjects.GetArrangePoints(takaraPointsGroup_Tf.position, takaraCount, takaraCount,
            0f, takaraInterval);

        //
        UnityEvent unityEvent = new UnityEvent();
        unityEvent.AddListener(OnComplete_RearrangeTakarasOnExcavArea);
        for (int i = 0; i < takara_Cps.Count; i++)
        {
            TargetTweening.JumpObject(takara_Cps[i].transform, takara_Cps[i].transform.position,
                takaraPoints[i], unityEvent, excavTakaraArrangeDur);
        }

        //
        yield return null;        
    }

    int movedTakaraCount;
    void OnComplete_RearrangeTakarasOnExcavArea()
    {
        movedTakaraCount++;

        if (movedTakaraCount < takara_Cps.Count)
        {
            return;
        }

        //
        for (int i = 0; i < takara_Cps.Count; i++)
        {
            takara_Cps[i].transform.SetParent(takaraPointsGroup_Tf, true);
        }

        //
        movedTakaraCount = 0;

        //
        AddGameStates(GameState_En.Done_RearrangeTakara);
    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Start round
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Start round

    //--------------------------------------------------
    public void StartRound(int rndId_pr, Hash128 hash_pr)
    {
        StartCoroutine(Corou_StartRound(rndId_pr, hash_pr));
    }

    IEnumerator Corou_StartRound(int rndId_pr, Hash128 hash_pr)
    {
        Hash128 hash_tp = RegRandHashValue();
        unitAbil_Cp.StartRoundAction(rndId_pr, hash_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash_tp));

        hashStates.Remove(hash_pr);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play round
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region PlayRound

    //-------------------------------------------------- Play round
    public void PlayRound(int roundIndex)
    {
        StartCoroutine(Corou_PlayRound(roundIndex));
    }

    IEnumerator Corou_PlayRound(int roundIndex)
    {
        AddGameStates(GameState_En.RoundStarted);

        //****************************** game events 13th. when round start
        Hash128 hash_tp = RegRandHashValue();
        unitAbil_Cp.HandleGameEventsTiming(GameEventsTiming.WhenRndStart, hash_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash_tp));

        //
        unitAbil_Cp.PlayRoundAction(roundIndex);
        yield return new WaitUntil(() => unitAbil_Cp.ExistGameStates(
            UnitsAbilityHandler.GameState_En.ActionFinished));
        unitAbil_Cp.RemoveGameStates(UnitsAbilityHandler.GameState_En.ActionFinished);

        //****************************** game events 14th. when round end
        Hash128 hash2_tp = RegRandHashValue();
        unitAbil_Cp.HandleGameEventsTiming(GameEventsTiming.WhenRndEnd, hash2_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash2_tp));

        //
        RemoveGameStates(GameState_En.RoundStarted);
        AddGameStates(GameState_En.RoundActionDone);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Finish round
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region FinishRound

    //-------------------------------------------------- Finish round
    public void FinishRound(int roundIndex)
    {
        StartCoroutine(Corou_FinishRound(roundIndex));
    }

    IEnumerator Corou_FinishRound(int roundIndex)
    {
        //
        unitAbil_Cp.FinishRound();
        yield return new WaitUntil(() => unitAbil_Cp.ExistGameStates(
            UnitsAbilityHandler.GameState_En.RoundFinished));
        unitAbil_Cp.RemoveGameStates(UnitsAbilityHandler.GameState_En.RoundFinished);

        //
        AddGameStates(GameState_En.RoundFinished);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// OnEvents
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region OnEvents

    #endregion

}
