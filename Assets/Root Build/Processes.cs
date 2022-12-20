using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Processes : MonoBehaviour 
{
    private Board board;

    private GameObject soilRep, rootRep, orgRep;
    private float fixedDeltaTime;

    void Start(){
        //initialise prefabs
        rootRep = GameObject.Find("RootPrefab");
        orgRep = GameObject.Find("OrgPrefab");

        board = new Board(rootRep, orgRep);

        //hide prefabs

        //make the program run at a readable speed
        LimitFrameRate();
    }

    void Update(){
        board.Generate();
    }

    private void LimitFrameRate(){
        this.fixedDeltaTime = Time.fixedDeltaTime;
        Time.fixedDeltaTime = this.fixedDeltaTime * 0.05f;
    }
}
