using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterSelection : MonoBehaviour,ILoadable
{   
    private ResourcesManager resourcesManager;    
    private UIManager uiManager;
    private UnitManager unitManager;

    private List<SelectableCharacter> characterList = new List<SelectableCharacter>();
    private CharacterClass[] Entry = new CharacterClass[3]; 

    public event EventHandler OnMainMenuClicked; 
    public event EventHandler OnSelectableUI; 

    private int spawnIndex = 0;
    private int currentSelectedCharacter = 0;
    private int selectedCharacter = 0;

    [SerializeField] private GameObject selectCamera;
 
    [SerializeField] private MainMenu menu;

    // Start is called before the first frame update
    private void Start()
    {
        resourcesManager = GameManager.GetManagerClass<ResourcesManager>();
        uiManager = GameManager.GetManagerClass<UIManager>();
        unitManager = GameManager.GetManagerClass<UnitManager>();

        resourcesManager.OnLoadedEnd += RESOURCESMANAGER_OnLoadedEnd;
        uiManager.OnDisableSelectableUI += UIMANAGER_ONQUITBUTTON;
        uiManager.OnSelectableConFirmButton += UIMANAGER_OnConfirmButton;
        uiManager.OnEntrySlotQuitButton += UIMANAGER_OnEntrySlotQuit;
        uiManager.OnEntryUIConfirmButton += UIMANAGER_EntryConfirm;

        selectCamera.SetActive(false);

        CharacterClass empty = new CharacterClass();
        empty.characterName = "";
        empty.characterImage = null;

        for(int i=0;i<Entry.Length;i++)
        {
            Entry[i] = empty;
        }
    }


    private void SpawnSelectableCharacter(int num)
    {       
        CharacterClass character = resourcesManager.GetCharacterClass(num);
       
        resourcesManager.SpawnObject(string.Format("selectable_{0}",character.characterName), new Vector3(6.89f + 1.5f * num, 0, 0.93f), this);
    }


    public void CharacterSelect(int num)
    {
        currentSelectedCharacter = num;

        for(int i=0;i<characterList.Count;i++)
        {
            if (i == num) continue;
            characterList[i].gameObject.SetActive(false);
        }
        characterList[num].SetSelectableState(SelectableCharacter.selectState.select);
        OnSelectableUI.Invoke(this,EventArgs.Empty);
        
        switch(num)
        {
            case 0:               
                selectCamera.transform.position = new Vector3(5.32f, 2.36f, 3.19f);
                selectCamera.transform.rotation = Quaternion.Euler(26.7f, 180, 0);
                break;

            case 1:               
                selectCamera.transform.position = new Vector3(7.13f, 2.24f, 2.85f);
                selectCamera.transform.rotation = Quaternion.Euler(26.7f, 180, 0);
                break;
            case 2:
                selectCamera.transform.position = new Vector3(8.67f, 3.08f, 3.59f);
                selectCamera.transform.rotation = Quaternion.Euler(26.5f, 180, 0);
                break;
            case 3:
                
                selectCamera.transform.position = new Vector3(10.28f, 2.99f, 3.63f);
                selectCamera.transform.rotation = Quaternion.Euler(26.7f, 180, 0);
                break;
            case 4:               
                selectCamera.transform.position = new Vector3(11.23f, 2.99f, 3.63f);
                selectCamera.transform.rotation = Quaternion.Euler(26.7f, 180, 0);
                break;
        }

        selectCamera.SetActive(true);
        uiManager.SetSelectableUI(resourcesManager.GetCharacterClass(num));
    }

    public void GetGameInstace(GameObject game, string code)
    {       
         SelectableCharacter character = game.GetComponent<SelectableCharacter>();
         character.SpawnCharacter(this, spawnIndex);
         spawnIndex++;
         characterList.Add(character);               
    }

    private void UIMANAGER_OnConfirmButton(object sender, EventArgs e)
    {
        CharacterClass cc = resourcesManager.GetCharacterClass(currentSelectedCharacter);
        for(int i = 0; i < Entry.Length; i++)
        {
            if (Entry[i].name == cc.characterName)
            {
                selectCamera.SetActive(false);
                OnMainMenuClicked.Invoke(this, EventArgs.Empty);
                return;
            }
        }
        
        for(int i=0;i<Entry.Length;i++)
        {
            if (Entry[i].name == "")
            {
                Entry[i] = cc;                
                break;
            }
        }

        unitManager.SetEntryArray(ref Entry);
        selectCamera.SetActive(false);
        OnMainMenuClicked.Invoke(this, EventArgs.Empty);
        uiManager.setEntryUI(ref Entry);
    }

    private void UIMANAGER_OnEntrySlotQuit(object sender, UIManager.EventStruct es)
    {
        CharacterClass empty = new CharacterClass();
        empty.characterName = "";
        empty.characterImage = null;

        Entry[es.num] = empty;

       
        uiManager.setEntryUI(ref Entry);
    }

    private void UIMANAGER_EntryConfirm(object sender, EventArgs e)
    {
        for(int i=0; i< Entry.Length;i++)
        {
            if (Entry[i].characterName == "")  return; 
        }

        menu.gameObject.SetActive(true);
        menu.OnLoad();
        StartCoroutine(load());
        IEnumerator load()
        {
            yield return new WaitForSeconds(2.0f);
          
            resourcesManager.LoadScene("TutoScene");
        }
        //resourcesManager.LoadScene("GameScene");
    }


    public void SetGame()
    {       
        OnMainMenuClicked.Invoke(this,EventArgs.Empty);
    }

    private void RESOURCESMANAGER_OnLoadedEnd(object sender, EventArgs e)
    {       
        for (int i = 0; i < 5; i++) SpawnSelectableCharacter(i);       
    }

    private void UIMANAGER_ONQUITBUTTON(object sender, EventArgs e)
    {
        selectCamera.SetActive(false);
        OnMainMenuClicked.Invoke(this,EventArgs.Empty);      
    }

    

   
}
