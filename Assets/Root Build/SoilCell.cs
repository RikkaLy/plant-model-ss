using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilCell : Cell
{
    //how much water and nitrate is contained in the soil cell, this will be accessed by root cells and organisms
    private float waterConcen;
    private float nitrate;

    public SoilCell(int _x, int _y, int _z) : base(_x, _y, _z)
    {}

    public void Setup(int _x, int _y, int _z)
    {
        x = _x;
        y = _y;
        z = _z;

        cellRep = GameObject.Find("soil" + "x" + x + "y" + y + "z" + z);
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

    public override void Generate(int neighbours)
    {
        
    }
}
