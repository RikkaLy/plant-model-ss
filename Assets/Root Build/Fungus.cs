using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fungus : Organism
{

    public Fungus(int _x, int _y, int _z) : base(_x, _y, _z)
    {
        x = _x;
        y = _y;
        z = _z;

    }

    private void checkConditions()
    {
        //check to see whether the plant needs water or nitrate
        //if yes, perform nutrient fixation
    }

    public override void Generate()
    {
        //decide where to grow based on soil quality
    }
}