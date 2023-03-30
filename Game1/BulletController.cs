using SingletonPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletController : MonoBehaviour
{
    ShootingGameManager _shootingGameManager;

    private bool _canAttack = true;

    private void Awake()
    {
        _shootingGameManager = ShootingGameManager.Instance;
    }

    public void SetCanAttack(bool canAttack)
    {
        _canAttack = canAttack;
    }

    public bool GetCanAttack()
    {
        return _canAttack;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameObject.CompareTag("MidoriBullet"))
        {
            if (other.CompareTag("PinkEnemy") || other.CompareTag("GreenEnemy") || other.CompareTag("YellowEnemy") || other.CompareTag("PurpleEnemy") || other.CompareTag("BossEnemy"))
            {
                StartCoroutine(_shootingGameManager.HitByBullet(gameObject, other.gameObject));
            }
        }
    }
}
