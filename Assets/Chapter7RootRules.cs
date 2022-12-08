using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Chapter7RootRules : MonoBehaviour
{
    // Each cell is now an object!
    private Chapter7RootRulesGOL gol;

    // Start is called before the first frame update
    void Start()
    {
        gol = new Chapter7RootRulesGOL();
        SetOrthographicCamera();
        LimitFrameRate();
    }

    // Update is called once per frame
    void Update()
    {
        // Reset board when mouse is pressed
        if (Input.GetMouseButtonDown(0))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        gol.Generate();
        gol.Display();
    }

    private void SetOrthographicCamera()
    {
        Camera.main.orthographic = true;
        Camera.main.orthographicSize = 10;
    }

    private void LimitFrameRate()
    {        
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 5;
    }
}

public class Chapter7RootRulesCell
{
    // GameObject properties, to visually represent and draw our cell
    private GameObject cellRep;
    private Material cellMaterial;

    // Size of the screen in meters, or Unity units
    private Vector2 screenSize;
    private float yScreenOffset; // Cells are spawned with a small offset so they spawn in more centered    
    private float xScreenOffset;

    // Location
    private float x, y;    

    public int State;
    public int Previous;
    public int Direction;

    public Chapter7RootRulesCell(int _x, int _y)
    {
        x = _x;
        y = _y;

        // How big our screen is in World Units
        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        xScreenOffset = 9f;
        yScreenOffset = 5f;

        State = 0;
        Previous = State;

        Direction = 0;

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

    public int GetDirection(){
        return Direction;
    }

    public void Display()
    {
        if (Previous == 0 && State == 1) // On reproduction
        {
            cellMaterial.color = Color.blue;
        }
        else if (State == 1) // On continuing to stay alive
        {
            cellMaterial.color = Color.black;
        }
        else if (Previous == 1 && State == 0) // On death
        {
            cellMaterial.color = Color.red;
        }
        else // On continuing to stay dead
        {
            cellMaterial.color = Color.white;
        }

        cellRep.transform.position = new Vector3((x * cellRep.transform.localScale.x) - screenSize.x - xScreenOffset, 
                                                 (y * cellRep.transform.localScale.x) - screenSize.y - yScreenOffset);
    }

    private void CreateGameObject()
    {
        cellRep = GameObject.CreatePrimitive(PrimitiveType.Quad);

        // Scale is halved so the entire board is displayed in screen
        cellRep.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Renderer r = cellRep.GetComponent<Renderer>();
        r.material = new Material(Shader.Find("Diffuse"));
        cellMaterial = r.material;
        Object.Destroy(cellRep.GetComponent<Collider>());
    }
}

public class Chapter7RootRulesGOL
{
    // Initialize rows, columns and set-up array
    private int columns, rows;    

    private Chapter7RootRulesCell[,] board;

    public Chapter7RootRulesGOL()
    {        
        columns = 72;
        rows = 41;
        board = new Chapter7RootRulesCell[columns, rows];
        Innit();
    }

    private void Innit()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                board[i, j] = new Chapter7RootRulesCell(i, j);
            }
        }
        board[35, 40].NewState(1);

    }

    // The process of creating the new generation
    public void Generate()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                board[i, j].SavePrevious();
            }
        }

        // Loop through every spot in our 2D array and check spots neighbors
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // Add up all the states in a 3x3 surrounding grid
                int neighbors = 0;
                int growth_point_x = 0;
                int growth_point_y = 0;
                int direction = 0;

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        neighbors += board[(x + i + columns) % columns, (y + j + rows) % rows].Previous;

                        if (board[(x + i + columns) % columns, (y + j + rows) % rows].Previous == 1){
                            growth_point_x = (x + i + columns) % columns;
                            growth_point_y = (y + j + rows) % rows;
                        }
                    }
                }

                // A little trick to subtract the current cell's state since
                // we added it in the above loop
                neighbors -= board[x,y].Previous;
                // Debug.Log(neighbors);

                direction = board[growth_point_x, growth_point_y].GetDirection();

                // Rules of Life
                if ((board[x,y].State != 1) && (neighbors < 3) && (y == growth_point_y - 1)) {

                    if (x == growth_point_x + direction) board[x,y].NewState(1);
                    else if (Random.Range(0.0f, 1.0f) < 0.2f) board[x,y].NewState(1);

                }
            }
        }
    }

    public void Display() // Draw the cells. Since the cells are now objects, they are responsible for their own drawing
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                board[i,j].Display();
            }
        }
    }
}