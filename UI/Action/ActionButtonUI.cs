using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ActionButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private Button button;
    [SerializeField] private Image SelectedGameObject;

    private ActionSystem actionSystem;
    private BaseAction baseAction;

    public void SetBaseAction(BaseAction baseAction)
    {
        if(actionSystem == null) actionSystem = GameManager.GetManagerClass<ActionSystem>();
        textMeshPro.text = baseAction.GetActionName().ToUpper();
        this.baseAction = baseAction;
        button.onClick.AddListener(() => {  actionSystem.SetSelctedAction(baseAction); } );
    }


    public void ClearBaseAction()
    {
        button.onClick.RemoveAllListeners();
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = actionSystem.GetSelectedAction();
        SelectedGameObject.gameObject.SetActive(selectedBaseAction == baseAction);
    }
    
}
