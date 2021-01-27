using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ScoreCnt : MonoBehaviour
{
    public string PathToSave;//= "/Saves/ScoreTables.bin"    
    public string EmptyScoreVal;// = "неуд.";
    private const float EmptyFieldFiller = -1.0f;
    Dictionary<string, Dictionary<string, Dictionary<string,float>>> scores;  // [scene][user][stageID] = score
    private List<string> Etaps_IDs =new List<string>();
    private string SavePath;
    private BinaryFormatter formatter;
    private const string SumKey = "sum";
    private static string LastUpdName;
    private static string LastUpdScene;
    //const int RowLimitTnScoreTable = 9;
    public List<string> GetSortedUsers(string scene)
    {
        Dictionary<string, float> tmpSumDict = new Dictionary<string, float>();
        foreach (var usr in scores[scene])
        {
            float sum = 0.0F;
            foreach (var stage in usr.Value)
            {
                if (!stage.Value.Equals(EmptyFieldFiller))
                {
                    sum += stage.Value;
                }
            }
            tmpSumDict.Add(usr.Key,sum);
        }
        var ScoreList = tmpSumDict.ToList();
        ScoreList.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
        List<string> out_sorted_usrs = new List<string>();
        foreach (var item in ScoreList)
        {
            out_sorted_usrs.Add(item.Key);
        }
        return out_sorted_usrs;
    }
    public Dictionary<string, Dictionary<string, string>> GetResultsByScene(string scene)
    {
        Dictionary <string, Dictionary<string, string>> tmpdict = new Dictionary<string, Dictionary<string, string>>();
        if (!scores.ContainsKey(scene))
        {
            return tmpdict;
        }
        foreach (var usr in scores[scene])
        {
            tmpdict.Add(usr.Key, new Dictionary<string, string>());
            float sum = 0.0F;                     
            foreach (var stage in usr.Value)
            {
                string stageid = stage.Key;
                float stageTime = stage.Value;
                if (stageTime.Equals(EmptyFieldFiller))
                {
                    tmpdict[usr.Key].Add(stageid, EmptyScoreVal);
                }
                else
                {
                    tmpdict[usr.Key].Add(stageid, stageTime.ToString("F2"));
                    sum += stageTime;
                }
            }
            tmpdict[usr.Key].Add(SumKey, sum.ToString("F2"));
        }
        return tmpdict;
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
            scores.Add(SceneName, new Dictionary<string, Dictionary<string, float>>());
        }
        return scores[SceneName].ContainsKey(uname);
    }
    public void AddName(string uname, string SceneName)
    {
        Dictionary<string, float> tmpd = new Dictionary<string, float>();
        foreach(string ID in Etaps_IDs)
        {
            tmpd.Add(ID,EmptyFieldFiller);
        }
        scores[SceneName].Add(uname, tmpd);
        SaveDictToFile();
    }
    public void UpdateValueIfGreatherViaInnerKeys(float score,string etap)
    {        
           if (scores[LastUpdScene][LastUpdName][etap].Equals(EmptyFieldFiller) || (scores[LastUpdScene][LastUpdName][etap].CompareTo(score) > 0))
        {
            scores[LastUpdScene][LastUpdName][etap] = score;
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
        Directory.CreateDirectory(SavePath.Substring(0, SavePath.LastIndexOf('/')));
        Stream stream = new FileStream(SavePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None);
        if (stream.Length != 0)
        {
            scores = (Dictionary<string, Dictionary<string, Dictionary<string, float>>>)formatter.Deserialize(stream);
        }
        else
        {
            scores = new Dictionary<string, Dictionary<string, Dictionary<string, float>>>();
        }
        stream.Close();
    }
    private void GetEtapsID()
    {
        GameObject ColCtl = GameObject.Find("Collectibles");
        Collectibles_ctl Collectibles_ctl_Targets = ColCtl.GetComponent<Collectibles_ctl>();
        foreach(string ID in Collectibles_ctl_Targets.EtapsID)
        {
            Etaps_IDs.Add(ID);
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        GetEtapsID();
        formatter = new BinaryFormatter();
        SavePath = Application.dataPath + PathToSave;
        UpdateDict();
    }
}
