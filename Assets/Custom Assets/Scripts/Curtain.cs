using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curtain : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    // Types
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing, Finished,
        CurtainDownFinished, CurtainUpFinished
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    // Fields
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField]
    Animator curtainAnim_Cp;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    //-------------------------------------------------- private fields

    #endregion

    //////////////////////////////////////////////////////////////////////
    // Properties
    //////////////////////////////////////////////////////////////////////
    #region Properties

    //-------------------------------------------------- public properties
    public GameState_En mainGameState
    {
        get
        {
            GameState_En result = GameState_En.Nothing;

            if (gameStates.Count > 0)
            {
                result = gameStates[0];
            }

            return result;
        }
        set
        {
            gameStates.Insert(0, value);
        }
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

    //////////////////////////////////////////////////////////////////////
    // Initialize
    //////////////////////////////////////////////////////////////////////
    #region Initialize

    //------------------------------
    public void Init()
    {
        InitComponents();

        mainGameState = GameState_En.Inited;
    }

    //------------------------------
    void InitComponents()
    {
        
    }

    #endregion

    //------------------------------
    public void CurtainDown()
    {
        curtainAnim_Cp.SetInteger("flag", 1);
    }

    void CurtainDownFinished()
    {
        mainGameState = GameState_En.CurtainDownFinished;
    }

    //------------------------------
    public void CurtainUp()
    {
        curtainAnim_Cp.SetInteger("flag", 2);
    }

    void CurtainUpFinished()
    {
        mainGameState = GameState_En.CurtainUpFinished;
    }

}
