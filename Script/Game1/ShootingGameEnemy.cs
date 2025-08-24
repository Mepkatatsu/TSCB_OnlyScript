using DG.Tweening;
using System.Collections;
using UnityEngine;

public class ShootingGameEnemy : MonoBehaviour
{
    [SerializeField] private int enemyMaxHp = 5;
    [SerializeField] private float attackDelay = 3;
    [SerializeField] private float speed;
    [SerializeField] private bool isBoss;

    public int EnemyHP { get; private set; } = 5;

    private bool _isAvailableAttack;

    private Sequence _sequence;

    public void InitializeEnemy()
    {
        StopAllCoroutines();

        _isAvailableAttack = false;

        SetEnemyMaxHP();

        StartCoroutine(!isBoss ? DoEnemyAttack() : DoBossMove());
    }

    public void SetEnemyMaxHP()
    {
        EnemyHP = enemyMaxHp;
    }

    public void StopMoving()
    {
        StopAllCoroutines();
        _sequence.Kill();
    }

    #region Boss enemy action
    private IEnumerator DoBossAttack()
    {
        while(true)
        {
            for (int i = 0; i < 5; i++)
            {
                // 죽었을 때는 스킬이 시전되지 않고, 적 소환하지 않음 (최소 1초 대기)

                while (!ShootingGameManager.Instance.IsAlivePlane)
                {
                    yield return new WaitForSeconds(1);
                }

                if (i < 4)
                {
                    StartCoroutine(ShootingGameManager.Instance.UseBossSkill(i));
                }
                else
                {
                    StartCoroutine(ShootingGameManager.Instance.DoNextPhase());
                }

                yield return new WaitForSeconds(4);
            }
        }
    }

    private IEnumerator DoBossMove()
    {
        _sequence = DOTween.Sequence();

        _sequence.Append(gameObject.GetComponent<RectTransform>().DOLocalMoveY(450, 2).SetEase(Ease.Linear));

        yield return new WaitForSeconds(2.5f);

        StartCoroutine(DoBossAttack());

        _sequence = DOTween.Sequence();

        _sequence.Append(gameObject.GetComponent<RectTransform>().DOLocalMoveX(-300, 2).SetEase(Ease.Linear));

        yield return new WaitForSeconds(2);

        _sequence = DOTween.Sequence();

        _sequence.Append(gameObject.GetComponent<RectTransform>().DOLocalMoveX(300, 4).SetEase(Ease.Linear))
            .Append(gameObject.GetComponent<RectTransform>().DOLocalMoveX(-300, 4).SetEase(Ease.Linear)).SetLoops(-1);
    }

    private IEnumerator DoEnemyAttack()
    {
        while(true)
        {
            if (!ShootingGameManager.Instance.IsAlivePlane)
            {
                _isAvailableAttack = false;
                yield return new WaitForSeconds(1);
            }
            else if (_isAvailableAttack == true)
            {
                ShootingGameManager.Instance.ShootEnemyBulletToMidoriPlane(gameObject);
                yield return new WaitForSeconds(attackDelay);
            }
            // 적 스폰 직후, 아군 부활 직후에 총알을 발사하지 않도록 최소 대기 시간 설정
            else
            {
                _isAvailableAttack = true;
                yield return new WaitForSeconds(1);
            }
        }
    }
    #endregion Boss enemy action

    #region Normal enemy action
    /*
    public IEnumerator DoBezierCurves(Vector2 startPosition, Vector2 controlPosition, Vector2 targetPosition)
    {
        float time = 0;

        while(true)
        {
            time += Time.deltaTime / 10;

            if(gameObject.GetComponent<RectTransform>().anchoredPosition == targetPosition)
            {
                yield break;
            }

            Vector2 position1 = Vector2.Lerp(startPosition, controlPosition, time);
            Vector2 position2 = Vector2.Lerp(controlPosition, targetPosition, time);

            gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(position1, position2, time);

            yield return null;
        }
    }
    */

    public void DoBezierCurves2(Vector2 startPosition, Vector2 controlPosition1, Vector2 controlPosition2, Vector2 targetPosition, float speed)
    {
        StartCoroutine(DoBezierCurves2Coroutine(startPosition, controlPosition1, controlPosition2, targetPosition, speed));
    }

    private IEnumerator DoBezierCurves2Coroutine(Vector2 startPosition, Vector2 controlPosition1, Vector2 controlPosition2, Vector2 targetPosition, float speed)
    {
        float time = 0;

        while (true)
        {
            time += Time.deltaTime / 10 * speed;

            if (!gameObject.activeSelf) yield break;

            if (time >= 1)
            {
                time = 0;
            }

            var position1 = Vector2.Lerp(startPosition, controlPosition1, time);
            var position2 = Vector2.Lerp(controlPosition1, controlPosition2, time);
            var position3 = Vector2.Lerp(controlPosition2, targetPosition, time);

            var position4 = Vector2.Lerp(position1, position2, time);
            var position5 = Vector2.Lerp(position2, position3, time);

            gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(position4, position5, time);

            yield return null;
        }
    }
    public void DoEnemyMove(Vector2 startPosition, Vector2 direction, float time)
    {
        StartCoroutine(DoEnemyMoveCoroutine(startPosition, direction, time));
    }

    private IEnumerator DoEnemyMoveCoroutine(Vector2 startPosition, Vector2 direction, float time)
    {
        while (gameObject.activeSelf)
        {
            _sequence = DOTween.Sequence();

            _sequence.Append(gameObject.GetComponent<RectTransform>().DOLocalMove(direction, time).SetEase(Ease.Linear));

            yield return new WaitForSeconds(time);

            gameObject.GetComponent<RectTransform>().anchoredPosition = startPosition;
        }
    }
    #endregion Normal enemy action

    public void SetEnemyHP(int hp)
    {
        EnemyHP = hp;
    }

    public float GetSpeed()
    {
        return speed;
    }
}
