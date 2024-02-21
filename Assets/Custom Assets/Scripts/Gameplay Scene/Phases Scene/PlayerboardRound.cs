using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerboardRound : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField] Animator borderAnim_Cp;
    [SerializeField] Button rndBtn_Cp;
    [SerializeField] Transform tokensGroup_Tf;
    [SerializeField] GameObject spMarkerPoint_GO;
    [SerializeField] GameObject shienUnit_GO;
    [SerializeField] Text spMarkersText_Cp;

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    PlayerFaction player_Cp;
    List<Transform> tokenPoint_Tfs = new List<Transform>();
    Dictionary<int, Transform> token_Tfs = new Dictionary<int, Transform>();
    Unit_Pb pbUnit_Cp;

    int roundId;
    UnityAction<int> onClickedAction;

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// properties
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Properties

    //--------------------------------------------------
    RoundData roundsData { get { return player_Cp.roundsData; } }
    TokensData tokensData { get { return player_Cp.tokensData; } }
    MarkersData markersData { get { return player_Cp.markersData; } }
    GameObject spawnEff_Pf { get { return controller_Cp.data_Cp.pbRnd_spawnEff_Pf; } }
    GameObject destEff_Pf { get { return controller_Cp.data_Cp.pbRnd_destEff_Pf; } }

    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Init
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Init

    //--------------------------------------------------
    public void Init(PlayerFaction player_Cp_tp, int roundId_tp, UnityAction<int> onClickedAction_tp)
    {
        SetComponents(player_Cp_tp, roundId_tp, onClickedAction_tp);
        InitComponents();
    }

    //--------------------------------------------------
    void SetComponents(PlayerFaction player_Cp_tp, int roundId_tp, UnityAction<int> onClickedAction_tp)
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        player_Cp = player_Cp_tp;
        roundId = roundId_tp;
        onClickedAction = onClickedAction_tp;

        for (int i = 0; i < tokensGroup_Tf.childCount; i++)
        {
            tokenPoint_Tfs.Add(tokensGroup_Tf.GetChild(i));
        }
        pbUnit_Cp = shienUnit_GO.GetComponent<Unit_Pb>();
    }

    //--------------------------------------------------
    void InitComponents()
    {
        SetActiveBorderAnim(player_Cp.isLocalPlayer ? true : false);
        SetActiveClikable(player_Cp.isLocalPlayer ? true : false);
        spMarkerPoint_GO.SetActive(false);
        shienUnit_GO.SetActive(false);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Internal methods
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Internal methods

    //--------------------------------------------------
    void AddTokenObj(GameObject token_Pf_tp, int index)
    {
        Transform token_Tf_tp = Instantiate(token_Pf_tp, tokenPoint_Tfs[index]).transform;
        token_Tf_tp.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        GameObject spawnEff_GO_tp = Instantiate(spawnEff_Pf, token_Tf_tp.position, token_Tf_tp.rotation);
        Destroy(spawnEff_GO_tp, 2f);

        token_Tfs.Add(index, token_Tf_tp);
    }

    //--------------------------------------------------
    void RemoveTokenObj()
    {
        foreach (Transform value_tp in token_Tfs.Values)
        {
            GameObject destEff_GO_tp = Instantiate(destEff_Pf, value_tp.transform.position, value_tp.transform.rotation);
            Destroy(destEff_GO_tp, 2f);
            Destroy(value_tp.gameObject);
        }
        token_Tfs.Clear();
    }

    //--------------------------------------------------
    void SetSpMarkersText(int count)
    {
        spMarkersText_Cp.text = count.ToString();

        if (count == 0)
        {
            spMarkerPoint_GO.SetActive(false);
        }
        else
        {
            if (!spMarkerPoint_GO.activeSelf)
            {
                spMarkerPoint_GO.SetActive(true);
            }
        }
    }

    //--------------------------------------------------
    void AddShienUnit(UnitCardData unitData_tp)
    {
        pbUnit_Cp.Init(unitData_tp);
        shienUnit_GO.SetActive(true);

        GameObject spawnEff_GO_tp = Instantiate(spawnEff_Pf, shienUnit_GO.transform.position,
            shienUnit_GO.transform.rotation);
        Destroy(spawnEff_GO_tp, 2f);
    }

    //--------------------------------------------------
    void RemovePbShienUnit()
    {
        shienUnit_GO.SetActive(false);

        GameObject destEff_GO_tp = Instantiate(destEff_Pf, shienUnit_GO.transform.position,
            shienUnit_GO.transform.rotation);
        Destroy(destEff_GO_tp, 2f);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// External interface
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region External interface

    //--------------------------------------------------
    public void SetActiveBorderAnim(bool flag)
    {
        borderAnim_Cp.SetTrigger(flag ? "show" : "hide");
    }

    //--------------------------------------------------
    public void SetActiveClikable(bool flag)
    {
        rndBtn_Cp.interactable = flag;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Save rounds value
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Save rounds value

    //--------------------------------------------------
    public void SaveRoundValue_Guard(RoundValue rndValue_tp)
    {
        // save value
        markersData.useSp -= roundsData[rndValue_tp.index].spCount;
        markersData.useSp += rndValue_tp.spCount;
        roundsData[rndValue_tp.index].spCount = rndValue_tp.spCount;

        // handle panel
        SetSpMarkersText(rndValue_tp.spCount);
    }

    //--------------------------------------------------
    public void SaveRoundValue_Shien(RoundValue rndValue_tp)
    {
        RemoveToken();

        // save value
        tokensData.useShien++;
        roundsData[roundId].actionType = rndValue_tp.actionType;
        roundsData[roundId].tokenType = rndValue_tp.tokenType;
        roundsData[roundId].shienUnitId = rndValue_tp.shienUnitId;
        roundsData[roundId].tarUnitIndex = rndValue_tp.tarUnitIndex;

        // add token and shien
        AddTokenObj(tokensData.shien_Pf, (rndValue_tp.tarUnitIndex + 2));
        UnitCardData unitData_tp = controller_Cp.dataManager_Cp.GetUnitCardDataFromCardIndex(rndValue_tp.shienUnitId);
        AddShienUnit(unitData_tp);
    }

    //--------------------------------------------------
    public void SaveRoundValue_Move(RoundValue rndValue_tp)
    {
        RemoveToken();

        // save value
        switch (rndValue_tp.tokenType)
        {
            case TokenType.Move1: tokensData.useMove1++; break;
            case TokenType.Move2: tokensData.useMove2++; break;
            case TokenType.Move3: tokensData.useMove3++; break;
        }
        roundsData[roundId].actionType = rndValue_tp.actionType;
        roundsData[roundId].tokenType = rndValue_tp.tokenType;
        roundsData[roundId].oriUnitIndex = rndValue_tp.oriUnitIndex;
        roundsData[roundId].tarUnitIndex = rndValue_tp.tarUnitIndex;

        // add token
        GameObject token_Pf_tp = null;
        switch (rndValue_tp.tokenType)
        {
            case TokenType.Move1: token_Pf_tp = tokensData.move1_Pf; break;
            case TokenType.Move2: token_Pf_tp = tokensData.move2_Pf; break;
            case TokenType.Move3: token_Pf_tp = tokensData.move3_Pf; break;
        }
        AddTokenObj(token_Pf_tp, (rndValue_tp.oriUnitIndex + 2));
    }

    //--------------------------------------------------
    public void SaveRoundValue_Atk(RoundValue rndValue_tp)
    {
        RemoveToken();

        // save value
        switch (rndValue_tp.tokenType)
        {
            case TokenType.Attack1: tokensData.useAtk1++; break;
            case TokenType.Attack2: tokensData.useAtk2++; break;
        }
        roundsData[roundId].actionType = rndValue_tp.actionType;
        roundsData[roundId].tokenType = rndValue_tp.tokenType;
        roundsData[roundId].oriUnitIndex = rndValue_tp.oriUnitIndex;
        roundsData[roundId].tarUnitIndex = rndValue_tp.tarUnitIndex;
        roundsData[roundId].atkType = rndValue_tp.atkType;

        // add token
        GameObject token_Pf_tp = null;
        switch (rndValue_tp.tokenType)
        {
            case TokenType.Attack1: token_Pf_tp = tokensData.atk1_Pf; break;
            case TokenType.Attack2: token_Pf_tp = tokensData.atk2_Pf; break;
        }

        int tokenIndex_tp = -1;
        if (rndValue_tp.tarUnitIndex == 0) { tokenIndex_tp = 1; }
        else if (rndValue_tp.tarUnitIndex == 1) { tokenIndex_tp = 0; }
        AddTokenObj(token_Pf_tp, tokenIndex_tp);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Remove rounds value
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Remove rounds value

    //--------------------------------------------------
    void RemoveToken()
    {
        RoundValue rndValue_tp = roundsData[roundId];
        switch (rndValue_tp.actionType)
        {
            case ActionType.Shien: RemoveRoundValue_Shien(rndValue_tp); break;
            case ActionType.Move: RemoveRoundValue_Move(rndValue_tp); break;
            case ActionType.Atk: RemoveRoundValue_Atk(rndValue_tp); break;
        }
    }

    //--------------------------------------------------
    public void RemoveRoundValue_Guard(RoundValue rndValue_tp)
    {
        markersData.useSp -= rndValue_tp.spCount;
        roundsData[roundId].spCount = 0;

        SetSpMarkersText(0);
    }

    //--------------------------------------------------
    public void RemoveRoundValue_Shien(RoundValue rndValue_tp)
    {
        tokensData.useShien--;
        roundsData[roundId].actionType = ActionType.Null;
        roundsData[roundId].tokenType = TokenType.Null;
        roundsData[roundId].shienUnitId = -1;
        roundsData[roundId].oriUnitIndex = -1;
        roundsData[roundId].tarUnitIndex = -1;

        RemoveTokenObj();
        RemovePbShienUnit();
    }

    //--------------------------------------------------
    public void RemoveRoundValue_Move(RoundValue rndValue_tp)
    {
        switch (rndValue_tp.tokenType)
        {
            case TokenType.Move1: tokensData.useMove1--; break;
            case TokenType.Move2: tokensData.useMove2--; break;
            case TokenType.Move3: tokensData.useMove3--; break;
        }
        roundsData[roundId].actionType = ActionType.Null;
        roundsData[roundId].tokenType = TokenType.Null;
        roundsData[roundId].oriUnitIndex = -1;
        roundsData[roundId].tarUnitIndex = -1;

        RemoveTokenObj();
    }

    //--------------------------------------------------
    public void RemoveRoundValue_Atk(RoundValue rndValue_tp)
    {
        switch (rndValue_tp.tokenType)
        {
            case TokenType.Attack1: tokensData.useAtk1--; break;
            case TokenType.Attack2: tokensData.useAtk2--; break;
        }
        roundsData[roundId].actionType = ActionType.Null;
        roundsData[roundId].tokenType = TokenType.Null;
        roundsData[roundId].oriUnitIndex = -1;
        roundsData[roundId].tarUnitIndex = -1;
        roundsData[roundId].atkType = AttackType.Null;

        RemoveTokenObj();
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Events from external
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Events from external

    //--------------------------------------------------
    public void OnClicked()
    {
        onClickedAction.Invoke(roundId);
    }

    #endregion

}
