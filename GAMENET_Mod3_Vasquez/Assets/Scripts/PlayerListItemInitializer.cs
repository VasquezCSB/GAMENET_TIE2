using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerListItemInitializer : MonoBehaviour
{
    [Header("UI Preferences")]
    public Text playerNameText;
    public Button playerReadyButton;
    public Image playerReadyImage;

    private bool isPlayerReady = false;

    public void Initialize(int playerID, string playerName)
    {
        playerNameText.text = playerName;

        if (PhotonNetwork.LocalPlayer.ActorNumber != playerID)
        {
            playerReadyButton.gameObject.SetActive(false);
        } else
        {
            ExitGames.Client.Photon.Hashtable 
                initializeProperties = new ExitGames.Client.Photon.Hashtable() { { Constants.PLAYER_READY, isPlayerReady} };
            PhotonNetwork.LocalPlayer.SetCustomProperties(initializeProperties);

            playerReadyButton.onClick.AddListener(() =>
            {
                isPlayerReady = !isPlayerReady;
                SetPlayerReady(isPlayerReady);

                ExitGames.Client.Photon.Hashtable newProperties =
                    new ExitGames.Client.Photon.Hashtable() { { Constants.PLAYER_READY, isPlayerReady } };

                PhotonNetwork.LocalPlayer.SetCustomProperties(newProperties);
            });

        }
    }

    public void SetPlayerReady(bool playerReady)
    {
        playerReadyImage.enabled = playerReady;

        if(playerReady )
        {
            playerReadyButton.GetComponentInChildren<Text>().text = "Ready!";
        } else
        {
            playerReadyButton.GetComponentInChildren<Text>().text = "Ready?";

        }
    }
}
