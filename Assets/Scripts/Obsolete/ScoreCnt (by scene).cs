//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Runtime.Serialization.Formatters.Binary;
//using UnityEngine;

//public class ScoreCnt : MonoBehaviour
//{
//    public string PathToSave;//= "/Saves/ScoreTables.bin"    
//    public string EmptyScoreVal;// = "неуд.";
//    private const float EmptyFieldFiller = -1.0f;
//    Dictionary<string, Dictionary<string, float>> scores;
//    private string SavePath;
//    private BinaryFormatter formatter;
//    private static string LastUpdName;
//    private static string LastUpdScene;
//    const int RowLimitTnScoreTable = 9;
//    public string GetSortedResultsByScene(string scene)
//    {
//        string res="";
//        Dictionary<string, float> tmpdict = new Dictionary<string, float>();
//        if (!scores.ContainsKey(scene))
//        {
//            return "";
//        }
//        float PlusMaxValue = scores[scene].Values.Max()+1.0f;
//        foreach (var item in scores[scene])
//        {
//            if (item.Value.Equals(EmptyFieldFiller))
//            {
//                tmpdict.Add(item.Key, PlusMaxValue);
//            }
//            else
//            {
//                tmpdict.Add(item.Key, item.Value);
//            }
//        }
//        var ScoreList = tmpdict.ToList();
//        ScoreList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
//        int RowLimiter = RowLimitTnScoreTable;
//        foreach (var item in ScoreList)
//        {
            
//            res+=item.Key+" : ";
//            res += item.Value.Equals(PlusMaxValue) ? EmptyScoreVal : item.Value.ToString("F2");
//            res +="\n";
//            RowLimiter -= 1;
//            if (RowLimiter <= 0)
//            {
//                break;
//            }
//        }
//        return res;
//    }
//    public void SetActiveKeys(string Uname, string scene)
//    {
//        LastUpdScene = scene;
//        LastUpdName = Uname;
//    }
//    public bool HaveUname(string uname, string SceneName)
//    {
//        UpdateDict();
//        if (!scores.ContainsKey(SceneName))
//        {
//            scores.Add(SceneName, new Dictionary<string, float>());
//        }
//        return scores[SceneName].ContainsKey(uname);
//    }
//    public void AddName(string uname, string SceneName)
//    {
//        scores[SceneName].Add(uname, EmptyFieldFiller);
//        SaveDictToFile();
//    }
//    public void UpdateValueIfGreatherViaInnerKeys(float score)
//    {        
//           if (scores[LastUpdScene][LastUpdName].Equals(EmptyFieldFiller) || (scores[LastUpdScene][LastUpdName].CompareTo(score) > 0))
//        {
//            scores[LastUpdScene][LastUpdName] = score;
//            SaveDictToFile();
//        }
//    }
//    private void SaveDictToFile()
//    {
//        Stream stream = new FileStream(SavePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);
//        formatter.Serialize(stream, scores);
//        stream.Close();
//    }
//    private void UpdateDict()
//    {
//        Directory.CreateDirectory(SavePath.Substring(0, SavePath.LastIndexOf('/')));
//        Stream stream = new FileStream(SavePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
//        if (stream.Length != 0)
//        {
//            scores = (Dictionary<string, Dictionary<string, float>>)formatter.Deserialize(stream);
//        }
//        else
//        {
//            scores = new Dictionary<string, Dictionary<string, float>>();
//        }
//        stream.Close();
//    }

//    // Start is called before the first frame update
//    void Awake()
//    {
//        formatter = new BinaryFormatter();
//        SavePath = Application.dataPath + PathToSave;
//        UpdateDict();
//    }
//}
