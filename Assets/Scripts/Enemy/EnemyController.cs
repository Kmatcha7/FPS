using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Investigate,
        Combat
    }

    public EnemyState currentState = EnemyState.Patrol;

    EnemyDetection detection;
    EnemyMovement movement;
    EnemyGun gun;

    Vector3 patrolPoint;
    Vector3 patrolCenter;
    float patrolWaitTimer;
    bool hasPatrolPoint;

    public float patrolRadius = 10f;
    // 捜索設定
    public float investigateWaitTime = 3f;

    // 戦闘中の移動設定
    public float combatMoveInterval = 2f;
    public float combatMoveRadius = 4f;

    Vector3 lastSeenPosition;
    Vector3 combatMovePoint;

    float investigateTimer;
    float combatMoveTimer;

    bool hasInvestigatePoint;

    void Awake()
    {
        detection = GetComponent<EnemyDetection>();
        movement = GetComponent<EnemyMovement>();
        gun = GetComponent<EnemyGun>();

        // 最初に配置された場所を巡回の中心にする
        patrolCenter = transform.position;
    }

   void Update()
    {
        // プレイヤーが見えている間、最後に見た位置を記録
        if (detection.IsPlayerVisible)
        {
            lastSeenPosition = detection.Player.position;
            currentState = EnemyState.Combat;
        }

        // 現在の状態に応じて行動
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Investigate:
                Investigate();
                break;

            case EnemyState.Combat:
                Combat();
                break;
        }
    }

    // ランダムな地点を巡回
    void Patrol()
    {
        if (!hasPatrolPoint)
        {
            if (movement.TryGetRandomPatrolPoint(
                patrolCenter,
                patrolRadius,
                out patrolPoint))
            {
                hasPatrolPoint = true;
                patrolWaitTimer = 0f;
                movement.MoveTo(patrolPoint);
            }

            return;
        }

        // 目的地に着いたら待機
        if (movement.HasReachedDestination())
        {
            movement.Stop();
            patrolWaitTimer += Time.deltaTime;

            if (patrolWaitTimer >= detection.enemyData.patrolWaitTime)
            {
                patrolWaitTimer = 0f;
                hasPatrolPoint = false;
            }
        }
    }

    // プレイヤーと戦闘
   void Combat()
    {
        // プレイヤーを見失ったら最後に見た場所を調べる
        if (!detection.IsPlayerVisible)
        {
            currentState = EnemyState.Investigate;
            investigateTimer = 0f;
            hasInvestigatePoint = false;

            return;
        }

        float distance = detection.DistanceToPlayer;

        // 射程外ならプレイヤーへ接近
        if (distance > detection.enemyData.attackRange)
        {
            movement.MoveTo(detection.Player.position);
            return;
        }

        // 射程内ではプレイヤーを見ながら射撃
        movement.LookAt(detection.Player.position);

        if (gun != null)
        {
            gun.Shoot(detection.Player);
        }

        // 一定時間ごとに戦闘中の移動先を変更
        combatMoveTimer -= Time.deltaTime;

        if (combatMoveTimer <= 0f)
        {
            combatMoveTimer = combatMoveInterval;

            if (movement.TryGetRandomCombatPoint(
                transform.position,
                combatMoveRadius,
                out combatMovePoint))
            {
                movement.MoveTo(combatMovePoint);
            }
        }
    }
    // 最後にプレイヤーを見た場所を調べる
    void Investigate()
    {
        // 最後に見た場所へ移動
        if (!hasInvestigatePoint)
        {
            movement.MoveTo(lastSeenPosition);
            hasInvestigatePoint = true;
            investigateTimer = 0f;
        }

        // 最後に見た場所へ向かっている途中
        if (!movement.HasReachedDestination())
        {
            return;
        }

        // 到着したら停止して少し待つ
        movement.Stop();
        investigateTimer += Time.deltaTime;

        // 一定時間見つからなければ巡回に戻る
        if (investigateTimer >= investigateWaitTime)
        {
            investigateTimer = 0f;
            hasInvestigatePoint = false;
            hasPatrolPoint = false;

            currentState = EnemyState.Patrol;
        }
    }

    // Sceneビューに巡回範囲を表示
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, patrolRadius);
        // 最後にプレイヤーを見た位置
        Gizmos.DrawSphere(lastSeenPosition, 0.3f);
    }
}