using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;

    public static GameManager instance;
    public List<GameObject> lapTriggers = new List<GameObject>();
    public GameObject[] finisherTextUI;

    // Start is called before the first frame update
    void Start()
    {
        if(PhotonNetwork.IsConnected)
        {
            if(playerPrefab != null)
            {
                StartCoroutine(DelayedPlayerSpawn());

            }
        }

        foreach(GameObject go in finisherTextUI)
        {
            go.SetActive(false);
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.NickName + " has joined the room!");
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " has joined the room " + PhotonNetwork.CurrentRoom.Name);
        Debug.Log("Room has now " + PhotonNetwork.CurrentRoom.PlayerCount + " /10 players.");
    }

    IEnumerator DelayedPlayerSpawn()
    {
        yield return new WaitForSeconds(3);
        int xRandomPoint = Random.Range(-10, 10);
        int zRandomPoint = Random.Range(-10, 10);
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(xRandomPoint, 7, zRandomPoint), Quaternion.identity);
    }
}
