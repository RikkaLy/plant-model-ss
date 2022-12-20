using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootBoundries : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        //if there is a collision, let it be known that there was an attempt to grow
        transform.parent.GetComponent<RootCell>().addGrowAttempt();
        Destroy(GetComponent<Collider>().gameObject);
    }
}
