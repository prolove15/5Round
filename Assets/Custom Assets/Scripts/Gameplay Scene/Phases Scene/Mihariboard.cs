using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mihariboard : MonoBehaviour
{

    Controller_Phases controller_Cp;
    PlayerFaction player_Cp;

    [ReadOnly] public List<UnitCardData> mUnitsData = new List<UnitCardData>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(PlayerFaction player_Cp_tp)
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        player_Cp = player_Cp_tp;

        mUnitsData = controller_Cp.dataManager_Cp.psMihariUnitCardsData[player_Cp.playerId].unitCards;
    }

}
