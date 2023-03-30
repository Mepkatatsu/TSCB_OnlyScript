using SingletonPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MidoriPlaneController : MonoBehaviour
{
    ShootingGameManager _shootingGameManager;

    private void Awake()
    {
        _shootingGameManager = ShootingGameManager.Instance;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("PinkEnemy") || other.CompareTag("GreenEnemy") || other.CompareTag("YellowEnemy") || other.CompareTag("PurpleEnemy") ||
            other.CompareTag("BossEnemy") || other.CompareTag("EnemyBullet") || other.CompareTag("EnemyLazer"))
        {
            StartCoroutine(_shootingGameManager.HitByEnemy(gameObject, other.gameObject));
        }
    }
}
