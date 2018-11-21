using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell  {

    enum CellState { None,Active}
    int X, Y;
    CellState State;
	// Use this for initialization


        public Cell(int x,int y)
    {
        X = x;
        Y = y;
        State = CellState.None;
    }
	
}
