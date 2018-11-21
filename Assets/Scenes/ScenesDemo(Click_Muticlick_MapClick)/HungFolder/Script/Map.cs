using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map  {
    [SerializeField]
    public Cell[,] mapCell;
  public  int width { get; private set; }
  public  int height { get; private set; }


    public Map(int width=100,int height=100)
    {
        this.width = width;
        this.height = height;

        mapCell = new Cell[width, height];
        for(int i=0; i<width;i++)
        { for (int j = 0; j < height; j++)
            {
                mapCell[i, j] = new Cell(i, j);
            }
        }
    }


    public Cell GetCellAt(int x, int y)
    {
        if (x < 0 || x > width - 1 || y < 0 || y > height - 1)
            return null;
        return mapCell[x, y];
    }
    // Use this for initialization
 
}
