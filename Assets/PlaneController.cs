
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneController : MonoBehaviour
{
    // 左右移動範圍稍微縮小
    private const float horizontalLimit = 3.5f;

    // 上下移動範圍維持不變
    private const float verticalMargin = 0.8f;

    private Camera mainCamera;
    private BoxCollider boxCollider;

    void Start()
    {
        mainCamera = Camera.main;
        boxCollider = GetComponent<BoxCollider>();
    }

    void Update()
    {
        Vector2 pointer = Vector2.zero;
        bool canMove = false;

        // 手機：手指觸控控制
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.isPressed)
        {
            pointer = Touchscreen.current
                .primaryTouch.position.ReadValue();

            canMove = true;
        }

        // 電腦：滑鼠直接控制，不需要按左鍵
#if UNITY_EDITOR || UNITY_STANDALONE
        else if (Mouse.current != null)
        {
            pointer = Mouse.current.position.ReadValue();
            canMove = true;
        }
#endif

        if (!canMove || mainCamera == null)
            return;

        // 計算飛機與攝影機距離
        float distance = Vector3.Dot(
            transform.position - mainCamera.transform.position,
            mainCamera.transform.forward
        );

        if (distance <= 0f)
            return;

        // 將滑鼠或手指座標轉成世界座標
        Vector3 target = mainCamera.ScreenToWorldPoint(
            new Vector3(pointer.x, pointer.y, distance)
        );

        // 計算螢幕上下邊界
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(
            new Vector3(0, 0, distance)
        );

        Vector3 topRight = mainCamera.ViewportToWorldPoint(
            new Vector3(1, 1, distance)
        );

        // 計算飛機碰撞框高度
        float halfHeight = 0f;

        if (boxCollider != null)
        {
            halfHeight = boxCollider.bounds.extents.y;
        }

        // 左右移動範圍
        float minX = -horizontalLimit;
        float maxX = horizontalLimit;

        // 上下移動範圍維持不變
        float minY = bottomLeft.y + halfHeight + verticalMargin;
        float maxY = topRight.y - halfHeight - verticalMargin;

        if (minY > maxY)
        {
            float centerY = (bottomLeft.y + topRight.y) / 2f;
            minY = centerY;
            maxY = centerY;
        }

        // 限制飛機移動範圍
        target.x = Mathf.Clamp(target.x, minX, maxX);
        target.y = Mathf.Clamp(target.y, minY, maxY);
        target.z = 0f;

        // 飛機跟隨滑鼠或手指移動
        transform.position = target;
    }
}
