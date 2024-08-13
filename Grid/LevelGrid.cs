using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class LevelGrid : MonoBehaviour,IManager
{
    public GameManager gameManager { get { return GameManager.gameManager; } }
  

    private GridSystem<GridObject> gridSystem;
    public event EventHandler OnAnyUnitMovedGridPosition;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;

    private Pathfinding pathFinding;

    private void Awake()
    {
        if (pathFinding == null)
        {
            pathFinding = GameManager.GetManagerClass<Pathfinding>();
            gameManager.OnGameSceneStart += GAMEMANAGER_OnGameSceneStart;
        }
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition , Unit unit)
    {
        pathFinding.Setup(width, height, cellSize);
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnit(unit);
    }
    /*
    public List<Unit> GetUnitAtGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).GetUnitList();
    }
    */
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnit(unit);
    }

    public void UnitMoveAtGridPosition(GridPosition prev, GridPosition next, Unit unit)
    {
        RemoveUnitAtGridPosition(prev,unit);
        AddUnitAtGridPosition(next, unit);
        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

    

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);
    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);
    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);
    public int GetHeight() => gridSystem.GetHeight();
    public int GetWidth() => gridSystem.GetWidth();

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.HasAnyUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        return gridObject.GetUnit();
    }

    private void GAMEMANAGER_OnGameSceneStart(object sender, EventArgs e)
    {
        gridSystem = new GridSystem<GridObject>(width, height, cellSize, new Vector3(0, 0, 0), (GridSystem<GridObject> g, GridPosition gridPosition) => 
        new GridObject(g, gridPosition));
    }

}
