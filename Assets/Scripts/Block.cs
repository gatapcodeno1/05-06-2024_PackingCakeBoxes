using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // block is gift , candy , cake
    
    public int _index;
    public SpriteRenderer _sprite;
    public string _name;
    public bool _canMove = false;
    
    
   
    
    public void SwapBlock(Block block){
        
        Sprite sprite = block._sprite.sprite;
        block._sprite.sprite = _sprite.sprite;
        this._sprite.sprite = sprite;
        bool newMove = block._canMove;
        block._canMove = _canMove;
        this._canMove = newMove;
        string newName = block._name;
        block._name = _name;
        this._name = newName;
    }
    

    
}
