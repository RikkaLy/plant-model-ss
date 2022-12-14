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
        State = 0;
        Previous = State;

        x = _x;
        y = _y;
        z = _z;

        cellRep = GameObject.Find("org" + x + y + z);
        CreateGameObject();
    }

    private void nutrientFixation(){}

    public void Generate(int neighbours){}
}

