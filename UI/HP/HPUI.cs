using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class HPUI : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;

    [SerializeField] private Image CharacterImage;
    
    [SerializeField] private Image BackGroundImage;   
    [SerializeField] private Image NameBackGroundImage;
    [SerializeField] private Image ShiledBackGroundImage;
    [SerializeField] private Image ShiledImage;

    [SerializeField] private TextMeshProUGUI NameText;

    [SerializeField] private SlotBar HealthBar;
    [SerializeField] private SlotBar ShieldBar;

    private HealthSystem healthSystem;
    private Canvas canvas;
    private bool isSpawned = false;
    private bool isAimeed = false;

    [SerializeField] private Vector2 offset;
   
    private float ScreenWidth;
    private float ScreenHeight;

    private LevelGrid levelGrid;
    

    public void OnSpawnInitialize(HealthSystem hs, int health, int shield , Sprite sprite , string name)
    {
        this.healthSystem = hs;
        healthSystem.OnDamaged += HealthChange;
       
        CharacterImage.sprite = sprite;
        NameText.text = name;

        HealthBar.OnSpawnInitliaze(health);
        ShieldBar.OnSpawnInitliaze(shield);
        isSpawned = true;

        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        levelGrid = GameManager.GetManagerClass<LevelGrid>();
        
      
        ScreenWidth = Screen.width;
        ScreenHeight = Screen.height;
    }

   

    private void HealthChange(object sender, HealthSystem.EventStruct e)
    {
        HealthBar.DamageAdjust(e.health,e.healthIndex);
        ShieldBar.DamageAdjust(e.shield,e.shieldIndex);
    }

    public void SetIsAimed(bool b)
    {
        isAimeed = b;
    }
   
    private void Update()
    {        
        if (!isSpawned) return;
        
        if (!isAimeed)
        {
            float ScreenX = ScreenWidth * Camera.main.WorldToViewportPoint(healthSystem.transform.position).x + offset.x;
            float ScreenY = ScreenHeight * Camera.main.WorldToViewportPoint(healthSystem.transform.position).y + offset.y;

            rectTransform.position = new Vector2(ScreenX, ScreenY);
        }

        else
        {
            rectTransform.anchoredPosition = new Vector2(777,272);
        }
       
     }
        
}
