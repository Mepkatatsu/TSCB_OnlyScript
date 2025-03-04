using UnityEngine;

public class MidoriBullet : MonoBehaviour
{
    private bool _isAvailableAttack = true;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PinkEnemy") || other.CompareTag("GreenEnemy") || other.CompareTag("YellowEnemy") || other.CompareTag("PurpleEnemy") || other.CompareTag("BossEnemy"))
        {
            StartCoroutine(ShootingGameManager.Instance.HitByBullet(gameObject, other.gameObject));
        }
    }

    public void SetIsAvailableAttack(bool isAvailableAttack)
    {
        _isAvailableAttack = isAvailableAttack;
    }

    public bool GetIsAvailableAttack()
    {
        return _isAvailableAttack;
    }
}
