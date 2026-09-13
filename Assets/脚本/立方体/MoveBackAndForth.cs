using UnityEngine;

public class MoveBackAndForth : MonoBehaviour
{
    [Header("移动参数")]
    public Transform targetPoint;      // 目标点
    public float moveSpeed = 2f;       // 移动到目标点的速度
    public float stayTime = 1f;        // 在目标点停留时间

    private Vector3 startPosition;
    private bool isMoving = true;
    private float timer = 0f;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (targetPoint == null) return;

        if (isMoving)
        {
            // 向目标点移动
            transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, moveSpeed * Time.deltaTime);

            // 到达目标点
            if (Vector3.Distance(transform.position, targetPoint.position) < 0.01f)
            {
                isMoving = false;
                timer = 0f;
            }
        }
        else
        {
            // 在目标点停留计时
            timer += Time.deltaTime;
            if (timer >= stayTime)
            {
                // 瞬移回原点
                transform.position = startPosition;
                isMoving = true;
            }
        }
    }
}