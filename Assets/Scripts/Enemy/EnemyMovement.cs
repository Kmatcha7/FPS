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

    // 指定した場所へ移動
    public void MoveTo(Vector3 position)
    {
        if (!agent.isOnNavMesh)
        {
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(position);
    }

    // 移動を停止
    public void Stop()
    {
        if (!agent.isOnNavMesh)
        {
            return;
        }

        agent.isStopped = true;
    }

    // 指定した方向を向く
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

    // 巡回エリア内からランダムな地点を探す
    public bool TryGetRandomPatrolPoint(
        Vector3 patrolCenter,
        float patrolRadius,
        out Vector3 patrolPoint)
    {
        Vector2 randomCircle = Random.insideUnitCircle;

        Vector3 randomPosition = patrolCenter + new Vector3(
            randomCircle.x * patrolRadius,
            0f,
            randomCircle.y * patrolRadius
        );

        // ランダム地点付近のNavMeshを探す
        if (NavMesh.SamplePosition(
            randomPosition,
            out NavMeshHit hit,
            2f,
            NavMesh.AllAreas))
        {
            patrolPoint = hit.position;
            return true;
        }

        patrolPoint = patrolCenter;
        return false;
    }

    // 目的地に到着したか確認
    public bool HasReachedDestination()
    {
         if (!agent.isOnNavMesh || agent.pathPending)
        {
            return false;
        }

        if (agent.remainingDistance > agent.stoppingDistance + 0.1f)
        {
            return false;
        }

        // ほぼ停止していれば到着
        return agent.velocity.sqrMagnitude < 0.01f;
    }
    public bool TryGetRandomCombatPoint(Vector3 center,float radius,out Vector3 combatPoint)
    {
        Vector2 randomCircle = Random.insideUnitCircle.normalized;
        Vector3 randomPosition = center + new Vector3(randomCircle.x * radius,0f,randomCircle.y * radius);
        // NavMesh上の移動可能な場所を探す
        if (NavMesh.SamplePosition(randomPosition,out NavMeshHit hit,2f,NavMesh.AllAreas))
        {
            combatPoint = hit.position;
            return true;
        }

        combatPoint = center;
        return false;
    }
}