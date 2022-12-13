using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilCell : Cell
{
    private float waterConcen;
    private float nitrate;
    private float density;

    public SoilCell(int _x, int _y, int _z) : base(_x, _y, _z)
    {
        
    }

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

    public void setDensity(float new_density){
        density += new_density;
    }

    public void Generate(int neighbours)
    {
        if ((State == 1) && (neighbours < 10)) NewState(0);    
        else if ((State == 1) && (neighbours > 12)) NewState(1);          
        else if ((State == 0) && (neighbours == 12)) NewState(1);         
    }
}
