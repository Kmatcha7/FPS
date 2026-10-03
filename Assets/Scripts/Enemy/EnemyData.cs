using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public int maxHP = 100;

    public float moveSpeed = 3.5f;
    public float rotationSpeed = 5f;

    public float detectionRange = 30f;
    public float attackRange = 15f;
    public float fieldOfView = 90f;
    // 巡回設定
    public float patrolRadius = 10f;
    public float patrolWaitTime = 2f;
}