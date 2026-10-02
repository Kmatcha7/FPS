using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KeyManager : MonoBehaviour
{
    public int key = 0;
    public Text keyText;

    // Start is called before the first frame update
    void Start()
    {
        keyText.text = "Key:" + key.ToString() + "/5";
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void KeyPlus()
    {
        key++;
        keyText.text = "Key:" + key.ToString() + "/5";
    }
    
}
