using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreCnt : MonoBehaviour
{
    public string PathToSave;//= "/Saves/ScoreTables.bin"    

        Dictionary<string, Dictionary<string, string>> scores;
        private string SavePath;
        private BinaryFormatter formatter;

        public bool HaveUname(string uname,string SceneName)
        {
        UpdateDict();
        if (!scores.ContainsKey(SceneName))
        {
            scores.Add(SceneName,new Dictionary<string, string>());
        }
        return scores[SceneName].ContainsKey(uname);
        }
        public void AddName(string uname,string SceneName)
        {
            scores[SceneName].Add(uname, "неуд.");
            SaveDictToFile();
        }
        private void SaveDictToFile()
        {
            Stream stream = new FileStream(SavePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, scores);
            stream.Close();
        }
        private void UpdateDict()
        {
            Stream stream = new FileStream(SavePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
            if(stream.Length != 0) { 
            scores = (Dictionary<string, Dictionary<string, string>>)formatter.Deserialize(stream);
            }
            else
            {
                scores = new Dictionary<string, Dictionary<string, string>>();
            }
            stream.Close();
        }
     
    // Start is called before the first frame update
    void Awake()
    {
        formatter = new BinaryFormatter();
        SavePath = Application.dataPath + PathToSave;
        UpdateDict();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
