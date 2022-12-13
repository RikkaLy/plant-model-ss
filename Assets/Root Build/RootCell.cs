using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootCell : Cell
{
    private float waterConcen;
    private float nitrate;

    public RootCell(int _x, int _y, int _z) : base(_x, _y, _z)
    {}

    public void Setup(int _x, int _y, int _z)
    {
        State = Random.Range(0, 2);
        Previous = State;

        x = _x;
        y = _y;
        z = _z;
        CreateGameObject();
    }

    public float getWater(){
        return waterConcen;
    }

    public float getNitrate(){
        return nitrate;
    }

    public void addWater(float water){
        waterConcen += water;
    }

    public void addNitrate(float _nitrate){
        nitrate += _nitrate;
    }

    public void checkConditions(){
        
    }

    public void Generate(int neighbours){}
}
