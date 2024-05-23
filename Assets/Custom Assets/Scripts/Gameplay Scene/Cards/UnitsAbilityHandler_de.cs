using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class UnitsAbilityHandler_de : MonoBehaviour
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

        Done_Atk_CheckUnitVisi, Done_Atk_CheckAtkPoss, Done_Atk_CheckAgi, Done_Atk_CheckHitSucc,
        Done_Atk_CalcDmg,
        Will_Atk_CheckUnitVisi, Will_Atk_CheckAtkPoss, Will_Atk_CheckAgi, Will_Atk_CheckHitSucc,
        Will_Atk_CalcDmg,

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
    public UnitsAbilityHandler_de eUnitAbil_Cp;

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
    public List<RoundValue_de> roundsData = new List<RoundValue_de>();

    [SerializeField]
    [ReadOnly]
    public int curRoundIndex;

    [HideInInspector]
    public RoundValue_de roundValue = new RoundValue_de();

    [HideInInspector]
    public bool isLocalPlayer;

    [HideInInspector]
    public int mPlayerID, ePlayerID;

    [ReadOnly]
    public float actionEffTextDur, noticeTextDur;

    // playing parameters
    [SerializeField]
    [ReadOnly]
    public AttackType atkType = new AttackType();

    [ReadOnly]
    public AtkSuccType atkSucc = new AtkSuccType();

    [HideInInspector]
    public bool isCritAtk;

    [ReadOnly]
    public int actionTarUnitIndex;

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
        get { return HashHandler.instance.hashes; }
    }

    public int playerAp
    {
        get { return startController_Cp.playerAPs[mPlayerID]; }
        set { startController_Cp.playerAPs[mPlayerID] = playerAp; }
    }

    public GameEventsData curGEventsData
    {
        get { return mPlayer_Cp.curGEventsData; }
    }

    public GameEventsData waitGEventsData
    {
        get { return mPlayer_Cp.waitGEventsData; }
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
    public Hash128 RegRandHashValue()
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

        ResetParamaters();

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

        ePlayer_Cp = mPlayer_Cp.playerID == 0 ? controller_Cp.player_Cps_de[1] : controller_Cp.player_Cps_de[0];

        eUnitAbil_Cp = ePlayer_Cp.unitAbil_Cp;

        player_Cps = controller_Cp.player_Cps_de;

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
        actionEffTextDur = btlUI_Cp.actionTxtDur;

        noticeTextDur = btlUI_Cp.noticeTxtDur;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Start round action
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Start round action

    //--------------------------------------------------
    public void StartRoundAction(int rndId_pr, Hash128 hash_pr)
    {
        StartCoroutine(Corou_StartRoundAction(rndId_pr, hash_pr));
    }

    IEnumerator Corou_StartRoundAction(int rndId_pr, Hash128 hash_pr)
    {
        // init round action values
        curRoundIndex = rndId_pr;
        roundValue = roundsData[curRoundIndex];

        if (roundValue.action == ActionType.Shien)
        {
            oriUnit_Cp = roundValue.shienUnit_Cp;
            tarUnit_Cp = bUnit_Cps[roundValue.tarUnitIndex];
        }
        else
        {
            oriUnit_Cp = bUnit_Cps[roundValue.originUnitIndex];
            tarUnit_Cp = eBUnit_Cps[roundValue.tarUnitIndex];
        }

        hashStates.Remove(hash_pr);
        yield return null;
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

        //
        Hash128 hash_tp = RegRandHashValue();
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
            case ActionType.Atk:
                PlayAtkAction(hash_tp);
                break;
            default:
                yield return new WaitForSeconds(emptyRoundDur);
                AddGameStates(GameState_En.ActionDone);
                break;
        }

        yield return new WaitUntil(() => ExistGameStates(GameState_En.ActionDone)
            || !hashStates.Contains(hash_tp));
        RemoveGameStates(GameState_En.ActionDone);
        hashStates.Remove(hash_tp);

        //
        RemoveGameStates(GameState_En.ActionStarted);
        AddGameStates(GameState_En.ActionFinished);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play guard action
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play guard action

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
            gEventsHandler_Cp.GameActionEff_Guard(bUnit_Cps[i].effTri_Tf, guardEffDur);
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

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play shien action
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play shien action

    //-------------------------------------------------- Play shien action
    void PlayShienAction()
    {
        StartCoroutine(Corou_PlayShienAction());
    }

    IEnumerator Corou_PlayShienAction()
    {
        UnitCard shienUnit_Cp_tp = roundValue.shienUnit_Cp;

        // perform shien action
        gEventsHandler_Cp.GameAction_Shien(shienUnit_Cp_tp, tarUnit_Cp);
        yield return new WaitUntil(() => ExistGameStates(GameState_En.GameActionFinished));
        RemoveGameStates(GameState_En.GameActionFinished);

        //****************************** game events 3th. After shien apply
        Hash128 hash_tp = RegRandHashValue();
        HandleGameEventsTiming(GameEventsTiming.AftShienApply, hash_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash_tp));

        //
        AddGameStates(GameState_En.ActionDone);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play move action
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play move action

    //-------------------------------------------------- Play move action
    void PlayMoveAction()
    {
        StartCoroutine(Corou_PlayMoveAction());
    }

    IEnumerator Corou_PlayMoveAction()
    {
        //
        int oriUnitIndex_tp = roundValue.originUnitIndex + 2;
        int tarUnitIndex_tp = roundValue.tarUnitIndex;
        Hash128 hash_tp = RegRandHashValue();

        gEventsHandler_Cp.GameAction_Move(oriUnitIndex_tp, tarUnitIndex_tp, hash_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash_tp));

        //****************************** game events 1th. After move
        Hash128 hash2_tp = RegRandHashValue();
        HandleGameEventsTiming(GameEventsTiming.AftMove, hash2_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash2_tp));

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
    public void PlayAtkAction(Hash128 hash_pr)
    {
        StartCoroutine(Corou_PlayAtkAction(hash_pr));
    }

    public IEnumerator Corou_PlayAtkAction(Hash128 hash_pr)
    {
        //
        DeterAtkType();

        oriUnit_Cp.unitInfo.realAtk = atkType;

        //
        Hash128 hash3_tp = RegRandHashValue();
        StartCoroutine(Corou_Atk_UnitVisible(hash3_tp));
        yield return new WaitUntil(() => !hashStates.Contains(hash3_tp));

        //
        Hash128 hash4_tp = RegRandHashValue();
        StartCoroutine(Corou_Atk_CheckAtkPoss(hash4_tp));
        yield return new WaitUntil(() => !hashStates.Contains(hash4_tp));

        if (atkSucc == AtkSuccType.Failed)
        {
            hashStates.Remove(hash_pr);
            yield break;
        }

        //
        Hash128 hash5_tp = RegRandHashValue();
        StartCoroutine(Corou_Atk_CheckAgi(hash5_tp));
        yield return new WaitUntil(() => !hashStates.Contains(hash5_tp));

        if (atkSucc == AtkSuccType.Failed)
        {
            hashStates.Remove(hash_pr);
            yield break;
        }

        //
        Hash128 hash6_tp = RegRandHashValue();
        StartCoroutine(Corou_Atk_CheckHitSucc(hash6_tp));
        yield return new WaitUntil(() => !hashStates.Contains(hash6_tp));

        if (atkSucc == AtkSuccType.Failed)
        {
            hashStates.Remove(hash_pr);
            yield break;
        }

        //
        Hash128 hash7_tp = RegRandHashValue();
        StartCoroutine(Corou_Atk_CalcDmg(hash7_tp));
        yield return new WaitUntil(() => !hashStates.Contains(hash7_tp));

        //
        if (atkSucc == AtkSuccType.Succ)
        {
            //****************************** game events 7th. after atk success
            Hash128 hash2_tp = RegRandHashValue();
            HandleGameEventsTiming(GameEventsTiming.AftAtkSucc, hash2_tp);
            yield return new WaitUntil(() => !hashStates.Contains(hash2_tp));
        }

        //
        if (atkType == AttackType.Spc1 || atkType == AttackType.Spc2)
        {
            //****************************** game events 17th. after spc atk
            Hash128 hash2_tp = RegRandHashValue();
            HandleGameEventsTiming(GameEventsTiming.AftSpcAtk, hash2_tp);
            yield return new WaitUntil(() => !hashStates.Contains(hash2_tp));
        }

        //
        if (roundValue.atkNum == 0)
        {
            //****************************** game events 6th. after atk
            Hash128 hash_tp = RegRandHashValue();
            HandleGameEventsTiming(GameEventsTiming.AftAtk, hash_tp);
            yield return new WaitUntil(() => !hashStates.Contains(hash_tp));
        }

        //
        hashStates.Remove(hash_pr);
    }

    //-------------------------------------------------- Determine attack type
    public void DeterAtkType()
    {
        // determine atk type
        UnitCardData oriUnitData = oriUnit_Cp.unitCardData;
        atkType = AttackType.Null;

        if (roundValue.atkType == AttackType.Normal)
        {
            if (oriUnitData.nlAtk.ap <= playerAp)
            {
                atkType = AttackType.Normal;
            }
        }
        else if (roundValue.atkType == AttackType.Spc1)
        {
            if (oriUnitData.spcAtk1.ap <= playerAp)
            {
                if (oriUnitData.spcAtk1.sp <= roundValue.spMarkerCount)
                {
                    atkType = AttackType.Spc1;
                }
                else if (oriUnitData.nlAtk.ap <= playerAp)
                {
                    atkType = AttackType.Normal;
                }
            }
            else if (oriUnitData.nlAtk.ap <= playerAp)
            {
                atkType = AttackType.Normal;
            }
        }
        else if (roundValue.atkType == AttackType.Spc2)
        {
            if (oriUnitData.spcAtk2.ap <= playerAp)
            {
                if (oriUnitData.spcAtk2.sp <= roundValue.spMarkerCount)
                {
                    atkType = AttackType.Spc2;
                }
                else if (oriUnitData.nlAtk.ap <= playerAp)
                {
                    atkType = AttackType.Normal;
                }
            }
            else if (oriUnitData.nlAtk.ap <= playerAp)
            {
                atkType = AttackType.Normal;
            }
        }
    }

    //-------------------------------------------------- 
    IEnumerator Corou_Atk_UnitVisible(Hash128 hash_pr)
    {
        //
        UnitInfo_de oriUnitInfo = oriUnit_Cp.unitInfo;
        UnitInfo_de tarUnitInfo = tarUnit_Cp.unitInfo;

        // step 1. flip over the unit card
        if (!oriUnitInfo.visible)
        {
            // action that flip over the unit card
            Hash128 hash_tp = RegRandHashValue();
            gEventsHandler_Cp.GameAction_FlipOverCard(oriUnit_Cp, true, hash_tp);
            yield return new WaitUntil(() => !hashStates.Contains(hash_tp));

            oriUnit_Cp.SetHpVisible(true);

            // trigger atk abilities of my unit
            Hash128 hash3_tp = RegRandHashValue();
            Trigger_AtkAbilities(hash3_tp);
            yield return new WaitUntil(() => !hashStates.Contains(hash3_tp));

            // trigger unique ability of my unit
            Hash128 hash2_tp = RegRandHashValue();
            Trigger_UniqAbil(oriUnit_Cp, hash2_tp);
            yield return new WaitUntil(() => !hashStates.Contains(hash2_tp));

            //****************************** game events 18th. after unit reverse
            Hash128 hash4_tp = RegRandHashValue();
            HandleGameEventsTiming(GameEventsTiming.AftUnitRev, hash4_tp);
            yield return new WaitUntil(() => !hashStates.Contains(hash4_tp));
        }

        //
        hashStates.Remove(hash_pr);
    }

    //--------------------------------------------------
    IEnumerator Corou_Atk_CheckAtkPoss(Hash128 hash_pr)
    {
        // step 2. check attack possibility        
        if (atkType == AttackType.Null)
        {
            string msg = "APが足りません。";

            SetActionEffText(msg);
            yield return new WaitForSeconds(actionEffTextDur);
            FinishActionEffText();

            atkSucc = AtkSuccType.Failed;

            hashStates.Remove(hash_pr);
            yield break;
        }

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

        SetActionEffText(dsc);
        yield return new WaitForSeconds(actionEffTextDur);
        FinishActionEffText();

        //
        hashStates.Remove(hash_pr);
    }

    //--------------------------------------------------
    IEnumerator Corou_Atk_CheckAgi(Hash128 hash_pr)
    {
        UnitInfo_de oriUnitInfo = oriUnit_Cp.unitInfo;

        // step 3. check agility
        if (oriUnitInfo.agi < roundValue.minAgi)
        {
            string msg = "しかし、Agiがラウンド攻撃条件を満たしていないため失敗";

            SetActionEffText(msg);
            yield return new WaitForSeconds(actionEffTextDur);
            FinishActionEffText();

            atkSucc = AtkSuccType.Failed;

            hashStates.Remove(hash_pr);
            yield break;
        }

        //
        hashStates.Remove(hash_pr);
    }

    //--------------------------------------------------
    IEnumerator Corou_Atk_CheckHitSucc(Hash128 hash_pr)
    {
        //
        UnitInfo_de oriUnitInfo = oriUnit_Cp.unitInfo;
        UnitInfo_de tarUnitInfo = tarUnit_Cp.unitInfo;

        // step 1. flip over the enemy unit card
        if (!tarUnitInfo.visible)
        {
            Hash128 hash3_tp = RegRandHashValue();
            gEventsHandler_Cp.GameAction_FlipOverCard(tarUnit_Cp, true, hash3_tp);
            yield return new WaitUntil(() => !hashStates.Contains(hash3_tp));

            tarUnit_Cp.SetHpVisible(true);

            // trigger unique ability of enemy unit
            Hash128 hash4_tp = RegRandHashValue();
            eUnitAbil_Cp.Trigger_UniqAbil(tarUnit_Cp, hash4_tp);
            yield return new WaitUntil(() => !hashStates.Contains(hash4_tp));
        }

        //****************************** game events 4th. before deter hit success
        Hash128 hash_tp = RegRandHashValue();
        HandleGameEventsTiming(GameEventsTiming.BefHitDet, hash_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash_tp));

        // step 4. determin hit success (throw dice)
        btlController_Cp.ThrowDice(mPlayerID);
        yield return new WaitUntil(() => btlController_Cp.ExistGameStates(
            Controller_BattlePhase.GameState_En.DiceThrown));
        btlController_Cp.RemoveGameStates(Controller_BattlePhase.GameState_En.DiceThrown);

        int dice = btlController_Cp.dice;
        int targetAgi = tarUnit_Cp.unitCardData.agi;

        if (dice < targetAgi)
        {
            SetNoticeEffText("失敗！");
            SetActionEffText("「攻撃失敗」相手のAGIを上回れなかった");

            float dur = noticeTextDur > actionEffTextDur ? noticeTextDur : actionEffTextDur;
            yield return new WaitForSeconds(dur);

            FinishNoticeEffText();
            FinishActionEffText();

            //
            atkSucc = AtkSuccType.Failed;
            isCritAtk = false;

            hashStates.Remove(hash_pr);
            yield break;
        }
        else if (dice >= 12)
        {
            SetNoticeEffText("クリティカル！");
            SetActionEffText("「クリティカル！」ダイスの目が12以上");

            float dur = noticeTextDur > actionEffTextDur ? noticeTextDur : actionEffTextDur;
            yield return new WaitForSeconds(dur);

            FinishNoticeEffText();
            FinishActionEffText();

            atkSucc = AtkSuccType.Succ;
            isCritAtk = true;
        }
        else
        {
            SetNoticeEffText("攻撃成功！");
            SetActionEffText("「攻撃成功！」相手のAGIを上回った！");

            float dur = noticeTextDur > actionEffTextDur ? noticeTextDur : actionEffTextDur;
            yield return new WaitForSeconds(dur);

            FinishNoticeEffText();
            FinishActionEffText();

            atkSucc = AtkSuccType.Succ;
            isCritAtk = false;
        }

        //****************************** game events 5th. after deter hit success
        Hash128 hash2_tp = RegRandHashValue();
        HandleGameEventsTiming(GameEventsTiming.AftHitDet, hash2_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash2_tp));

        //
        hashStates.Remove(hash_pr);
    }

    //--------------------------------------------------
    IEnumerator Corou_Atk_CalcDmg(Hash128 hash_pr)
    {
        if (atkType == AttackType.Normal)
        {
            //****************************** game events 15th. before calc dmg on normal atk
            Hash128 hash3_tp = RegRandHashValue();
            HandleGameEventsTiming(GameEventsTiming.BefNlAtkDmgCalc, hash3_tp);
            yield return new WaitUntil(() => !hashStates.Contains(hash3_tp));
        }
        else if (atkType == AttackType.Spc1 || atkType == AttackType.Spc2)
        {
            //****************************** game events 16th. before calc dmg on special atk
            Hash128 hash4_tp = RegRandHashValue();
            HandleGameEventsTiming(GameEventsTiming.BefSpcAtkDmgCalc, hash4_tp);
            yield return new WaitUntil(() => !hashStates.Contains(hash4_tp));
        }

        UnitInfo_de oriUnitInfo = oriUnit_Cp.unitInfo;
        UnitInfo_de tarUnitInfo = tarUnit_Cp.unitInfo;

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

        //****************************** game events 23th. when dmg calc
        Hash128 hash_tp = RegRandHashValue();
        HandleGameEventsTiming(GameEventsTiming.WhenDmgCalc, hash_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash_tp));

        // action to attack target unit
        gEventsHandler_Cp.GameAction_AtkTargetUnit();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.Done_AtkerReturned));
        RemoveGameStates(GameState_En.Done_AtkerReturned);

        // calculate hp
        int tarHp_pr = Mathf.Clamp(tarUnitInfo.curHp - dmg, 0, tarUnitInfo.maxHP);

        SetNoticeEffText("HP : " + tarUnitInfo.curHp + " → " + tarHp_pr);
        tarUnit_Cp.SetHp(tarHp_pr);

        yield return new WaitUntil(() => tarUnit_Cp.ExistGameStates(UnitCard.GameState_En.HpProcFinished));
        tarUnit_Cp.RemoveGameStates(UnitCard.GameState_En.HpProcFinished);

        yield return new WaitForSeconds(noticeTextDur);

        FinishNoticeEffText();

        // handle rest sp markers
        Hash128 hash5_tp = RegRandHashValue();
        HandleRestSpMarkers(hash5_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash5_tp));

        //****************************** game events 24th. after dmg calc
        Hash128 hash2_tp = RegRandHashValue();
        HandleGameEventsTiming(GameEventsTiming.AftDmgCalc, hash2_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash2_tp));

        // Kill target unit
        if (tarUnitInfo.curHp == 0)
        {
            
        }

        //
        hashStates.Remove(hash_pr);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Trigger atk abilities
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Trigger atk abilities

    //--------------------------------------------------
    public void Trigger_AtkAbilities(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Trigger_AtkAbilities(hash_pr));
    }

    IEnumerator Corou_Trigger_AtkAbilities(Hash128 hash_pr)
    {
        Hash128 hash_tp = RegRandHashValue();

        if (atkType == AttackType.Normal)
        {
            Trigger_NlAbil(hash_tp);
        }
        else if (atkType == AttackType.Spc1)
        {
            Trigger_Spc1Abil(hash_tp);
        }
        else if (atkType == AttackType.Spc2)
        {
            Trigger_Spc2Abil(hash_tp);
        }
        else
        {
            hashStates.Remove(hash_tp);
        }

        yield return new WaitUntil(() => !hashStates.Contains(hash_tp));

        hashStates.Remove(hash_pr);
    }

    //--------------------------------------------------
    void Trigger_NlAbil(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Trigger_NlAbil(hash_pr));
    }

    IEnumerator Corou_Trigger_NlAbil(Hash128 hash_pr)
    {
        NormalAttack nlAbil_tp = oriUnit_Cp.unitCardData.nlAtk;

        //
        Hash128 hash_tp = RegRandHashValue();

        if (nlAbil_tp.contents.Count > 0)
        {
            gEventsHandler_Cp.SendMessage("Handle_NlAbil_" + nlAbil_tp.id.ToString() + "th", hash_tp);
        }
        else
        {
            hashStates.Remove(hash_tp);
        }

        yield return new WaitUntil(() => !hashStates.Contains(hash_tp));

        //
        hashStates.Remove(hash_pr);
    }

    //--------------------------------------------------
    void Trigger_Spc1Abil(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Trigger_Spc1Abil(hash_pr));
    }

    IEnumerator Corou_Trigger_Spc1Abil(Hash128 hash_pr)
    {
        SpecialAttack1 spc1Abil_tp = oriUnit_Cp.unitCardData.spcAtk1;

        //
        Hash128 hash_tp = RegRandHashValue();

        gEventsHandler_Cp.SendMessage("Handle_Spc1Abil_" + spc1Abil_tp.id.ToString() + "th", hash_tp);

        yield return new WaitUntil(() => !hashStates.Contains(hash_tp));

        //
        hashStates.Remove(hash_pr);
    }

    //--------------------------------------------------
    void Trigger_Spc2Abil(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Trigger_Spc2Abil(hash_pr));
    }

    IEnumerator Corou_Trigger_Spc2Abil(Hash128 hash_pr)
    {
        SpecialAttack2 spc2Abil_tp = oriUnit_Cp.unitCardData.spcAtk2;

        //
        Hash128 hash_tp = RegRandHashValue();

        gEventsHandler_Cp.SendMessage("Handle_Spc2Abil_" + spc2Abil_tp.id.ToString() + "th", hash_tp);

        yield return new WaitUntil(() => !hashStates.Contains(hash_tp));

        //
        hashStates.Remove(hash_pr);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Trigger unique ability
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Trigger unique ability

    //--------------------------------------------------
    public void Trigger_UniqAbil(UnitCard unit_Cp_pr, Hash128 hash_pr)
    {
        StartCoroutine(Corou_Trigger_UniqAbil(unit_Cp_pr, hash_pr));
    }

    IEnumerator Corou_Trigger_UniqAbil(UnitCard unit_Cp_pr, Hash128 hash_pr)
    {
        UniqueAbility uniqAbil = unit_Cp_pr.unitCardData.uniqAbil;

        //
        Hash128 hash_tp = RegRandHashValue();

        gEventsHandler_Cp.SendMessage("Handle_UniqAbil_" + uniqAbil.id.ToString() + "th", hash_tp);

        yield return new WaitUntil(() => !hashStates.Contains(hash_tp));

        //
        hashStates.Remove(hash_pr);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle game events timing
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle game events timing

    //--------------------------------------------------
    public void HandleGameEventsTiming(GameEventsTiming timing_pr, Hash128 hash_pr)
    {
        StartCoroutine(Corou_HandleGameEventsTiming(timing_pr, hash_pr));
    }

    IEnumerator Corou_HandleGameEventsTiming(GameEventsTiming timing_pr, Hash128 hash_pr)
    {
        //
        int waitGEventsCount = 0;
        if (waitGEventsData.Contains(timing_pr))
        {
            waitGEventsCount = waitGEventsData[timing_pr];
        }

        curGEventsData.InsertEvent(timing_pr);

        yield return new WaitUntil(() => curGEventsData[timing_pr] == waitGEventsCount);

        curGEventsData.Remove(timing_pr);

        hashStates.Remove(hash_pr);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle ui
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle ui

    //--------------------------------------------------
    void SetActionEffText(string dsc)
    {
        btlUI_Cp.SetActionEffText(dsc);
    }

    //--------------------------------------------------
    void FinishActionEffText()
    {
        btlUI_Cp.FinishActionEffText();
    }

    //--------------------------------------------------
    void SetNoticeEffText(string dsc)
    {
        btlUI_Cp.SetNoticeEffText(dsc);
    }

    //--------------------------------------------------
    void FinishNoticeEffText()
    {
        btlUI_Cp.FinishNoticeEffText();
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Functionalities
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Functionalities

    //--------------------------------------------------
    public void HandleRestSpMarkers(Hash128 hash_pr)
    {
        StartCoroutine(Corou_HandleRestSpMarkers(hash_pr));
    }

    IEnumerator Corou_HandleRestSpMarkers(Hash128 hash_pr)
    {
        // reduce sp markers count if atkType is spc
        if (atkSucc == AtkSuccType.Succ)
        {
            if (atkType == AttackType.Spc1)
            {
                mPlayer_Cp.markersData.usedSpMarkers.count += oriUnit_Cp.unitCardData.spcAtk1.sp;
                roundValue.spMarkerCount -= oriUnit_Cp.unitCardData.spcAtk1.sp;
            }
            else if (atkType == AttackType.Spc2)
            {
                mPlayer_Cp.markersData.usedSpMarkers.count += oriUnit_Cp.unitCardData.spcAtk2.sp;
                roundValue.spMarkerCount -= tarUnit_Cp.unitCardData.spcAtk2.sp;
            }
        }

        //
        hashStates.Remove(hash_pr);
        yield return null;
    }

    #endregion

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

        ResetParamaters();

        //
        RemoveGameStates(GameState_En.RoundWillFinish);
        AddGameStates(GameState_En.RoundFinished);
        yield return null;
    }

    //--------------------------------------------------
    void ResetParamaters()
    {
        atkType = AttackType.Null;
        atkSucc = AtkSuccType.Null;
        isCritAtk = false;
        actionTarUnitIndex = -1;
    }

    #endregion

}
