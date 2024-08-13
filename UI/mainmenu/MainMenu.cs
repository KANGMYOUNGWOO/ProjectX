using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FronkonGames;
using Febucci.UI;
using UnityEngine.UI;
using FronkonGames.SpritesMojo;
using UnityEngine.EventSystems;
using TMPro;


public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextAnimator_TMP press;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image image;
    private bool isStarted = false;

    private Material dissolveMat;
    private CharacterSelection characterSelection;

    private void Start()
    {
        dissolveMat = Dissolve.CreateMaterial();
        Dissolve.Slide.Set(dissolveMat, 0);
        Dissolve.Shape.Set(dissolveMat, DissolveShape.LuminousSpiral_1);
        image.material = dissolveMat;

        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry_PointerDown = new EventTrigger.Entry();
        entry_PointerDown.eventID = EventTriggerType.PointerDown;
        entry_PointerDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        eventTrigger.triggers.Add(entry_PointerDown);
        
      characterSelection = GameObject.Find("CharacterSelection").GetComponent<CharacterSelection>();
    }

    IEnumerator dissolveFunc()
    {
        WaitForSeconds wait = new WaitForSeconds(0.001f);
        float dissolveFloat = 0f;
        float dissolveSpeed = 0.01f;
        press.gameObject.SetActive(false);
        int num = 0;
        while (dissolveFloat < 1)
        {
            if (dissolveFloat > 0.5f) dissolveSpeed = 0.03f;
            if (dissolveSpeed >0.7f) dissolveSpeed = 0.2f;
            dissolveFloat += dissolveSpeed;
            Dissolve.Slide.Set(dissolveMat, dissolveFloat);
            num += 1;
            yield return wait;

        }
       
        characterSelection.SetGame();
        gameObject.SetActive(false);
    }

    IEnumerator AntidissolveFunc()
    {
        WaitForSeconds wait = new WaitForSeconds(0.001f);
        float dissolveFloat = 1f;
        float dissolveSpeed = -0.01f;
        press.gameObject.SetActive(false);
        int num = 0;
        while (dissolveFloat > 0)
        {
            if (dissolveFloat > 0.5f) dissolveSpeed = -0.03f;
            if (dissolveSpeed > 0.7f) dissolveSpeed = -0.2f;
            dissolveFloat += dissolveSpeed;
            Dissolve.Slide.Set(dissolveMat, dissolveFloat);
            num += 1;
            yield return wait;

        }
       
    }

    public void OnPointerDown(PointerEventData data)
    {
        if (!isStarted) 
        {
            isStarted = true; 
            StartCoroutine(dissolveFunc());             
        }
    }

    public void OnLoad()
    {
        text.gameObject.SetActive(true);
        text.text = "<swing> Loading </swing>";
        StartCoroutine(AntidissolveFunc());
              
    }

}
