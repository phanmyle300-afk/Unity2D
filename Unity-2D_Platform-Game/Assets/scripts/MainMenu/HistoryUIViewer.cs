using UnityEngine;
using TMPro;

public class HistoryUIViewer : MonoBehaviour
{
    [Header("Thành phần hiển thị chữ TMP")]
    public TextMeshProUGUI historyContentText;

    private void OnEnable()
    {
        // Mỗi khi bảng HistoryPanel bật lên (Active), tự động tải dữ liệu mới
        DisplayHistory();
    }

    public void DisplayHistory()
    {
        if (historyContentText == null) return;

        if (GameHistoryManager.Instance == null)
        {
            historyContentText.text = "Chưa có dữ liệu lịch sử!";
            return;
        }

        RunHistoryData historyData = GameHistoryManager.Instance.LoadHistory();

        if (historyData == null || historyData.records.Count == 0)
        {
            historyContentText.text = "Chưa có lịch sử lượt chơi nào!";
            return;
        }

        string fullText = "";
        foreach (RunRecord record in historyData.records)
        {
            fullText += $"<b>Lượt {record.runNumber}</b> | {record.dateTime}\n";
            fullText += $"  • Màn cao nhất: <color=#FFD700>{record.maxLevel}</color>\n";
            fullText += $"  • Thời gian chơi: {record.totalTime}\n";
            fullText += "----------------------------------------\n\n";
        }

        historyContentText.text = fullText;
    }
}