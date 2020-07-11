using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ScoreCnt : MonoBehaviour
{
    public string PathToSave;//= "/Saves/ScoreTables.bin"    
    public string EmptyScoreVal;// = "неуд.";
    Dictionary<string, Dictionary<string, string>> scores;
    private string SavePath;
    private BinaryFormatter formatter;
    private static string LastUpdName;
    private static string LastUpdScene;
    public string GetSortedResultsByScene(string scene)
    {
        string res="";
        const float EmptyHolder = -0.1f; // mix str and float in one dictionary is an bad idea(((((((((((((((
        Dictionary<string, float> tmpdict = new Dictionary<string, float>();
        if (!scores.ContainsKey(scene))
        {
            return "";
        }
        foreach (var item in scores[scene])
        {
            if (item.Value.Equals(EmptyScoreVal))
            {
                tmpdict.Add(item.Key, EmptyHolder);
            }
            else
            {
                tmpdict.Add(item.Key, float.Parse(item.Value));
            }
        }
        float PlusMaxValue = tmpdict.Values.Max()+1.0f;
        HashSet < string > EmptyKeys= new HashSet<string>();
        foreach (var item in tmpdict)
        {
            if (item.Value.Equals(EmptyHolder))
            {
                EmptyKeys.Add(item.Key);
            }
        }
        foreach (var item in EmptyKeys)
        {
            tmpdict[item] = PlusMaxValue;
        }
        var ScoreList = tmpdict.ToList();
        ScoreList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
        foreach (var item in ScoreList)
        {
            res+=item.Key+" : ";
            res += item.Value.Equals(PlusMaxValue) ? EmptyScoreVal : item.Value.ToString("F2");
            res +="\n";
        }
        return res;
    }
    public void SetActiveKeys(string Uname, string scene)
    {
        LastUpdScene = scene;
        LastUpdName = Uname;
    }
    public bool HaveUname(string uname, string SceneName)
    {
        UpdateDict();
        if (!scores.ContainsKey(SceneName))
        {
            scores.Add(SceneName, new Dictionary<string, string>());
        }
        return scores[SceneName].ContainsKey(uname);
    }
    public void AddName(string uname, string SceneName)
    {
        scores[SceneName].Add(uname, EmptyScoreVal);
        SaveDictToFile();
    }
    public void UpdateValueIfGreatherViaInnerKeys(float score)
    {        
          if (scores[LastUpdScene][LastUpdName].Equals(EmptyScoreVal) || float.Parse(scores[LastUpdScene][LastUpdName]).CompareTo(score) > 0)
        {
            scores[LastUpdScene][LastUpdName] = score.ToString();
            SaveDictToFile();
        }
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
        if (stream.Length != 0)
        {
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
}
