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

    private GameObject soilRep, rootRep, orgRep;
    private GameObject soilParent, rootParent, orgParent;
   
    public Board (GameObject sRep, GameObject rRep, GameObject oRep) 
    {
        columns = 10;
        rows = 10;
        z_rows  = 10;

        soilRep = sRep;
        rootRep = rRep;
        orgRep = oRep;

        //so that new cells can be put into 'folders'
        soilParent = GameObject.Find("All_Soil_Cells");
        rootParent = GameObject.Find("All_Root_Cells");
        orgParent = GameObject.Find("All_Organisms");

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
                    //create a gameobject with the require characterisitics
                    GameObject new_gameobject = cellInit(soilRep, "soil", typeof(SoilCell), soilParent, x, y, z);
                    //add it to it's board
                    soilCube[z, x, y] = new_gameobject.GetComponent(typeof(SoilCell)) as SoilCell;
                    //run the setup function
                    soilCube[z, x, y].Setup(x, y, z);

                    //repeat for each cell
                    new_gameobject = cellInit(rootRep, "root", typeof(RootCell), rootParent, x, y, z);
                    rootBoard[z, x, y] = new_gameobject.GetComponent(typeof(RootCell)) as RootCell;
                    rootBoard[z, x, y].Setup(x, y, z);

                    new_gameobject = cellInit(orgRep, "org", typeof(Organism), orgParent, x, y, z);
                    organismBoard[z, x, y] = new_gameobject.GetComponent(typeof(Organism)) as Organism;
                    organismBoard[z, x, y].Setup(x, y, z);
                }
            }
        }

        rootBoard[z_rows/2, columns/2, rows-1].newState(1);
    }

    private GameObject cellInit(GameObject rep, string repName, System.Type type, GameObject parent, int x, int y, int z)
    {
        //duplicate the prefab for the cell and give it a name
        GameObject board_pos = GameObject.Instantiate(rep);
        string name = repName + x + y + z;
        board_pos.name = name;
        //add the functionality script as a component
        board_pos.AddComponent(type);
        //add it to a 'folder' for easy access
        board_pos.transform.parent = parent.transform;
        return board_pos;
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
