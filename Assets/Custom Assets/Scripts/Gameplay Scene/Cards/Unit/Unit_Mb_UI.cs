using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit_Mb_UI : MonoBehaviour
{

    [SerializeField] Image frontSide_Cp;

    public UnitCardData unitData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(UnitCardData unitData_tp)
    {
        unitData = unitData_tp;
        frontSide_Cp.sprite = unitData.frontSide;
    }

}
