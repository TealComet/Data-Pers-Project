using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartManager : MonoBehaviour
{
    public static StartManager Instance;
    
    // Inputed TMPro
    public TextMeshProUGUI startPlayerNameText;

    // Best Data TMPro
    private TextMeshProUGUI bestDataText;

    // Data to save
    public string bestPlayerName;
    public int bestScoreValue;

    // Persistence between scenes
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }

        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        LoadBestPlayerAndBestScore();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckName();
    }

    // Method to check if a name exists
    private void CheckName()
    {
        try
        {
            startPlayerNameText = GameObject.Find("PlayerName").GetComponent<TextMeshProUGUI>();
            Debug.Log($"Start Player Name = {startPlayerNameText.text}.");
        }

        catch(NullReferenceException exc)
        {
            Debug.Log($"No PlayerName : {exc}");
        }
    }

    // Class of data to save
    [Serializable]
    public class DataToSave
    {
        public string bestPlayerData;
        public int bestScoreData;
    }

    // Data persistence between sessions - Saving the best player and best score on game over
    public void SaveBestPlayerAndBestScore()
    {
        DataToSave data = new DataToSave();

        data.bestPlayerData = bestPlayerName;
        Debug.Log($"Saved Player = {data.bestPlayerData}.");

        data.bestScoreData = bestScoreValue;
        Debug.Log($"Saved Score = {data.bestScoreData}.");

        string json = JsonUtility.ToJson(data);
    
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    // Data persistence between sessions - Loading the color on game start
    public void LoadBestPlayerAndBestScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            DataToSave data = JsonUtility.FromJson<DataToSave>(json);

            bestPlayerName = data.bestPlayerData;
            Debug.Log($"Loaded Player = {data.bestPlayerData}.");

            bestScoreValue = data.bestScoreData;
            Debug.Log($"Loaded Score = {data.bestScoreData}.");

            bestDataText = GameObject.Find("BestNameAndScore").GetComponent<TextMeshProUGUI>();
            bestDataText.text = $"Best Score - {data.bestPlayerData} : {data.bestScoreData}";
        }

        else
        {
            Debug.Log("No JSON file found.");
        }
    }
}
