using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Display() // Drawing the cells. Cells with a state of 1 are black, cells with a state of 0 are white
    {
        if (numberOfCells <= cellCapacity)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                GameObject newCell = GameObject.CreatePrimitive(PrimitiveType.Quad);
                numberOfCells++;
                Renderer r = newCell.GetComponent<Renderer>();
                r.material = new Material(Shader.Find("Diffuse"));
                Object.Destroy(newCell.GetComponent<BoxCollider>());
                if (cells[i] == 1)
                {
                    r.material.color = Color.black;
                }
                else
                {
                    r.material.color = Color.white;
                }

                // Set position based to lower left of screen, with screen offset
                newCell.transform.position = new Vector3(i - screenSize.x - xScreenOffset,
                                                generation - screenSize.y + yScreenOffset);
            }
        }
    }

}
