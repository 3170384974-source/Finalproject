using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ClickSequence : MonoBehaviour
{
    [Header("点击次数设置")]
    public int totalClicks = 4;

    [Header("每轮激活的对象组（按顺序）")]
    public GameObject[] firstClickObjects;
    public GameObject[] secondClickObjects;
    public GameObject[] thirdClickObjects;

    [Header("第四次点击关闭的对象")]
    public GameObject objectToDisable;

    [Header("依次激活间隔时间")]
    public float intervalTime = 0.5f;         // 每个对象激活的时间间隔

    [Header("UI按钮（可选）")]
    public Button clickButton;

    private int currentClickCount = 0;

    void Start()
    {
        if (clickButton != null)
        {
            clickButton.onClick.AddListener(OnButtonClicked);
        }
    }

    public void OnButtonClicked()
    {
        currentClickCount++;

        switch (currentClickCount)
        {
            case 1:
                StartCoroutine(ActivateObjectsSequentially(firstClickObjects));
                break;

            case 2:
                StartCoroutine(ActivateObjectsSequentially(secondClickObjects));
                break;

            case 3:
                StartCoroutine(ActivateObjectsSequentially(thirdClickObjects));
                break;

            case 4:
                if (objectToDisable != null)
                {
                    objectToDisable.SetActive(false);
                }
                currentClickCount = 0;
                break;
        }
    }

    // 协程：依次激活对象，每个间隔intervalTime秒
    IEnumerator ActivateObjectsSequentially(GameObject[] objects)
    {
        if (objects == null) yield break;

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] != null)
            {
                objects[i].SetActive(true);
                yield return new WaitForSeconds(intervalTime);
            }
        }
    }
}