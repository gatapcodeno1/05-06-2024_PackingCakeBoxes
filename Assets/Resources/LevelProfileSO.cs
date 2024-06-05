using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "LevelProfileSO",menuName = "SO/LevelProfileSO")]
public class LevelProfileSO : ScriptableObject
{
    public int level;
    public int width;
    public int height;
    public List<BlockType> blocksList;
}
