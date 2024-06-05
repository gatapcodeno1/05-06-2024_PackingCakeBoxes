using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance => instance;

    [SerializeField] private int currentLevel;
    public int CurrentLevel => currentLevel;

    [SerializeField] private int maxLevel;
    public int MaxLevel => maxLevel;
    private void Awake()
    {
        // Kiểm tra nếu instance đã tồn tại
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            LevelManager.instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start(){
       maxLevel = PlayerPrefs.GetInt("maxLevel",1);
    }
    public void SetLevel(int level){
        this.currentLevel = level;
    }
    public void SetLevelMax(int level){
        if(maxLevel < level) maxLevel = level;
        PlayerPrefs.SetInt("maxLevel",maxLevel);
    }


}
