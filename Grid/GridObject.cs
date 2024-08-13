

using System.Collections.Generic;

public class GridObject 
{
    private GridPosition gridPosition;
    private GridSystem<GridObject>   GridSystem;

    private List<Unit> unitList;

    public GridObject(GridSystem<GridObject> gridSystem , GridPosition gridPosition)
    {
        this.GridSystem = gridSystem;
        this.gridPosition = gridPosition;
        unitList = new List<Unit>();
    }

    public void AddUnit(Unit unit)
    {
        unitList.Add(unit);
    }

    public void RemoveUnit(Unit unit)
    {
        unitList.Remove(unit);
    }

    public List<Unit> GetUnitList()
    {
        return unitList;
    }

    public bool HasAnyUnit()
    {
        return unitList.Count > 0;
    }

    public Unit GetUnit()
    {
        if(HasAnyUnit())  return unitList[0];
        else return null;
    }

}
