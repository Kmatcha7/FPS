using UnityEngine;
using UnityEngine.AI;

public class MovingEnemyScript : MonoBehaviour
{
    NavMeshAgent agent;
    GameObject player;

    public float detectionRange = 30f;
    public float attackRange = 15f;
    public float rotationSpeed = 5f;
	public Transform eyePoint;
	public bool canSeePlayer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
		canSeePlayer = CanSeePlayer();
        if (player == null || !agent.isOnNavMesh)
        {
            return;
        }

        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > detectionRange)
        {
            agent.isStopped = true;
            return;
        }

        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.transform.position);
        }
        else
        {
            agent.isStopped = true;
            LookAtPlayer();
        }
		
    }

    void LookAtPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude == 0f)
        {
            return;
        }

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
	bool CanSeePlayer()
	{
		if (eyePoint == null)
		{
			return false;
		}

		Vector3 target = player.transform.position + Vector3.up;

		Vector3 direction = target - eyePoint.position;
		float distance = direction.magnitude;

		if (Physics.Raycast(eyePoint.position, direction.normalized, out RaycastHit hit, distance))
		{
			return hit.collider.CompareTag("Player");
		}

		return false;
	}
}
