using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
//using UnityEngine.UIElements;

public class Shooting : MonoBehaviourPunCallbacks
{
    public Camera camera;
    public GameObject hitEffectPrefab;

    [Header("HP Related Stuff")]
    public float startHealth = 100;
    public Image healthBar;

    public GameObject gunHolder;
    [SerializeField] Image target;
    public GameObject noGun;

    public bool canFire;
    private float health;

    // Start is called before the first frame update
    void Start()
    {
        health = startHealth;
        healthBar.fillAmount = health / startHealth;
        canFire = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && photonView.IsMine && canFire == true)
        {
            Fire();
        }
    }

    public void Fire()
    {
        RaycastHit hit;
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        if(Physics.Raycast(ray, out hit, 200))
        {
            Debug.Log(hit.collider.gameObject.name);

            //GameObject hitEffectGameObject = Instantiate(hitEffectPrefab, hit.point, Quaternion.identity);
            //Destroy(hitEffectGameObject, 0.2f);

            //RpcTarget.All makes it so that all players can see the Hiteffect
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

        if (health <= 0)
        {
            //hit.collider.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.AllBuffered, 25);

            Die();
            //Who raycast hit + killed + owner(you) 
            Debug.Log(info.Sender.NickName + " killed " + info.photonView.Owner.NickName);
            //photonView.RPC("Win", RpcTarget.AllBuffered, 1);

        }
    }
    public void Die()
    {
        if (photonView.IsMine)
        {
            StartCoroutine(RespawnCountdown());

            //Debug.Log(info.Sender.NickName + " killed " + info.photonView.Owner.NickName);
            //photonView.RPC("Win", RpcTarget.AllBuffered, 1);
        }

    }

    [PunRPC]
    public void CreateHitEffects(Vector3 position)
    {
        GameObject hitEffectGameObject = Instantiate(hitEffectPrefab, position, Quaternion.identity);
        Destroy(hitEffectGameObject, 0.2f);
    }

    [PunRPC]
    public void Gun()
    {
        gunHolder.SetActive(true);
        //Destroy(noGun, 0.2f);
    }

    //Respawns Char when dead
    IEnumerator RespawnCountdown()
    {

        GameObject respawnText = GameObject.Find("RespawnText");

        float respawnTime = 6.0f;

        while (respawnTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            respawnTime--;

            transform.GetComponent<Player>().enabled = false;
            respawnText.GetComponent<TextMeshProUGUI>().text = "You are killed. Respawning in " + respawnTime.ToString(".00");
        }

        respawnText.GetComponent<TextMeshProUGUI>().text = "";

        //Spawn player at new random point
        int randomPointX = Random.Range(-10, 10);
        int randomPointZ = Random.Range(-10, 10);

        this.transform.position = new Vector3(randomPointX, 7, randomPointZ);
        transform.GetComponent<Player>().enabled = true;

        photonView.RPC("RegainHealth", RpcTarget.AllBuffered);

    }

    [PunRPC]
    public void RegainHealth()
    {
        health = 100;
        healthBar.fillAmount = health / startHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Gun") && photonView.IsMine)
        {
            Destroy(other.gameObject);
            //gunHolder.SetActive(true);
            photonView.RPC("Gun", RpcTarget.All);
            target.gameObject.SetActive(true);
            canFire = true;
        }
    }

}
