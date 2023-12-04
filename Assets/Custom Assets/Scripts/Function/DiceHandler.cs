using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceHandler : MonoBehaviour
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
        DiceRollStarted, DiceRollDone, DiceRollFinished,
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
    public GameObject dice1_Pf, dice2_Pf;

    [SerializeField]
    public Transform dice1InstPoint_Tf, dice2InstPoint_Tf;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    public int dice1Amount, dice2Amount, diceTotalAmount;

    //-------------------------------------------------- private fields
    GameObject dice1_GO, dice2_GO;

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

        mainGameState = GameState_En.Inited;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle dice
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle dice

    //-------------------------------------------------- Throw dice
    public void ThrowDice()
    {
        StartCoroutine(Corou_ThrowDice());
    }

    IEnumerator Corou_ThrowDice()
    {
        //
        AddGameStates(GameState_En.DiceRollStarted);

        //
        dice1_GO = Instantiate(dice1_Pf, dice1InstPoint_Tf.position, Random.rotation);
        dice2_GO = Instantiate(dice2_Pf, dice2InstPoint_Tf.position, Random.rotation);

        // each dice will be fall to the ground by gravity
        // also, they have box collider
        // after 1 second, they would be stopped.
        // dice x axis is pointing 4, y axis is pointing 2, z axis is pointing 1.
        // write code here to find out the top surface number

        Rigidbody dice1Rb_Cp = dice1_GO.GetComponent<Rigidbody>();
        Rigidbody dice2Rb_Cp = dice2_GO.GetComponent<Rigidbody>();

        yield return new WaitUntil(() => dice1Rb_Cp.IsSleeping() && dice2Rb_Cp.IsSleeping());
        yield return new WaitForSeconds(2f);

        dice1Amount = DetermineTopSurfaceNumber(dice1_GO);
        dice2Amount = DetermineTopSurfaceNumber(dice2_GO);

        diceTotalAmount = dice1Amount + dice2Amount;

        //******************** 36th timing. after dice
        

        //
        RemoveGameStates(GameState_En.DiceRollStarted);
        AddGameStates(GameState_En.DiceRollDone);
    }

    int DetermineTopSurfaceNumber(GameObject dice)
    {
        // Determine the top surface number based on the local up direction
        Vector3 localUp = dice.transform.InverseTransformDirection(Vector3.up);
        float dotX = Vector3.Dot(localUp, Vector3.right);
        float dotY = Vector3.Dot(localUp, Vector3.up);
        float dotZ = Vector3.Dot(localUp, Vector3.forward);

        // Identify the axis with the maximum dot product
        if (Mathf.Abs(dotX) > Mathf.Abs(dotY) && Mathf.Abs(dotX) > Mathf.Abs(dotZ))
        {
            return (dotX > 0) ? 4 : 3;
        }
        else if (Mathf.Abs(dotY) > Mathf.Abs(dotX) && Mathf.Abs(dotY) > Mathf.Abs(dotZ))
        {
            return (dotY > 0) ? 2 : 5;
        }
        else
        {
            return (dotZ > 0) ? 1 : 6;
        }
    }

    //-------------------------------------------------- 
    public void ResetDiceRoll()
    {
        StartCoroutine(Corou_ResetDiceRoll());
    }

    IEnumerator Corou_ResetDiceRoll()
    {
        Destroy(dice1_GO);
        Destroy(dice2_GO);

        dice1Amount = 0;
        dice2Amount = 0;

        yield return new WaitUntil(() => dice1_GO == null && dice2_GO == null);

        //
        AddGameStates(GameState_En.DiceRollFinished);
    }

    #endregion
}
