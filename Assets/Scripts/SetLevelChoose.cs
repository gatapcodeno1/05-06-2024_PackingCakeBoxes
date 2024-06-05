using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetLevelChoose : MonoBehaviour
{

    public Transform openLevel;
    public Transform winStar;
    public Transform loseStar;
    public Transform lockLevel;
    public int level;
    
    void Start(){
        if(this.level < LevelManager.Instance.MaxLevel){
            openLevel.gameObject.SetActive(true);
            lockLevel.gameObject.SetActive(false);
            winStar.gameObject.SetActive(true);
            loseStar.gameObject.SetActive(false);
        }
        else if(this.level == LevelManager.Instance.MaxLevel){
            openLevel.gameObject.SetActive(true);
            lockLevel.gameObject.SetActive(false);
            winStar.gameObject.SetActive(false);
            loseStar.gameObject.SetActive(true);
        }
        else{
            openLevel.gameObject.SetActive(false);
            lockLevel.gameObject.SetActive(true);
            winStar.gameObject.SetActive(false);
            loseStar.gameObject.SetActive(true);
        }
        
    }
    public void SetLevel(){
        if(this.level <= LevelManager.Instance.MaxLevel){
             LevelManager.Instance.SetLevel(level);
             SceneManager.LoadScene("PlayScene");
        }
        
    }


}
