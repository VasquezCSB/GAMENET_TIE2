using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KillListing : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI killerDisplay;
    [SerializeField] TextMeshProUGUI killedDisplay;

    [SerializeField] TextMeshProUGUI howDisplay;
    [SerializeField] Image howImageDisplay;

    private static KillListing instance;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10f);
    }

    public void SetNames(string killerName, string killedName)
    {
        killerDisplay.text = killerName;
        killedDisplay.text = killedName;

    }

    public void SetNamesAndHow(string killerName, string killedName, string how)
    {
        killerDisplay.text = killerName;
        killedDisplay.text = killedName;

        howDisplay.text = how;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
