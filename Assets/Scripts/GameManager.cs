using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int _level;
    [SerializeField] private int _width = 4;
    [SerializeField] private int _height = 4;
    [SerializeField] private Node _nodePrefab;
    [SerializeField] private Block _blockPrefab;
    [SerializeField] private List<BlockType> _types;
    [SerializeField] private List<Node> _nodes;
    [SerializeField] private List<Block> _block;
    [SerializeField] private float _multiValue;
    [SerializeField] private float _initBlockScale;
    [SerializeField] private float _initCandyScale;
    [SerializeField] private float _timePlay;
    [SerializeField] private TextMeshProUGUI _txtTime;
    [SerializeField] private LevelProfileSO levelProfile;
    
    [SerializeField] Transform completeLevel;
    [SerializeField] Transform failLevel;
     [SerializeField] private GameState _state;
    void Start(){
        this.LoadLevelSO();
        
    }

    // load level by levelSO
    protected void LoadLevelSO()
    {
        _level = LevelManager.Instance.CurrentLevel;
        string resPath = "Levels/Level" + _level;
        this.levelProfile = Resources.Load<LevelProfileSO>(resPath);    
        SetUpLevel(levelProfile);
        ChangeGameState(GameState.GenerateLevel);
    }
    void SetUpLevel(LevelProfileSO lv){
        this._height = lv.height;
        this._width = lv.width;
        this._types = lv.blocksList;
    }
    void Update(){
        if(_state == GameState.Win || _state == GameState.Lose) return;
        SetTime();
        GetDirectionInput();

    }
    void SetTime(){
        
        if(_timePlay < 0){
            ChangeGameState(GameState.Lose);
            return;
        }
        _timePlay -= Time.deltaTime;
        int timeRounded = Mathf.RoundToInt(_timePlay);
        _txtTime.text = (timeRounded/60).ToString("D2") + " : " + (timeRounded%60).ToString("D2");
    }
    void GetDirectionInput(){
        if(_state != GameState.WatingInput) return;
        if(Input.GetKeyDown(KeyCode.LeftArrow)) Shift(Vector2.left);
        if(Input.GetKeyDown(KeyCode.RightArrow)) Shift(Vector2.right);
        if(Input.GetKeyDown(KeyCode.UpArrow)) Shift(Vector2.up);
        if(Input.GetKeyDown(KeyCode.DownArrow)) Shift(Vector2.down);
    }
    bool CheckMove(int i){
        return i>=0 && i < _height * _width;
    }
    bool CheckCanMoveTo(int i){
        return _block[i]._sprite.sprite == null;
    }
    bool NotMoveNextColumn(Vector2 dir , int i){
        if(dir == Vector2.up){
            if((i + dir.y) % _height == 0 ){
            return false;
            }
        }
        if(dir == Vector2.down){
            if(i % _height == 0){
                return false;
            }
        }
        
        return true;
    }
    void Shift(Vector2 dir){
        ChangeGameState(GameState.Moving);
       
        _block = _block.OrderBy(b => b._index).ToList();
        int time = dir.y != 0 ? _height : _width;
        do{
        if(dir == Vector2.right){
            for(int i = _block.Count - 1  ; i >= 0 ; i--){
                if(!CheckMove(i+_height) || _block[i]._sprite.sprite == null) continue;
                if(_block[i]._canMove && _block[i+_height]._canMove && CheckCanMoveTo(i+_height) ) _block[i].SwapBlock(_block[i+_height]);   
            }
        }
        if(dir == Vector2.left){
            for(int i = 0  ; i < _block.Count ; i++){
                if(!CheckMove(i-_height) || _block[i]._sprite.sprite == null) continue;
                if(_block[i]._canMove && _block[i-_height]._canMove && CheckCanMoveTo(i-_height)) _block[i].SwapBlock(_block[i-_height]);
                
            }
        }
        if(dir == Vector2.up){
            for(int i = _block.Count - 1 ; i >= 0; i--){
               if(!CheckMove(i+1)|| _block[i]._sprite.sprite == null) continue;
               if(_block[i]._canMove && _block[i+1]._canMove && CheckCanMoveTo(i+1) && NotMoveNextColumn(dir,i)) _block[i].SwapBlock(_block[i+1]);
                
            } 
        }
        if(dir == Vector2.down){
            for(int i = 0  ; i < _block.Count ; i++){
               if(!CheckMove(i-1) || _block[i]._sprite.sprite == null) continue;
               if(_block[i]._canMove && _block[i-1]._canMove && CheckCanMoveTo(i-1) && NotMoveNextColumn(dir,i)) _block[i].SwapBlock(_block[i-1]);
            } 
        }
        }while(time-- > 0);
        if(dir.y!= 0){
            if(CheckPlayerWin(dir)) {ChangeGameState(GameState.Win); Debug.Log("Winne"); return;}
        }
        ChangeGameState(GameState.WatingInput);
    }


    bool CheckPlayerWin(Vector2 dir){
        for(int i = 0 ; i < _block.Count ; i++){
                if(!CheckMove(i+1)) continue;
                if((i+1)%_height == 0) continue;
                if(_block[i]._name == "GiftBox" && _block[i+1]._name == "Cake"){
                    if(dir == Vector2.up){
                        _block[i+1]._sprite.sprite = null;
                        _block[i].SwapBlock(_block[i+1]);
                    }
                    else{
                        _block[i+1]._sprite.sprite = null;
                         
                    }
                    return true;
                }
            }
            return false;
    }

    private void ChangeGameState(GameState newState){
        _state = newState;
        switch(newState){
            case GameState.GenerateLevel:
            GenerateGrid();
            break;
            case GameState.SpawnBlocks:
            SpawnCandies();
            break;
            case GameState.Win:
            completeLevel.gameObject.SetActive(true);
            LevelManager.Instance.SetLevelMax(_level+1);
            break;
            case GameState.Lose:
            failLevel.gameObject.SetActive(true);
            break;
        }
    }

    void GenerateGrid(){
        _nodes = new List<Node>();
        _block = new List<Block>();
       
        _multiValue = (3 - _width) * (_initBlockScale/8f);
        _nodePrefab.transform.localScale = Vector3.one * (_multiValue + _initBlockScale);
        float _xScale = _nodePrefab.transform.localScale.x;
        float _yScale = _nodePrefab.transform.localScale.y;
        

        for(int x = -_width/2 ; x <= (_width - 1)/2 ; x++){
            for(int y = -_height/2 ; y <= (_height-1)/2 ; y++){
                var node = Instantiate(_nodePrefab,new Vector2(x*2*_xScale,y*2*_yScale) + (1-_width%2) * Vector2.one * _xScale , Quaternion.identity);
                _nodes.Add(node);
                var newBlock = Instantiate(_blockPrefab , node.transform.position , Quaternion.identity);
                newBlock.transform.localScale = Vector3.one * (_multiValue + _initCandyScale);
                newBlock._index = _nodes.Count()-1;
                _block.Add(newBlock);
            }
        }
        ChangeGameState(GameState.SpawnBlocks);
    }

    

    void SpawnCandies(){
        for(int i = 0 ; i < _types.Count ; i++){
           _block[_types[i].index]._sprite.sprite = _types[i].blockType._sprite.sprite;
           _block[_types[i].index]._canMove = _types[i].blockType._canMove;  
           _block[_types[i].index]._name = _types[i].name;  
        }
       
        ChangeGameState(GameState.WatingInput);
    }


    
    

}

[Serializable]
public struct BlockType{
    public string name;
    public int index;
    public Block blockType;
    
}

public enum GameState{
    GenerateLevel,
    SpawnBlocks,
    WatingInput,
    Moving,
    Win,
    Lose
}