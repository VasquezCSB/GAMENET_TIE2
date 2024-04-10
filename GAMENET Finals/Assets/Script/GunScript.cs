using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GunScript : MonoBehaviourPunCallbacks
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            NoGun();
        }
    }

    [PunRPC]
    public void NoGun()
    {
        Destroy(gameObject);
    }
}
