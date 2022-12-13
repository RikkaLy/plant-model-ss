using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cell : MonoBehaviour
{
    public GameObject cellRep;
    public GameObject clone;

    public float x, y, z;
    public int State, Previous;

    public Cell(int _x, int _y, int _z){

        State = Random.Range(0, 2);
        Previous = State;

        x = _x;
        y = _y;
        z = _z;
        CreateGameObject();

    }

    public void SavePrevious()
    {
        Previous = State;
    }

    public void NewState(int newState)
    {
        State = newState;
    }

    public void Display(){
        if(State == 0){
            clone.SetActive(false);
        }else{
           clone.SetActive(true); 
        }
    }

    public void CreateGameObject()
    {
        clone = GameObject.Instantiate(cellRep);

        // Scale is halved so the entire board is displayed in screen
        //cellRep.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        clone.transform.position = new Vector3(x, y, z);
    }

}