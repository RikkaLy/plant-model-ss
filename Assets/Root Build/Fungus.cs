using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fungus : Organism
{
    private float nitrate;

    public Fungus(int _x, int _y, int _z) : base(_x, _y, _z)
    {
        State = Random.Range(0, 2);
        Previous = State;

        x = _x;
        y = _y;
        z = _z;
        CreateGameObject();

    }

    private void checkConditions(){}
    private void Generate(){}
}