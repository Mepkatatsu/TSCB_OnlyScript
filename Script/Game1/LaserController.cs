using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private EdgeCollider2D _edgeCollider;

    private List<Vector2> _linePositionList;
    private Vector2 _startPosition;
    private Vector2 _targetPosition;

    private bool _isAvailableMoveLaser;

    private const int XLeftEnd = -390;
    private const int XRightEnd = 390;
    private const int YDownEnd = -570;
    private const int YUpEnd = 570;

    public void SetIsAvailableMoveLaser(bool canMoveLaser)
    {
        _isAvailableMoveLaser = canMoveLaser;
    }

    private static Vector2 ExtendDirection(Vector2 startPosition, Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - startPosition;

        // 0으로 나누지 않도록 예외 처리
        if (direction.x == 0)
        {
            direction.x = startPosition.x;
            direction.y = (direction.y < 0) ? YDownEnd : YUpEnd;

            return direction;
        }
        else if (direction.y == 0)
        {
            direction.x = (direction.x < 0) ? XLeftEnd : XRightEnd;
            direction.y = startPosition.y;

            return direction;
        }

        // x, y축 기준 화면 밖으로 이동하기 위해 필요한 거리
        var xToEnd = (direction.x < 0) ? XLeftEnd - startPosition.x : XRightEnd - startPosition.x;
        var yToEnd = (direction.y < 0) ? YDownEnd - startPosition.y : YUpEnd - startPosition.y;

        // 화면 밖까지 나가려면 얼마나 이동해야 하는지 비율
        var xToEndRate = xToEnd / direction.x;
        var yToEndRate = yToEnd / direction.y;

        // 비율에 따라 거리 연장
        if (xToEndRate > yToEndRate)
        {
            direction.x *= yToEndRate;
            direction.x += startPosition.x;

            direction.y = (direction.y < 0) ? YDownEnd : YUpEnd;
        }
        else
        {
            direction.x = (direction.x < 0) ? XLeftEnd : XRightEnd;

            direction.y *= xToEndRate;
            direction.y += startPosition.y;
        }

        return direction;
    }

    public IEnumerator ShootLaser()
    {
        
        var laserColor = Color.white;

        // ColorUtility.TryParseHtmlString("#D1B2FF", out laserColor);

        _lineRenderer.startColor = laserColor;
        _lineRenderer.endColor = laserColor;
        
        ShootingGameManager.Instance.SetIsShootingLaser(true);

        while (_lineRenderer.startWidth < 0.3f)
        {
            _lineRenderer.startWidth += 0.01f;
            _lineRenderer.endWidth += 0.01f;

            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.5f);

        while (_lineRenderer.startWidth > 0f)
        {
            _lineRenderer.startWidth -= 0.01f;
            _lineRenderer.endWidth -= 0.01f;

            yield return new WaitForSeconds(0.01f);
        }

        _lineRenderer.startColor = Color.red;
        _lineRenderer.endColor = Color.red;

        ShootingGameManager.Instance.SetIsShootingLaser(false);
    }

    private void SetColliderPosition()
    {
        _linePositionList.Clear();

        for (int i = 0; i < _lineRenderer.positionCount; i++)
        {
            Vector2 position = _lineRenderer.GetPosition(i);
            _linePositionList.Add(position);
        }

        _edgeCollider.SetPoints(_linePositionList);
    }

    public IEnumerator ChasePlane()
    {
        _isAvailableMoveLaser = true;

        _lineRenderer.startWidth = 0.01f;
        _lineRenderer.endWidth = 0.01f;

        _lineRenderer.startColor = Color.red;
        _lineRenderer.endColor = Color.red;

        while (_isAvailableMoveLaser)
        {
            // 보스의 눈에서 레이저가 나가도록 위치 설정
            _startPosition = new Vector2(ShootingGameManager.Instance.bossEnemy.GetComponent<RectTransform>().anchoredPosition.x + 28,
                ShootingGameManager.Instance.bossEnemy.GetComponent<RectTransform>().anchoredPosition.y - 2);

            _targetPosition = ExtendDirection(_startPosition,
                ShootingGameManager.Instance.midoriPlane.GetComponent<RectTransform>().anchoredPosition);

            _lineRenderer.SetPosition(0, _startPosition);
            _lineRenderer.SetPosition(1, _targetPosition);

            yield return null;
        }

        while (!_isAvailableMoveLaser)
        {
            _startPosition = new Vector2(ShootingGameManager.Instance.bossEnemy.GetComponent<RectTransform>().anchoredPosition.x + 28,
                ShootingGameManager.Instance.bossEnemy.GetComponent<RectTransform>().anchoredPosition.y - 2);

            _lineRenderer.SetPosition(0, _startPosition);

            SetColliderPosition();

            yield return null;
        }
    }

    private void Start()
    {
        _linePositionList = new List<Vector2>();

        _edgeCollider = gameObject.GetComponent<EdgeCollider2D>();

        _lineRenderer = GetComponent<LineRenderer>();

        //_lineRenderer.startWidth = 0.01f;
        //_lineRenderer.endWidth = 0.01f;

        _lineRenderer.startColor = Color.red;
        _lineRenderer.endColor = Color.red;
    }
}
