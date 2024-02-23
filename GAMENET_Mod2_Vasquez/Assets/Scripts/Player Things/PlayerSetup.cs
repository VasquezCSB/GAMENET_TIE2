using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public GameObject fpsMode1;
    public GameObject nonFpsMode1;

    public GameObject playerUIPrefab;
    //public GameObject killFeedCanvasPrefab;

    public PlayerMovementController playerMovementController;
    public KillFeed killFeed;
    public Camera fpsCamera;
    public Avatar fpsAvatar, nonFpsAvatar;

    public TextMeshProUGUI playerNickName;
    public NetworkManager networkManager;

    private Animator animator;
    private Shooting shooting;

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<Text>().text = photonView.Owner.NickName;
        //playerNickName = GetComponent<PhotonView>().playerName;
        //networkManager = NetworkManager.instance.gameObject.GetComponent<NetworkManager>();

        //playerNickName.text = networkManager.playerName;

        playerMovementController = this.GetComponent<PlayerMovementController>();

        animator = this.GetComponent<Animator>();

        fpsMode1.SetActive(photonView.IsMine);
        nonFpsMode1.SetActive(!photonView.IsMine);
        animator.SetBool("isLocalPlayer", photonView.IsMine);

        shooting = this.GetComponent<Shooting>();

        //Other way: animator.avatar = photonView.IsMine ? fpsAvatar : nonFpsAvatar;
        if (photonView.IsMine)
        {
            this.animator.avatar = fpsAvatar;
        }
        else
        {
            this.animator.avatar = nonFpsAvatar;

        }

        //Makes a player able to control only their assigned prefab
        if (photonView.IsMine)
        {
            //Finds Player UI and attaches its elements here
            GameObject playerUI = Instantiate(playerUIPrefab);
            //GameObject killFeedCanvas = Instantiate(killFeedCanvasPrefab);

            playerMovementController.fixedTouchField = playerUI.transform.Find("RotationTouchField").GetComponent<FixedTouchField>();
            playerMovementController.joystick = playerUI.transform.Find("Fixed Joystick").GetComponent<Joystick>();

            //killFeed.killListingPrefab = killFeedCanvas.transform.Find("KillListing").GetComponent<KillFeed>();

            fpsCamera.enabled = true;

            //Adding onClick listener
            playerUI.transform.Find("Fire Button").GetComponent<Button>().onClick.AddListener(() => shooting.Fire());
        }
        else
        {
            playerMovementController.enabled = false;
            GetComponent<RigidbodyFirstPersonController>().enabled = false;
            fpsCamera.enabled = false;
        }
    }
}
