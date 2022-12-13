using UnityEngine;
using System.Collections;
 using System.Collections.Generic;

public class CubeGOL : MonoBehaviour 
{
    private GOL gol;
    public GameObject tilePrefab;

    void Start(){
        gol = new GOL(tilePrefab);
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

public class CubeCell
{
    private GameObject cellRep;
    private GameObject clone;
    private Material cellMaterial;

    private float x, y, z;
    public int State, Previous;

    public CubeCell(GameObject tilePrefab, int _x, int _y, int _z){

        State = Random.Range(0, 2);
        Previous = State;
        cellRep = tilePrefab;
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

    private void CreateGameObject()
    {
        clone = GameObject.Instantiate(cellRep);

        // Scale is halved so the entire board is displayed in screen
        //cellRep.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        clone.transform.position = new Vector3(x, y, z);
    }

}

public class GOL
{
    private int columns, rows, z_rows;
    private CubeCell[,,] board;
    private GameObject cellRep;
   
    public GOL (GameObject tilePrefab) 
    {
        columns = 10;
        rows = 10;
        z_rows  = 10;
        board = new CubeCell[z_rows, columns, rows];
        cellRep = tilePrefab;
        Innit();
    }

    private void Innit()
    {
        for (int z = 0; z < z_rows; z++)
        {
            for (int a = 0; a < columns; a++) 
            {
                for (int b = 0; b < rows; b++) 
                {
                    CubeCell newCell = new CubeCell(cellRep, a, b, z);
                    board[z, a, b] = newCell;
                }
            }
        }
    }

    public void Display(){
        for (int h = 0; h < z_rows; h++)
        {
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    board[h,i,j].Display();
                    // Debug.Log(h + " " + i + " " + j);
                }
            }
        }

    }

    public void Generate()
    {
        for (int h = 0; h < z_rows; h++)
        {
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    board[h, i, j].SavePrevious();
                }
            }
        }

        for (int z = 0; z < z_rows; z++)
        {
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    // Add up all the states in a 3x3 surrounding grid
                    int neighbors = 0;
                    for(int h = -1; h <= 1; h++)
                    {
                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                neighbors += board[(z + h + z_rows) % z_rows, (x + i + columns) % columns, (y + j + rows) % rows].Previous;
                            }
                        }
                    }

                    // A little trick to subtract the current cell's state since
                    // we added it in the above loop
                    neighbors -= board[z,x,y].Previous;
                    // Debug.Log(neighbors);

                    // Rules of Life
                    if ((board[z,x,y].State == 1) && (neighbors < 10)) board[z,x,y].NewState(0);           // Loneliness
                    else if ((board[z,x,y].State == 1) && (neighbors > 12)) board[z,x,y].NewState(1);           // Overpopulation
                    else if ((board[z,x,y].State == 0) && (neighbors == 12)) board[z,x,y].NewState(1);           // Reproduction
                    // else Debug.Log(":(");    
                }
            }
        }

    }
}
