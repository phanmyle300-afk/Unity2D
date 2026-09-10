using UnityEngine;
using UnityEngine.EventSystems;

public class MobileUIController : MonoBehaviour
{
    private MainCharacterController playerController;

    private void Start()
    {
        // Tự động tìm nhân vật trong Scene hiện tại
        FindPlayer();
    }

    public void FindPlayer()
    {
        GameObject playerObj = GameObject.FindWithTag("Player"); 
        // Nếu nhân vật không dùng Tag "Player", code sẽ tìm theo Component
        if (playerObj == null)
        {
            playerController = FindObjectOfType<MainCharacterController>();
        }
        else
        {
            playerController = playerObj.GetComponent<MainCharacterController>();
        }
    }

    // Các hàm gọi trung gian an toàn (ngăn lỗi MissingReferenceException)
    public void BtnLeftDown()
    {
        if (playerController != null) playerController.PointerDownLeft();
    }

    public void BtnRightDown()
    {
        if (playerController != null) playerController.PointerDownRight();
    }

    public void BtnMoveUp()
    {
        if (playerController != null) playerController.PointerUpMove();
    }

    public void BtnJumpDown()
    {
        if (playerController != null) playerController.PointerDownJump();
    }

    public void BtnPunchDown()
    {
        if (playerController != null) playerController.PointerDownPunch();
    }
}