using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Processes : MonoBehaviour 
{
    private Board board;

    void Start(){
        board = new Board();
        //Debug.Log(board);
        LimitFrameRate();
    }

    void Update(){
        board.Generate();
        board.Display();
    }

    private void LimitFrameRate(){
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 5;
    }
}
