using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Organism : Cell
{
    //energy is supplied by the plant
    private float energy;

    public Organism(int _x, int _y, int _z) : base(_x, _y, _z)
    {}

    public void Setup(int _x, int _y, int _z)
    {
        x = _x;
        y = _y;
        z = _z;

        cellRep = GameObject.Find("org" + "x" + x + "y" + y + "z" + z);
        CreateGameObject();
    }

    private void nutrientFixation()
    {
        //this function is passed the name of a variable (water, nitrate)
        //it then collects that variable from the soil and passes it to the nearest root
    }

    public override void Generate(int neighbours)
    {

    }
}

