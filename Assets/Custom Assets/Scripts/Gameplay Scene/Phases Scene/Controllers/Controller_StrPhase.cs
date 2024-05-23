using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Controller_StrPhase : MonoBehaviour
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
        PhaseStarted, PhaseFinished,
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
    public UI_StrPhase strUI_Cp;

    [SerializeField]
    float simulateComPlayerInterval = 1f;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    [ReadOnly]
    public int selectedRoundIndex;

    [ReadOnly]
    public TokenType selectedTokenType = new TokenType();

    [ReadOnly]
    public int shienUnitIndex, shienTargetVanUnitIndex;

    [ReadOnly]
    public int moveVanUnitIndex, moveRearUnitIndex;

    [ReadOnly]
    public int atkAllyVanUnitIndex, atkEnemyVanUnitIndex;

    [ReadOnly]
    public AttackType atkType;

    [ReadOnly]
    public bool p1ReadyState, p2ReadyState;

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;

    DataManager_Gameplay dataManager_Cp;

    List<Player_Phases> player_Cps = new List<Player_Phases>();

    Player_Phases localPlayer_Cp, otherPlayer_Cp, comPlayer_Cp;

    Transform cam_Tf;

    StatusManager statusManager_Cp;

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

        InitComponents();

        InitVariables();

        InitStatusPanel();

        //
        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();

        dataManager_Cp = controller_Cp.dataManager_Cp;

        player_Cps = controller_Cp.player_Cps_de;

        localPlayer_Cp = controller_Cp.localPlayer_Cp_de;

        otherPlayer_Cp = controller_Cp.otherPlayer_Cp_de;

        comPlayer_Cp = controller_Cp.comPlayer_Cp_de;

        cam_Tf = controller_Cp.cam_Tf;

        statusManager_Cp = controller_Cp.statusManager_Cp;

        localPlayerID = controller_Cp.localPlayerID_de;
    }

    //--------------------------------------------------
    void InitComponents()
    {
        strUI_Cp.Init();
    }

    //--------------------------------------------------
    void InitVariables()
    {
        //
        selectedRoundIndex = -1;

        //
        shienUnitIndex = -1;
        shienTargetVanUnitIndex = -1;

        //
        moveVanUnitIndex = -1;
        moveRearUnitIndex = -1;

        //
        atkAllyVanUnitIndex = -1;
        atkEnemyVanUnitIndex = -1;
        atkType = AttackType.Null;
    }

    //--------------------------------------------------
    void InitStatusPanel()
    {
        statusManager_Cp.opponentReadyState = false;
        statusManager_Cp.InitInstructions_StrPhase();
    }

    #endregion

    //--------------------------------------------------
    public void PlayPhase()
    {
        StartCoroutine(Corou_PlayPhase());
    }

    IEnumerator Corou_PlayPhase()
    {
        mainGameState = GameState_En.PhaseStarted;

        //
        strUI_Cp.MoveToPlayerboard();

        //
        SimulateComPlayer();

        // play status panel
        PlayStatusPanel();

        //
        yield return null;
    }

    void PlayStatusPanel()
    {
        statusManager_Cp.maxLeftTime = 180;
        statusManager_Cp.leftTime = statusManager_Cp.maxLeftTime;
        statusManager_Cp.StartLeftTimeCounting();
    }

    //-------------------------------------------------- move camera
    public void MoveCamToPlayerboard()
    {
        //UnityEvent unityEvent = new UnityEvent();
        //unityEvent.AddListener(OnComplete_MovecamToPlayerboard);
        //TargetTweening.TranslateGameObject(cam_Tf, localPlayer_Cp.playerBLookPoint_Tf, unityEvent);
        cam_Tf.SetPositionAndRotation(localPlayer_Cp.playerBLookPoint_Tf.position,
            localPlayer_Cp.playerBLookPoint_Tf.rotation);
    }

    void OnComplete_MovecamToPlayerboard()
    {

    }

    public void MoveCamToEnemyPbLookPoint()
    {
        //UnityEvent unityEvent = new UnityEvent();
        //unityEvent.AddListener(OnComplete_MoveCamToEnemyPbLookPoint);
        //TargetTweening.TranslateGameObject(cam_Tf, localPlayer_Cp.enemyPbLookPoint_Tf, unityEvent);
        cam_Tf.SetPositionAndRotation(localPlayer_Cp.enemyPbLookPoint_Tf.position,
            localPlayer_Cp.enemyPbLookPoint_Tf.rotation);
    }
    void OnComplete_MoveCamToEnemyPbLookPoint()
    {

    }

    //-------------------------------------------------- On Playerboard RoundPanel
    public void On_PbRoundPanel(int index)
    {
        //
        if (strUI_Cp.mainGameState != UI_StrPhase.GameState_En.OnPlayerboardPanel)
        {
            return;
        }
        if (strUI_Cp.ExistAnyGameStates(UI_StrPhase.GameState_En.OnActionWindowPanel,
            UI_StrPhase.GameState_En.OnCardDetailPanel))
        {
            return;
        }

        selectedRoundIndex = index;

        // reset values
        selectedTokenType = TokenType.Null;

        shienUnitIndex = -1;
        shienTargetVanUnitIndex = -1;

        moveVanUnitIndex = -1;
        moveRearUnitIndex = -1;

        atkAllyVanUnitIndex = -1;
        atkEnemyVanUnitIndex = -1;
        atkType = AttackType.Normal;

        // refresh values
        RoundValue_de roundValue = localPlayer_Cp.roundsData[selectedRoundIndex];
        switch (roundValue.token.type)
        {
            case TokenType.Shien:
                shienUnitIndex = roundValue.originUnitIndex;
                shienTargetVanUnitIndex = roundValue.tarUnitIndex;
                break;
            case TokenType.Move:
                moveRearUnitIndex = roundValue.originUnitIndex;
                moveVanUnitIndex = roundValue.tarUnitIndex;
                break;
            case TokenType.Attack:
                atkAllyVanUnitIndex = roundValue.originUnitIndex;
                atkEnemyVanUnitIndex = roundValue.tarUnitIndex;
                atkType = roundValue.atkType;
                break;
        }

        //
        strUI_Cp.MoveToActionWindow(index);
    }

    //-------------------------------------------------- On ActionWindow panel
    void UpdateActionWindow(TokenType tokenType_pr)
    {
        selectedTokenType = tokenType_pr;

        // reset action window
        switch (selectedTokenType)
        {
            case TokenType.Shien:
                UpdateShienToken();
                break;
            case TokenType.Move:
                UpdateMoveToken();
                break;
            case TokenType.Attack:
                UpdateAtkToken();
                break;
        }
    }

    void UpdateShienToken()
    {
        if (shienUnitIndex == -1 || shienTargetVanUnitIndex == -1)
        {
            return;
        }

        UnityEvent unityEvent = new UnityEvent();
        unityEvent.AddListener(OnComplete_ResetToken);
        localPlayer_Cp.ResetRoundToken(selectedRoundIndex, unityEvent);
    }

    void OnComplete_ResetToken()
    {
        localPlayer_Cp.SetShienToken(selectedRoundIndex, shienUnitIndex, shienTargetVanUnitIndex);

        //
        strUI_Cp.RefreshAwMihariUnits();
    }

    void UpdateMoveToken()
    {
        if (moveVanUnitIndex == -1 || moveRearUnitIndex == -1)
        {
            return;
        }

        UnityEvent unityEvent = new UnityEvent();
        localPlayer_Cp.ResetRoundToken(selectedRoundIndex, unityEvent);

        localPlayer_Cp.SetMoveToken(selectedRoundIndex, moveRearUnitIndex, moveVanUnitIndex);
    }

    void UpdateAtkToken()
    {
        if (atkAllyVanUnitIndex == -1 || atkEnemyVanUnitIndex == -1)
        {
            return;
        }

        UnityEvent unityEvent = new UnityEvent();
        localPlayer_Cp.ResetRoundToken(selectedRoundIndex, unityEvent);

        localPlayer_Cp.SetAtkToken(selectedRoundIndex, atkAllyVanUnitIndex, atkEnemyVanUnitIndex, atkType);
    }

    void ResetActionWindow()
    {
        UnityEvent unityEvent = new UnityEvent();
        localPlayer_Cp.ResetRoundToken(selectedRoundIndex, unityEvent);

        localPlayer_Cp.ResetSpMarker(selectedRoundIndex);

        strUI_Cp.RefreshActionWindow();
    }

    void SetActionWindowAtkArrow()
    {
        if (atkAllyVanUnitIndex == -1 || atkEnemyVanUnitIndex == -1)
        {
            strUI_Cp.aw_at_arrow_RT.gameObject.SetActive(false);
            return;
        }

        //strUI_Cp.aw_at_arrow_RT.gameObject.SetActive(true);

        //RectTransform fromArrowPoint_RT_tp = atkAllyVanUnitIndex == 0 ? strUI_Cp.aw_at_allyVan1ArrowPoint_RT
        //    : strUI_Cp.aw_at_allyVan2ArrowPoint_RT;
        //RectTransform toArrowPoint_RT_tp = atkEnemyVanUnitIndex == 0 ? strUI_Cp.aw_at_enemyVan1ArrowPoint_RT
        //    : strUI_Cp.aw_at_enemyVan2ArrorPoint_RT;
        //RectTransform arrow_RT_tp = strUI_Cp.aw_at_arrow_RT;

        //// Calculate the direction from 'from' to 'to' points
        //Vector2 dir = toArrowPoint_RT_tp.anchoredPosition - fromArrowPoint_RT_tp.anchoredPosition;

        //// Set the position of the arrow in the middle of the 'from' and 'to' points
        //arrow_RT_tp.anchoredPosition = (fromArrowPoint_RT_tp.anchoredPosition + toArrowPoint_RT_tp.anchoredPosition) / 2f;

        //// Calculate the rotation angle based on the direction
        //float rotAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //// Set the rotation of the arrow
        //arrow_RT_tp.rotation = Quaternion.Euler(0f, 0f, rotAngle);
    }

    //--------------------------------------------------
    public void MoveToBattlePhase()
    {
        mainGameState = GameState_En.PhaseFinished;
    }

    //--------------------------------------------------
    public void SetReadyState(int playerID_pr, bool state_pr)
    {
        //
        if (playerID_pr == 0)
        {
            p1ReadyState = state_pr;
        }
        else if (playerID_pr == 1)
        {
            p2ReadyState = state_pr;
        }

        //
        if (playerID_pr == otherPlayer_Cp.playerID)
        {
            statusManager_Cp.opponentReadyState = true;
        }

        //
        if (p1ReadyState && p2ReadyState)
        {
            MoveToBattlePhase();
        }
    }

    //--------------------------------------------------
    public void ForceMoveToBattlePhase()
    {
        strUI_Cp.DisableAllPanel();
        StopCoroutine(corouSimComPlayer);

        SetReadyState(0, true);
        SetReadyState(1, true);
    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Events from UI
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region EventsFromUI

    //-------------------------------------------------- On Playerboard
    public void On_Pb_ToBattlePhase()
    {
        strUI_Cp.DisableAllPanel();

        SetReadyState(localPlayerID, true);
    }

    //-------------------------------------------------- On ActionWindow
    public void On_Aw_IncSpMarker()
    {
        localPlayer_Cp.IncSpMarker(selectedRoundIndex);

        strUI_Cp.RefreshAwGuardPanel();
    }

    public void On_Aw_DecSpMarker()
    {
        localPlayer_Cp.DecSpMarker(selectedRoundIndex);

        strUI_Cp.RefreshAwGuardPanel();
    }

    public void On_Aw_ShienUnitSelected(int index)
    {
        shienUnitIndex = index;

        UnitCardData unitData = dataManager_Cp.GetUnitCardDataFromCardIndex(localPlayer_Cp.mUnit_Cps[index].cardIndex);

        //
        strUI_Cp.aw_sh_unitText_Cp.text = "ユニット : " + unitData.name;
        strUI_Cp.aw_sh_shienText_Cp.text = "しえん : " + unitData.shienAbil;
        strUI_Cp.aw_sh_descText_Cp.text = unitData.shienDes_Leg;
    }

    public void On_Aw_ShienUnitReset()
    {
        shienUnitIndex = -1;

        //
        strUI_Cp.aw_sh_unitText_Cp.text = "ユニット : " + "None";
        strUI_Cp.aw_sh_shienText_Cp.text = string.Empty;
        strUI_Cp.aw_sh_descText_Cp.text = string.Empty;
    }

    public void On_Aw_ShienTargetVanUnitSelected(int index)
    {
        if (shienTargetVanUnitIndex == index)
        {
            shienTargetVanUnitIndex = -1;
        }
        else
        {
            shienTargetVanUnitIndex = index;
        }

        //
        if (shienTargetVanUnitIndex == 0)
        {
            strUI_Cp.aw_sh_van1Bgd_GO.SetActive(true);
            strUI_Cp.aw_sh_van2Bgd_GO.SetActive(false);
        }
        else if (shienTargetVanUnitIndex == 1)
        {
            strUI_Cp.aw_sh_van1Bgd_GO.SetActive(false);
            strUI_Cp.aw_sh_van2Bgd_GO.SetActive(true);
        }
        else
        {
            strUI_Cp.aw_sh_van1Bgd_GO.SetActive(false);
            strUI_Cp.aw_sh_van2Bgd_GO.SetActive(false);
        }
    }

    public void On_Aw_MoveVanUnitSelected(int index)
    {
        if (moveVanUnitIndex == index)
        {
            moveVanUnitIndex = -1;
        }
        else
        {
            moveVanUnitIndex = index;
        }

        //
        strUI_Cp.aw_mo_van1Bgd_GO.SetActive(false);
        strUI_Cp.aw_mo_van2Bgd_GO.SetActive(false);

        if (moveVanUnitIndex == 0)
        {
            strUI_Cp.aw_mo_van1Bgd_GO.SetActive(true);
        }
        else if (moveVanUnitIndex == 1)
        {
            strUI_Cp.aw_mo_van2Bgd_GO.SetActive(true);
        }

        //
        strUI_Cp.SetAwMoDescription(localPlayerID, moveVanUnitIndex, moveRearUnitIndex);
    }

    public void On_Aw_MoveRearUnitSelected(int index)
    {
        if (moveRearUnitIndex == index)
        {
            moveRearUnitIndex = -1;
        }
        else
        {
            moveRearUnitIndex = index;
        }

        //
        strUI_Cp.aw_mo_rear1Bgd_GO.SetActive(false);
        strUI_Cp.aw_mo_rear2Bgd_GO.SetActive(false);
        strUI_Cp.aw_mo_rear3Bgd_GO.SetActive(false);

        if (moveRearUnitIndex == 0)
        {
            strUI_Cp.aw_mo_rear1Bgd_GO.SetActive(true);
        }
        else if (moveRearUnitIndex == 1)
        {
            strUI_Cp.aw_mo_rear2Bgd_GO.SetActive(true);
        }
        else if (moveRearUnitIndex == 2)
        {
            strUI_Cp.aw_mo_rear3Bgd_GO.SetActive(true);
        }

        //
        strUI_Cp.SetAwMoDescription(localPlayerID, moveVanUnitIndex, moveRearUnitIndex);
    }

    public void On_Aw_At_AllyVanUnitSelected(int index)
    {
        if (atkAllyVanUnitIndex == index)
        {
            atkAllyVanUnitIndex = -1;
        }
        else
        {
            atkAllyVanUnitIndex = index;
        }

        //
        strUI_Cp.aw_at_allyVanBgd1_GO.SetActive(false);
        strUI_Cp.aw_at_allyVanBgd2_GO.SetActive(false);

        if (atkAllyVanUnitIndex == 0)
        {
            strUI_Cp.aw_at_allyVanBgd1_GO.SetActive(true);
        }
        else if (atkAllyVanUnitIndex == 1)
        {
            strUI_Cp.aw_at_allyVanBgd2_GO.SetActive(true);
        }

        //
        strUI_Cp.SetAwAtDescription(localPlayerID, atkAllyVanUnitIndex, atkEnemyVanUnitIndex);

        //
        if (atkAllyVanUnitIndex != -1)
        {
            int selectedAllyVanUnitCardIndex_tp = localPlayer_Cp.bUnit_Cps[atkAllyVanUnitIndex].cardIndex;
            UnitCardData unitData = dataManager_Cp.GetUnitCardDataFromCardIndex(selectedAllyVanUnitCardIndex_tp);

            strUI_Cp.SetAwAtAtkCondText(false, unitData.nlAtk.ap,
                unitData.spcAtk1.ap, unitData.spcAtk1.sp,
                unitData.spcAtk2.ap, unitData.spcAtk2.sp);

            // set active of attack method
            strUI_Cp.SetActive_Aw_At_AtkMethodBgd(atkType, true);
        }
        else
        {
            strUI_Cp.SetAwAtAtkCondText(true);

            // set active of attack method
            strUI_Cp.Reset_Aw_At_AtkMethodBgds();
        }

        // place arrow image
        SetActionWindowAtkArrow();
    }

    public void On_Aw_At_EnemyVanUnitSelected(int index)
    {
        if (atkEnemyVanUnitIndex == index)
        {
            atkEnemyVanUnitIndex = -1;
        }
        else
        {
            atkEnemyVanUnitIndex = index;
        }

        //
        strUI_Cp.aw_at_enemyVanBgd1_GO.SetActive(false);
        strUI_Cp.aw_at_enemyVanBgd2_GO.SetActive(false);

        if (atkEnemyVanUnitIndex == 0)
        {
            strUI_Cp.aw_at_enemyVanBgd1_GO.SetActive(true);
        }
        else if (atkEnemyVanUnitIndex == 1)
        {
            strUI_Cp.aw_at_enemyVanBgd2_GO.SetActive(true);
        }

        //
        strUI_Cp.SetAwAtDescription(localPlayerID, atkAllyVanUnitIndex, atkEnemyVanUnitIndex);

        // place arrow image
        SetActionWindowAtkArrow();
    }

    public void On_Aw_At_AtkMethodSelected(AttackType atkType_pr)
    {
        //
        strUI_Cp.Reset_Aw_At_AtkMethodBgds();

        if (atkType_pr == AttackType.Normal)
        {
            strUI_Cp.SetActive_Aw_At_AtkMethodBgd(AttackType.Normal, true);
        }
        else if (atkType_pr == AttackType.Spc1)
        {
            strUI_Cp.SetActive_Aw_At_AtkMethodBgd(AttackType.Spc1, true);
        }
        else if (atkType_pr == AttackType.Spc2)
        {
            strUI_Cp.SetActive_Aw_At_AtkMethodBgd(AttackType.Spc2, true);
        }

        // set to atkType in roundValue
        atkType = atkType_pr;
    }

    public void On_Aw_Update(TokenType tokenType_pr)
    {
        UpdateActionWindow(tokenType_pr);
    }

    public void On_Aw_Reset()
    {
        ResetActionWindow();
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Simulate computer player
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region SimuateComPlayer

    //--------------------------------------------------
    public void SimulateComPlayer()
    {
        corouSimComPlayer = StartCoroutine(Corou_SimulateComPlayer());
    }

    Coroutine corouSimComPlayer;
    IEnumerator Corou_SimulateComPlayer()
    {
        // set token
        for (int i = 0; i < comPlayer_Cp.roundsData.Count; i++)
        {
            bool setSuccessFlag = false;

            while (!setSuccessFlag)
            {
                //
                int randIndex = Random.Range(0, 4);
                switch (randIndex)
                {
                    case 0:
                        setSuccessFlag = Com_SetShienTokenToRound(i);
                        break;
                    case 1:
                        setSuccessFlag = Com_SetMoveTokenToRound(i);
                        break;
                    case 2:
                        setSuccessFlag = Com_SetAtkTokenToRound(i);
                        break;
                    case 3:
                        setSuccessFlag = true;
                        break;
                }

                //
                yield return new WaitForSeconds(1f);

                // check token or marker are exist
                if (comPlayer_Cp.tokensData.usedShienToken.count < comPlayer_Cp.tokensData.totalShienToken.count)
                {
                    continue;
                }
                if (comPlayer_Cp.tokensData.usedMoveToken.count < comPlayer_Cp.tokensData.totalMoveToken.count)
                {
                    continue;
                }
                if (comPlayer_Cp.tokensData.usedAtkToken.count < comPlayer_Cp.tokensData.totalAtkToken.count)
                {
                    continue;
                }
            }

            yield return new WaitForSeconds(simulateComPlayerInterval);
        }

        // set sp marker
        while (comPlayer_Cp.markersData.usedSpMarkers.count < comPlayer_Cp.markersData.totalSpMarkers.count)
        {
            int randRoundIndex = Random.Range(0, comPlayer_Cp.roundsData.Count);

            RoundValue_de roundValue = comPlayer_Cp.roundsData[randRoundIndex];
            if (roundValue.token.type == TokenType.Attack || roundValue.token.type == TokenType.Null)
            {
                Com_SetSpMarkerToRound(randRoundIndex);
            }
        }

        // set ready state
        SetReadyState(comPlayer_Cp.playerID, true);
    }

    //--------------------------------------------------
    bool Com_SetSpMarkerToRound(int roundIndex_pr)
    {
        bool result = true;

        int restSpMarkersCount = comPlayer_Cp.markersData.totalSpMarkers.count -
            comPlayer_Cp.markersData.usedSpMarkers.count;
        if (restSpMarkersCount == 0)
        {
            return false;
        }

        int randCount = Random.Range(1, restSpMarkersCount + 1);

        comPlayer_Cp.SetSpMarker(roundIndex_pr, randCount);

        return result;
    }

    //--------------------------------------------------
    bool Com_SetShienTokenToRound(int roundIndex_pr)
    {
        bool result = true;

        //
        int restShienTokenCount = comPlayer_Cp.tokensData.totalShienToken.count -
            comPlayer_Cp.tokensData.usedShienToken.count;
        if (restShienTokenCount == 0)
        {
            return false;
        }

        //
        int randShienUnitIndex = -1;
        do
        {
            randShienUnitIndex = Random.Range(0, comPlayer_Cp.mUnit_Cps.Count);
        }
        while (comPlayer_Cp.mUnit_Cps[randShienUnitIndex] == null);

        //
        int randShienTargetUnitIndex_tp = Random.Range(0, 2);

        comPlayer_Cp.SetShienToken(roundIndex_pr, randShienUnitIndex, randShienTargetUnitIndex_tp);

        return result;
    }

    //--------------------------------------------------
    bool Com_SetMoveTokenToRound(int roundIndex_pr)
    {
        bool result = true;

        //
        int restMoveTokenCount = comPlayer_Cp.tokensData.totalMoveToken.count -
            comPlayer_Cp.tokensData.usedMoveToken.count;
        if (restMoveTokenCount == 0)
        {
            return false;
        }

        //
        int randMoveRearUnitIndex = Random.Range(0, 3);
        int randMoveVanUnitIndex = Random.Range(0, 2);

        comPlayer_Cp.SetMoveToken(roundIndex_pr, randMoveRearUnitIndex, randMoveVanUnitIndex);

        //
        return result;
    }

    //--------------------------------------------------
    bool Com_SetAtkTokenToRound(int roundIndex_pr)
    {
        bool result = true;

        //
        int restAtkTokenCount = comPlayer_Cp.tokensData.totalAtkToken.count -
            comPlayer_Cp.tokensData.usedAtkToken.count;
        if (restAtkTokenCount == 0)
        {
            return false;
        }

        //
        int randAtkAllyUnitIndex = Random.Range(0, 2);
        int randAtkEnemyUnitIndex = Random.Range(0, 2);
        AttackType randAtkType = (AttackType)(Random.Range(1, 4));

        comPlayer_Cp.SetAtkToken(roundIndex_pr, randAtkAllyUnitIndex, randAtkEnemyUnitIndex, randAtkType);

        return result;
    }

    #endregion

}
