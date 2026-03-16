using UnityEngine;

public class CalculateForce : MonoBehaviour
{
    private FrogInput input;
    private Rigidbody2D rb;

    [Header("Jump Settings")]
    public float forceMultiplier = 10f;
    public float maxForce = 20f;
    
    Vector2 touchPos;
    Vector2 endPos;
    bool isGrounded = true; // Ban đầu cho phép nhảy
    bool isAiming = false;  // Biến phụ để biết đang trong trạng thái kéo

    void Start()
    {
        input = GetComponent<FrogInput>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 1. Chỉ bắt đầu kéo nếu đang đứng trên lá
        if (isGrounded && input.downInput)
        {
            isAiming = true;
            // Dùng input.mousePosition từ Script FrogInput cho đồng bộ
            touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // Reset lại flag của Input để không lặp lại
            input.downInput = false; 
        }

        // 2. Trong khi đang kéo (Aiming)
        if (isAiming)
        {
            if (input.dragInput)
            {
                // TODO: Cập nhật LineRenderer vẽ đường dự đoán ở đây
                Vector2 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 tempDirection = touchPos - currentPos;
                // Debug.DrawLine(touchPos, currentPos, Color.red);
            }

            // 3. Thả tay để nhảy
            if (input.upInput)
            {
                isAiming = false; // Thoát trạng thái nhắm
                isGrounded = false; // Bắt đầu bay, không được nhảy tiếp

                endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 direction = touchPos - endPos;
                
                // Tính toán lực
                float distance = direction.magnitude;
                float finalForce = Mathf.Min(distance * forceMultiplier, maxForce);
                
                // Thực hiện nhảy
                rb.AddForce(direction.normalized * finalForce, ForceMode2D.Impulse);

                input.upInput = false; // Reset flag
            }
        }
    }

    // Xử lý khi chạm đất để reset isGrounded
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Untagged"))
        {
            isGrounded = true;
            rb.linearVelocity = Vector2.zero; // Dừng ếch lại khi chạm lá
        }
    }
}