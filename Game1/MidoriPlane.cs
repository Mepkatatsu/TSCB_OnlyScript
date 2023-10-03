using SingletonPattern;
using UnityEngine;

public class MidoriPlane : MonoBehaviour
{
    private ShootingGameManager _shootingGameManager;

    private void Awake()
    {
        _shootingGameManager = ShootingGameManager.Instance;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("PinkEnemy") || other.CompareTag("GreenEnemy") || other.CompareTag("YellowEnemy") || other.CompareTag("PurpleEnemy") ||
            other.CompareTag("BossEnemy") || other.CompareTag("EnemyBullet") || other.CompareTag("EnemyLaser"))
        {
            StartCoroutine(_shootingGameManager.HitByEnemy(gameObject, other.gameObject));
        }
    }
}
