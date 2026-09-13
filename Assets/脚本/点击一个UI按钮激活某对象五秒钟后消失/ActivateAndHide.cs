using UnityEngine;
using System.Collections;

public class ActivateAndHide : MonoBehaviour
{
    [Header("目标对象")]
    public GameObject[] objectsToHide;  // 改为数组，可拖拽多个对象

    [Header("时间设置")]
    public float hideDelay = 5f;        // 隐藏延迟时间

    void Start()
    {
        // 确保所有对象初始为可见状态（可根据需求调整）
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }

    // UI按钮调用的方法
    public void OnActivateButtonClick()
    {
        StopAllCoroutines();
        StartCoroutine(HideAfterDelay());
    }

    IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);

        // 隐藏所有拖拽的对象
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }
}