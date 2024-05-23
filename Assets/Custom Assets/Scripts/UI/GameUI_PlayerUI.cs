using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI_PlayerUI : MonoBehaviour
{

    [SerializeField] Image avatarImage_Cp;
    [SerializeField] Text playerNameText_Cp;
    [SerializeField] public Text playerApText_Cp;
    [SerializeField] GameObject fireImage_GO;
    [SerializeField] GameObject isReady_GO;

    public void Init()
    {

    }

    public void SetPlayerName(string playerName_tp)
    {
        playerNameText_Cp.text = playerName_tp;
    }

    public void SetPlayerAp(int playerAp_tp)
    {
        playerApText_Cp.text = "AP: " + playerAp_tp.ToString();
    }

    public void SetActiveFireImage(bool flag)
    {
        fireImage_GO.SetActive(flag);
    }

    public void SetActiveIsReady(bool flag)
    {
        isReady_GO.SetActive(flag);
    }

}
