using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GridSystem <TGridObject> 
{
    private int width; 
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gridObjectArray;

    public GridSystem(int width,int height , float cellSize , Vector3 originPosition , Func<GridSystem<TGridObject>, GridPosition , TGridObject> createGridObject)
    {  
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridObjectArray = new TGridObject[width, height];

        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x,z);
                gridObjectArray[x, z] = createGridObject(this, gridPosition);
            }

        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize + originPosition;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {        
         return new GridPosition(
         Mathf.RoundToInt(worldPosition.x / cellSize),
         Mathf.RoundToInt(worldPosition.z / cellSize)
     );

    }

    public TGridObject GetGridObject(GridPosition gridPosition)
    {  if (gridPosition.x >= 0 && gridPosition.z >= 0 && gridPosition.x < width && gridPosition.z < height)
            return gridObjectArray[gridPosition.x, gridPosition.z];
        else return gridObjectArray[0,0];
    }

    public void CreateDebugObjects(GameObject debug)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition position = new GridPosition(x,z);
                GameObject.Instantiate(debug, GetWorldPosition(position), Quaternion.identity);               
            }
        }
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 && 
                gridPosition.z >= 0 && 
                 gridPosition.x < width && 
                  gridPosition.z < height;
    }

    public int GetWidth()
    {
        return width;
    }

    public int GetHeight()
    {
        return height;
    }

}
