using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectableCharacter : MonoBehaviour
{
    private int num;
    private bool isSpawned = false;
    [SerializeField]private LayerMask layer;

    private Animator animator;
    private CharacterSelection characterSelection;
   
    public enum selectState
    {
        ready,
        select,
    }

    
    public void SpawnCharacter(CharacterSelection cs, int num)
    {
        this.characterSelection = cs;
        this.num = num;
        cs.OnMainMenuClicked += CHARACTERSELECTION_OnMainMenuClicked;
        cs.OnSelectableUI += CHARACTERSELCTION_OnSelectableUI;

        animator = GetComponentInChildren<Animator>();
       
    }

    public void SetSelectableState(selectState sel)
    {
        transform.rotation = Quaternion.Euler(0, -20, 0);

        switch(sel)
        {
            case selectState.ready:
                //animator.SetInteger("Select", 0);
                break;
            case selectState.select:
                //animator.SetInteger("Select", 1);
                break;
        }       
    }

    private void Update()
    {
        if (!isSpawned) return;
        if (Input.GetMouseButtonDown(0))
        {            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (EventSystem.current.IsPointerOverGameObject()) return;           
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 30f,layer))
            {               
                if (raycastHit.collider.gameObject == this.gameObject)
                {                 
                    characterSelection.CharacterSelect(num);
                }
            }
        }
    }

    private void CHARACTERSELECTION_OnMainMenuClicked(object sender, EventArgs e)
    {
        isSpawned = true;
        gameObject.SetActive(true);
        transform.rotation = Quaternion.identity;
    }

    private void CHARACTERSELCTION_OnSelectableUI(object sender, EventArgs e)
    {
        isSpawned = false;
        animator.transform.localPosition = Vector3.zero;
        animator.transform.localRotation = Quaternion.identity;
    }
    
    // Start is called before the first frame update    
}
