using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExcelDataReader;
using System.Data;
using System.IO;
using System;

public class Data_Phases : MonoBehaviour
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
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField] public ParEffects_Cs parEffs; // it can remove later
    [SerializeField] public Sprite p1Van1Sprite, p1Van2Sprite, p2Van1Sprite, p2Van2Sprite;
    [SerializeField] public GameObject pbRnd_spawnEff_Pf, pbRnd_destEff_Pf;
    [SerializeField] public GameObject hlEff_Pf;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    public Dictionary<Hash128, object> randObjects = new Dictionary<Hash128, object>();

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    DataManager_Gameplay dataManager_Cp;

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

    #endregion

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

    //-------------------------------------------------- Init
    public void Init()
    {
        AddMainGameState(GameState_En.Nothing);

        SetComponents();

        mainGameState = GameState_En.Inited;
    }

    //-------------------------------------------------- Set components
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        dataManager_Cp = controller_Cp.dataManager_Cp;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle random game objects
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region #Handle random game objects

    //--------------------------------------------------
    public Hash128 RegRandObjHash()
    {
        object obj_tp = null;
        Hash128 hash_tp = HashHandler.RegRandHash();

        randObjects.Add(hash_tp, obj_tp);

        return hash_tp;
    }

    //--------------------------------------------------
    public void RemoveRandObj(Hash128 hash_pr)
    {
        if (randObjects[hash_pr].GetType() == typeof(GameObject))
        {
            GameObject gObj_tp = (GameObject)randObjects[hash_pr];
            DestroyImmediate(gObj_tp);
        }

        randObjects.Remove(hash_pr);

        HashHandler.RemoveHash(hash_pr);
    }

    #endregion

}
