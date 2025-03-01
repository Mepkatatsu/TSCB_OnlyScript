using SingletonPattern;
using UnityEngine;

public class MidoriBullet : MonoBehaviour
{
    private ShootingGameManager _shootingGameManager;

    private bool _isAvailableAttack = true;

    private void Awake()
    {
        if(_shootingGameManager == null)
            _shootingGameManager = ShootingGameManager.Instance;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PinkEnemy") || other.CompareTag("GreenEnemy") || other.CompareTag("YellowEnemy") || other.CompareTag("PurpleEnemy") || other.CompareTag("BossEnemy"))
        {
            StartCoroutine(_shootingGameManager.HitByBullet(gameObject, other.gameObject));
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
