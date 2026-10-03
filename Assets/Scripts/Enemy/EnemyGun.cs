using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    public Transform muzzlePoint;
    public GameObject tracerPrefab;
    public AudioClip gunSound;

    // 射撃設定
    public int damage = 1;
    public float fireRate = 2f;
    public float range = 50f;
    public float tracerTime = 0.05f;
    public float spread = 3f;

    AudioSource audioSource;
    float nextFireTime;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // 3D空間で聞こえる音にする
        audioSource.spatialBlend = 1f;
        audioSource.minDistance = 10f;
        audioSource.maxDistance = 50f;
    }

    // プレイヤーへ射撃
    public void Shoot(Transform target)
    {
        if (target == null || muzzlePoint == null)
        {
            return;
        }

        if (Time.time < nextFireTime)
        {
            return;
        }

        nextFireTime = Time.time + 1f / fireRate;

       // プレイヤーの中心を狙う
        Vector3 targetPosition = target.position;
        Vector3 direction = (targetPosition - muzzlePoint.position).normalized;
        // 射撃方向にランダムなブレを加える
        float randomX = Random.Range(-spread, spread);
        float randomY = Random.Range(-spread, spread);

        Quaternion spreadRotation = Quaternion.Euler(randomX, randomY, 0f);
        direction = spreadRotation * direction;

        Vector3 endPoint =
            muzzlePoint.position + direction * range;

        // 銃声を再生
        if (gunSound != null)
        {
            audioSource.PlayOneShot(gunSound);
        }

        // 銃口からRaycastを飛ばす
        if (Physics.Raycast(
            muzzlePoint.position,
            direction,
            out RaycastHit hit,
            range))
        {
            endPoint = hit.point;

            IDamageable damageable =
                hit.collider.GetComponentInParent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
        }

        // 銃口から着弾地点まで弾道を表示
        if (tracerPrefab != null)
        {
            GameObject tracer = Instantiate(tracerPrefab);

            TracerScript tracerScript =
                tracer.GetComponent<TracerScript>();

            if (tracerScript != null)
            {
                tracerScript.Show(
                    muzzlePoint.position,
                    endPoint,
                    tracerTime
                );
            }
        }
    }
}