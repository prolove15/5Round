using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using cakeslice;
using System.Linq;
using DG.Tweening;
using UnityEditor.PackageManager;

public class UnitCard : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    // Types
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing, Finished,
        ZoomInOut,
        HpProcStarted, HpProcFinished,

    }

    public enum UnitPositionType_PartyDecision
    {
        Candidate, Van, Rear
    }

    public enum UnitPositionType_SetupStand
    {
        Candidate, Stand
    }

    public enum UnitPositionType_Phases
    {
        Mihari, Battle, Van, Rear,
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    // Fields
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField]
    GameObject costPanel_GO;

    [SerializeField]
    TextMeshProUGUI costText_Cp;

    [SerializeField]
    MeshRenderer frontSideMeshR_Cp, backSideMeshR_Cp;

    [SerializeField]
    Outline hlEffect_Cp;

    [SerializeField]
    MeshRenderer[] selectableMeshR_Cps;

    [SerializeField]
    Color enableColor, disableColor;

    [SerializeField]
    Sprite emptyFrontSide, emptyBackSide;

    [SerializeField]
    LongPressDetector longPressDetector_Cp;

    // phases scene
    [SerializeField]
    public Transform effTgr_Tf;

    [SerializeField]
    Transform hpPoint_Tf;

    [SerializeField]
    GameObject hp_Pf;

    [SerializeField]
    float hpTweenDur = 0.5f;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    //
    [ReadOnly]
    public int playerID;

    [ReadOnly]
    public int cardIndex;

    [ReadOnly]
    public UnitCardData unitCardData;

    // party decision section
    [ReadOnly]
    public UnitPositionType_PartyDecision posType_PartyDecision;

    [ReadOnly]
    public int posIndex_PartyDecision;

    // setup stand section
    [ReadOnly]
    public UnitPositionType_SetupStand posType_SetupStand;

    [ReadOnly]
    public int posIndex_SetupStand;

    // phases scene
    [ReadOnly]
    public UnitPositionType_Phases posType_Phases;

    [ReadOnly]
    public int posIndex_Phases;

    public UnitInfo unitInfo = new UnitInfo();

    public List<ItemCardData> equipItems = new List<ItemCardData>();

    public List<UnitCard> shienUnit_Cps = new List<UnitCard>();

    public bool m_placedPosture;

    //-------------------------------------------------- private fields
    //
    Controller_Phases phasesController_Cp;

    UI_BattlePhase btlUI_Cp;

    //
    Vector3 originPosBeforeZoom;

    Vector3 originScaleBeforeZoom;

    Quaternion originRotBeforeZoom;

    bool m_showToOpposite;

    [SerializeField]
    GameObject hp_GO;

    RectTransform hp_RT;

    float hpHeight;

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

    public int cost
    {
        set { costText_Cp.text = value.ToString(); }
    }

    public Sprite frontSide
    {
        get { return placedPosture ? unitCardData.frontSide : unitCardData.backSide; }
        set { frontSideMeshR_Cp.material.mainTexture = value.texture; }
    }

    public Sprite backSide
    {
        get { return placedPosture ? unitCardData.backSide : unitCardData.frontSide; }
        set { backSideMeshR_Cp.material.mainTexture = value.texture; }
    }

    public bool activate
    {
        get
        {
            return activeCollider;
        }
        set
        {
            activeCollider = value;

            activeEnableColor = value;
        }
    }

    public bool activeEnableColor
    {
        set
        {
            if (value)
            {
                for (int i = 0; i < selectableMeshR_Cps.Length; i++)
                {
                    selectableMeshR_Cps[i].material.color = enableColor;
                }
            }
            else
            {
                for (int i = 0; i < selectableMeshR_Cps.Length; i++)
                {
                    selectableMeshR_Cps[i].material.color = disableColor;
                }
            }
        }
    }

    public bool activeCollider
    {
        get { return hlEffect_Cp.GetComponent<Collider>().enabled; }
        set { hlEffect_Cp.GetComponent<Collider>().enabled = value; }
    }

    public bool activeCostPanel
    {
        get { return costPanel_GO.activeInHierarchy; }
        set { costPanel_GO.SetActive(value); }
    }

    public bool enableZoom
    {
        get { return longPressDetector_Cp.enableLongPress; }
        set { longPressDetector_Cp.enableLongPress = value; }
    }

    public bool enableLongPress
    {
        get { return longPressDetector_Cp.enableLongPress; }
        set { longPressDetector_Cp.enableLongPress = value; }
    }

    public bool enableClickDetect
    {
        get { return longPressDetector_Cp.enableClickDetect; }
        set { longPressDetector_Cp.enableClickDetect = value; }
    }

    public bool isHighlighted
    {
        get { return hlEffect_Cp.enabled; }
        set { hlEffect_Cp.enabled = value; }
    }

    public bool placedPosture
    {
        get { return m_placedPosture; }
        set
        {
            m_placedPosture = value;

            if (value)
            {
                frontSide = unitCardData.frontSide;
                backSide = unitCardData.backSide;
            }
            else
            {
                frontSide = unitCardData.backSide;
                backSide = unitCardData.frontSide;
            }
        }
    }

    public bool showToOpposite
    {
        get { return m_showToOpposite; }
        set
        {
            m_showToOpposite = value;

            for (int i = 0; i < selectableMeshR_Cps.Length; i++)
            {
                selectableMeshR_Cps[i].enabled = value;
            }
        }
    }

    //-------------------------------------------------- private properties
    GameObject controller_GO
    {
        get { return GameObject.FindWithTag("GameController"); }
    }

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
        if (enableZoom)
        {
            if (gameStates.Contains(GameState_En.ZoomInOut))
            {
                if (Input.GetMouseButtonDown(0) &&
                    !LongPressDetector.CheckObjectLineage(LongPressDetector.GetPointedObject(), transform))
                {
                    OnZoomInEvent();
                }
            }
        }
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
    // Initialize
    //////////////////////////////////////////////////////////////////////
    #region Initialize

    //------------------------------
    public void Init()
    {
        gameStates.Add(GameState_En.Nothing);

        InitComponents();

        mainGameState = GameState_En.Inited;
    }

    //------------------------------
    void InitComponents()
    {
        isHighlighted = false;

        InitLongPressDetector();
    }

    //------------------------------
    void InitLongPressDetector()
    {
        longPressDetector_Cp.onLongPress.AddListener(OnLongPressed);

        longPressDetector_Cp.onClicked.AddListener(OnClicked);
    }

    #endregion

    //------------------------------
    public void OnClicked()
    {
        if (gameStates.Contains(GameState_En.ZoomInOut))
        {
            return;
        }

        isHighlighted = !isHighlighted;

        controller_GO.SendMessage("OnUnitCardClicked", this);
    }

    //////////////////////////////////////////////////////////////////////
    // Zoom In/Out
    //////////////////////////////////////////////////////////////////////
    #region ZoomInOut

    //------------------------------
    void OnLongPressed()
    {
        OnZoomOutEvent();
    }

    void OnZoomOutEvent()
    {
        gameStates.Add(GameState_En.ZoomInOut);

        //
        originPosBeforeZoom = transform.position;
        originScaleBeforeZoom = transform.localScale;
        originRotBeforeZoom = transform.rotation;

        //
        UnityEvent unityEvent = new UnityEvent();
        unityEvent.AddListener(OnCompleteZoomOut);

        Vector3 newScale = new Vector3(3f, 3f, 3f);
        Quaternion adjustRot_tp = Quaternion.Euler(90f, 0f, 0f);

        controller_GO.SendMessage("ZoomInOutStarted");

        ZoomInOut.ZoomOut(transform, Camera.main, 0.2f, newScale, adjustRot_tp, unityEvent, 0.3f);
    }

    void OnCompleteZoomOut()
    {
        
    }

    void OnZoomInEvent()
    {
        UnityEvent unityEvent = new UnityEvent();
        unityEvent.AddListener(OnCompleteZoomIn);

        ZoomInOut.ZoomIn(transform, originPosBeforeZoom, originScaleBeforeZoom, originRotBeforeZoom,
            unityEvent, 0.3f);
    }

    void OnCompleteZoomIn()
    {
        controller_GO.SendMessage("ZoomInOutFinished");

        gameStates.Remove(GameState_En.ZoomInOut);
    }

    #endregion

    //------------------------------
    public void SetAllStates(int cardIndex_pr, Sprite frontSide_pr, Sprite backSide_pr, bool activeCostPanel_pr,
        int cost_pr, bool activeCollider_pr, bool activeEnableColor_pr)
    {
        cardIndex = cardIndex_pr;
        frontSide = frontSide_pr;
        backSide = backSide_pr;
        activeCostPanel = activeCostPanel_pr;
        cost = cost_pr;
        activeCollider = activeCollider;
        activeEnableColor = activeEnableColor_pr;
    }

    //------------------------------
    public void SetAsEmptyState()
    {
        SetAllStates(0, emptyFrontSide, emptyBackSide, false, 0, false, false);
    }

    //------------------------------
    public void SetAsVirtualState()
    {
        transform.localScale = Vector3.zero;
    }

    //------------------------------
    public void SetUnitDataFromUnitCardData(UnitCardData unitData_pr)
    {
        SetAllStates(unitData_pr.id, unitData_pr.frontSide, unitData_pr.backSide,
            true, unitData_pr.cost, true, true);
    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Phases scene
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region PhasesScene

    //--------------------------------------------------
    public void Init_Phases()
    {
        phasesController_Cp = controller_GO.GetComponent<Controller_Phases>();
        btlUI_Cp = phasesController_Cp.btlController_Cp.btlUI_Cp;
    }

    //--------------------------------------------------
    public void InitUnitInfo()
    {
        unitInfo.cost = unitCardData.cost;
        unitInfo.maxHP = unitCardData.hp;
        unitInfo.curHp = unitCardData.hp;
        unitInfo.baseAtk = unitCardData.atk;
        unitInfo.baseAgi = unitCardData.agi;

        unitInfo.unitData = unitCardData;
    }

    //--------------------------------------------------
    public void InitEquipItems()
    {
        Controller_Phases controller_Cp = controller_GO.GetComponent<Controller_Phases>();
        ItemCardsData itemCardsData = controller_Cp.dataManager_Cp.dataStorage.itemCardsData;
        equipItems.Add(itemCardsData.itemCards[0]);
        equipItems.Add(itemCardsData.itemCards[1]);
        equipItems.Add(itemCardsData.itemCards[2]);
    }

    //--------------------------------------------------
    public void InitHpPanel()
    {
        hp_GO = Instantiate(hp_Pf, hpPoint_Tf.transform);
        hp_GO.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        hp_RT = hp_GO.transform.Find("HpBar").GetComponent<RectTransform>();
        hpHeight = hp_RT.sizeDelta.y;
    }

    //--------------------------------------------------
    public void SetStatus_Phases(int playerID_pr,
        UnitCardData unitCardData_pr, bool visible_pr = true, bool showToOpposite_pr = true)
    {
        playerID = playerID_pr;
        frontSide = unitCardData_pr.frontSide;
        backSide = unitCardData_pr.backSide;
        cardIndex = unitCardData_pr.id;
        unitCardData = unitCardData_pr;
        SetVisible(visible_pr);
        showToOpposite = showToOpposite_pr;
    }

    //--------------------------------------------------
    public void SetVisible(bool flag)
    {
        unitInfo.visible = flag;
        placedPosture = flag;
    }

    //--------------------------------------------------
    public void SetHp(int hp_pr)
    {
        AddGameStates(GameState_En.HpProcStarted);

        //
        float hpRatioBegin = (float)(unitInfo.curHp) / (float)(unitInfo.maxHP);

        unitInfo.curHp = hp_pr;
        float hpRatioLast = (float)(hp_pr) / (float)(unitInfo.maxHP);
        
        //
        DOTween.To(() => hpRatioBegin, x => hpRatioBegin = x, hpRatioLast, hpTweenDur)
            .OnUpdate(() => { hp_RT.sizeDelta = new Vector2(hp_RT.sizeDelta.x, hpRatioBegin * hpHeight); })
            .OnComplete(() => { OnComplete_SetHp(); });
    }

    void OnComplete_SetHp()
    {
        RemoveGameStates(GameState_En.HpProcStarted);
        AddGameStates(GameState_En.HpProcFinished);
    }

    //--------------------------------------------------
    public void SetHpVisible(bool visible_pr)
    {
        hp_GO.SetActive(visible_pr);

        unitInfo.hpVisible = visible_pr;
    }

    //--------------------------------------------------
    void HandleHpPanel()
    {
        if (!unitInfo.hpVisible)
        {
            return;
        }

        Vector3 hpScreenPoint_tp = Camera.main.WorldToScreenPoint(hpPoint_Tf.position);
        Vector3 unitScreenPoint_tp = Camera.main.WorldToScreenPoint(transform.position);

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(unitScreenPoint_tp), out hit))
        {
            if (hit.transform.gameObject.GetComponentInParent<UnitCard>() == this)
            {
                hp_GO.SetActive(true);
            }
            else
            {
                hp_GO.SetActive(false);
            }
        }

        hp_GO.transform.SetPositionAndRotation(hpScreenPoint_tp, Quaternion.identity);
    }

    #endregion

}
