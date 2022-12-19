using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board
{
    public int columns, rows, z_rows;

    //'boards' of each cell so that they can be enabled rather than cloned
    private List<GameObject> rootBoard;

    private GameObject rootRep, orgRep;
    private GameObject rootParent, orgParent;
   
    public Board (GameObject rRep, GameObject oRep) 
    {

        rootRep = rRep;
        orgRep = oRep;

        //so that new cells can be put into 'folders'
        orgParent = GameObject.Find("All_Organisms");

        rootBoard = new List<GameObject>();
        rootBoard.Add(GameObject.Find("RootPrefab"));
    }

    public void Generate()
    {
        List<GameObject> new_children = new List<GameObject>();

        for (int i = 0; i < rootBoard.Count; i++)
        {
            //see if the size changed during runtime
            RootCell component = rootBoard[i].GetComponent<RootCell>();
            int check_size = component.getChildrenSize();
            component.Generate();

            if (component.getChildrenSize() > check_size)
            {
                //add new children later to not mess with loop
                new_children.Add(component.getRecentChild());
            }
        }

        rootBoard.AddRange(new_children);

    }
}
