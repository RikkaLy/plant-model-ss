using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootCell : Cell
{
    //how much water and nitrate the root cell is holding, it will pass these up to the plant
    private float waterConcen;
    private float nitrate;

    public RootCell(int _x, int _y, int _z) : base(_x, _y, _z)
    {}

    public void Setup(int _x, int _y, int _z)
    {
        x = _x;
        y = _y;
        z = _z;

        cellRep = GameObject.Find("root" + "x" + x + "y" + y + "z" + z);
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

    public void Osmosis()
    {
        //get water from the soil and from fungi
        //give fungi sugars in exchange
    }

    public void Nitrification()
    {
        //get nitrate from the soil, fungi, and bacteria
        //give sugars and carbon in exchange
    }

    public override void Generate(int neighbours)
    {

    }
}
