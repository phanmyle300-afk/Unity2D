using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finishPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            // Thêm an toàn gọi lưu lịch sử khi qua màn (không làm đơ game)
            if (GameHistoryManager.Instance != null)
            {
                GameHistoryManager.Instance.UpdateReachedLevel("Hoàn thành Màn");
                GameHistoryManager.Instance.EndRunAndSaveHistory();
            }

            // Load the next level
            sceneController.instance.NextLevel();
        }
    }
}