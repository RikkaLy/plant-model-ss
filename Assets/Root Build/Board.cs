using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board
{
    public int columns, rows, z_rows;

    //'boards' of each cell so that they can be enabled rather than cloned
    private GameObject[,,] board;

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

        board = new GameObject[z_rows, columns, rows];
        Innit();
    }

    public void Innit()
    {
        GameObject new_gameobject;
        for (int z = 0; z < z_rows; z++)
        {
            for (int x = 0; x < columns; x++) 
            {
                for (int y = 0; y < rows; y++) 
                {
                    //instantiate to reduce redundancy
                    GameObject new_soil = GameObject.Instantiate(soilRep);
                    string name = "soil" + "x" + x + "y" + y + "z" + z;
                    new_soil.name = name;
                    new_soil.transform.parent = soilParent.transform;
                    new_soil.GetComponent<SoilCell>().Setup(x, y, z);
                    board[z, x, y] = new_soil;
                }
            }
        }

        new_gameobject = cellInit(rootRep, "root", typeof(RootCell), rootParent, columns/2, rows-1, z_rows/2);
        board[z_rows/2, columns/2, rows-1] = new_gameobject;
        board[z_rows/2, columns/2, rows-1].GetComponent<RootCell>().Setup(columns/2, rows-1, z_rows/2);
    }

    private GameObject cellInit(GameObject rep, string repName, System.Type type, GameObject parent, int x, int y, int z)
    {
        //duplicate the prefab for the cell and give it a name
        GameObject board_pos = GameObject.Instantiate(rep);
        string name = repName + "x" + x + "y" + y + "z" + z;
        board_pos.name = name;
        //add the functionality script as a component
        board_pos.AddComponent(type);
        //add it to a 'folder' for easy access
        board_pos.transform.parent = parent.transform;
        return board_pos;
    }

    public void Generate()
    {
        for (int z = 0; z < z_rows; z++)
        {
            for (int x = 0; x < columns; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    //generate the individual logic for each cell based on it's neighbours
                    //in the future we will pass references or objects for the cell to perform it's processes on
                    int neighbours = 0;
                    board[z,x,y].GetComponent<Cell>().Generate(neighbours);            
                }
            }
        }

    }
}
