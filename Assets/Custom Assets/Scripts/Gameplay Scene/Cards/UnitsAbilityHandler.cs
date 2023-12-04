using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitsAbilityHandler : MonoBehaviour
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
        ActionStarted, ActionDone, ActionFinished, RoundWillFinish, RoundFinished,
        GameActionStarted, GameActionFinished,
        GameEventsStarted, GameEventsFinished,
        AtkProceed, AtkFailed,
        Done_Atk_CheckUnitVisi, Done_Atk_CheckAtkPoss, Done_Atk_CheckAgi, Done_Atk_CheckHitSucc, Done_Atk_CalcDmg,
        Done_AtkerJumped, Done_AtkerReturned, Done_DeadUnit,
        HandleAbilStarted, HandleAbilDone, HandleAbilFinished,
        BUnitUIStarted, BUnitUISelected,
    }

    [Serializable]
    public class EffectPrefab_Cs
    {
        [SerializeField]
        public GameObject guardEff_Pf;

        [SerializeField]
        public GameObject shienEff_Pf;

        [SerializeField]
        public GameObject moveEff1_Pf, moveEff2_Pf;

        [SerializeField]
        public GameObject atkJumpEff_Pf;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    // components
    [SerializeField]
    public GameEventsHandler gEventsHandler_Cp;

    // guard action
    [SerializeField]
    public GameObject guardEff_Pf;

    [SerializeField]
    public float guardEffDur = 5f;

    // shien action
    [SerializeField]
    public GameObject shienEff_Pf;

    [SerializeField]
    public float shienEffDur = 5f;

    // move action
    [SerializeField]
    public GameObject moveEff1_Pf, moveEff2_Pf;

    [SerializeField]
    public float moveUnitDur = 0.7f, moveEffDur = 3f;

    // atk action
    [SerializeField]
    public GameObject atkJumpEff_Pf;

    [SerializeField]
    public float atkJumpDur = 0.7f, atkJumpStayDur = 2f;

    // extra
    [SerializeField]
    public float emptyRoundDur = 1f;

    [SerializeField]
    public EffectPrefab_Cs effPfs;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    // variables
    [ReadOnly]
    public UnitCard oriUnit_Cp, tarUnit_Cp;

    //-------------------------------------------------- private fields
    // components
    [HideInInspector]
    public Controller_Phases controller_Cp;

    [HideInInspector]
    public Controller_StartPhase startController_Cp;

    [HideInInspector]
    public Controller_BattlePhase btlController_Cp;

    [HideInInspector]
    public UI_BattlePhase btlUI_Cp;

    [HideInInspector]
    public DataManager_Gameplay dataManager_Cp;

    [HideInInspector]
    public Player_Phases mPlayer_Cp, ePlayer_Cp;

    [HideInInspector]
    public UnitsAbilityHandler eUnitAbil_Cp;

    [HideInInspector]
    public List<Player_Phases> player_Cps = new List<Player_Phases>();

    [SerializeField]
    [ReadOnly]
    public List<UnitCard> bUnit_Cps = new List<UnitCard>();

    [SerializeField]
    [ReadOnly]
    public List<UnitCard> mUnit_Cps = new List<UnitCard>();

    [SerializeField]
    [ReadOnly]
    public List<UnitCard> eBUnit_Cps = new List<UnitCard>();

    [HideInInspector]
    public List<TakaraCardData> takaraDatas = new List<TakaraCardData>();

    [HideInInspector]
    public List<Transform> bUnitPoint_Tfs = new List<Transform>();

    [HideInInspector]
    public List<Transform> eBUnitPoint_Tfs = new List<Transform>();

    // player data
    [HideInInspector]
    public DataStorage_Gameplay dataStorage;

    [HideInInspector]
    public GameEventsData gEventsData = new GameEventsData();

    [HideInInspector]
    public List<RoundValue> roundsData = new List<RoundValue>();

    [SerializeField]
    [ReadOnly]
    public int curRoundIndex;

    [HideInInspector]
    public RoundValue roundValue = new RoundValue();

    [HideInInspector]
    public bool isLocalPlayer;

    [HideInInspector]
    public int mPlayerID, ePlayerID;

    [HideInInspector]
    public string atkFailedMsg;

    [SerializeField]
    [ReadOnly]
    public AttackType atkType = new AttackType();

    [HideInInspector]
    public bool atkSucc, isCritAtk;

    [ReadOnly]
    public int actionTarUnitIndex;

    [ReadOnly]
    public float actionEffTxtDur, noticeTxtDur;

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

    public int playerAp
    {
        get { return startController_Cp.playerAPs[mPlayerID]; }
        set { startController_Cp.playerAPs[mPlayerID] = playerAp; }
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

        startController_Cp = controller_Cp.startController_Cp;

        btlController_Cp = controller_Cp.btlController_Cp;

        btlUI_Cp = btlController_Cp.btlUI_Cp;

        dataManager_Cp = controller_Cp.dataManager_Cp;

        mPlayer_Cp = gameObject.GetComponent<Player_Phases>();

        ePlayer_Cp = mPlayer_Cp.playerID == 0 ? controller_Cp.player_Cps[1] : controller_Cp.player_Cps[0];

        eUnitAbil_Cp = ePlayer_Cp.unitAbil_Cp;

        player_Cps = controller_Cp.player_Cps;

        bUnitPoint_Tfs = mPlayer_Cp.bUnitPoint_Tfs;

        eBUnitPoint_Tfs = ePlayer_Cp.bUnitPoint_Tfs;

        //
        dataStorage = dataManager_Cp.dataStorage;

        gEventsData = dataStorage.gameEventsData;

        roundsData = mPlayer_Cp.roundsData;

        bUnit_Cps = mPlayer_Cp.bUnit_Cps;

        mUnit_Cps = mPlayer_Cp.mUnit_Cps;

        eBUnit_Cps = ePlayer_Cp.bUnit_Cps;

        isLocalPlayer = mPlayer_Cp.isLocalPlayer ? true : false;

        mPlayerID = mPlayer_Cp.playerID;

        ePlayerID = ePlayer_Cp.playerID;
    }

    //--------------------------------------------------
    void InitVariables()
    {
        gEventsHandler_Cp.Init();

        //
        actionEffTxtDur = btlUI_Cp.actionTxtDur;

        noticeTxtDur = btlUI_Cp.noticeTxtDur;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play round action
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region PlayRoundAction

    //--------------------------------------------------
    public void PlayRoundAction(int roundIndex)
    {
        StartCoroutine(Corou_PlayRoundAction(roundIndex));
    }

    IEnumerator Corou_PlayRoundAction(int roundIndex)
    {
        AddGameStates(GameState_En.ActionStarted);

        // init round action values
        curRoundIndex = roundIndex;
        roundValue = roundsData[curRoundIndex];

        if (roundValue.action == ActionType.Shien)
        {
            oriUnit_Cp = roundValue.shienUnit_Cp;
            tarUnit_Cp = bUnit_Cps[roundValue.targetUnitIndex];
        }
        else
        {
            oriUnit_Cp = bUnit_Cps[roundValue.originUnitIndex];
            tarUnit_Cp = eBUnit_Cps[roundValue.targetUnitIndex];
        }

        //
        if (curRoundIndex == 0)
        {
            //******************** 24th timing. battle phase
            HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.BtlPhase);
            yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
            RemoveGameStates(GameState_En.GameEventsFinished);
        }

        //******************** 16th ~ 20th timing. battle phase round
        if (curRoundIndex == 0)
        {
            HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.Btl1thRnd);
        }
        else if (curRoundIndex == 1)
        {
            HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.Btl2thRnd);
        }
        else if (curRoundIndex == 2)
        {
            HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.Btl3thRnd);
        }
        else if (curRoundIndex == 3)
        {
            HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.Btl4thRnd);
        }
        else if (curRoundIndex == 4)
        {
            HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.Btl5thRnd);
        }

        yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
        RemoveGameStates(GameState_En.GameEventsFinished);

        //
        switch (roundValue.action)
        {
            case ActionType.Guard:
                PlayGuardAction();
                break;
            case ActionType.Shien:
                PlayShienAction();
                break;
            case ActionType.Move:
                PlayMoveAction();
                break;
            case ActionType.Attack:
                PlayAtkAction();
                break;
            default:
                yield return new WaitForSeconds(emptyRoundDur);
                AddGameStates(GameState_En.ActionDone);
                break;
        }

        yield return new WaitUntil(() => ExistGameStates(GameState_En.ActionDone));
        RemoveGameStates(GameState_En.ActionDone);

        //******************** 8th timing. until next round
        HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.UntilNextRnd);
        yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
        RemoveGameStates(GameState_En.GameEventsFinished);

        //
        RemoveGameStates(GameState_En.ActionStarted);
        AddGameStates(GameState_En.ActionFinished);
    }

    //-------------------------------------------------- Play guard action
    void PlayGuardAction()
    {
        StartCoroutine(Corou_PlayGuardAction());
    }

    IEnumerator Corou_PlayGuardAction()
    {
        // handle data
        for (int i = 0; i < 2; i++)
        {
            bUnit_Cps[i].unitInfo.defCorr += roundValue.spMarkerCount;
        }

        // handle effect
        for (int i = 0; i < 2; i++)
        {
            gEventsHandler_Cp.GameActionEff_Guard(bUnit_Cps[i].effTgr_Tf, guardEffDur);
        }

        if (mPlayer_Cp.playerID == 0)
        {
            btlUI_Cp.SetActionEffText("Player1 前衛 DEF +" + roundValue.spMarkerCount);
        }
        else if (mPlayer_Cp.playerID == 1)
        {
            btlUI_Cp.SetActionEffText("Player2 前衛 DEF +" + roundValue.spMarkerCount);
        }

        yield return new WaitForSeconds(guardEffDur);

        btlUI_Cp.FinishActionEffText();

        //
        AddGameStates(GameState_En.ActionDone);
    }

    //-------------------------------------------------- Play shien action
    void PlayShienAction()
    {
        StartCoroutine(Corou_PlayShienAction());
    }

    IEnumerator Corou_PlayShienAction()
    {
        UnitCard shienUnit_Cp_tp = roundValue.shienUnit_Cp;

        // perform shien action
        gEventsHandler_Cp.GameAction_Shien(oriUnit_Cp, shienUnit_Cp_tp);
        yield return new WaitUntil(() => ExistGameStates(GameState_En.GameActionFinished));
        RemoveGameStates(GameState_En.GameActionFinished);

        //******************** 12th timing. after shien effect apply
        HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.BtlPhaseAftShienApply);
        yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
        RemoveGameStates(GameState_En.GameEventsFinished);

        //******************** 35th timing. take shien
        HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.TakeShien);
        yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
        RemoveGameStates(GameState_En.GameEventsFinished);

        //
        AddGameStates(GameState_En.ActionDone);
    }

    //-------------------------------------------------- Play move action
    void PlayMoveAction()
    {
        StartCoroutine(Corou_PlayMoveAction());
    }

    IEnumerator Corou_PlayMoveAction()
    {
        //
        gEventsHandler_Cp.GameAction_Move();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.GameActionFinished));
        RemoveGameStates(GameState_En.GameActionFinished);

        //******************** 15th timing. When moved
        HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.WhenMoved);
        yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
        RemoveGameStates(GameState_En.GameEventsFinished);

        //
        AddGameStates(GameState_En.ActionDone);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play attack action
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region PlayAtkAction

    //-------------------------------------------------- Play atk action
    void PlayAtkAction()
    {
        StartCoroutine(Corou_PlayAtkAction());
    }

    IEnumerator Corou_PlayAtkAction()
    {
        //
        DeterAtkType();

        //
        //if (atkType == AttackType.Normal)
        //{
        //    //******************** 27th timing. normal attack
        //    HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.NlAtk);
        //    yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
        //    RemoveGameStates(GameState_En.GameEventsFinished);
        //}
        //else if (atkType == AttackType.Spc1 || atkType == AttackType.Spc2)
        //{
        //    //******************** 28th timing. special attack
        //    HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.SpcAtk);
        //    yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
        //    RemoveGameStates(GameState_En.GameEventsFinished);
        //}

        ////
        //bool isSpcAtkWithDefCorr = false;
        //if (atkType == AttackType.Spc1)
        //{
        //    foreach (SpecialAttack1Content spcAtk1Cont_tp in oriUnit_Cp.unitCardData.spcAtk1.contents)
        //    {
        //        if (spcAtk1Cont_tp.type == SpecialAttack1Type.Type02)
        //        {
        //            isSpcAtkWithDefCorr = true;
        //            break;
        //        }
        //    }
        //}
        //else if (atkType == AttackType.Spc2)
        //{
        //    foreach (SpecialAttack2Content spcAtk2Cont_tp in oriUnit_Cp.unitCardData.spcAtk2.contents)
        //    {
        //        if (spcAtk2Cont_tp.type == SpecialAttack2Type.Type02)
        //        {
        //            isSpcAtkWithDefCorr = true;
        //            break;
        //        }
        //    }
        //}

        //if (isSpcAtkWithDefCorr)
        //{
        //    //******************** 34th timing. special attack with def correction
        //    HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.SpcAtkWithDefCorr);
        //    yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
        //    RemoveGameStates(GameState_En.GameEventsFinished);
        //}

        //
        AttackProceeding();

        yield return new WaitUntil(() => ExistAnyGameStates(GameState_En.AtkProceed, GameState_En.AtkFailed));
        if (ExistGameStates(GameState_En.AtkProceed))
        {
            RemoveGameStates(GameState_En.AtkProceed);
        }
        else if (ExistGameStates(GameState_En.AtkFailed))
        {
            btlUI_Cp.SetActionEffText(atkFailedMsg);
            yield return new WaitUntil(() => btlUI_Cp.ExistGameStates(UI_BattlePhase.GameState_En.Done_ActionTxtShow));
            btlUI_Cp.FinishActionEffText();

            RemoveGameStates(GameState_En.AtkFailed);
        }

        //
        AddGameStates(GameState_En.ActionDone);
    }

    //-------------------------------------------------- Attack proceeding
    void AttackProceeding()
    {
        StartCoroutine(Corou_AttackProceeding());
    }

    IEnumerator Corou_AttackProceeding()
    {
        //
        StartCoroutine(Corou_Atk_UnitVisible());
        yield return new WaitUntil(() => ExistGameStates(GameState_En.Done_Atk_CheckUnitVisi));
        RemoveGameStates(GameState_En.Done_Atk_CheckUnitVisi);

        //
        StartCoroutine(Corou_Atk_CheckAtkPoss());
        yield return new WaitUntil(() => ExistAnyGameStates(GameState_En.Done_Atk_CheckAtkPoss,
            GameState_En.AtkFailed));
        if (ExistGameStates(GameState_En.Done_Atk_CheckAtkPoss))
        {
            RemoveGameStates(GameState_En.Done_Atk_CheckAtkPoss);
        }
        else if (ExistGameStates(GameState_En.AtkFailed))
        {
            yield break;
        }

        //
        StartCoroutine(Corou_Atk_CheckAgi());
        yield return new WaitUntil(() => ExistAnyGameStates(GameState_En.Done_Atk_CheckAgi,
            GameState_En.AtkFailed));
        if (ExistGameStates(GameState_En.Done_Atk_CheckAgi))
        {
            RemoveGameStates(GameState_En.Done_Atk_CheckAgi);
        }
        else if (ExistGameStates(GameState_En.AtkFailed))
        {
            yield break;
        }

        //
        StartCoroutine(Corou_Atk_CheckHitSucc());
        yield return new WaitUntil(() => ExistAnyGameStates(GameState_En.Done_Atk_CheckHitSucc,
            GameState_En.AtkFailed));
        if (ExistGameStates(GameState_En.Done_Atk_CheckHitSucc))
        {
            RemoveGameStates(GameState_En.Done_Atk_CheckHitSucc);
        }
        else if (ExistGameStates(GameState_En.AtkFailed))
        {
            yield break;
        }

        //
        StartCoroutine(Corou_Atk_CalcDmg());
        yield return new WaitUntil(() => ExistGameStates(GameState_En.Done_Atk_CalcDmg));
        RemoveGameStates(GameState_En.Done_Atk_CalcDmg);

        //
        AddGameStates(GameState_En.AtkProceed);
    }

    IEnumerator Corou_Atk_UnitVisible()
    {
        UnitInfo oriUnitInfo = oriUnit_Cp.unitInfo;
        UnitInfo tarUnitInfo = tarUnit_Cp.unitInfo;

        // step 1. check the visible of unit
        if (!oriUnitInfo.visible)
        {
            UnityEvent unityEvent = new UnityEvent();
            unityEvent.AddListener(gEventsHandler_Cp.OnComplete_GameAction_FlipOverCard);
            gEventsHandler_Cp.GameAction_FlipOverCard(oriUnit_Cp, true, unityEvent);
            yield return new WaitUntil(() => ExistGameStates(GameState_En.GameActionFinished));
            RemoveGameStates(GameState_En.GameActionFinished);

            oriUnit_Cp.SetHpVisible(true);

            //******************** 9th timing. battle phase reverse effect trigger
            HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.BtlRevEffTgr);
            yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
            RemoveGameStates(GameState_En.GameEventsFinished);
        }
        
        if (!tarUnitInfo.visible)
        {
            UnityEvent unityEvent = new UnityEvent();
            unityEvent.AddListener(gEventsHandler_Cp.OnComplete_GameAction_FlipOverCard);
            gEventsHandler_Cp.GameAction_FlipOverCard(tarUnit_Cp, true, unityEvent);
            yield return new WaitUntil(() => ExistGameStates(GameState_En.GameActionFinished));
            RemoveGameStates(GameState_En.GameActionFinished);

            tarUnit_Cp.SetHpVisible(true);

            //******************** 9th timing. battle phase reverse effect trigger
            eUnitAbil_Cp.HandleGameEventsTiming(tarUnit_Cp, GameEventsTiming.BtlRevEffTgr);
            yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
            RemoveGameStates(GameState_En.GameEventsFinished);
        }

        //******************** 22th timing. next attack
        //HandleGameEventsTiming(tarUnit_Cp, GameEventsTiming.NextAtk);
        //yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
        //RemoveGameStates(GameState_En.GameEventsFinished);

        //
        AddGameStates(GameState_En.Done_Atk_CheckUnitVisi);
    }

    IEnumerator Corou_Atk_CheckAtkPoss()
    {
        // step 2. check attack possibility        
        if (atkType == AttackType.Null)
        {
            atkFailedMsg = "APが足りません。";
            AddGameStates(GameState_En.AtkFailed);

            yield break;
        }

        // message
        string dsc = string.Empty;
        dsc = "「" + oriUnit_Cp.unitCardData.name + "」" + "が敵の「" + tarUnit_Cp.unitCardData.name + "」に";
        switch (atkType)
        {
            case AttackType.Normal:
                dsc += "通常攻撃！";
                break;
            case AttackType.Spc1:
                dsc += "特殊攻撃1！";
                break;
            case AttackType.Spc2:
                dsc += "特殊攻撃2！";
                break;
        }

        btlUI_Cp.SetActionEffText(dsc);
        yield return new WaitForSeconds(actionEffTxtDur);
        btlUI_Cp.FinishActionEffText();

        //
        AddGameStates(GameState_En.Done_Atk_CheckAtkPoss);
    }

    IEnumerator Corou_Atk_CheckAgi()
    {
        UnitInfo oriUnitInfo = oriUnit_Cp.unitInfo;
        UnitInfo tarUnitInfo = tarUnit_Cp.unitInfo;

        // step 3. check agility
        if (oriUnitInfo.agi < roundValue.minAgi)
        {
            atkFailedMsg = "しかし、Agiがラウンド攻撃条件を満たしていないため失敗";
            AddGameStates(GameState_En.AtkFailed);

            yield break;
        }

        //
        AddGameStates(GameState_En.Done_Atk_CheckAgi);
    }

    IEnumerator Corou_Atk_CheckHitSucc()
    {
        UnitInfo oriUnitInfo = oriUnit_Cp.unitInfo;
        UnitInfo tarUnitInfo = tarUnit_Cp.unitInfo;

        // check hit success

        //******************** 1th timing. before hit determine
        HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.BefHitDet);
        yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
        RemoveGameStates(GameState_En.GameEventsFinished);

        // step 4. check hit success (throw dice)
        btlController_Cp.ThrowDice(mPlayerID);
        yield return new WaitUntil(() => btlController_Cp.ExistGameStates(
            Controller_BattlePhase.GameState_En.DiceThrown));
        btlController_Cp.RemoveGameStates(Controller_BattlePhase.GameState_En.DiceThrown);

        int dice = btlController_Cp.dice;
        int targetAgi = tarUnit_Cp.unitCardData.agi;

        if (dice < targetAgi)
        {
            btlUI_Cp.SetNoticeEffText("失敗！");
            yield return new WaitForSeconds(noticeTxtDur);
            btlUI_Cp.FinishNoticeEffText();
            
            atkFailedMsg = "「攻撃失敗」相手のAGIを上回れなかった";
            AddGameStates(GameState_En.AtkFailed);

            atkSucc = false;
            isCritAtk = false;

            yield break;
        }
        else if (dice >= 12)
        {
            btlUI_Cp.SetNoticeEffText("クリティカル！");
            btlUI_Cp.SetActionEffText("「クリティカル！」ダイスの目が12以上");

            float dur = noticeTxtDur > actionEffTxtDur ? noticeTxtDur : actionEffTxtDur;
            yield return new WaitForSeconds(noticeTxtDur);

            btlUI_Cp.FinishNoticeEffText();
            btlUI_Cp.FinishActionEffText();

            atkSucc = true;
            isCritAtk = true;
        }
        else
        {
            btlUI_Cp.SetNoticeEffText("攻撃成功！");
            btlUI_Cp.SetActionEffText("「攻撃成功！」相手のAGIを上回った！");

            float dur = noticeTxtDur > actionEffTxtDur ? noticeTxtDur : actionEffTxtDur;
            yield return new WaitForSeconds(noticeTxtDur);

            btlUI_Cp.FinishNoticeEffText();
            btlUI_Cp.FinishActionEffText();

            atkSucc = true;
            isCritAtk = false;
        }

        if (dice >= 8)
        {
            //******************** 26th timing. hit success over 8
            HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.HitSuccOver8);
            yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
            RemoveGameStates(GameState_En.GameEventsFinished);
        }
        if (dice >= 6 && dice < 8)
        {
            //******************** 29th timing. hit success over 6
            HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.HitSuccOver6);
            yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
            RemoveGameStates(GameState_En.GameEventsFinished);
        }

        //******************** 2th timing. when hit determine
        HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.WhenHitDet);
        yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
        RemoveGameStates(GameState_En.GameEventsFinished);

        //******************** 30th timing. after hit determine, destroy item

        //
        AddGameStates(GameState_En.Done_Atk_CheckHitSucc);
    }

    IEnumerator Corou_Atk_CalcDmg()
    {
        UnitInfo oriUnitInfo = oriUnit_Cp.unitInfo;
        UnitInfo tarUnitInfo = tarUnit_Cp.unitInfo;

        // calculate damage
        int atkDmg = oriUnitInfo.atk;
        int defCorr = tarUnitInfo.def;
        int dmg = 0;

        if (isCritAtk)
        {
            dmg = atkDmg;
        }
        else
        {
            dmg = Mathf.Clamp(oriUnitInfo.atk - tarUnitInfo.defCorr, 0, int.MaxValue);
        }

        //******************** 23th timing. next take damage
        //HandleGameEventsTiming(tarUnit_Cp, GameEventsTiming.NextTakeDmg);
        //yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
        //RemoveGameStates(GameState_En.GameEventsFinished);

        // action to attack target unit
        gEventsHandler_Cp.GameAction_AtkTargetUnit();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.Done_AtkerReturned));
        RemoveGameStates(GameState_En.Done_AtkerReturned);

        //
        if (dmg > 0)
        {
            //******************** 33th timing. when take damage
            eUnitAbil_Cp.HandleGameEventsTiming(tarUnit_Cp, GameEventsTiming.TakeDmg);
            yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
            RemoveGameStates(GameState_En.GameEventsFinished);
        }

        // calculate hp
        int tarHp_pr = Mathf.Clamp(tarUnitInfo.curHp - dmg, 0, tarUnitInfo.maxHP);

        btlUI_Cp.SetNoticeEffText("HP : " + tarUnitInfo.curHp + " → " + tarHp_pr);
        tarUnit_Cp.SetHp(tarHp_pr);

        yield return new WaitUntil(() => tarUnit_Cp.ExistGameStates(UnitCard.GameState_En.HpProcFinished));
        tarUnit_Cp.RemoveGameStates(UnitCard.GameState_En.HpProcFinished);

        yield return new WaitForSeconds(noticeTxtDur);

        btlUI_Cp.FinishNoticeEffText();

        //
        if (dmg == 0)
        {
            //******************** 32th timing. Hp don't reduce
            eUnitAbil_Cp.HandleGameEventsTiming(tarUnit_Cp, GameEventsTiming.HpNoReduce);
            yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
            RemoveGameStates(GameState_En.GameEventsFinished);
        }

        //******************** 3th timing. after attack
        HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.AftAtk);
        yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
        RemoveGameStates(GameState_En.GameEventsFinished);

        //******************** 4th timing. after attack success
        HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.AftAtkSucc);
        yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
        RemoveGameStates(GameState_En.GameEventsFinished);

        // Kill target unit
        if (tarUnitInfo.curHp == 0)
        {
            eUnitAbil_Cp.HandleGameEventsTiming(tarUnit_Cp, GameEventsTiming.UnitDeath);
            yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
            RemoveGameStates(GameState_En.GameEventsFinished);
        }

        //
        AddGameStates(GameState_En.Done_Atk_CalcDmg);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle game events timing
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle game events timing

    //--------------------------------------------------
    public void HandleGameEventsTiming(UnitCard unit_Cp_pr, GameEventsTiming timing_pr)
    {
        gEventsHandler_Cp.HandleGameEventsTiming(timing_pr);
    }

    #endregion

    //-------------------------------------------------- Determine attack type
    void DeterAtkType()
    {
        // determine atk type
        UnitCardData oriUnitData = oriUnit_Cp.unitCardData;
        UnitCardData tarUnitData = tarUnit_Cp.unitCardData;

        if (roundValue.atkType == AttackType.Normal)
        {
            if (oriUnitData.nlAtk.ap >= playerAp)
            {
                atkType = AttackType.Normal;
            }
        }
        else if (roundValue.atkType == AttackType.Spc1)
        {
            if (oriUnitData.spcAtk1.ap >= playerAp)
            {
                if (oriUnitData.spcAtk1.sp <= roundValue.spMarkerCount)
                {
                    atkType = AttackType.Spc1;
                }
                else
                {
                    atkType = AttackType.Normal;
                }
            }
        }
        else if (roundValue.atkType == AttackType.Spc2)
        {
            if (oriUnitData.spcAtk2.ap >= playerAp)
            {
                if (oriUnitData.spcAtk2.sp <= roundValue.spMarkerCount)
                {
                    atkType = AttackType.Spc2;
                }
                else
                {
                    atkType = AttackType.Normal;
                }
            }
        }
        else if (roundValue.atkType == AttackType.Null)
        {
            atkType = AttackType.Null;
        }

        //
        oriUnit_Cp.unitInfo.occuredAtk = atkType;
    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Finish round action
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region FinishRoundAction

    //--------------------------------------------------
    public void FinishRound()
    {
        StartCoroutine(Corou_FinishRound());
    }

    IEnumerator Corou_FinishRound()
    {
        //
        AddGameStates(GameState_En.RoundWillFinish);

        //
        //******************** 37th timing. battle phase round end
        HandleGameEventsTiming(oriUnit_Cp, GameEventsTiming.RndEnd);

        yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
        RemoveGameStates(GameState_En.GameEventsFinished);

        //
        RemoveGameStates(GameState_En.RoundWillFinish);
        AddGameStates(GameState_En.RoundFinished);
    }

    #endregion

}
