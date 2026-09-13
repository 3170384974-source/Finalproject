using UnityEngine;

public class FloatingLight : MonoBehaviour
{
    [Header("父物体缩放参数")]
    public float scaleMin = 0.9f;            // 最小缩放
    public float scaleMax = 1.1f;            // 最大缩放
    public float scaleSpeed = 2f;            // 缩放速度

    [Header("子灯光强度波动参数")]
    public string childLightName = "Point Light"; // 子灯光对象名称
    public float minIntensity = 0.5f;
    public float maxIntensity = 2f;
    public float waveSpeed = 3f;

    private Light targetLight;
    private Vector3 initialScale;

    void Start()
    {
        // 记录初始缩放
        initialScale = transform.localScale;

        // 查找子物体中的灯光组件
        Transform lightTransform = FindChildByName(transform, childLightName);
        if (lightTransform != null)
        {
            targetLight = lightTransform.GetComponent<Light>();
            if (targetLight == null)
            {
                Debug.LogWarning($"未在子物体 {childLightName} 上找到Light组件");
            }
        }
        else
        {
            Debug.LogWarning($"未找到名为 {childLightName} 的子物体");
        }
    }

    void Update()
    {
        // 1. 父物体自身放大缩小
        float scaleValue = Mathf.Lerp(scaleMin, scaleMax, Mathf.PingPong(Time.time * scaleSpeed, 1f));
        transform.localScale = initialScale * scaleValue;

        // 2. 子灯光强度波动
        if (targetLight != null)
        {
            float t = Mathf.PingPong(Time.time * waveSpeed, 1f);
            targetLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, t);
        }
    }

    // 递归查找子物体（按名称）
    Transform FindChildByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            Transform result = FindChildByName(child, name);
            if (result != null)
                return result;
        }
        return null;
    }
}