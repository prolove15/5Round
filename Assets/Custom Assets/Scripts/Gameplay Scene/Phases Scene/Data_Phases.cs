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

    //-------------------------------------------------- effect class
    [Serializable]
    public class ParEffects_Cs
    {
        public GameObject atkCorr_Pf, defCorr_Pf, hitCorr_Pf, dmgCorr_Pf, dmg_Pf;
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
    public ParEffects_Cs parEffects;

    #endregion

}
