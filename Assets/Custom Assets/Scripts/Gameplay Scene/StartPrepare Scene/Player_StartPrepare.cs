using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_StartPrepare : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    // Types
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing, Finished
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    // Fields
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields

    //-------------------------------------------------- public fields
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

    //------------------------------ Update is called once per frame
    void Update()
    {

    }

    //////////////////////////////////////////////////////////////////////
    // Initialize
    //////////////////////////////////////////////////////////////////////
    #region Initialize

    //------------------------------
    public void Init()
    {

    }

    #endregion

}
