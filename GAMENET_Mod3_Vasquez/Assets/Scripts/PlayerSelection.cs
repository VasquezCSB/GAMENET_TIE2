using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerSelection : MonoBehaviour
{
    public GameObject[] selectablePlayers;
    public int playerSelectionNumber;

    // Start is called before the first frame update
    void Start()
    {
        playerSelectionNumber = 0;
        ActivatePlayer(playerSelectionNumber);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ActivatePlayer(int x)
    {
        foreach(GameObject go in selectablePlayers)
        {
            go.SetActive(false);
        }

        selectablePlayers[x].SetActive(true);

        //Setting player selection for vehicle

        ExitGames.Client.Photon.Hashtable playerSelectionProperties 
            = new ExitGames.Client.Photon.Hashtable() { { Constants.PLAYER_SELECTION_NUMBER, playerSelectionNumber } };

        PhotonNetwork.LocalPlayer.SetCustomProperties(playerSelectionProperties);
    }

    public void goToNextPlayer()
    {
        playerSelectionNumber++;

        if(playerSelectionNumber >= selectablePlayers.Length)
        {
            playerSelectionNumber = 0;
        }

        ActivatePlayer(playerSelectionNumber);
    }

    public void goToPrevPlayer()
    {
        playerSelectionNumber--;

        if (playerSelectionNumber < 0)
        {
            playerSelectionNumber = selectablePlayers.Length - 1;
        }

        ActivatePlayer(playerSelectionNumber);
    }
}
