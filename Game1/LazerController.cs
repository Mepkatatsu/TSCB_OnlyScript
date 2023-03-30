using SingletonPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerController : MonoBehaviour
{
    LineRenderer _lineRenderer;
    EdgeCollider2D _edgeCollider;
    ShootingGameManager _shootingGameManager;

    private List<Vector2> _linePositionList;
    private Vector2 _startPosition;
    private Vector2 _targetPosition;

    private bool _canMoveLazer = false;

    public const int XLeftEnd = -390;
    public const int XRIghtEnd = 390;
    public const int YDownEnd = -570;
    public const int YUpEnd = 570;

    private void Awake()
    {
        _shootingGameManager = ShootingGameManager.Instance;
    }

    public void SetCanMoveLazer(bool canMoveLazer)
    {
        _canMoveLazer = canMoveLazer;
    }

    private Vector2 ExtendDirection(Vector2 startPosition, Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - startPosition;

        // 0���� ������ �ʵ��� ���� ó��
        if (direction.x == 0)
        {
            direction.x = startPosition.x;
            direction.y = (direction.y < 0) ? YDownEnd : YUpEnd;

            return direction;
        }
        else if (direction.y == 0)
        {
            direction.x = (direction.x < 0) ? XLeftEnd : XRIghtEnd;
            direction.y = startPosition.y;

            return direction;
        }

        float xToEnd, yToEnd, xToEndRate, yToEndRate;

        // x, y�� ���� ȭ�� ������ �̵��ϱ� ���� �ʿ��� �Ÿ�
        xToEnd = (direction.x < 0) ? XLeftEnd - startPosition.x : XRIghtEnd - startPosition.x;
        yToEnd = (direction.y < 0) ? YDownEnd - startPosition.y : YUpEnd - startPosition.y;

        // ȭ�� �۱��� �������� �󸶳� �̵��ؾ� �ϴ��� ����
        xToEndRate = xToEnd / direction.x;
        yToEndRate = yToEnd / direction.y;

        // ������ ���� �Ÿ� ����
        if (xToEndRate > yToEndRate)
        {
            direction.x *= yToEndRate;
            direction.x += startPosition.x;

            direction.y = (direction.y < 0) ? YDownEnd : YUpEnd;
        }
        else
        {
            direction.x = (direction.x < 0) ? XLeftEnd : XRIghtEnd;

            direction.y *= xToEndRate;
            direction.y += startPosition.y;
        }

        return direction;
    }

    public IEnumerator ShootLazer()
    {
        
        Color lazerColor = Color.white;

        // ColorUtility.TryParseHtmlString("#D1B2FF", out lazerColor);

        _lineRenderer.startColor = lazerColor;
        _lineRenderer.endColor = lazerColor;
        
        _shootingGameManager.SetIsShootingLazer(true);

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

        _shootingGameManager.SetIsShootingLazer(false);
    }

    public void SetColliderPosition()
    {
        Vector2 position;

        _linePositionList.Clear();

        for (int i = 0; i < _lineRenderer.positionCount; i++)
        {
            position = _lineRenderer.GetPosition(i);
            _linePositionList.Add(position);
        }

        _edgeCollider.SetPoints(_linePositionList);
    }

    public IEnumerator ChasePlane()
    {
        _canMoveLazer = true;

        _lineRenderer.startWidth = 0.01f;
        _lineRenderer.endWidth = 0.01f;

        _lineRenderer.startColor = Color.red;
        _lineRenderer.endColor = Color.red;

        while (_canMoveLazer)
        {
            // ������ ������ �������� �������� ��ġ ����
            _startPosition = new Vector2(_shootingGameManager._bossEnemy.GetComponent<RectTransform>().anchoredPosition.x + 28,
            _shootingGameManager._bossEnemy.GetComponent<RectTransform>().anchoredPosition.y - 2);

            _targetPosition = ExtendDirection(_startPosition,
                _shootingGameManager._midoriPlane.GetComponent<RectTransform>().anchoredPosition);

            _lineRenderer.SetPosition(0, _startPosition);
            _lineRenderer.SetPosition(1, _targetPosition);

            yield return null;
        }

        while (!_canMoveLazer)
        {
            _startPosition = new Vector2(_shootingGameManager._bossEnemy.GetComponent<RectTransform>().anchoredPosition.x + 28,
            _shootingGameManager._bossEnemy.GetComponent<RectTransform>().anchoredPosition.y - 2);

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
