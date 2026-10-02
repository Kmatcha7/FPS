using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public static float timer = 0f;

    public static void Reset()
    {
        timer = 0f;
    }

    void Start()
    {
        Reset();
    }

    void Update()
    {
        timer += Time.deltaTime;
    }
}
