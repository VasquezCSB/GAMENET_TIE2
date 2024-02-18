using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {

        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (playerPrefab != null)
            {
                StartCoroutine(DelayedPlayerSpawn());
            }
        }


    }

    IEnumerator DelayedPlayerSpawn()
    {
        yield return new WaitForSeconds(3);
        Transform spawnPoint = SpawnManager.instance.GetSpawnPoint();
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(spawnPoint.position.x, 0, spawnPoint.position.z), spawnPoint.rotation);

        //int randomPointX = Random.Range(-10, 10);
        //int randomPointZ = Random.Range(-10, 10);
        //PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(randomPointX, 0, randomPointZ), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
