using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilCell : Cell
{
    public SoilCell(int _x, int _y, int _z) : base(_x, _y, _z)
    {}

    public override float getWater(float removed_water){
        if (waterConcen - removed_water >= 0) 
        {
            waterConcen = waterConcen - removed_water;
            return removed_water;
        }
        return 0f;
    }

    public override float getNitrate(float removed_nitrate){
        if (nitrate - removed_nitrate >= 0)
        {
            nitrate = nitrate - removed_nitrate;
            return removed_nitrate;
        }
        return 0f;
    }


    public override void Generate()
    {
        
    }
}
