using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public EnemyData enemyData;
    public Transform eyePoint;

    public Transform Player { get; private set; }
    public float DistanceToPlayer { get; private set; }
    public bool IsPlayerDetected { get; private set; }
    public bool IsPlayerVisible { get; private set; }

    void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            Player = playerObject.transform;
        }
    }

    void Update()
    {
        if (Player == null)
        {
            return;
        }

        DistanceToPlayer = Vector3.Distance(transform.position, Player.position);

        IsPlayerDetected = DistanceToPlayer <= enemyData.detectionRange;
        IsPlayerVisible = IsPlayerDetected && CanSeePlayer();
    }

    bool CanSeePlayer()
    {
        if (eyePoint == null)
        {
            return false;
        }

        Vector3 targetPosition = Player.position + Vector3.up;
        Vector3 direction = targetPosition - eyePoint.position;

        float angle = Vector3.Angle(transform.forward, direction);

        if (angle > enemyData.fieldOfView / 2f)
        {
            return false;
        }

        if (Physics.Raycast(
            eyePoint.position,
            direction.normalized,
            out RaycastHit hit,
            direction.magnitude))
        {
            return hit.collider.CompareTag("Player");
        }

        return false;
    }
}