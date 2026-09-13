using UnityEngine;

public class ThreeButtonsActivator : MonoBehaviour
{
    [Header("三个按钮")]
    public GameObject button1;
    public GameObject button2;
    public GameObject button3;

    [Header("目标对象")]
    public GameObject targetObject;     // 三个按钮都点击后激活的对象

    private bool isButton1Clicked = false;
    private bool isButton2Clicked = false;
    private bool isButton3Clicked = false;

    void Start()
    {
        // 确保目标对象初始隐藏
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }

    // 分别给三个按钮绑定的方法
    public void OnButton1Click()
    {
        isButton1Clicked = true;
        CheckAllButtonsClicked();
    }

    public void OnButton2Click()
    {
        isButton2Clicked = true;
        CheckAllButtonsClicked();
    }

    public void OnButton3Click()
    {
        isButton3Clicked = true;
        CheckAllButtonsClicked();
    }

    private void CheckAllButtonsClicked()
    {
        if (isButton1Clicked && isButton2Clicked && isButton3Clicked)
        {
            // 三个按钮都被点击了
            if (targetObject != null)
            {
                targetObject.SetActive(true);
                Debug.Log("三个按钮都已点击，目标对象已激活！");
            }
        }
    }

    // 重置所有状态（可选）
    public void ResetAll()
    {
        isButton1Clicked = false;
        isButton2Clicked = false;
        isButton3Clicked = false;

        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }
}