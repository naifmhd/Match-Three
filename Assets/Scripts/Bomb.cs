using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BombType
{
    None,
    Column,
    Row,
    Adjacent,
    Color
}

public class Bomb : GamePiece
{
    public BombType bombType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
