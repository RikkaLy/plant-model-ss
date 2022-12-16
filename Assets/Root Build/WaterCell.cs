using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCell : Cell
{
    private bool isFalling;

    public WaterCell(int _x, int _y, int _z) : base(_x, _y, _z)
    {
        x = _x;
        y = _y;
        z = _z;
        CreateGameObject();

    }

    private void soilSaturation()
    {
        //add water to the surrounding soil
    }

}
