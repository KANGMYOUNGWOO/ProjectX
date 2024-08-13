using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionBusyUI : MonoBehaviour
{
    private ActionSystem actionSystem;
    
    private void Start()
    {
        actionSystem = GameManager.GetManagerClass<ActionSystem>();
        actionSystem.OnIsBusyChanged += ActionSystem_OnBusyChanged;
        Control(false);
    }


    private void Control(bool bis)
    {
        gameObject.SetActive(bis);
    }

    private void ActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        Control(isBusy);
    }


}
