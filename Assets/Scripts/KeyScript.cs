using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    GameObject keyManagerObject;
    // Start is called before the first frame update
    void Start()
    {
        keyManagerObject = GameObject.Find("KeyManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            keyManagerObject.GetComponent<KeyManager>().KeyPlus();
            Destroy(gameObject);
        }
    }
}
