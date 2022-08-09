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
    public static HighScoreManager instance;

    public string[] textNames = new string[5];
    public float[] scores = new float[5];

    private void Awake()
    {
        Load();
        if (instance == null)
        {
            instance = this;
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

    [SerializeField]
    public GameObject[] gameObjectsTopScore;

    private void DisplayBestScores()
    {
        HighScoreManager.instance.Load();

        for (int i = 0; i < gameObjectsTopScore.Length; i++)
        {
            gameObjectsTopScore[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{ HighScoreManager.instance.scores[i]:F0}";
            gameObjectsTopScore[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + HighScoreManager.instance.textNames[i];
        }
    }





}
