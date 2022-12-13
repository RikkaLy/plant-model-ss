using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Processes : MonoBehaviour 
{
    private Board board;

    void Start(){
        board = new Board();

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
