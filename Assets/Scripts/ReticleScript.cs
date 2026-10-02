using UnityEngine;

public class ReticleScript : MonoBehaviour
{
    public GunScript gun;
    public RectTransform top;
    public RectTransform bottom;
    public RectTransform left;
    public RectTransform right;

    public float baseDistance = 10f;
    public float spreadMultiplier = 0.2f;
    public float moveSpeed = 10f;

    void Update()
    {
        float distance = baseDistance + gun.CalculateSpread() * spreadMultiplier;

        top.anchoredPosition = Vector2.Lerp(top.anchoredPosition, new Vector2(0, distance), Time.deltaTime * moveSpeed);
        bottom.anchoredPosition = Vector2.Lerp(bottom.anchoredPosition, new Vector2(0, -distance), Time.deltaTime * moveSpeed);
        left.anchoredPosition = Vector2.Lerp(left.anchoredPosition, new Vector2(-distance, 0), Time.deltaTime * moveSpeed);
        right.anchoredPosition = Vector2.Lerp(right.anchoredPosition, new Vector2(distance, 0), Time.deltaTime * moveSpeed);
    }
}