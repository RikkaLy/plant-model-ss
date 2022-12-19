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

    public Board board;

    public RootCell(float _x, float _y, float _z) : base(_x, _y, _z)
    {
        Setup(_x, _y, _z);
    }

    public void newPrevious(GameObject new_cell) 
    {
        previous = new_cell;
        next = new List<GameObject>();
    }

    public void rootProcess(SoilCell soil)
    {
        //get water and nitrate from the soil
        waterConcen = waterConcen + soil.getWater(0.1f);
        nitrate = nitrate + soil.getNitrate(0.1f);
        //Debug.Log("root water: " + waterConcen + " soil water: " + soil.waterConcen);
        //Debug.Log("root nitrate: " + nitrate + " soil nitrate: " + soil.nitrate);
    }

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
        if (plantCell.getWater() < -20 && plantCell.getNitrate() < -20) {
            //50% chance that a root will grow
            if (Random.Range(0.0f, 1.0f) < 0.3f) 
            {
                //30% chance that the root will grow unaligned
                if (Random.Range(0.0f, 1.0f) < 0.3f)
                {
                    new_x = x + Random.Range(-1, 2);
                    new_z = z + Random.Range(-1, 2);
                }

                if (GameObject.Find("root" + "x" + new_x + "y" + new_y + "z" + new_z) == null)
                {
                    //clone this cell
                    GameObject new_root = GameObject.Instantiate(gameObject);
                    
                    new_root.GetComponent<RootCell>().Setup(new_x, new_y, new_z);
                    new_root.transform.parent = rootParent.transform;
                    new_root.transform.position = new Vector3(new_x, new_y, new_z);

                    string name = "root" + "x" + new_x + "y" + new_y + "z" + new_z;
                    new_root.name = name;

                    //set new root's 'previous root' as this cell
                    new_root.GetComponent<RootCell>().newPrevious(gameObject);
                    //add to list of 'next roots'
                    next.Add(new_root);
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
