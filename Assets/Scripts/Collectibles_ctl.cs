﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Collectibles_ctl : MonoBehaviour
{
    public static Collectibles_ctl instance { get; private set; }
    public string NDZ_tag;
    public string Orig_tag;
    public string ID_tag;
    public string Ori_gold_tag;
    public string Ori_slv_tag;
    public string Garl_tag;
    public Text NDZ;
    public Text Orig;
    public Text Garl;
    public Image Ori_gold;
    public Image Ori_silv;
    //public Image garl;
    public Image ID;
    public string[] CollectOrder;
    public string[] CollectTagsToTrack;
    public int[] CollectNumber;
    public List<string> EtapsID;
    private string LastEtap;
    public string GetLastEtap { get { return LastEtap; } }
    int max_NDZ;
    int max_orig;
    int max_garl;
    private void Awake()
    {
        if(CollectNumber.Length != EtapsID.Count)
        {
            Debug.LogError("CollectNumber and EtapsID not equal");
            UnityEditor.EditorApplication.isPlaying = false;
        }
        LastEtap = EtapsID[EtapsID.Count - 1];
        max_NDZ = GameObject.FindGameObjectsWithTag(NDZ_tag).Length;
        max_orig = GameObject.FindGameObjectsWithTag(Orig_tag).Length;
        max_garl = GameObject.FindGameObjectsWithTag(Garl_tag).Length;
        instance = this;
        UpdateCol();
    }
    private void FixedUpdate()
    {
        UpdateCol();
    }
    public void UpdateCol()
    {
        NDZ.text = string.Format("{1}/{0}", max_NDZ, max_NDZ-GameObject.FindGameObjectsWithTag(NDZ_tag).Length);
        Orig.text = string.Format("{1}/{0}", max_orig, max_orig- GameObject.FindGameObjectsWithTag(Orig_tag).Length);
        Garl.text = string.Format("{1}/{0}", max_garl, max_garl - GameObject.FindGameObjectsWithTag(Garl_tag).Length);
        Ori_gold.enabled = (GameObject.FindGameObjectsWithTag(Ori_gold_tag).Length ==0);
        Ori_silv.enabled = (GameObject.FindGameObjectsWithTag(Ori_slv_tag).Length == 0);
        //garl.enabled = (GameObject.FindGameObjectsWithTag(Garl_tag).Length == 0);
        ID.enabled = (GameObject.FindGameObjectsWithTag(ID_tag).Length == 0);

    }
    public bool IsEtapComplete(string etap)
    {
        int EtapID = EtapsID.IndexOf(etap);
        int MustCollected = 0;
        for(int i =0; i<=EtapID; i++)
        {
            MustCollected += CollectNumber[i];
        }
        foreach(var i in CollectOrder)
        {
            if(GameObject.Find(i) != null)
            {
                break;
            }
            MustCollected--;
        }
        return MustCollected <= 0;
    }
}

