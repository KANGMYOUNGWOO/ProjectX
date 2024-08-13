using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntryUI : MonoBehaviour
{
    [SerializeField] private Button ConfirmButton;

    public UIManager uiManager { get; set; }

    [SerializeField]private List<EntrySlot> entrySlots = new List<EntrySlot>();

    public void OnInitialize()
    {
        for(int i=0;i<entrySlots.Count;i++)
        {
            entrySlots[i].SetImage(null);
        }

        ConfirmButton.gameObject.SetActive(false);
    }

    public void SetImage(int num, Sprite sprite)
    {
        gameObject.SetActive(true);
        
        entrySlots[num].SetImage(sprite);        
    }

    public void SetConfirmButton(bool isActive)
    {
        ConfirmButton.gameObject.SetActive(isActive);
    }


    public void OnQuitButton(int num)
    {
        entrySlots[num].SetImage(null);
        uiManager.OnEntrySlotQuit(num);
    }

    public void OnConfirmButton()
    {
        uiManager.OnEntryConfirm();
        gameObject.SetActive(false);
    }
}
