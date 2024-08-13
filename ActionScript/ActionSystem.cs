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

    #region TryHandleUnitSelection�� ���� ����
    /*
      TryHandleUnitSelection �� ���콺 �������� Ŭ�� ���� �� ������ �����ϰ� ���ִ� �Լ���.
      Ray�� ���� ���� ������Ʈ�� unit�̶�� �� ������ ��ũ��Ʈ�� ������
      setselctUnit=> �� ���õ� ���ֿ� �ִ´�.
      
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

    #region  HandleSelectedAction�� ���� ����
    /*
      slectedUnit�� ������ ���¿��� ��ư�� ���� selectedAction�� �������ٸ�
      HandleSelectedAction�� �ش� �׼��� ���콺�� ������ ���� ���� ��ȿ���� �˻��Ѵ�.
      
      ��ȿ�� �Է��� ���Դٸ� selectedAction�� setBusy�� �ɾ� 
      �ش� �ൿ�� ������ ������ �ٸ� �ൿ�� �� �� ���� �����.
      
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
