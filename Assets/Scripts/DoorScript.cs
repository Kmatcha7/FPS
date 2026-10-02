using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
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
        //Debug.Log("hi");
        if(other.gameObject.tag == "Player")
        {
            //keyManagerObject.GetComponent<KeyManager>().key;
            if(keyManagerObject.GetComponent<KeyManager>().key == 5)
            {
                Destroy(gameObject);
            }
        }
        
    }
    
}
