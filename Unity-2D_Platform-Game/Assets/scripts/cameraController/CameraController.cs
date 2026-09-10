using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform character;
    private Vector3 s = Vector3.zero;
    
    [Header("Cấu hình vị trí & Tốc độ")]
    public Vector3 offset = new Vector3(0, 1f, -10f); // Offset chuẩn cho 2D
    public float smoothTime = 0.15f; // Thời gian làm mượt (càng nhỏ bám càng nhanh)

    private void Start()
    {
        FindPlayer();
    }

    private void FindPlayer()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            character = playerObj.transform;
        }
    }

    private void LateUpdate()
    {
        // Tự tìm lại nhân vật nếu bị lỡ kết nối
        if (character == null)
        {
            FindPlayer();
            return;
        }

        Vector3 targetPosition = character.position + offset;
        
        // Dùng SmoothDamp mượt mà, dùng smoothTime trực tiếp (không nhân fixedDeltaTime)
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref s, smoothTime);
    }
}