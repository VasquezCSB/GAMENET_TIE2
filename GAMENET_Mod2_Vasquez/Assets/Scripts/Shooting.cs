using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class Shooting : MonoBehaviourPunCallbacks
{
    public Camera camera;
    public GameObject hitEffectPrefab;
    public static KillListing killListing;
    public TMP_Text playerName;
    public TMP_Text enemyName;

    //HP Related
    [Header("HP Related Stuff")]
    //public float startHealth = 100;
    public Image healthBar;
    private float health;

    private Animator animator;
    private int winCounter;


    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
        healthBar.fillAmount = health / startHealth;
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Fire()
    {
        RaycastHit hit;
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        
        //When Raycast hit, it will say name of game object and instantiate the hitEffectPrefab
        if (Physics.Raycast(ray, out hit, 200))
        {
            Debug.Log(hit.collider.gameObject.name);
            //Only way is with MonoBehaviourPunCallbacks
            photonView.RPC("CreateHitEffects", RpcTarget.All, hit.point);
            
            if (hit.collider.gameObject.CompareTag("Player") && !hit.collider.gameObject.GetComponent<PhotonView>().IsMine)
            {
                hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 25);
            }
        }
    }

    [PunRPC]
    public void TakeDamage(int damage, PhotonMessageInfo info)
    {
        this.health -= damage;
        this.healthBar.fillAmount = health / startHealth;

        if(health == 0)
        {
            //hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 25);

            Die();
            //Who raycast hit + killed + owner(you) 
            Debug.Log(info.Sender.NickName + " killed " + info.photonView.Owner.NickName);
            photonView.RPC("Win", RpcTarget.AllBuffered, 1);

        }
    }

    [PunRPC]
    public void Win(int winCount, PhotonMessageInfo info)
    {
        winCounter += winCount;
        killListing.SetNames(info.Sender.NickName, info.photonView.Owner.NickName);

        if (winCounter == 5)
        {
            Debug.Log(info.Sender.NickName + " wins ");
            //Debug.Log(info.Sender.NickName + " killed " + info.photonView.Owner.NickName);
        }
    }

    [PunRPC]
    public void CreateHitEffects(Vector3 position)
    {
        GameObject hitEffectGameObject = Instantiate(hitEffectPrefab, position, Quaternion.identity);
        Destroy(hitEffectGameObject, 0.2f);
    }

    public void Die()
    {
        if(photonView.IsMine)
        {
            animator.SetBool("isDead", true);
            StartCoroutine(RespawnCountdown());

            //Debug.Log(info.Sender.NickName + " killed " + info.photonView.Owner.NickName);
            photonView.RPC("Win", RpcTarget.AllBuffered, 1);
        }

    }

    //Respawns Char when dead
    IEnumerator RespawnCountdown()
    {

        GameObject respawnText = GameObject.Find("RespawnText");

        float respawnTime = 5.0f;

        while (respawnTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            respawnTime--;

            transform.GetComponent<PlayerMovementController>().enabled = false;
            respawnText.GetComponent<TextMeshProUGUI>().text = "You are killed. Respawning in " + respawnTime.ToString(".00");
        }
        
        animator.SetBool("isDead", false);
        respawnText.GetComponent<TextMeshProUGUI>().text = "";

        //Spawn player at new random point
        int randomPointX = Random.Range(-20, 20);
        int randomPointZ = Random.Range(-20, 20);

        this.transform.position = new Vector3(randomPointX, 0, randomPointZ);
        transform.GetComponent<PlayerMovementController>().enabled = true;

        photonView.RPC("RegainHealth", RpcTarget.AllBuffered);
        
    }

    [PunRPC]
    public void RegainHealth()
    {
        health = 100;
        healthBar.fillAmount = health / startHealth;
    }
}
