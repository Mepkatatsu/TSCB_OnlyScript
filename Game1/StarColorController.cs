using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StarColorController : MonoBehaviour
{
    [SerializeField] public float startDelayTime;

    private const float starSpeed = 1.0f;

    private void Start()
    {
        gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        Invoke(nameof(StarTwinkle), startDelayTime);
        Invoke(nameof(StarFallLoop), StarFallStart());
    }

    private void StarTwinkle()
    {
        Sequence starTwinkle = DOTween.Sequence();

        starTwinkle.Append(gameObject.GetComponent<Image>().DOFade(1, 1)).Append(gameObject.GetComponent<Image>().DOFade(0, 1)).SetLoops(-1, LoopType.Yoyo);
    }

    private float StarFallStart()
    {
        Sequence starFall = DOTween.Sequence();

        float spendTime = (gameObject.GetComponent<RectTransform>().anchoredPosition.y + 600) / 100 * starSpeed;

        starFall.Append(gameObject.GetComponent<RectTransform>().DOLocalMoveY(-600, spendTime).SetEase(Ease.Linear))
            .Append(gameObject.GetComponent<RectTransform>().DOLocalMoveY(600, 0f));

        return spendTime;
    }

    private void StarFallLoop()
    {
        Sequence starFall = DOTween.Sequence();

        float spendTime = (600 + 600) / 100 * starSpeed;

        starFall.Append(gameObject.GetComponent<RectTransform>().DOLocalMoveY(-600, spendTime).SetEase(Ease.Linear))
            .Append(gameObject.GetComponent<RectTransform>().DOLocalMoveY(600, 0f)).SetLoops(-1);
    }
}
