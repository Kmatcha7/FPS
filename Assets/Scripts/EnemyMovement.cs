using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public EnemyData enemyData;

    NavMeshAgent agent;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        agent.speed = enemyData.moveSpeed;
    }

    public void MoveTo(Vector3 position)
    {
        if (!agent.isOnNavMesh)
        {
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(position);
    }

    public void Stop()
    {
        if (!agent.isOnNavMesh)
        {
            return;
        }

        agent.isStopped = true;
    }

    public void LookAt(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude == 0f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            enemyData.rotationSpeed * Time.deltaTime
        );
    }
}