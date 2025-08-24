using UnityEngine;

public class MidoriPlane : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("PinkEnemy") || other.CompareTag("GreenEnemy") || other.CompareTag("YellowEnemy") || other.CompareTag("PurpleEnemy") ||
            other.CompareTag("BossEnemy") || other.CompareTag("EnemyBullet") || other.CompareTag("EnemyLaser"))
        {
            StartCoroutine(ShootingGameManager.Instance.HitByEnemy(gameObject, other.gameObject));
        }
    }
}
