using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    [SerializeField] public GameObject theCamera;
    [SerializeField] TextMeshProUGUI playerNameText;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<LapController>().enabled = photonView.IsMine;
        if (photonView.IsMine)
        {
            transform.GetComponent<Player>().enabled = true;
            theCamera.GetComponent<Camera>().enabled = true;
        }else
        {
            transform.GetComponent<Player>().enabled = false;
            theCamera.GetComponent<Camera>().enabled = false;
        }

        playerNameText.text = photonView.Owner.NickName;
    }

}
