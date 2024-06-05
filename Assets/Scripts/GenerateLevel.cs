using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    public int maxLevelgenerate;
    public Transform prefabLevel;
    
    void Start(){
        Generate();
    }

    void Generate(){
        for(int i = 1 ; i <= maxLevelgenerate ; i++){
            var newPrefab = Instantiate(prefabLevel , Vector2.zero , Quaternion.identity , transform);
            newPrefab.GetComponent<SetLevelChoose>().openLevel.GetComponent<TextMeshProUGUI>().text = i.ToString();
            newPrefab.GetComponent<SetLevelChoose>().level = i;
        }
    }
}
