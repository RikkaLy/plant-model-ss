using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Processes : MonoBehaviour 
{
    private Board board;

    private GameObject soilRep, rootRep, orgRep;

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
        QualitySettings.vSyncCount = 4;
        //framerate
        //Application.targetFrameRate = 5;
    }
}
