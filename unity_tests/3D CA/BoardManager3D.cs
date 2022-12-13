using UnityEngine;
using System.Collections;
 using System.Collections.Generic;

public class BoardManager3D : MonoBehaviour 
{
    private BoardManagerGOL gol;

    void Start(){
        gol = new BoardManagerGOL();
        LimitFrameRate();
    }

    void Update(){
        gol.Generate();
        gol.Display();
    }

    private void LimitFrameRate(){
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 5;
    }
}

public class Cell3D
{
    private GameObject cellRep;
    private Material cellMaterial;

    private float x, y;
    public int State, Previous;

    public Cell3D(GameObject tilePrefab, int _x, int _y){

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

public class BoardManagerGOL
{
    public GameObject tilePrefab;
   
    private int columns, rows;
    private Cell3D[,] board;
   
    public BoardManagerGOL () 
    {
        columns = 10;
        rows = 10;
        board = new Cell3D[columns, rows];
        Innit();
    }

    private void Innit()
    {
        for (int a = 0; a < columns; a++) 
        {
            for (int b = 0; b < rows; b++) 
            {
                Cell3D newCell = new Cell3D(tilePrefab, a, b);
                board[a, b] = newCell;
            }
        }
    }

    public void Display(){
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                board[i,j].Display();
            }
        }

    }

    public void Generate()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                board[i, j].SavePrevious();
            }
        }

        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // Add up all the states in a 3x3 surrounding grid
                int neighbors = 0;
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        neighbors += board[(x + i + columns) % columns, (y + j + rows) % rows].Previous;
                    }
                }

                // A little trick to subtract the current cell's state since
                // we added it in the above loop
                neighbors -= board[x,y].Previous;
                // Debug.Log(neighbors);

                // Rules of Life
                if ((board[x,y].State == 1) && (neighbors < 2)) board[x,y].NewState(0);           // Loneliness
                else if ((board[x,y].State == 1) && (neighbors > 3)) board[x,y].NewState(0);           // Overpopulation
                else if ((board[x,y].State == 0) && (neighbors == 3)) board[x,y].NewState(1);           // Reproduction
                // else Debug.Log(":(");    
            }
        }

    }
}
