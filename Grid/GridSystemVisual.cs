using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private GridVisualObject gridSystemVidualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualMaterialList;

    LevelGrid levelGrid;
    ActionSystem actionSystem;
    GridVisualObject[,] gridVisualObjectArray;

    
    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material material;
    }

    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        RedSoft,
        Yellow,
    }

    private void Start()
    {       
        actionSystem = GameManager.GetManagerClass<ActionSystem>();
        actionSystem.visual = this;
        actionSystem.OnSelectedActionChanged += UpdateGridVisual;
    }

    public void CreateGrid()
    {
        levelGrid = GameManager.GetManagerClass<LevelGrid>();
        gridVisualObjectArray = new GridVisualObject[levelGrid.GetWidth(), levelGrid.GetHeight()];
        levelGrid.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
        for (int x = 0; x < levelGrid.GetWidth(); x++)
        {
            for (int z = 0; z < levelGrid.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 vec = levelGrid.GetWorldPosition(gridPosition);

                GridVisualObject t = Instantiate(gridSystemVidualSinglePrefab, new Vector3(vec.x , 0.1f, vec.z), Quaternion.identity);                
                gridVisualObjectArray[x, z] = t;
            }
        }
    }

   


    public void HideAllGridPosition()
    {
        for (int x = 0; x < gridVisualObjectArray.GetLength(0); x++)
        {
            for (int z = 0; z < gridVisualObjectArray.GetLength(1); z++)
            {
                gridVisualObjectArray[x, z].Hide();           
            }
        }
    }

    public void ShowAllGridPosition(List<GridPosition> gridPositionList , GridVisualType gridVisualType)
    {
        if (gridPositionList == null) return;

        for(int i=0;i<gridPositionList.Count;i++)
        {
           
            gridVisualObjectArray[gridPositionList[i].x,gridPositionList[i].z].Show(GetGridVisualTypeMaterial(gridVisualType));           
        }
       
    }

    private void UpdateGridVisual(object sender, EventArgs e)
    {       
        Unit selectedUnit = actionSystem.GetSelectedUnit();
        BaseAction selectedAction = actionSystem.GetSelectedAction();

       

        GridVisualType gridVisualType;
        HideAllGridPosition();
        switch (selectedAction)
        {
            default:
                gridVisualType = GridVisualType.White;
                break;
            case MoveAction moveAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case ShootAction shotAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositonRange(selectedUnit.GetGridPosition(), shotAction.GetShootRange(), GridVisualType.RedSoft);
                break;
        }
       
        ShowAllGridPosition(selectedAction.GetValidActionGridPositionList(), gridVisualType);
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs a)
    {
       
        Unit selectedUnit = actionSystem.GetSelectedUnit();
        BaseAction selectedAction = actionSystem.GetSelectedAction();

        GridVisualType gridVisualType;
        HideAllGridPosition();

        switch (selectedAction)
        {
            default:
                gridVisualType = GridVisualType.White;
                break;
            case MoveAction moveAction:
                gridVisualType = GridVisualType.Blue;
                break;
            case ShootAction shotAction:
                gridVisualType = GridVisualType.Red;
                ShowGridPositonRange(selectedUnit.GetGridPosition(), shotAction.GetShootRange(), GridVisualType.RedSoft);
                break;
        }
        
        ShowAllGridPosition(selectedAction.GetValidActionGridPositionList(),gridVisualType);    
       
    }

    private void  ShowGridPositonRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {

        List<GridPosition> gridPositionList = new List<GridPosition>();
        for(int x = -range; x <= range; x++)
        {
            for(int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                if (testDistance > range) continue;
                if (!levelGrid.IsValidGridPosition(testGridPosition)) continue;
                if (testGridPosition == gridPosition) continue;              
                gridPositionList.Add(testGridPosition);
               
            }
        }
        ShowAllGridPosition(gridPositionList, gridVisualType);
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        for(int i=0;i<gridVisualMaterialList.Count;i++)
        {
            if(gridVisualMaterialList[i].gridVisualType == gridVisualType)
            {
                return gridVisualMaterialList[i].material;
            }
        }

        Debug.LogError("찾으려는 타입이 없습니다");
        return null;
    }

}
