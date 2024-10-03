using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour,IManager,ILoadable
{
    #region Manager
    public GameManager gameManager { get { return GameManager.gameManager; } }
    private ResourcesManager resourcesManager;
    #endregion

    #region Event

    public event EventHandler OnDisableSelectableUI;
    public event EventHandler OnSelectableConFirmButton;
    public event EventHandler OnEntryUIConfirmButton;

    public event EventHandler<EventStruct> OnEntrySlotQuitButton;
    #endregion

    #region UIElement
    public AccuracyUI accuracyUI { get; private set; }
    public SelectableUI selectableUI { get; private set; }
    public EntryUI entryUI { get; private set; }
    #endregion

    #region EventHandler<T>

    public struct EventStruct
    {
        public int num;
        public EventStruct(int n)
        {

        this.num = n; 
        }
         
    }
    #endregion 

    private void Start()
    {
        resourcesManager = GameManager.GetManagerClass<ResourcesManager>();
        gameManager.OnGameSceneStart += GAMEMANAGER_OnGameStart;
        resourcesManager.SpawnObject("SelectableUI",this);
        resourcesManager.SpawnObject("EntryUI",this);
    }

    #region Accuracy
    public void SetAccuracyUI(Vector3 pos, int accuracy, ShootAction shoot)
    {
        accuracyUI.SpanwUI(pos, accuracy,shoot);
    }

    public void DisableAccuracyUI()
    {
        accuracyUI.DesPawnUI();
    }
    #endregion
    #region Selectable
    public void SetSelectableUI(CharacterClass cc)
    {
        selectableUI.SpawnUI(cc);
    }

    public void DiasableSelectalbeUI()
    {
        OnDisableSelectableUI.Invoke(this,EventArgs.Empty);
    }

    public void OnSelectableConfirm()
    {        
        OnSelectableConFirmButton.Invoke(this,EventArgs.Empty);
    }
   
    #endregion
    #region Entry
    public void setEntryUI(ref CharacterClass[] cc)
    {
        Debug.Log(cc[0]);
        entryUI.SetConfirmButton(true);
        for(int i=0; i < cc.Length;i++)
        {
            entryUI.SetImage(i,cc[i].characterImage);
            if (cc[i].characterName == "") entryUI.SetConfirmButton(false);
        }
    }

    public void OnEntrySlotQuit(int n)
    {
        EventStruct es = new EventStruct(n);
        OnEntrySlotQuitButton.Invoke(this, es);
    }

    public void OnEntryConfirm()
    {
        Debug.Log("????");
        OnEntryUIConfirmButton.Invoke(this,EventArgs.Empty);
    }
    #endregion

    public void GetGameInstace(GameObject game, string code)
    {
        Transform can = GameObject.Find("Canvas").GetComponent<Canvas>().transform;
        game.transform.parent = can;

        switch (code)
        {
            case "AccuracyUI": accuracyUI = game.GetComponent<AccuracyUI>();
                accuracyUI.uiManager = this;                
                game.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                break;

            case "SelectableUI": selectableUI = game.GetComponent<SelectableUI>();
                selectableUI.uiManager = this;               
                game.GetComponent<RectTransform>().anchoredPosition = new Vector2(-3,71);
                game.SetActive(false);
                break;

            case "EntryUI" : entryUI = game.GetComponent<EntryUI>();
                entryUI.uiManager = this;
                game.GetComponent<RectTransform>().anchoredPosition = new Vector2(-1077,-551);
                entryUI.OnInitialize();
                game.SetActive(false);
                break;
        }
       
    }

    private void GAMEMANAGER_OnGameStart(object sender ,EventArgs e)
    {
        resourcesManager.SpawnObject("AccuracyUI", this);
    }

    public Canvas GetCanvas() { return GameObject.Find("Canvas").GetComponent<Canvas>(); }
}
