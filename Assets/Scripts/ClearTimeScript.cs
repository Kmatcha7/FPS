using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearTimeScript : MonoBehaviour
{
    public Text TimeText;
    // Start is called before the first frame update
    void Start()
    {
        TimeText.text = "クリアタイム:" + string.Format("{0:F1}", PlayerScript.cleartime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
