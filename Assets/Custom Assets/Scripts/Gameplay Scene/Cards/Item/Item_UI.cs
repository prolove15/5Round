using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item_UI : MonoBehaviour
{

    [SerializeField] Image itemImage_Cp;

    public void SetItemData(ItemCardData itemData_tp)
    {
        itemImage_Cp.sprite = itemData_tp.frontSide;
    }

}
