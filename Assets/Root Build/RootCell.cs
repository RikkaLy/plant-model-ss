using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RootCell : Cell
{

    public List<GameObject> next;
    public GameObject previous;

    public PlantCell plantCell;
    public GameObject rootParent;

    private bool max_children = false;
    public int grow_attempts;

    public Board board;

    public RootCell(float _x, float _y, float _z) : base(_x, _y, _z)
    {
        Setup(_x, _y, _z);
    }

    public void newPrevious(GameObject new_cell) 
    {
        //change the 'parent' cell carried over from cloning
        previous = new_cell;
        next = new List<GameObject>();
    }

    public void rootProcess(SoilCell soil)
    {
        //get water and nitrate from the soil
        waterConcen = waterConcen + soil.getWater(0.5f);
        nitrate = nitrate + soil.getNitrate(0.5f);
    }

    public void addGrowAttempt()
    {
        grow_attempts += 1;
    }

    // public bool checkGrowth(Vector3 vector)
    // {
    //     if (grow_attempts.Contains(vector)) return false;
    //     else return true;
    // }

    public void nutrientTransfer()
    {
        //pass the nutrients upwards
        previous.GetComponent<Cell>().addWater(waterConcen);
        waterConcen = 0;
        previous.GetComponent<Cell>().addNitrate(nitrate);
        nitrate = 0;
    }

    public override void Generate()
    {
        float new_x = x;
        float new_y = y -1;
        float new_z = z;

        nutrientTransfer();
        if (plantCell.getWater() < 0 && plantCell.getNitrate() < 0 && max_children == false) {
            //30% chance that a root will grow
            if (Random.Range(0.0f, 1.0f) < 0.3f) 
            {
                //50% chance that the root will grow unaligned
                if (Random.Range(0.0f, 1.0f) < 0.7f)
                {
                    new_x = x + Random.Range(-1, 2);
                    new_z = z + Random.Range(-1, 2);
                }

                if (GameObject.Find("root" + "x" + new_x + "y" + new_y + "z" + new_z) == null)
                {
                    //clone this cell
                    GameObject new_root = GameObject.Instantiate(gameObject);
                    RootBoundries rootBounds = new_root.transform.GetChild(0).gameObject.GetComponent<RootBoundries>();
                    
                    //perform setup of x,y,z
                    new_root.GetComponent<RootCell>().Setup(new_x, new_y, new_z);
                    //change the position
                    new_root.transform.parent = rootParent.transform;
                    new_root.transform.position = new Vector3(new_x, new_y, new_z);

                    //set new root's 'previous root' as this cell
                    new_root.GetComponent<RootCell>().newPrevious(gameObject);
                    
                    if(rootBounds.triggered == true){
                        //note that the root tried to grow, add attempt to a list so it doesn't try again
                        addGrowAttempt();
                        Debug.Log(this.name + "triggered " + new_root.transform.position.x + "x, " + new_root.transform.position.y + "y, and " + new_root.transform.position.z);
                        //destroy the new cell
                        Destroy(new_root);
                    }else
                    {
                        //change name for id purposes
                        string name = "root" + "x" + new_x + "y" + new_y + "z" + new_z;
                        new_root.name = name;

                        //add to list of 'next roots'
                        next.Add(new_root);
                    }
                }

                //if this cell has 2 children stop it from generating more
                if(next.Count == 2 || grow_attempts > 3){
                    max_children = true;
                }

                
            }
            
        }
    }

    public int getChildrenSize() 
    {
        return next.Count;
    }

    public GameObject getRecentChild() 
    {
        return next.Last();
    }

    void OnTriggerStay(Collider collisionInfo) 
    {
        if (collisionInfo.gameObject.GetComponent<SoilCell>() != null) 
        {
            rootProcess(collisionInfo.gameObject.GetComponent<SoilCell>());
        }
    }
}
