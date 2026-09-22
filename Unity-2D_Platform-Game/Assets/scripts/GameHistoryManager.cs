using UnityEngine;
using System;
using System.Collections.Generic;
using Firebase.Auth;
using Firebase.Firestore;

[System.Serializable]
public class RunRecord
{
    public int runNumber;
    public string dateTime;
    public string maxLevel;
    public string totalTime;
}

[System.Serializable]
public class RunHistoryData
{
    public List<RunRecord> records = new List<RunRecord>();
}

public class GameHistoryManager : MonoBehaviour
{
    public static GameHistoryManager Instance;

    private const string HISTORY_KEY = "PlayerRunHistoryData";
    private FirebaseFirestore db;

    [HideInInspector] public float currentRunTimer = 0f;
    [HideInInspector] public string currentLevelReached = "Màn 1";
    [HideInInspector] public bool isRunActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            db = FirebaseFirestore.DefaultInstance;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isRunActive)
        {
            currentRunTimer += Time.deltaTime;
        }
    }

    public void StartNewRun()
    {
        currentRunTimer = 0f;
        currentLevelReached = "Màn 1";
        isRunActive = true;
    }

    public void UpdateReachedLevel(string levelName)
    {
        currentLevelReached = levelName;
    }

    public void EndRunAndSaveHistory()
    {
        if (!isRunActive) return;

        isRunActive = false;
        RunHistoryData data = LoadHistory();

        int nextRunNumber = data.records.Count + 1;
        TimeSpan t = TimeSpan.FromSeconds(currentRunTimer);
        string formattedTime = string.Format("{0:D2} phút {1:D2} giây", t.Minutes, t.Seconds);
        string currentDateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

        RunRecord newRecord = new RunRecord
        {
            runNumber = nextRunNumber,
            dateTime = currentDateTime,
            maxLevel = currentLevelReached,
            totalTime = formattedTime
        };

        data.records.Insert(0, newRecord);

        if (data.records.Count > 30)
        {
            data.records.RemoveAt(data.records.Count - 1);
        }

        // 1. Lưu Offline vào máy
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(HISTORY_KEY, json);
        PlayerPrefs.Save();

        // 2. Lưu Online lên Firestore Database
        SaveToFirestore(json);
    }

    private void SaveToFirestore(string jsonHistory)
    {
        if (FirebaseAuth.DefaultInstance != null && FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            
            DocumentReference userDoc = db.Collection("users").Document(userId);
            Dictionary<string, object> historyData = new Dictionary<string, object>
            {
                { "historyJson", jsonHistory }
            };

            userDoc.SetAsync(historyData, SetOptions.MergeAll).ContinueWith(task => 
            {
                if (task.IsCompleted)
                {
                    Debug.Log("🔥 Đã lưu Lịch sử lên Firestore thành công!");
                }
                else
                {
                    Debug.LogError("❌ Lỗi lưu Firestore: " + task.Exception);
                }
            });
        }
    }

    public RunHistoryData LoadHistory()
    {
        if (PlayerPrefs.HasKey(HISTORY_KEY))
        {
            string json = PlayerPrefs.GetString(HISTORY_KEY);
            return JsonUtility.FromJson<RunHistoryData>(json);
        }
        return new RunHistoryData();
    }

    public void FetchHistoryFromFirebase(Action onComplete)
    {
        if (FirebaseAuth.DefaultInstance != null && FirebaseAuth.DefaultInstance.CurrentUser != null)
        {
            string userId = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
            DocumentReference userDoc = db.Collection("users").Document(userId);

            userDoc.GetSnapshotAsync().ContinueWith(task => 
            {
                if (task.IsCompleted && task.Result.Exists)
                {
                    DocumentSnapshot snapshot = task.Result;
                    if (snapshot.ContainsField("historyJson"))
                    {
                        string json = snapshot.GetValue<string>("historyJson");
                        PlayerPrefs.SetString(HISTORY_KEY, json);
                        PlayerPrefs.Save();
                        Debug.Log("🔥 Đã đồng bộ Lịch sử từ Firestore về máy!");
                    }
                }
                onComplete?.Invoke();
            });
        }
    }
}