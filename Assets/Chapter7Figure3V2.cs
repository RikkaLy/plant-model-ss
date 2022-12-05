using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Example 7.3 Game of Life OOP
/// </summary>

public class Chapter7Figure3V2 : MonoBehaviour
{
    // Each cell is now an object!
    private Chapter7Figure3GOLV2 gol;

    // Start is called before the first frame update
    void Start()
    {
        gol = new Chapter7Figure3GOLV2();
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
    }
}

public class Chapter7Figure3CellV2
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

    public List<int> states = new List<int>();
    float colour;
    string result;

    public int State;
    public int Previous;

    public Chapter7Figure3CellV2(int _x, int _y)
    {
        x = _x;
        y = _y;

        // How big our screen is in World Units
        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        xScreenOffset = 9f;
        yScreenOffset = 5f;

        for(int i = 0; i < 10; i++){
            states.Add(0);
        }
        states.Add(Random.Range(0, 2));
        State = states[9];
        Previous = State;
        CreateGameObject();
    }   

    public void NewState(int newState)
    {
        State = newState;
        states.Add(newState);
        states.RemoveAt(0);

    }

        public void SavePrevious()
    {
        Previous = State;
    }

    public void Display()
    {
        for (int i = 0; i < states.Count; i++){
            colour = colour + states[i];
        }

        // if(colour < 1){
        //     cellMaterial.color = new Color(0.5f, 0.5f, 0.5f, 1);
        // }else if(colour < 2){
        //     cellMaterial.color = new Color(0, 1, 1, 1);
        // }else if(colour < 3){
        //     cellMaterial.color = new Color(0, 0, 1, 1);
        // }else if (colour < 4){
        //     cellMaterial.color = new Color(, 0, 0, 1);
        // }else{
        //     cellMaterial.color = new Color(0, 0, 0, 1);
        // }

        // string result = "List contents: ";
        // foreach (var item in states)
        // {
        //     result += item.ToString() + ", ";
        // }
        // Debug.Log(result);

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

public class Chapter7Figure3GOLV2
{
    // Initialize rows, columns and set-up array
    private int columns, rows;    

    private Chapter7Figure3CellV2[,] board;

    public Chapter7Figure3GOLV2()
    {        
        columns = 72;
        rows = 41;
        board = new Chapter7Figure3CellV2[columns, rows];
        Innit();
    }

    private void Innit()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                board[i, j] = new Chapter7Figure3CellV2(i, j);
            }
        }
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
                // else Debug.Log(":(");                                                                                          // else do nothing!
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