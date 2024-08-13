using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ActionSystemUI : MonoBehaviour
{
    [SerializeField] private ActionButtonUI ActionButtonPrefab;
    [SerializeField] private Transform actionButtonContainerTransform;

    private List<ActionButtonUI> actionButtonUIList;
    private ActionSystem actionSystem;
    

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        actionSystem = GameManager.GetManagerClass<ActionSystem>();
        actionSystem.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        actionSystem.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
    }

    private void CreateUnitActionButtons()
    {
        Unit selectedUnit = actionSystem.GetSelectedUnit();
        BaseAction[] baseActions; 
        selectedUnit.GetBaseActionArray(out baseActions);
              
        for(int i=0;i<actionButtonUIList.Count;i++)
        {
            actionButtonUIList[i].ClearBaseAction();
            actionButtonUIList[i].gameObject.SetActive(false);
        }

        for(int actionIndex = 0; actionIndex < baseActions.Length; actionIndex++)
        {
            ActionButtonUI action = actionButtonUIList.Count >= baseActions.Length ?
             actionButtonUIList[actionIndex] : Instantiate(ActionButtonPrefab, actionButtonContainerTransform)
; 
            if (actionButtonUIList.Count < baseActions.Length) actionButtonUIList.Add(action);
           
            action.gameObject.SetActive(true);
            action.SetBaseAction(baseActions[actionIndex]);     
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        CreateUnitActionButtons();
        UpdateSelectedVisual();
    }

    private void UpdateSelectedVisual()
    {
        for (int i = 0; i < actionButtonUIList.Count; i++)
        {
            actionButtonUIList[i].UpdateSelectedVisual();
        }
    }
}
