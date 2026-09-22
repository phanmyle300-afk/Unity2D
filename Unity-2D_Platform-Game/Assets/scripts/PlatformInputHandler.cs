using UnityEngine;

public class PlatformInputHandler : MonoBehaviour
{
    [Header("Khung chứa các nút bấm cảm ứng")]
    public GameObject mobileControlsPanel;

    [Header("Chế độ Test trên Máy tính")]
    [Tooltip("Tick chọn ô này nếu muốn buộc hiện nút bấm để test trên Máy tính")]
    public bool forceShowMobileControls = false;

    private void Awake()
    {
        ApplyPlatformSettings();
    }

    private void Start()
    {
        ApplyPlatformSettings();
    }

    private void ApplyPlatformSettings()
    {
        if (mobileControlsPanel == null) return;

        // 1. Nếu tick chọn ép buộc hiện để test
        if (forceShowMobileControls)
        {
            mobileControlsPanel.SetActive(true);
            return;
        }

        // 2. Nếu đang bấm Play trực tiếp trong Unity Editor (Máy tính) -> Luôn ẨN nút
        #if UNITY_EDITOR
            mobileControlsPanel.SetActive(false);
        // 3. Khi xuất file (Build) ra thiết bị Android / iOS thực tế -> HỆN nút
        #elif UNITY_ANDROID || UNITY_IOS
            mobileControlsPanel.SetActive(true);
        #else
            mobileControlsPanel.SetActive(false);
        #endif
    }
}