using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerment : MonoBehaviour
{
    public void LoadSceneByName(string sceneName)
    {
        
        SceneManager.LoadScene(sceneName);
    }

    public void LoadPlayScene(){
        LoadSceneByName("PlayScene");
    }
    public void LoadHomeScene(){
        LoadSceneByName("HomeScene");
    }
    public void LoadHowToPlayScene(){
        LoadSceneByName("HowToPlayeScene");
    }

    public void LoadSelectLevelScene(){
        LoadSceneByName("SelectLevelScene");
    }


    public void NextLevelScene(){
        int currentLevel = LevelManager.Instance.CurrentLevel;
        LevelManager.Instance.SetLevel(currentLevel+1);
        LoadSceneByName("PlayScene");
    }
}
