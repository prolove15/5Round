using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StartPrepare : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    // Types
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing, Finished,
        ZoomInOut
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    // Fields
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField]
    public RectTransform decisionBtn_RT;

    [SerializeField]
    public RectTransform lookHandBtn_RT, returnToSelectionBtn_RT;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    //-------------------------------------------------- private fields
    Controller_StartPrepare controller_Cp;

    Cards_StartPrepare cardManager_Cp;

    TakedCards_StartPrepare takedCardManager_Cp;

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
    // ManageGameState
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
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_StartPrepare>();

        cardManager_Cp = controller_Cp.cardsManager_Cps[controller_Cp.localPlayerID];
        
        takedCardManager_Cp = controller_Cp.takedCardsManager_Cps[controller_Cp.localPlayerID];
    }

    #endregion

    //------------------------------
    Dictionary<string, bool> decisionBtnState = new Dictionary<string, bool>();

    public void SetActiveDecisionBtn(bool flag, string condition = "ForceSet")
    {
        string forceSet_tp = "ForceSet";
        string cameraMoving_tp = "CameraMoving";

        //
        decisionBtnState[condition] = flag;

        //
        if (decisionBtnState.ContainsKey(forceSet_tp))
        {
            if (decisionBtnState[forceSet_tp])
            {
                if(decisionBtnState.ContainsKey(cameraMoving_tp))
                {
                    decisionBtn_RT.gameObject.SetActive(decisionBtnState[cameraMoving_tp]);
                }
                else
                {
                    decisionBtn_RT.gameObject.SetActive(decisionBtnState[forceSet_tp]);
                }
            }
            else
            {
                decisionBtn_RT.gameObject.SetActive(decisionBtnState[forceSet_tp]);
            }
        }
        else
        {
            if (decisionBtnState.ContainsKey(cameraMoving_tp))
            {
                decisionBtn_RT.gameObject.SetActive(decisionBtnState[cameraMoving_tp]);
            }
        }
    }

    //------------------------------
    public void OnZoomInOutStarted()
    {
        gameStates.Add(GameState_En.ZoomInOut);
    }

    public void OnZoomInOutFinished()
    {
        gameStates.Remove(GameState_En.ZoomInOut);
    }

    //////////////////////////////////////////////////////////////////////
    // OnEvent
    //////////////////////////////////////////////////////////////////////
    #region OnEvent

    //------------------------------
    public void OnDecisionBtnClicked()
    {
        if (gameStates.Contains(GameState_En.ZoomInOut))
        {
            return;
        }

        controller_Cp.OnClickDecisionBtn();
    }

    //------------------------------
    public void OnLookHand()
    {
        if (gameStates.Contains(GameState_En.ZoomInOut))
        {
            return;
        }

        takedCardManager_Cp.OnLookHand();
    }

    //------------------------------
    public void OnReturnToSelection()
    {
        if (gameStates.Contains(GameState_En.ZoomInOut))
        {
            return;
        }

        cardManager_Cp.OnReturnToSelection();
    }

    #endregion

}
