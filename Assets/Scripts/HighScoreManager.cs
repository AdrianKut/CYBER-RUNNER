using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using System.Linq;

public class HighScoreManager : MonoBehaviour
{
    private HighScoreManager() { }
    public static HighScoreManager Instance;

    [SerializeField] private string[] textNames = new string[5];
    [SerializeField] private float[] scores = new float[5];

    public string[] GetTextNamesArray()
    {
        return textNames;
    }

    public float[] GetScoresArray()
    {
        return scores;
    }

    public int GetScoreLength()
    {
        return scores.Length;
    }

    public int GetTextNamesLength()
    {
        return textNames.Length;
    }

    public void SetScore(float value, int index)
    {
        scores[index] = value;
    }

    public void SetTextName(string value, int index)
    {
        textNames[index] = value;
    }

    public float GetSelectedScoreValue(int index)
    {
        return scores[index];
    }

    private void Awake()
    {
        Load();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [System.Serializable]
    class SaveData
    {
        public string[] textNames;
        public float[] scores;
    }

    public void Save()
    {
        SaveData data = new SaveData();
        data.textNames = textNames;
        data.scores = scores;
        string json = JsonUtility.ToJson(data);
        BinaryFormatter bFormatter = new BinaryFormatter();
        using (Stream output = File.Create(Application.persistentDataPath + "/savefile.dat"))
        {
            bFormatter.Serialize(output, json);
        }
    }

    public void Load()
    {
        BinaryFormatter bFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savefile.dat";
        if (File.Exists(path))
        {
            using (Stream input = File.OpenRead(path))
            {
                string json = (string)bFormatter.Deserialize(input);
                SaveData data = JsonUtility.FromJson<SaveData>(json);
                textNames = data.textNames;
                scores = data.scores;
            }
        }
    }

    private void OnEnable()
    {
        DisplayBestScores();
    }

    [SerializeField] private GameObject[] gameObjectsTopScore;
    private void DisplayBestScores()
    {
        Instance.Load();
        for (int i = 0; i < gameObjectsTopScore.Length; i++)
        {
            gameObjectsTopScore[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{ HighScoreManager.Instance.scores[i]:F0}";
            gameObjectsTopScore[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + HighScoreManager.Instance.textNames[i];
        }
    }
}
