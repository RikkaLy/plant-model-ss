using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cell : MonoBehaviour
{
    //how much water and nitrate the root cell is holding, it will pass these up to the plant
    public float waterConcen;
    public float nitrate;

    public float x, y, z;

    public Cell(float _x, float _y, float _z){

        Setup(_x, _y, _z);

    }

    public void Setup(float _x, float _y, float _z)
    {
        x = _x;
        y = _y;
        z = _z;
    }


    public virtual float getWater(float removed_water = 0)
    {
        return waterConcen;
    }

    public virtual float getNitrate(float removed_nitrate = 0)
    {
        return nitrate;
    }

    public void addWater(float water)
    {
        waterConcen += water;
    }

    public void addNitrate(float _nitrate)
    {
        nitrate += _nitrate;
    }

    public virtual void Generate(){}


}