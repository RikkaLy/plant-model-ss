using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cell : MonoBehaviour
{
    //the gameobject that is created to represent the cell
    public GameObject cellRep;

    public float x, y, z;
    public int State, Previous;

    public Cell(int _x, int _y, int _z){

        //by default all cells are disabled
        cellRep = new GameObject();

        x = _x;
        y = _y;
        z = _z;
        CreateGameObject();

    }

    public void SavePrevious()
    {
        Previous = State;
    }

    public void newState(int newState)
    {
        Previous = State;
        State = newState;
    }

    public virtual void Generate(int neighbours){}

    public void CreateGameObject()
    {
        cellRep.transform.position = new Vector3(x, y, z);
    }

}