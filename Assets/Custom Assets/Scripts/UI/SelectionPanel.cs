using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectionPanel : MonoBehaviour
{
        
    [SerializeField] Button selectBtn_Cp;
    [SerializeField] GameObject checkbox_GO;
    [SerializeField] Image bgdImage_Cp;

    int m_baseIndex;
    UnityAction<int, bool> onClickedAction;
    Hash128 hash;
    bool isSelected;

    public int baseIndex { get { return m_baseIndex; } set { m_baseIndex = value; } }

    //--------------------------------------------------
    public void Init(int index_tp, Hash128 hash_tp, UnityAction<int, bool> onClickedAction_tp = null)
    {
        baseIndex = index_tp;
        onClickedAction = onClickedAction_tp;
        hash = hash_tp;

        //
        SetSelectedStatus(false);
    }

    public void Init(Hash128 hash_tp)
    {
        hash = hash_tp;

        //
        SetSelectedStatus(true);
    }

    //--------------------------------------------------
    public void ResetPanel()
    {
        SetSelectedStatus(false);
    }

    public void SetSelectedStatus(bool flag)
    {
        checkbox_GO.SetActive(flag);
        isSelected = flag;

        OnChangeSelectedStatus();
    }

    public void SetInteract(bool flag)
    {
        selectBtn_Cp.interactable = flag;
    }

    public Hash128 GetHash()
    {
        return hash;
    }

    public bool IsSelected()
    {
        return isSelected;
    }

    public void SetBgd(Sprite bgdSprite_tp)
    {
        bgdImage_Cp.sprite = bgdSprite_tp;
    }

    //--------------------------------------------------
    void OnChangeSelectedStatus()
    {
        if (isSelected)
        {
            List<SelectionPanel> selectionGroup = new List<SelectionPanel>();
            foreach (SelectionPanel value in FindObjectsOfType<SelectionPanel>())
            {
                if (value.GetHash() == hash && value != this)
                {
                    selectionGroup.Add(value);
                }
            }
            foreach (SelectionPanel value in selectionGroup)
            {
                value.SetSelectedStatus(!isSelected);
            }
        }
    }

    //--------------------------------------------------
    public void OnClickBtn()
    {
        SetSelectedStatus(!isSelected);
        OnChangeSelectedStatus();
        onClickedAction?.Invoke(baseIndex, isSelected);
    }

}
