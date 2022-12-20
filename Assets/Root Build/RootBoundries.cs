using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootBoundries : MonoBehaviour
{
    public bool triggered = false;
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<RootCell>() != null) 
        {
            triggered = true;
        }
        
    }
}
