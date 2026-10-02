using UnityEngine;
using System.Collections;

public class TracerScript : MonoBehaviour
{
    LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    public void Show(Vector3 startPoint, Vector3 endPoint, float duration)
    {
        line.SetPosition(0, startPoint);
        line.SetPosition(1, endPoint);
        StartCoroutine(HideAfterTime(duration));
    }

    IEnumerator HideAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}