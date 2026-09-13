using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineFlowController : MonoBehaviour
{
    [Header("线段设置")]
    public Transform pointA;           // 起点
    public Transform pointB;           // 终点
    public GameObject blockPrefab;     // 方块预制体

    [Header("流动参数")]
    public float flowSpeed = 2f;       // 流动速度
    public float blockSize = 0.5f;     // 方块大小
    public int blockCount = 10;        // 方块数量

    private List<GameObject> blocks = new List<GameObject>();
    private float totalLength;

    void Start()
    {
        if (pointA == null || pointB == null || blockPrefab == null)
        {
            Debug.LogError("请设置所有引用！");
            return;
        }

        CreateBlocks();
    }

    void Update()
    {
        if (blocks.Count == 0) return;

        FlowBlocks();
    }

    void CreateBlocks()
    {
        // 计算两点之间的距离
        totalLength = Vector3.Distance(pointA.position, pointB.position);

        // 根据方块数量和间距创建方块
        float spacing = totalLength / blockCount;

        for (int i = 0; i < blockCount; i++)
        {
            // 计算每个方块的初始位置
            float t = (float)i / blockCount;
            Vector3 position = Vector3.Lerp(pointA.position, pointB.position, t);

            // 实例化方块
            GameObject block = Instantiate(blockPrefab, position, Quaternion.identity);
            block.transform.localScale = Vector3.one * blockSize;

            // 让方块朝向线段方向
            Vector3 direction = (pointB.position - pointA.position).normalized;
            block.transform.rotation = Quaternion.LookRotation(direction);

            blocks.Add(block);
        }
    }

    void FlowBlocks()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            // 计算当前位置的进度 (0~1)
            float progress = (i + Time.time * flowSpeed) % blockCount / blockCount;

            // 更新位置
            blocks[i].transform.position = Vector3.Lerp(
                pointA.position,
                pointB.position,
                progress
            );
        }
    }

    // 可视化辅助线（编辑器内显示）
    void OnDrawGizmosSelected()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}