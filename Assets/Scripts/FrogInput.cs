using UnityEngine;
using UnityEngine.InputSystem;

public class FrogInput : MonoBehaviour
{
    [HideInInspector] public bool downInput;
    [HideInInspector] public bool upInput;
    [HideInInspector] public bool dragInput;

    void Update()
    {
        // 1. Kiểm tra trạng thái nhấn (Chuột trái hoặc Chạm màn hình)
        // Pointer.current tự động nhận diện cả Chuột và Cảm ứng
        if (Pointer.current == null) return;

        bool isPressed = Pointer.current.press.isPressed;

        // 2. Xử lý Down (Vừa chạm)
        if (Pointer.current.press.wasPressedThisFrame)
        {
            downInput = true;
            dragInput = true; // Bắt đầu kéo
            upInput = false;
        }

        // 3. Xử lý Up (Thả tay)
        if (Pointer.current.press.wasReleasedThisFrame)
        {
            downInput = false;
            dragInput = false;
            upInput = true;
        }
        
        // 4. Giữ nguyên dragInput nếu vẫn đang đè tay
        dragInput = isPressed;
    }

    // Hàm bổ trợ để Reset upInput sau khi đã xử lý Jump
    public void UseUpInput()
    {
        upInput = false;
    }
}