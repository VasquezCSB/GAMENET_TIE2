using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class UsernameDisplay : MonoBehaviour
{
    [SerializeField] PhotonView playerPV;
    [SerializeField] TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text.text = playerPV.Owner.NickName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
