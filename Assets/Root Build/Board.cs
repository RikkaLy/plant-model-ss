using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board
{
    public int columns, rows, z_rows;

    //'boards' of each cell so that they can be enabled rather than cloned
    private SoilCell[,,] soilCube;
    private RootCell[,,] rootBoard;
    private Organism[,,] organismBoard;
   
    public Board () 
    {
        columns = 10;
        rows = 10;
        z_rows  = 10;

        soilCube = new SoilCell[z_rows, columns, rows];
        rootBoard = new RootCell[z_rows, columns, rows];
        organismBoard = new Organism[z_rows, columns, rows];
        Innit();
    }

    public void Innit()
    {
        for (int z = 0; z < z_rows; z++)
        {
            for (int x = 0; x < columns; x++) 
            {
                for (int y = 0; y < rows; y++) 
                {
                    //duplicate the prefab for the cell and give it a name
                    GameObject board_pos = GameObject.Instantiate(GameObject.Find("SoilPrefab"));
                    string name = "soil"+ x + y + z;
                    board_pos.name = name;
                    //add the functionality script as a component
                    board_pos.AddComponent<SoilCell>();
                    //add it to a 'folder' for easy access
                    board_pos.transform.parent = GameObject.Find("All_Soil_Cells").transform;
                    //add it to the board to it can be updated
                    soilCube[z, x, y] = board_pos.GetComponent(typeof(SoilCell)) as SoilCell;
                    //run the setup function
                    soilCube[z, x, y].Setup(x, y, z);

                    //repeat for each cell
                    board_pos = GameObject.Instantiate(GameObject.Find("RootPrefab"));
                    name = "root"+ x + y + z;
                    board_pos.name = name;
                    board_pos.AddComponent<RootCell>();
                    board_pos.transform.parent = GameObject.Find("All_Root_Cells").transform;
                    rootBoard[z, x, y] = board_pos.GetComponent(typeof(RootCell)) as RootCell;
                    rootBoard[z, x, y].Setup(x, y, z);

                    board_pos = GameObject.Instantiate(GameObject.Find("OrgPrefab"));
                    name = "org"+ x + y + z;
                    board_pos.name = name;
                    board_pos.AddComponent<Organism>();
                    board_pos.transform.parent = GameObject.Find("All_Organisms").transform;
                    organismBoard[z, x, y] = board_pos.GetComponent(typeof(Organism)) as Organism;
                    organismBoard[z, x, y].Setup(x, y, z);
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
                    //enable or disable the cells in relation to their state
                    soilCube[h, i, j].Display();
                    rootBoard[h, i, j].Display();
                    organismBoard[h,i,j].Display();
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
                    //save the state so it can be used to inform the rules
                    soilCube[h,i,j].SavePrevious();
                    rootBoard[h,i,j].SavePrevious();
                    organismBoard[h,i,j].SavePrevious();
                }
            }
        }

        for (int z = 0; z < z_rows; z++)
        {
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {

                    //running all the neighbour codes at the same time to save processing time
                    int soilNeighbors = 0;
                    int rootNeighbors = 0;
                    int orgNeighbors = 0;

                    for(int h = -1; h <= 1; h++)
                    {
                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                //add all enabled 27 cubes around and including the selected cell
                                soilNeighbors += soilCube[(z + h + z_rows) % z_rows, (x + i + columns) % columns, (y + j + rows) % rows].Previous;
                                rootNeighbors += rootBoard[(z + h + z_rows) % z_rows, (x + i + columns) % columns, (y + j + rows) % rows].Previous;
                                orgNeighbors += organismBoard[(z + h + z_rows) % z_rows, (x + i + columns) % columns, (y + j + rows) % rows].Previous;
                            }
                        }
                    }

                    //remove the current cell's state
                    soilNeighbors -= soilCube[z,x,y].Previous;  
                    rootNeighbors -= rootBoard[z,x,y].Previous; 
                    orgNeighbors -= organismBoard[z,x,y].Previous; 

                    //generate the individual logic for each cell based on it's neighbours
                    soilCube[z,x,y].Generate(soilNeighbors);
                    rootBoard[z,x,y].Generate(rootNeighbors);
                    organismBoard[z,x,y].Generate(orgNeighbors);              
                }
            }
        }

    }
}
