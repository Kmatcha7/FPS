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
        if (gun == null)
        {
            return;
        }

        // 銃の現在の拡散値からレティクルの広がりを計算
        float spread = gun.CalculateSpread();
        float targetDistance = baseDistance + spread * spreadMultiplier;

        // レティクルを滑らかに移動
        MoveReticle(top, new Vector2(0f, targetDistance));
        MoveReticle(bottom, new Vector2(0f, -targetDistance));
        MoveReticle(left, new Vector2(-targetDistance, 0f));
        MoveReticle(right, new Vector2(targetDistance, 0f));
    }

    // 指定した位置へ滑らかに移動
    void MoveReticle(RectTransform part, Vector2 targetPosition)
    {
        if (part == null)
        {
            return;
        }

        part.anchoredPosition = Vector2.Lerp(
            part.anchoredPosition,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }
}