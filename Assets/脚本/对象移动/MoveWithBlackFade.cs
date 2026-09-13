using UnityEngine;
using System.Collections;

public class MoveWithBlackFade : MonoBehaviour
{
    [Header("移动设置")]
    public Transform targetObject;      // 要移动的对象
    public float moveDistance = 5f;     // Z轴移动距离
    public float moveDuration = 1f;     // 移动持续时间

    [Header("黑幕设置")]
    public CanvasGroup blackOverlay;    // 黑幕Canvas Group
    public float fadeDuration = 0.5f;   // 渐变时间

    private Vector3 originalPosition;
    private bool isMoving = false;

    void Start()
    {
        if (targetObject != null)
        {
            originalPosition = targetObject.position;
        }

        // 初始化黑幕为透明
        if (blackOverlay != null)
        {
            blackOverlay.alpha = 0f;
            blackOverlay.gameObject.SetActive(false);
        }
    }

    // UI按钮调用的方法
    public void OnMoveButtonClick()
    {
        if (!isMoving && targetObject != null)
        {
            StartCoroutine(MoveWithFade());
        }
    }

    IEnumerator MoveWithFade()
    {
        isMoving = true;

        // 1. 黑幕淡入
        yield return StartCoroutine(FadeBlack(0f, 1f));

        // 2. 瞬间移动对象
        Vector3 targetPosition = targetObject.position + Vector3.forward * moveDistance;
        targetObject.position = targetPosition;

        // 短暂等待（可选）
        yield return new WaitForSeconds(0.2f);

        // 3. 黑幕淡出
        yield return StartCoroutine(FadeBlack(1f, 0f));

        isMoving = false;
    }

    IEnumerator FadeBlack(float from, float to)
    {
        if (blackOverlay == null) yield break;

        blackOverlay.gameObject.SetActive(true);

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            blackOverlay.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }

        blackOverlay.alpha = to;

        if (to == 0f)
        {
            blackOverlay.gameObject.SetActive(false);
        }
    }

    // 重置位置的方法（可选）
    public void ResetPosition()
    {
        if (targetObject != null && !isMoving)
        {
            targetObject.position = originalPosition;
        }
    }
}