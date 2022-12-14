using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Processes : MonoBehaviour 
{
    private Board board;

    private GameObject soilRep, rootRep, orgRep;

    void Start(){
        //initialise prefabs
        soilRep = GameObject.Find("SoilPrefab");
        rootRep = GameObject.Find("RootPrefab");
        orgRep = GameObject.Find("OrgPrefab");

        board = new Board(soilRep, rootRep, orgRep);

        //hide prefabs

        //make the program run at a readable speed
        LimitFrameRate();
    }

    void Update(){
        board.Generate();
        board.Display();
    }

    private void LimitFrameRate(){
        QualitySettings.vSyncCount = 0;
        //framerate
        Application.targetFrameRate = 5;
    }
}
