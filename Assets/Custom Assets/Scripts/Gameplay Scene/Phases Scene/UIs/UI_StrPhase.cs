using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StrPhase : MonoBehaviour
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
        OnPlayerboardPanel, OnActionWindowPanel, OnMiharidaiPanel, OnBattleboardPanel, OnCardDetailPanel,
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
    public GameObject playerBPanel_GO;

    [SerializeField]
    public GameObject actionWPanel_GO;

    [SerializeField]
    public GameObject miharidaiPanel_GO;

    [SerializeField]
    public GameObject battleBPanel_GO;

    [SerializeField]
    public GameObject cardDetailPanel_GO;

    // playerboard panel
    [SerializeField]
    public Text pb_apText_Cp, pb_spMarkerText_Cp, pb_shienText_Cp, pb_move1Text_Cp, pb_move2Text_Cp,
        pb_move3Text_Cp, pb_atk1Text_Cp, pb_atk2Text_Cp;

    // action window panel
    [SerializeField]
    public Button aw_guardBtn_Cp, aw_shienBtn_Cp, aw_moveBtn_Cp, aw_atkBtn_Cp;

    [SerializeField]
    Text roundIndexText_Cp;

    [SerializeField]
    public GameObject aw_guardPanel_GO, aw_shienPanel_GO, aw_movePanel_GO, aw_atkPanel_GO;

    [SerializeField]
    public GameObject aw_p1bUnitsPanel_GO, aw_p2bUnitsPanel_GO;

    [SerializeField]
    public Text aw_gu_guardDesText_Cp, aw_gu_spMarkerText_Cp;

    [SerializeField]
    public Button aw_gu_incBtn_Cp, aw_gu_decBtn_Cp;

    [SerializeField]
    public Text aw_sh_descText_Cp, aw_sh_unitText_Cp, aw_sh_shienText_Cp;

    [SerializeField]
    public GameObject aw_sh_selectUnitsPanel_GO;

    [SerializeField]
    public GameObject aw_sh_mihariUnitsGroup_GO;

    [SerializeField]
    public GameObject aw_sh_van1Bgd_GO, aw_sh_van2Bgd_GO;

    [SerializeField]
    Image aw_sh_van1Bgd_Cp, aw_sh_van2Bgd_Cp;

    [SerializeField]
    public Text aw_mo_descText_Cp;

    [SerializeField]
    public GameObject aw_mo_van1Bgd_GO, aw_mo_van2Bgd_GO, aw_mo_rear1Bgd_GO, aw_mo_rear2Bgd_GO, aw_mo_rear3Bgd_GO;

    [SerializeField]
    Image aw_mo_van1Bgd_Cp, aw_mo_van2Bgd_Cp;

    [SerializeField]
    Button aw_mo_rear1Btn_Cp, aw_mo_rear2Btn_Cp, aw_mo_rear3Btn_Cp;

    [SerializeField]
    public RectTransform aw_mo_arrow_RT;

    [SerializeField]
    public Text aw_at_descText_Cp;

    [SerializeField]
    public GameObject aw_at_allyVanBgd1_GO, aw_at_allyVanBgd2_GO, aw_at_enemyVanBgd1_GO, aw_at_enemyVanBgd2_GO;

    [SerializeField]
    Button aw_at_allyVan1Btn_Cp, aw_at_allyVan2Btn_Cp;

    [SerializeField]
    Image aw_at_allyVan1Bgd_Cp, aw_at_allyVan2Bgd_Cp, aw_at_enemyVan1Bgd_Cp, aw_at_enemyVan2Bgd_Cp;

    [SerializeField]
    public RectTransform aw_at_allyVan1ArrowPoint_RT, aw_at_allyVan2ArrowPoint_RT, aw_at_enemyVan1ArrowPoint_RT,
        aw_at_enemyVan2ArrorPoint_RT;

    [SerializeField]
    public RectTransform aw_at_arrow_RT;

    [SerializeField]
    public Text aw_at_normalAtkText_Cp, aw_at_spc1AtkText_Cp, aw_at_spc2AtkText_Cp;

    [SerializeField]
    public GameObject aw_at_nlAtkBgd_GO, aw_at_spc1AtkBgd_GO, aw_at_spc2AtkBgd_GO;

    // battleboard panel
    [SerializeField]
    public Text bb_discardUnitText_Cp;

    [SerializeField]
    public Text bb_p1mihariText_Cp, bb_p1kenText_Cp, bb_p1maText_Cp, bb_p1yumiText_Cp,
        bb_p1fushiText_Cp, bb_p1ryuText_Cp;

    [SerializeField]
    public Text bb_p2mihariText_Cp, bb_p2kenText_Cp, bb_p2maText_Cp, bb_p2yumiText_Cp,
        bb_p2fushiText_Cp, bb_p2ryuText_Cp;

    [SerializeField]
    public Transform bb_p1UnitUIsGroup_Tf, bb_p2UnitUIsGroup_Tf;

    // card details panel
    [SerializeField]
    public Image cd_cardImage_Cp;

    [SerializeField]
    public Text cd_cardNameText_Cp;

    [SerializeField]
    public Text cd_costText_Cp, cd_atrText_Cp, cd_hpText_Cp, cd_atkText_Cp, cd_agiText_Cp,
        cd_defCorrText_Cp, cd_accuracyCorrText_Cp, cd_CTCorrText_Cp, cd_normalAtkCorrText_Cp, cd_spcAtkCorrText_Cp,
        cd_dmgCorrText_Cp, cd_indirDmgCorrText_Cp, cd_shienEffectCorrText_Cp,
        cd_diceEffectCorrText_Cp;

    [SerializeField]
    public RectTransform cd_equipItemsContent_RT;

    [SerializeField]
    GameObject cd_item_Pf;

    // faction images
    [SerializeField]
    Sprite p1Van1Sprite, p1Van2Sprite, p2Van1Sprite, p2Van2Sprite;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    // action window
    [ReadOnly]
    public List<UnitUI_Phases> aw_sh_mUnit_Cps = new List<UnitUI_Phases>();

    [ReadOnly]
    public List<UnitUI_Phases> aw_p1bUnitUI_Cps = new List<UnitUI_Phases>();

    [ReadOnly]
    public List<UnitUI_Phases> aw_p2bUnitUI_Cps = new List<UnitUI_Phases>();

    // battlebaord
    [ReadOnly]
    public List<UnitUI_Phases> bb_p1UnitUI_Cps = new List<UnitUI_Phases>();

    [ReadOnly]
    public List<UnitUI_Phases> bb_p2UnitUI_Cps = new List<UnitUI_Phases>();

    [ReadOnly]
    public List<GameObject> cd_equipItem_GOs = new List<GameObject>();

    public float equipItemInterval = 0.1f;

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;

    Controller_StrPhase strController_Cp;

    DataManager_Gameplay dataManager_Cp;

    StatusManager statusManager_Cp;

    List<Player_Phases> player_Cps = new List<Player_Phases>();

    Player_Phases localPlayer_Cp, otherPlayer_Cp;

    int localPlayerID;

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

        strController_Cp = controller_Cp.strController_Cp;

        dataManager_Cp = controller_Cp.dataManager_Cp;

        statusManager_Cp = controller_Cp.statusManager_Cp;

        player_Cps = controller_Cp.player_Cps;

        localPlayer_Cp = controller_Cp.localPlayer_Cp;

        otherPlayer_Cp = controller_Cp.otherPlayer_Cp;

        localPlayerID = controller_Cp.localPlayerID;
    }

    //--------------------------------------------------
    void InitVariables()
    {
        //
        DisableAllPanel();

        //
        InitPlayerboardPanel();

        InitActionWindowPanel();

        InitBattleboardPanel();
    }

    //--------------------------------------------------
    void InitPlayerboardPanel()
    {
        RefreshPbPlayerInfoPanel();
    }

    //--------------------------------------------------
    void InitActionWindowPanel()
    {
        //
        aw_guardPanel_GO.SetActive(false);
        aw_shienPanel_GO.SetActive(false);
        aw_movePanel_GO.SetActive(false);
        aw_atkPanel_GO.SetActive(false);

        // init guard panel
        RefreshAwGuardPanel();

        // init shien panel
        aw_sh_mUnit_Cps = new List<UnitUI_Phases>(aw_sh_mihariUnitsGroup_GO.GetComponentsInChildren<UnitUI_Phases>());
        for (int i = 0; i < aw_sh_mUnit_Cps.Count; i++)
        {
            int index = i;
            aw_sh_mUnit_Cps[i].GetComponent<Button>().onClick.AddListener(() => On_Aw_Sh_ShienUnitBtn(index));
        }
        aw_sh_van1Bgd_Cp.sprite = localPlayerID == 0 ? p1Van1Sprite : p2Van1Sprite;
        aw_sh_van2Bgd_Cp.sprite = localPlayerID == 0 ? p1Van2Sprite : p2Van2Sprite;

        RefreshAwShienPanel();

        RefreshAwMihariUnits();

        // init move panel
        aw_mo_van1Bgd_Cp.sprite = localPlayerID == 0 ? p1Van1Sprite : p2Van1Sprite;
        aw_mo_van2Bgd_Cp.sprite = localPlayerID == 0 ? p1Van2Sprite : p2Van2Sprite;

        RefreshAwMovePanel();

        // init atk panel
        if (localPlayerID == 0)
        {
            aw_at_allyVan1Bgd_Cp.sprite = p1Van1Sprite;
            aw_at_allyVan2Bgd_Cp.sprite = p1Van2Sprite;
            aw_at_enemyVan1Bgd_Cp.sprite = p2Van1Sprite;
            aw_at_enemyVan2Bgd_Cp.sprite = p2Van2Sprite;
        }
        else if (localPlayerID == 1)
        {
            aw_at_allyVan1Bgd_Cp.sprite = p2Van1Sprite;
            aw_at_allyVan2Bgd_Cp.sprite = p2Van2Sprite;
            aw_at_enemyVan1Bgd_Cp.sprite = p1Van1Sprite;
            aw_at_enemyVan2Bgd_Cp.sprite = p1Van2Sprite;
        }

        RefreshAwAtkPanel();

        //// init battle board (removed due to request of client)
        //aw_p1bUnitUI_Cps = new List<UnitUI_Phases>(aw_p1bUnitsPanel_GO.GetComponentsInChildren<UnitUI_Phases>());
        //for (int i = 0; i < aw_p1bUnitUI_Cps.Count; i++)
        //{
        //    int index = i;
        //    aw_p1bUnitUI_Cps[i].GetComponent<Button>().onClick.AddListener(() => On_Aw_BUnitUI(0, index));
        //}

        //aw_p2bUnitUI_Cps = new List<UnitUI_Phases>(aw_p2bUnitsPanel_GO.GetComponentsInChildren<UnitUI_Phases>());
        //for (int i = 0; i < aw_p2bUnitUI_Cps.Count; i++)
        //{
        //    int index = i;
        //    aw_p2bUnitUI_Cps[i].GetComponent<Button>().onClick.AddListener(() => On_Aw_BUnitUI(1, index));
        //}

        //RefreshAwBattleUnits();
    }

    //--------------------------------------------------
    void InitBattleboardPanel()
    {
        //
        bb_p1UnitUI_Cps = new List<UnitUI_Phases>(bb_p1UnitUIsGroup_Tf.GetComponentsInChildren<UnitUI_Phases>());
        for (int i = 0; i < bb_p1UnitUI_Cps.Count; i++)
        {
            int index = i;
            bb_p1UnitUI_Cps[i].GetComponent<Button>().onClick.AddListener(() => On_Bb_Unit(0, index));
        }

        bb_p2UnitUI_Cps = new List<UnitUI_Phases>(bb_p2UnitUIsGroup_Tf.GetComponentsInChildren<UnitUI_Phases>());
        for (int i = 0; i < bb_p2UnitUI_Cps.Count; i++)
        {
            int index = i;
            bb_p2UnitUI_Cps[i].GetComponent<Button>().onClick.AddListener(() => On_Bb_Unit(1, index));
        }

        //
        RefreshBattleInfoPanel();

        RefreshBattleboardUnits();
    }

    #endregion

    //-------------------------------------------------- Disable all UI panel
    public void DisableAllPanel()
    {
        playerBPanel_GO.SetActive(false);
        actionWPanel_GO.SetActive(false);
        miharidaiPanel_GO.SetActive(false);
        battleBPanel_GO.SetActive(false);
        cardDetailPanel_GO.SetActive(false);
    }

    //-------------------------------------------------- Refresh action window
    public void RefreshPbPlayerInfoPanel()
    {
        //
        Controller_StartPhase startController_Cp_tp = controller_Cp.startController_Cp;
        int ap = startController_Cp_tp.playerAPs[localPlayerID];

        //
        MarkersData markersData = localPlayer_Cp.markersData;

        int usedSpCount = markersData.usedSpMarkers.count;
        int totalSpCount = markersData.totalSpMarkers.count;

        //
        TokensData tokensData = localPlayer_Cp.tokensData;

        int usedShienTokenCount = tokensData.usedShienToken.count;
        int totalShienTokenCount = tokensData.totalShienToken.count;

        int usedMove1TokenCount = tokensData.usedMove1Token.count;
        int totalMove1TokenCount = tokensData.totalMove1Token.count;
        int usedMove2TokenCount = tokensData.usedMove2Token.count;
        int totalMove2TokenCount = tokensData.totalMove2Token.count;
        int usedMove3TokenCount = tokensData.usedMove3Token.count;
        int totalMove3TokenCount = tokensData.totalMove3Token.count;

        int usedAtk1TokenCount = tokensData.usedAtk1Token.count;
        int totalAtk1TokenCount = tokensData.totalAtk1Token.count;
        int usedAtk2TokenCount = tokensData.usedAtk2Token.count;
        int totalAtk2TokenCount = tokensData.totalAtk2Token.count;

        //
        pb_apText_Cp.text = "AP : " + ap + "/" + ap;
        pb_spMarkerText_Cp.text = "SPマ ーカー : " + usedSpCount + "/" + totalSpCount;
        pb_shienText_Cp.text = "しえん : " + usedShienTokenCount + "/" + totalShienTokenCount;
        pb_move1Text_Cp.text = "いどう1 : " + usedMove1TokenCount + "/" + totalMove1TokenCount;
        pb_move2Text_Cp.text = "いどう2 : " + usedMove2TokenCount + "/" + totalMove2TokenCount;
        pb_move3Text_Cp.text = "いどう3 : " + usedMove3TokenCount + "/" + totalMove3TokenCount;
        pb_atk1Text_Cp.text = "こうげき" + (localPlayerID == 0 ? "(赤)" : "青") + " : "
            + usedAtk1TokenCount + "/" + totalAtk1TokenCount;
        pb_atk2Text_Cp.text = "こうげき" + (localPlayerID == 0 ? "(赤)" : "緑") + " : "
            + usedAtk2TokenCount + "/" + totalAtk2TokenCount;
    }

    //-------------------------------------------------- Reset action window UI panel
    public void RefreshActionWindow()
    {
        RefreshAwGuardPanel();
        RefreshAwShienPanel();
        RefreshAwMovePanel();
        RefreshAwAtkPanel();
        RefreshAwMihariUnits();
    }

    public void RefreshAwGuardPanel()
    {
        //
        aw_gu_guardDesText_Cp.text = "ラウンド中\r\n前衛DEF+" + localPlayer_Cp.markersData.usedSpMarkers.count;

        //
        aw_gu_spMarkerText_Cp.text = localPlayer_Cp.markersData.usedSpMarkers.count
            + "/" + localPlayer_Cp.markersData.totalSpMarkers.count + " 使用";

        // set active inc/dec button
        if (localPlayer_Cp.markersData.usedSpMarkers.count == 0)
        {
            aw_gu_decBtn_Cp.interactable = false;
        }
        else
        {
            aw_gu_decBtn_Cp.interactable = true;
        }

        if (localPlayer_Cp.roundsData[strController_Cp.selectedRoundIndex].spMarkerCount == 0)
        {
            aw_gu_decBtn_Cp.interactable = false;
        }

        if (localPlayer_Cp.markersData.usedSpMarkers.count == localPlayer_Cp.markersData.totalSpMarkers.count)
        {
            aw_gu_incBtn_Cp.interactable = false;
        }
        else
        {
            aw_gu_incBtn_Cp.interactable = true;
        }
    }

    public void RefreshAwShienPanel()
    {
        RoundValue roundValue = localPlayer_Cp.roundsData[strController_Cp.selectedRoundIndex];

        // check token count
        if (localPlayer_Cp.tokensData.usedShienToken.count == localPlayer_Cp.tokensData.totalShienToken.count
            && roundValue.token.type != TokenType.Shien)
        {
            aw_shienBtn_Cp.interactable = false;
        }
        else
        {
            aw_shienBtn_Cp.interactable = true;
        }

        //
        if (roundValue.token.type != TokenType.Shien)
        {
            aw_sh_descText_Cp.text = string.Empty;
            aw_sh_unitText_Cp.text = "ユニット : None";
            aw_sh_shienText_Cp.text = string.Empty;

            aw_sh_van1Bgd_GO.SetActive(false);
            aw_sh_van2Bgd_GO.SetActive(false);
        }
        else
        {
            UnitCardData unitData = roundValue.shienUnit_Cp.unitCardData;
            aw_sh_descText_Cp.text = unitData.shienDes_Leg;
            aw_sh_unitText_Cp.text = "ユニット : " + unitData.name;
            aw_sh_shienText_Cp.text = "しえん : " + unitData.shienName_Leg;

            if (roundValue.targetUnitIndex == 0)
            {
                aw_sh_van1Bgd_GO.SetActive(true);
                aw_sh_van2Bgd_GO.SetActive(false);
            }
            else if (roundValue.targetUnitIndex == 1)
            {
                aw_sh_van1Bgd_GO.SetActive(false);
                aw_sh_van2Bgd_GO.SetActive(true);
            }
        }
    }

    public void RefreshAwMovePanel()
    {
        RoundValue roundValue = localPlayer_Cp.roundsData[strController_Cp.selectedRoundIndex];

        // check token count
        TokensData tokensData = localPlayer_Cp.tokensData;

        if (tokensData.usedMoveToken.count == tokensData.totalMoveToken.count
            && roundValue.token.type != TokenType.Move)
        {
            aw_moveBtn_Cp.interactable = false;
        }
        else
        {
            aw_moveBtn_Cp.interactable = true;
        }

        if (tokensData.usedMove1Token.count == tokensData.totalMove1Token.count)
        {
            aw_mo_rear1Btn_Cp.interactable = false;
        }
        else
        {
            aw_mo_rear1Btn_Cp.interactable = true;
        }

        if (tokensData.usedMove2Token.count == tokensData.totalMove2Token.count)
        {
            aw_mo_rear2Btn_Cp.interactable = false;
        }
        else
        {
            aw_mo_rear2Btn_Cp.interactable = true;
        }

        if (tokensData.usedMove3Token.count == tokensData.totalMove3Token.count)
        {
            aw_mo_rear3Btn_Cp.interactable = false;
        }
        else
        {
            aw_mo_rear3Btn_Cp.interactable = true;
        }

        //
        if (roundValue.token.type != TokenType.Move)
        {
            aw_mo_descText_Cp.text = string.Empty;

            aw_mo_van1Bgd_GO.SetActive(false);
            aw_mo_van2Bgd_GO.SetActive(false);
            aw_mo_rear1Bgd_GO.SetActive(false);
            aw_mo_rear2Bgd_GO.SetActive(false);
            aw_mo_rear3Bgd_GO.SetActive(false);

            aw_mo_arrow_RT.gameObject.SetActive(false);
        }
        else
        {
            aw_mo_descText_Cp.text = roundValue.targetUnitIndex == 0 ? "赤" : "青" + "が\r\n"
                + "後衛" + (roundValue.originUnitIndex + 1).ToString() + "と入替";

            aw_mo_van1Bgd_GO.SetActive(false);
            aw_mo_van2Bgd_GO.SetActive(false);
            if (roundValue.targetUnitIndex == 0)
            {
                aw_mo_van1Bgd_GO.SetActive(true);
            }
            else if (roundValue.targetUnitIndex == 1)
            {
                aw_mo_van2Bgd_GO.SetActive(true);
            }

            aw_mo_rear1Bgd_GO.SetActive(false);
            aw_mo_rear2Bgd_GO.SetActive(false);
            aw_mo_rear3Bgd_GO.SetActive(false);
            if (roundValue.originUnitIndex == 0)
            {
                aw_mo_rear1Bgd_GO.SetActive(true);
            }
            else if (roundValue.originUnitIndex == 1)
            {
                aw_mo_rear2Bgd_GO.SetActive(true);
            }
            else if (roundValue.originUnitIndex == 2)
            {
                aw_mo_rear3Bgd_GO.SetActive(true);
            }

            aw_mo_arrow_RT.gameObject.SetActive(false); // it should be fixed
        }
    }

    public void RefreshAwAtkPanel()
    {
        RoundValue roundValue = localPlayer_Cp.roundsData[strController_Cp.selectedRoundIndex];

        // check token count
        TokensData tokensData = localPlayer_Cp.tokensData;

        if (tokensData.usedAtkToken.count == tokensData.totalAtkToken.count
            && roundValue.token.type != TokenType.Attack)
        {
            aw_atkBtn_Cp.interactable = false;
        }
        else
        {
            aw_atkBtn_Cp.interactable = true;
        }

        if (tokensData.usedAtk1Token.count == tokensData.totalAtk1Token.count)
        {
            aw_at_allyVan1Btn_Cp.interactable = false;
        }
        else
        {
            aw_at_allyVan1Btn_Cp.interactable = true;
        }

        if (tokensData.usedAtk2Token.count == tokensData.totalAtk2Token.count)
        {
            aw_at_allyVan2Btn_Cp.interactable = false;
        }
        else
        {
            aw_at_allyVan2Btn_Cp.interactable = true;
        }

        //
        if (roundValue.token.type != TokenType.Attack)
        {
            aw_at_descText_Cp.text = string.Empty;

            aw_at_allyVanBgd1_GO.SetActive(false);
            aw_at_allyVanBgd2_GO.SetActive(false);
            aw_at_enemyVanBgd1_GO.SetActive(false);
            aw_at_enemyVanBgd2_GO.SetActive(false);
            aw_at_arrow_RT.gameObject.SetActive(false);

            aw_at_normalAtkText_Cp.text = "通常\r\n";
            aw_at_spc1AtkText_Cp.text = "特殊1\r\n";
            aw_at_spc2AtkText_Cp.text = "特殊2\r\n";
        }
        else
        {
            aw_at_descText_Cp.text = (roundValue.originUnitIndex == 0 ? "赤" : "紫") + "が"
                + (roundValue.targetUnitIndex == 0 ? "青" : "緑") + "へ攻撃";

            aw_at_allyVanBgd1_GO.SetActive(false);
            aw_at_allyVanBgd2_GO.SetActive(false);
            switch (roundValue.originUnitIndex)
            {
                case 0:
                    aw_at_allyVanBgd1_GO.SetActive(true);
                    break;
                case 1:
                    aw_at_allyVanBgd2_GO.SetActive(true);
                    break;
            }

            aw_at_enemyVanBgd1_GO.SetActive(false);
            aw_at_enemyVanBgd2_GO.SetActive(false);
            switch (roundValue.targetUnitIndex)
            {
                case 0:
                    aw_at_enemyVanBgd1_GO.SetActive(true);
                    break;
                case 1:
                    aw_at_enemyVanBgd2_GO.SetActive(true);
                    break;
            }

            aw_at_arrow_RT.gameObject.SetActive(false); // it will be fixed
        }

        //
        Reset_Aw_At_AtkMethodBgds();
        SetActive_Aw_At_AtkMethodBgd(roundValue.atkType, true);
    }

    public void RefreshAwMihariUnits()
    {
        //
        for (int i = 0; i < aw_sh_mUnit_Cps.Count; i++)
        {
            aw_sh_mUnit_Cps[i].gameObject.SetActive(true);

            if (localPlayer_Cp.mUnit_Cps[i] != null)
            {
                aw_sh_mUnit_Cps[i].frontSprite = localPlayer_Cp.mUnit_Cps[i].unitCardData.frontSide;
            }
        }

        //
        List<RoundValue> roundsData_tp = localPlayer_Cp.roundsData;
        for (int i = 0; i < roundsData_tp.Count; i++)
        {
            if (roundsData_tp[i].token.type == TokenType.Shien)
            {
                aw_sh_mUnit_Cps[roundsData_tp[i].originUnitIndex].gameObject.SetActive(false);
            }
        }
    }

    public void RefreshAwBattleUnits()
    {
        for (int i = 0; i < aw_p1bUnitUI_Cps.Count; i++)
        {
            if (localPlayerID == 0)
            {
                aw_p1bUnitUI_Cps[i].frontSprite = player_Cps[0].bUnit_Cps[i].unitCardData.frontSide;
            }
            else
            {
                aw_p1bUnitUI_Cps[i].frontSprite = player_Cps[0].bUnit_Cps[i].frontSide;
            }
        }

        for (int i = 0; i < aw_p2bUnitUI_Cps.Count; i++)
        {
            if (localPlayerID == 1)
            {
                aw_p2bUnitUI_Cps[i].frontSprite = player_Cps[1].bUnit_Cps[i].unitCardData.frontSide;
            }
            else
            {
                aw_p2bUnitUI_Cps[i].frontSprite = player_Cps[1].bUnit_Cps[i].frontSide;
            }
        }
    }

    //--------------------------------------------------
    public void RefreshBattleInfoPanel()
    {
        //
        BattleInfo localBInfo = localPlayer_Cp.battleInfo;

        bb_discardUnitText_Cp.text = "捨て札 : " + localBInfo.discardUnitCount.ToString();

        bb_p1mihariText_Cp.text = "みはり : " + localBInfo.mihariUnitCount.ToString();
        bb_p1kenText_Cp.text = "けん : " + localBInfo.ken + " (死 : " + localBInfo.deadKen + ")";
        bb_p1maText_Cp.text = "ま : " + localBInfo.ma + " (死 : " + localBInfo.deadMa + ")";
        bb_p1yumiText_Cp.text = "ゆみ : " + localBInfo.yumi + " (死 : " + localBInfo.deadYumi + ")";
        bb_p1fushiText_Cp.text = "ふし : " + localBInfo.fushi + " (死 : " + localBInfo.deadFushi + ")";
        bb_p1ryuText_Cp.text = "りゅう : " + localBInfo.ryu + " (死 : " + localBInfo.deadRyu + ")";

        //
        BattleInfo otherBInfo = otherPlayer_Cp.battleInfo;

        bb_p2mihariText_Cp.text = "みはり : " + otherBInfo.mihariUnitCount.ToString();
        bb_p2kenText_Cp.text = "けん : " + otherBInfo.ken + " (死 : " + otherBInfo.deadKen + ")";
        bb_p2maText_Cp.text = "ま : " + otherBInfo.ma + " (死 : " + otherBInfo.deadMa + ")";
        bb_p2yumiText_Cp.text = "ゆみ : " + otherBInfo.yumi + " (死 : " + otherBInfo.deadYumi + ")";
        bb_p2fushiText_Cp.text = "ふし : " + otherBInfo.fushi + " (死 : " + otherBInfo.deadFushi + ")";
        bb_p2ryuText_Cp.text = "りゅう : " + otherBInfo.ryu + " (死 : " + otherBInfo.deadRyu + ")";
    }

    public void RefreshBattleboardUnits()
    {
        for (int i = 0; i < bb_p1UnitUI_Cps.Count; i++)
        {
            if (localPlayerID == 0)
            {
                bb_p1UnitUI_Cps[i].frontSprite = player_Cps[0].bUnit_Cps[i].unitCardData.frontSide;
            }
            else
            {
                bb_p1UnitUI_Cps[i].frontSprite = player_Cps[0].bUnit_Cps[i].frontSide;
            }
        }

        for (int i = 0; i < bb_p2UnitUI_Cps.Count; i++)
        {
            if (localPlayerID == 1)
            {
                bb_p2UnitUI_Cps[i].frontSprite = player_Cps[1].bUnit_Cps[i].unitCardData.frontSide;
            }
            else
            {
                bb_p2UnitUI_Cps[i].frontSprite = player_Cps[1].bUnit_Cps[i].frontSide;
            }
        }
    }

    //--------------------------------------------------
    void RefreshCardDetail(int playerID_pr, UnitCard unit_Cp_pr)
    {
        // set unit details
        RefreshUnitDetail(playerID_pr, unit_Cp_pr);

        // set equip item details
        RefreshEquipItems(unit_Cp_pr);
    }

    void RefreshUnitDetail(int playerID_pr, UnitCard unit_Cp_pr)
    {
        UnitCardData unitData = unit_Cp_pr.unitCardData;

        // set unit details
        if (playerID_pr != localPlayerID && !unit_Cp_pr.placedPosture)
        {
            cd_cardImage_Cp.sprite = unitData.backSide;
        }
        else
        {
            cd_cardImage_Cp.sprite = unitData.frontSide;
        }

        UnitInfo unitInfo_tp = unit_Cp_pr.unitInfo;

        cd_cardNameText_Cp.text = unitData.name;
        cd_costText_Cp.text = "コスト : " + unitInfo_tp.cost;
        cd_atrText_Cp.text = "属性 : " + unitData.attrib.ToString();
        cd_hpText_Cp.text = "HP : " + unitInfo_tp.curHp + "/" + unitInfo_tp.maxHP;
        cd_atkText_Cp.text = "ATK : " + unitInfo_tp.baseAtk + "+" + unitInfo_tp.atkCorr;
        cd_agiText_Cp.text = "AGI : " + unitInfo_tp.baseAgi + "+" + unitInfo_tp.agiCorr;
        cd_defCorrText_Cp.text = "DEF : " + (unitInfo_tp.defCorr >= 0 ? "+" : "-") + unitInfo_tp.defCorr;
        cd_accuracyCorrText_Cp.text = "命中 : " + (unitInfo_tp.hitCorr >= 0 ? "+" : "-")
            + unitInfo_tp.hitCorr;
        cd_CTCorrText_Cp.text = "CT値 : " + (unitInfo_tp.ctCorr >= 0 ? "+" : "-") + unitInfo_tp.ctCorr;
        cd_normalAtkCorrText_Cp.text = "通常攻撃 : " + (unitInfo_tp.nlAtkCorr >= 0 ? "+" : "-")
            + unitInfo_tp.nlAtkCorr;
        cd_spcAtkCorrText_Cp.text = "特殊攻撃 : " + (unitInfo_tp.spcAtkCorr >= 0 ? "+" : "-")
            + unitInfo_tp.spcAtkCorr;
        cd_dmgCorrText_Cp.text = "ダメ : " + (unitInfo_tp.dmgCorr >= 0 ? "+" : "-")
            + unitInfo_tp.dmgCorr;
        cd_indirDmgCorrText_Cp.text = "間接ダメ : " + (unitInfo_tp.indirDmgCorr >= 0 ? "+" : "-")
            + unitInfo_tp.indirDmgCorr;
        cd_shienEffectCorrText_Cp.text = "しえん効果 : " + (unitInfo_tp.shienEffCorr >= 0 ? "+" : "-")
            + unitInfo_tp.shienEffCorr;
        cd_diceEffectCorrText_Cp.text = "ダイス 効果 : " + (unitInfo_tp.diceEffCorr >= 0 ? "+" : "-")
            + unitInfo_tp.diceEffCorr;
    }

    void RefreshEquipItems(UnitCard unit_Cp_pr)
    {
        // clear old equip item datas
        for (int i = 0; i < cd_equipItem_GOs.Count; i++)
        {
            Destroy(cd_equipItem_GOs[i]);
        }
        cd_equipItem_GOs.Clear();

        // instant new equip items
        for (int i = 0; i < unit_Cp_pr.equipItems.Count; i++)
        {
            ItemCardData itemData = unit_Cp_pr.equipItems[i];
            GameObject item_GO_tp = Instantiate(cd_item_Pf, cd_equipItemsContent_RT);
            cd_equipItem_GOs.Add(item_GO_tp);
            item_GO_tp.GetComponent<ItemCard>().itemData = itemData;
        }

        // Initialize the initial X position to the left edge of the parent container
        cd_equipItemsContent_RT.sizeDelta = new Vector2(equipItemInterval * (cd_equipItem_GOs.Count + 1)
            + cd_item_Pf.GetComponent<RectTransform>().sizeDelta.x * cd_equipItem_GOs.Count,
            cd_equipItemsContent_RT.sizeDelta.y);

        float currentXPosition = equipItemInterval;
        for (int i = 0; i < cd_equipItem_GOs.Count; i++)
        {
            RectTransform item_RT_tp = cd_equipItem_GOs[i].GetComponent<RectTransform>();

            // Set the position of the item from left to right
            item_RT_tp.anchoredPosition = new Vector2(currentXPosition, item_RT_tp.anchoredPosition.y);

            // Update the currentXPosition for the next item
            currentXPosition += item_RT_tp.sizeDelta.x + equipItemInterval;
        }

        // set equip item frontside image
        for (int i = 0; i < cd_equipItem_GOs.Count; i++)
        {
            cd_equipItem_GOs[i].GetComponent<Image>().sprite = unit_Cp_pr.equipItems[i].frontSide;
        }
    }

    //--------------------------------------------------
    void SetActivePanel(GameState_En gameState_pr, bool flag)
    {
        switch (gameState_pr)
        {
            case GameState_En.OnPlayerboardPanel:
                playerBPanel_GO.SetActive(flag);
                break;
            case GameState_En.OnActionWindowPanel:
                actionWPanel_GO.SetActive(flag);
                break;
            case GameState_En.OnMiharidaiPanel:
                miharidaiPanel_GO.SetActive(flag);
                break;
            case GameState_En.OnBattleboardPanel:
                battleBPanel_GO.SetActive(flag);
                break;
            case GameState_En.OnCardDetailPanel:
                cardDetailPanel_GO.SetActive(flag);
                break;
        }
    }

    public void MoveToPlayerboard()
    {
        //
        RefreshPbPlayerInfoPanel();

        //
        SetActivePanel(mainGameState, false);

        mainGameState = GameState_En.OnPlayerboardPanel;
        SetActivePanel(mainGameState, true);

        // set status manager
        statusManager_Cp.SetInstructions_StrPhase(0);

        //
        strController_Cp.MoveCamToPlayerboard();
    }

    public void MoveToActionWindow(int roundIndex_pr)
    {
        //
        SetRoundIndexText(roundIndex_pr);

        RefreshActionWindow();

        DisableActionWindowActionPanels();
        aw_guardPanel_GO.SetActive(true);

        // show action window panel
        SetActivePanel(mainGameState, false);

        mainGameState = GameState_En.OnActionWindowPanel;
        SetActivePanel(GameState_En.OnActionWindowPanel, true);

        // set status manager
        statusManager_Cp.SetInstructions_StrPhase(1);

        // move camera to miharidai
        strController_Cp.MoveCamToEnemyPbLookPoint();
    }

    void MoveToMiharidai()
    {
        SetActivePanel(mainGameState, false);

        mainGameState = GameState_En.OnMiharidaiPanel;
        SetActivePanel(mainGameState, true);

        //
        strController_Cp.MoveCamToEnemyPbLookPoint();
    }

    void MoveToBattleboard()
    {
        localPlayer_Cp.RefreshBattleInfo();

        RefreshBattleInfoPanel();

        //
        SetActivePanel(mainGameState, false);

        mainGameState = GameState_En.OnBattleboardPanel;
        SetActivePanel(mainGameState, true);

        // set status manager
        statusManager_Cp.SetInstructions_StrPhase(2);
    }

    void MoveToCardDetail(int playerID_pr, UnitCard unit_Cp_pr)
    {
        RefreshCardDetail(playerID_pr, unit_Cp_pr);

        //
        AddGameStates(GameState_En.OnCardDetailPanel);
        SetActivePanel(GameState_En.OnCardDetailPanel, true);

        // set status manager
        statusManager_Cp.SetInstructions_StrPhase(3);
    }

    void ReactivateAnotherPanel(GameState_En gameState_pr)
    {
        switch (gameState_pr)
        {
            case GameState_En.OnPlayerboardPanel:
                MoveToPlayerboard();
                break;
            case GameState_En.OnBattleboardPanel:
                MoveToBattleboard();
                break;
        }
    }

    //--------------------------------------------------
    void DisableActionWindowActionPanels()
    {
        aw_guardPanel_GO.SetActive(false);
        aw_shienPanel_GO.SetActive(false);
        aw_movePanel_GO.SetActive(false);
        aw_atkPanel_GO.SetActive(false);
    }

    //-------------------------------------------------- 
    public void SetAwMoDescription(int playerID_pr, int moveVanUnitIndex_pr, int moveRearUnitIndex_pr)
    {
        if (moveVanUnitIndex_pr == -1 || moveRearUnitIndex_pr == -1)
        {
            aw_mo_descText_Cp.text = string.Empty;
        }
        else
        {
            if (playerID_pr == 0)
            {
                aw_mo_descText_Cp.text = (moveVanUnitIndex_pr == 0 ? "赤" : "紫") + "が\r\n"
                    + "後衛" + moveRearUnitIndex_pr.ToString() + "と入替";
            }
            else if (playerID_pr == 1)
            {
                aw_mo_descText_Cp.text = (moveVanUnitIndex_pr == 0 ? "青" : "緑") + "が\r\n"
                    + "後衛" + moveRearUnitIndex_pr.ToString() + "と入替";
            }
        }
    }

    //--------------------------------------------------
    public void SetAwAtDescription(int localPlayerID, int atkAllyIndex_pr, int atkEnemyIndex_pr)
    {
        //if (atkAllyIndex_pr == -1 || atkEnemyIndex_pr == -1)
        //{
        //    aw_at_descText_Cp.text = string.Empty;
        //}
        //else
        //{
        //    if (localPlayerID == 0)
        //    {
        //        aw_at_descText_Cp.text = (atkAllyIndex_pr == 0 ? "赤" : "紫") + "が"
        //            + (atkEnemyIndex_pr == 0 ? "青" : "緑") + "へ攻撃";
        //    }
        //    else if (localPlayerID == 1)
        //    {
        //        aw_at_descText_Cp.text = (atkAllyIndex_pr == 0 ? "青" : "緑") + "が"
        //            + (atkEnemyIndex_pr == 0 ? "赤" : "紫") + "へ攻撃";
        //    }
        //}
    }

    //--------------------------------------------------
    public void SetAwAtAtkCondText(bool isEmpty = false, int normalAp = 0, int spc1Ap = 0, int spc1Sp = 0,
        int spc2Ap = 0, int spc2Sp = 0)
    {
        if (isEmpty)
        {
            aw_at_normalAtkText_Cp.text = "通常\r\n";
            aw_at_spc1AtkText_Cp.text = "特殊1\r\n";
            aw_at_spc2AtkText_Cp.text = "特殊2\r\n";
        }
        else
        {
            aw_at_normalAtkText_Cp.text = "通常\r\n" + "AP" + normalAp.ToString() + " SP0";
            aw_at_spc1AtkText_Cp.text = "特殊1\r\n" + "AP" + spc1Ap.ToString() + " SP"
                + spc1Sp.ToString();
            aw_at_spc2AtkText_Cp.text = "特殊2\r\n" + "AP" + spc2Ap.ToString() + " SP"
                + spc2Sp.ToString();
        }
    }

    //--------------------------------------------------
    public void SetActive_Aw_At_AtkMethodBgd(AttackType atkType, bool flag)
    {
        if (atkType == AttackType.Normal)
        {
            aw_at_nlAtkBgd_GO.SetActive(flag);
        }
        else if (atkType == AttackType.Spc1)
        {
            aw_at_spc1AtkBgd_GO.SetActive(flag);
        }
        else if (atkType == AttackType.Spc2)
        {
            aw_at_spc2AtkBgd_GO.SetActive(flag);
        }
    }

    //--------------------------------------------------
    public void SetRoundIndexText(int roundIndex_pr)
    {
        roundIndexText_Cp.text = "ラウンド " + (roundIndex_pr + 1);
    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Reset
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Reset

    //-------------------------------------------------- Reset
    public void Reset_Aw_At_AtkMethodBgds()
    {
        SetActive_Aw_At_AtkMethodBgd(AttackType.Normal, false);
        SetActive_Aw_At_AtkMethodBgd(AttackType.Spc1, false);
        SetActive_Aw_At_AtkMethodBgd(AttackType.Spc2, false);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// OnEvents
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region OnEvents

    //-------------------------------------------------- playerboard
    public void On_Pb_ToBattleboard()
    {
        MoveToBattleboard();
    }

    public void On_Pb_ToMiharidai()
    {
        MoveToMiharidai();
    }

    public void On_Pb_ToBattlePhase()
    {
        strController_Cp.On_Pb_ToBattlePhase();
    }

    //-------------------------------------------------- action window
    public void On_Aw_Update()
    {
        TokenType selectedTokenType_tp = TokenType.Null;
        if (aw_shienPanel_GO.activeSelf)
        {
            selectedTokenType_tp = TokenType.Shien;
        }
        if (aw_movePanel_GO.activeSelf)
        {
            selectedTokenType_tp = TokenType.Move;
        }
        if (aw_atkPanel_GO.activeSelf)
        {
            selectedTokenType_tp = TokenType.Attack;
        }

        strController_Cp.On_Aw_Update(selectedTokenType_tp);

        //
        SetActivePanel(mainGameState, false);

        mainGameState = GameState_En.OnPlayerboardPanel;
        ReactivateAnotherPanel(mainGameState);
    }

    public void On_Aw_ResetBtn()
    {
        strController_Cp.On_Aw_Reset();

        //
        SetActivePanel(mainGameState, false);

        mainGameState = GameState_En.OnPlayerboardPanel;
        ReactivateAnotherPanel(mainGameState);
    }

    public void On_Aw_BUnitUI(int playerID_pr, int index_pr)
    {
        //
        UnitCard selectedUnit_Cp = player_Cps[playerID_pr].bUnit_Cps[index_pr];

        // check valid to show
        if (playerID_pr != localPlayerID && !selectedUnit_Cp.placedPosture)
        {
            return;
        }

        //
        MoveToCardDetail(playerID_pr, selectedUnit_Cp);
    }

    public void On_Aw_GuardBtn()
    {
        DisableActionWindowActionPanels();
        aw_guardPanel_GO.SetActive(true);
    }

    public void On_Aw_ShienBtn()
    {
        DisableActionWindowActionPanels();
        aw_shienPanel_GO.SetActive(true);
    }

    public void On_Aw_MoveBtn()
    {
        DisableActionWindowActionPanels();
        aw_movePanel_GO.SetActive(true);
    }

    public void On_Aw_AtkBtn()
    {
        DisableActionWindowActionPanels();
        aw_atkPanel_GO.SetActive(true);
    }

    public void On_Aw_Gu_Inc()
    {
        strController_Cp.On_Aw_IncSpMarker();
    }

    public void On_Aw_Gu_Dec()
    {
        strController_Cp.On_Aw_DecSpMarker();
    }

    public void On_Aw_Sh_SelectShienUnit()
    {
        aw_sh_selectUnitsPanel_GO.SetActive(true);
    }

    public void On_Aw_Sh_Reset()
    {
        strController_Cp.On_Aw_ShienUnitReset();
    }

    public void On_Aw_Sh_Van1()
    {
        strController_Cp.On_Aw_ShienTargetVanUnitSelected(0);
    }

    public void On_Aw_Sh_Van2()
    {
        strController_Cp.On_Aw_ShienTargetVanUnitSelected(1);
    }

    public void On_Aw_Sh_ShienUnitBtn(int index)
    {
        aw_sh_selectUnitsPanel_GO.SetActive(false);

        //
        strController_Cp.On_Aw_ShienUnitSelected(index);
    }

    public void On_Aw_Mo_Van1()
    {
        strController_Cp.On_Aw_MoveVanUnitSelected(0);
    }

    public void On_Aw_Mo_Van2()
    {
        strController_Cp.On_Aw_MoveVanUnitSelected(1);
    }

    public void On_Aw_Mo_Rear1()
    {
        strController_Cp.On_Aw_MoveRearUnitSelected(0);
    }

    public void On_Aw_Mo_Rear2()
    {
        strController_Cp.On_Aw_MoveRearUnitSelected(1);
    }

    public void On_Aw_Mo_Rear3()
    {
        strController_Cp.On_Aw_MoveRearUnitSelected(2);
    }

    public void On_Aw_At_AllyVan1()
    {
        strController_Cp.On_Aw_At_AllyVanUnitSelected(0);
    }

    public void On_Aw_At_AllyVan2()
    {
        strController_Cp.On_Aw_At_AllyVanUnitSelected(1);
    }

    public void On_Aw_At_EnemyVan1()
    {
        strController_Cp.On_Aw_At_EnemyVanUnitSelected(0);
    }

    public void On_Aw_At_EnemyVan2()
    {
        strController_Cp.On_Aw_At_EnemyVanUnitSelected(1);
    }

    public void On_Aw_At_AtkMethod(int atkIndex)
    {
        AttackType atkType_pr = AttackType.Null;

        switch (atkIndex)
        {
            case 1:
                atkType_pr = AttackType.Normal;
                break;
            case 2:
                atkType_pr = AttackType.Spc1;
                break;
            case 3:
                atkType_pr = AttackType.Spc2;
                break;
        }

        //
        strController_Cp.On_Aw_At_AtkMethodSelected(atkType_pr);
    }

    //-------------------------------------------------- miharidai
    public void On_Md_ToPlayerboard()
    {
        MoveToPlayerboard();
    }

    public void On_Md_ToBattleboard()
    {
        MoveToBattleboard();
    }

    //-------------------------------------------------- battleboard
    public void On_Bb_Unit(int playerID_pr, int index_pr)
    {
        UnitCard selectedUnit_Cp_tp = player_Cps[playerID_pr].bUnit_Cps[index_pr];

        // check valid to show
        if (playerID_pr != localPlayerID && !selectedUnit_Cp_tp.placedPosture)
        {
            return;
        }

        //
        MoveToCardDetail(playerID_pr, selectedUnit_Cp_tp);
    }

    public void On_Bb_ToMiharidai()
    {
        MoveToMiharidai();
    }

    public void On_Bb_ToPlayerboard()
    {
        MoveToPlayerboard();
    }

    //-------------------------------------------------- card detail
    public void On_Cd_Close()
    {
        RemoveGameStates(GameState_En.OnCardDetailPanel);
        SetActivePanel(GameState_En.OnCardDetailPanel, false);
    }

    #endregion

}
