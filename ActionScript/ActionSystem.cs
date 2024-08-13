using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class ActionSystem : MonoBehaviour,IManager
{
    public GameManager gameManager { get { return GameManager.gameManager; } }

    public GameObject g;
    private GridSystem<GridObject> gridSystem;

    private Unit SelectedUnit;
    private BaseAction SelectedAction;

    private bool isBusy = false;
    private bool isGameStarted = false;

    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<bool> OnIsBusyChanged;
    public event EventHandler OnActionStarted;

    public delegate void GameSceneDelegate();
  
    LevelGrid levelGrid;
    public GridSystemVisual visual { get; set; }
    [SerializeField] LayerMask UnitLayer;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.OnGameSceneStart += GAMEMANAGER_OnGameSceneStart;      
    }

    private void SetG()
    {
        levelGrid = GameManager.GetManagerClass<LevelGrid>();
        gridSystem.CreateDebugObjects(g);

    }

    #region TryHandleUnitSelection에 대한 설명
    /*
      TryHandleUnitSelection 은 마우스 왼쪽으로 클릭 했을 때 유닛을 선택하게 해주는 함수다.
      Ray를 쏴서 맞은 오브젝트가 unit이라면 그 유닛의 스크립트를 가져와
      setselctUnit=> 현 선택된 유닛에 넣는다.
      
     */
    #endregion
    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f, UnitLayer))
            {
                if (raycastHit.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    if (unit.GetISEnemy()) return false;
                    if(unit == SelectedUnit) return false;                   
                    SetSelectedUnit(unit);
                   
                    return true;
                }
            }
        }
        return false;
    }


    private void SetBusy()
    {
        isBusy = true;

        OnIsBusyChanged?.Invoke(this,isBusy);
    }

    private void ClearBusy()
    {
        isBusy = false;

        OnSelectedActionChanged?.Invoke(this,EventArgs.Empty);
        OnIsBusyChanged?.Invoke(this, isBusy);
    }

    public Unit GetSelectedUnit()
    {
        return SelectedUnit;
    }

    public BaseAction GetSelectedAction()
    {
        return SelectedAction; 
    }

    private void SetSelectedUnit(Unit unit)
    {
        SelectedUnit = unit;       
        SetSelctedAction(unit.GetAction<MoveAction>());

        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    public void SetSelctedAction(BaseAction baseAction)
    {
        SelectedAction = baseAction;
        OnSelectedActionChanged.Invoke(this,EventArgs.Empty);
      
    }

    #region  HandleSelectedAction에 대한 설명
    /*
      slectedUnit이 정해진 상태에서 버튼을 눌러 selectedAction이 정해졌다면
      HandleSelectedAction은 해당 액션이 마우스로 선택한 지역 에서 유효한지 검사한다.
      
      유효한 입력이 들어왔다면 selectedAction에 setBusy를 걸어 
      해당 행동이 끝나기 전까지 다른 행동을 할 수 없게 만든다.
      
     */
    #endregion

    private void HandleSelectedAction()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (SelectedUnit == null) return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f))
            {
                GridPosition mouseGridPosition = levelGrid.GetGridPosition(raycastHit.point);
                if (!SelectedAction.IsValidAction(mouseGridPosition)) return;
                if (!SelectedUnit.TrySpendActionPointsToTakeAction(SelectedAction)) return;
             
                SetBusy();
                SelectedAction.TakeAction(mouseGridPosition, ClearBusy);

                OnActionStarted?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void GAMEMANAGER_OnGameSceneStart(object sender, EventArgs e)
    {
        gridSystem = new GridSystem<GridObject>(20, 40, 2, new Vector3(0, 0, 0), (GridSystem<GridObject> g, GridPosition gridPosition) => 
        new GridObject(g, gridPosition));
        SetG();
        visual.CreateGrid();
        visual.HideAllGridPosition();
        isGameStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameStarted) return;
        TryHandleUnitSelection();
        if (isBusy) return;
        //if (EventSystem.current.IsPointerOverGameObject()) return;
        if (SelectedUnit != null)
        {          
            if (Input.GetKeyDown(KeyCode.T)) SelectedUnit.GetAction<MoveAction>().GetValidActionGridPositionList();
            HandleSelectedAction();
        }
    }
}
