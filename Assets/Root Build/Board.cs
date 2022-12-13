using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board
{
    public int columns, rows, z_rows;
    private SoilCell[,,] soilCube;
    private RootCell[,,] rootBoard;
    private Organism[,,] organismBoard;
   
    public Board () 
    {
        columns = 10;
        rows = 10;
        z_rows  = 10;
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
                    GameObject board_pos = new GameObject();
                    soilCube[z, x, y] = board_pos.AddComponent<SoilCell>();
                    soilCube[z, x, y].Setup(x, y, z);

                    rootBoard[z, x, y] = board_pos.AddComponent<RootCell>();
                    rootBoard[z, x, y].Setup(x, y, z);

                    organismBoard[z, x, y] = board_pos.AddComponent<Organism>();
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
                    soilCube[h,i,j].Display();
                    rootBoard[h,i,j].Display();
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

                    int soilNeighbors = 0;
                    int rootNeighbors = 0;
                    int orgNeighbors = 0;

                    for(int h = -1; h <= 1; h++)
                    {
                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                soilNeighbors += soilCube[(z + h + z_rows) % z_rows, (x + i + columns) % columns, (y + j + rows) % rows].Previous;
                                rootNeighbors += rootBoard[(z + h + z_rows) % z_rows, (x + i + columns) % columns, (y + j + rows) % rows].Previous;
                                orgNeighbors += organismBoard[(z + h + z_rows) % z_rows, (x + i + columns) % columns, (y + j + rows) % rows].Previous;
                            }
                        }
                    }
                    soilNeighbors -= soilCube[z,x,y].Previous;  
                    rootNeighbors -= rootBoard[z,x,y].Previous; 
                    orgNeighbors -= organismBoard[z,x,y].Previous; 

                    soilCube[z,x,y].Generate(soilNeighbors);
                    rootBoard[z,x,y].Generate(rootNeighbors);
                    organismBoard[z,x,y].Generate(orgNeighbors);              
                }
            }
        }

    }
}
