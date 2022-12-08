using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfoTransfer : MonoBehaviour
{
    // Each cell is now an object!
    private InfoTransferGOL gol;

    public static InfoTransfer Instance { get; private set; }
    void Awake(){Instance = this;}

    // Start is called before the first frame update
    void Start()
    {
        gol = new InfoTransferGOL();
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

    public void colourChange(IEnumerator cor){
        StartCoroutine(cor);
    }
}

public class Organism{
    public int value;
    private int x;
    private int y;

    private Vector2 screenSize;
    private float yScreenOffset; // Cells are spawned with a small offset so they spawn in more centered    
    private float xScreenOffset;

    private GameObject cellRep;
    private Material cellMaterial;

    public Organism(int pos_x, int pos_y, int newValue){
        x = pos_x;
        y = pos_y;
        value = newValue;

        screenSize = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        xScreenOffset = 9f;
        yScreenOffset = 5f;

        CreateGameObject();
    }

    public void setValue(int newValue){
        value = newValue;
    }

    public void Display(){
        cellMaterial.color = Color.red;

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

public class InfoTransferCell
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

    public int value = 0;
    public bool transfer = false;

    public InfoTransferCell(int _x, int _y)
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
        if(value == 100){
            InfoTransfer.Instance.colourChange(infoHold());
            value = 0;
            transfer = true;
        }
        else if (Previous == 0 && State == 1) // On reproduction
        {
            cellMaterial.color = Color.blue;
        }
        else if (State == 1) // On continuing to stay alive
        {
            cellMaterial.color = Color.black;
        }
        else // On continuing to stay dead
        {
            cellMaterial.color = Color.white;
        }

        cellRep.transform.position = new Vector3((x * cellRep.transform.localScale.x) - screenSize.x - xScreenOffset, 
                                                 (y * cellRep.transform.localScale.x) - screenSize.y - yScreenOffset);
    }

    IEnumerator infoHold(){
        cellMaterial.color = Color.magenta;
        yield return new WaitForSeconds(.5f);
    }

    public void setValue(int newValue){
        // Debug.Log(newValue +" " +x + " "+y);
        if (newValue == 100){
            value = newValue;
        }else if (newValue == 0){
            value = newValue;
        }
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

public class InfoTransferGOL
{
    // Initialize rows, columns and set-up array
    private int columns, rows;    

    private InfoTransferCell[,] board;
    private Organism[,] organism_board;

    public InfoTransferGOL()
    {        
        columns = 72;
        rows = 41;
        board = new InfoTransferCell[columns, rows];
        organism_board = new Organism[columns, rows];
        Innit();
    }

    private void Innit()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                board[i, j] = new InfoTransferCell(i, j);
                organism_board[i, j] = new Organism(i, j, 0);
            }
        }
        board[columns/2, rows -1].NewState(1);
        organism_board[25, 10].setValue(100);

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

                bool newTransfer = false;

                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        InfoTransferCell n_cell = board[(x + i + columns) % columns, (y + j + rows) % rows];
                        neighbors += n_cell.Previous;

                        if (n_cell.Previous == 1){
                            growth_point_x = (x + i + columns) % columns;
                            growth_point_y = (y + j + rows) % rows;
                        }
                        if ((organism_board[(x + i + columns) % columns, (y + j + rows) % rows].value == 100) && board[x, y].State == 1){
                            newTransfer = true;
                            // Debug.Log("touched organism");
                        }
                        else if ((n_cell.transfer == true) && (board[x, y].State == 1) && (board[x, y].value == 0) && (board[x, y].transfer == false)){
                            // board[(x + i + columns) % columns, (y + j + rows) % rows].setValue(0);
                            newTransfer = true;
                            // Debug.Log("near high value");
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
                // Debug.Log(newTransfer);
                if(newTransfer == true) board[x, y].setValue(100);
                newTransfer = false;
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
        organism_board[25,10].Display();
    }
}