using System.Collections;
using System.Collections.Generic;
using TcgEngine.UI;
using UnityEngine;

public class UI_PanelCanvas : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Types
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing,
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField] GameObject cardView_GO;
    [SerializeField] GameObject choiceHandler_GO;
    [SerializeField] GameObject statusPanel_GO;
    [SerializeField] GameObject optionPanel_GO;
    [SerializeField] GameObject actionPanel_GO;
    [SerializeField] GameObject noticePanel_GO;
    [SerializeField] GameObject modalNoticePanel_GO;

    //-------------------------------------------------- public fields
    [ReadOnly] public List<GameState_En> gameStates = new List<GameState_En>();

    [ReadOnly] public ActionPanelHandler actionPanel_Cp;
    [ReadOnly] public NoticePanel notice_Cp;
    [ReadOnly] public NoticePanel modalNotice_Cp;

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    PlayerFaction localPlayer_Cp;
    CardView cardView_Cp;
    GameStatus gameStatus_Cp;
    ChoiceHandler choiceHandler_Cp;
    Animator optionPanelAnim_Cp;

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
    public void AddMainGameState(GameState_En value = GameState_En.Nothing)
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

        SetComponents();
        InitComponents();

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        // set external components
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        localPlayer_Cp = controller_Cp.localPlayer_Cp;
        cardView_Cp = cardView_GO.GetComponent<CardView>();
        gameStatus_Cp = statusPanel_GO.GetComponent<GameStatus>();

        // set internal components
        choiceHandler_Cp = choiceHandler_GO.GetComponent<ChoiceHandler>();
        optionPanelAnim_Cp = optionPanel_GO.GetComponent<Animator>();
        actionPanel_Cp = actionPanel_GO.GetComponent<ActionPanelHandler>();
        notice_Cp = noticePanel_GO.GetComponent<NoticePanel>();
        modalNotice_Cp = modalNoticePanel_GO.GetComponent<NoticePanel>();
    }

    //--------------------------------------------------
    void InitComponents()
    {
        // set active components
        cardView_GO.SetActive(false);
        choiceHandler_GO.SetActive(false);
        statusPanel_GO.SetActive(false);
        optionPanel_GO.SetActive(false);
        actionPanel_GO.SetActive(false);
        noticePanel_GO.SetActive(false);
        modalNoticePanel_GO.SetActive(false);

        // init components
        actionPanel_Cp.Init(localPlayer_Cp);
        cardView_Cp.Init();
        gameStatus_Cp.Init();
        notice_Cp.Init();
        modalNotice_Cp.Init();
        optionPanelAnim_Cp.GetComponent<CanvasGroup>().alpha = 0f;
        optionPanel_GO.SetActive(true);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// External interface
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region External interface

    //--------------------------------------------------
    public void ShowActionPanel(int playerId_tp, int rndId_tp)
    {
        actionPanel_Cp.ShowPanel(playerId_tp, rndId_tp);
    }

    //--------------------------------------------------
    public void ViewCard(Unit_Bb bbUnit_Cp_tp)
    {
        cardView_Cp.ShowBbUnitUI(bbUnit_Cp_tp);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// External callback
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region External callback

    //--------------------------------------------------
    public void OnClickOptionBtn()
    {
        optionPanelAnim_Cp.SetTrigger("show");
    }

    //--------------------------------------------------
    public void OnClickContinueBtn()
    {
        optionPanelAnim_Cp.SetTrigger("hide");
    }

    //--------------------------------------------------
    public void OnClickInterruptBtn()
    {
        controller_Cp.ReturnToMainScene();
    }

    #endregion

}
