using UnityEngine;
using System.Collections;
 using System.Collections.Generic;

public class BoardManager : MonoBehaviour {
 
    public GameObject tilePrefab;
    public int x,z;
   
    public List<Cell> board;
   
    public void Start () {
        board = new List<Cell>();

        for (int a = 0; a < x; a++) {
            for (int b = 0; b < z; b++) {
                Cell newCell = new Cell(tilePrefab, a, b);
                board.Add(newCell);
            }
        }
    }

    public void Update(){
        foreach (Cell cell in board){
            cell.Display();
        }
    }
}

public class Cell
{
    private GameObject cellRep;
    private Material cellMaterial;

    private float x, y;
    public int State, Previous;

    public Cell(GameObject tilePrefab, int _x, int _y){

        State = Random.Range(0, 2);
        Previous = State;
        cellRep = tilePrefab;
        x = _x;
        y = _y;
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
            cellRep.SetActive(false);
        }else{
           cellRep.SetActive(true); 
        }
    }

    private void CreateGameObject()
    {
        cellRep = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // Scale is halved so the entire board is displayed in screen
        //cellRep.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        cellRep.transform.position = new Vector3(x, y, 0);
    }

}