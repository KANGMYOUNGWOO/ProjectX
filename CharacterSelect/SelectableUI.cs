using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;


public class SelectableUI : MonoBehaviour
{
    public UIManager uiManager { get; set; }
    private CharacterClass cc;

    [SerializeField] private Image nameImage;
    [SerializeField] private Image skillImage;
    [SerializeField] private Image BackGround;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI skillText;
    [SerializeField] private TextMeshProUGUI skillNameText;
    
    [SerializeField] private Button CancleButton;
    [SerializeField] private Button ConfirmButton;

    [SerializeField] private List<SelectableSkillIcon> Skills = new List<SelectableSkillIcon>();
    [SerializeField] private List<SelectableStat> stats = new List<SelectableStat>();
    
   

    public void SpawnUI(CharacterClass cc)
    {
        gameObject.SetActive(true);

        this.cc = cc;

        nameText.text = cc.characterName;
        nameText.gameObject.SetActive(false);
        skillImage.gameObject.SetActive(false);

        ConfirmButton.enabled = false;
        CancleButton.enabled = false;

        BackGround.rectTransform.DOSizeDelta(new Vector2(1179,1099.8f),0.3f).SetEase(Ease.InOutFlash).From(new Vector2(1179,0));
        nameImage.rectTransform.DOSizeDelta(new Vector2(576.2f,241.3f),0.3f).SetEase(Ease.InOutFlash).From(new Vector2(576.2f,0));
        CancleButton.image.rectTransform.DOSizeDelta(new Vector2(226.1f, 226.1f), 0.3f).SetEase(Ease.InOutFlash).From(new Vector2(226.1f,0));
        ConfirmButton.image.rectTransform.DOSizeDelta(new Vector2(226.1f, 226.1f), 0.3f).SetEase(Ease.InOutFlash).From(new Vector2(226.1f,0)).OnComplete(()=>OnSpawnEnd(cc));
        
        for(int i=0;i<Skills.Count;i++)
        {
            Skills[i].OnSpawned(cc.skillImage[i]);
            stats[i].OnSpawned();
            stats[i].gameObject.SetActive(false);
        }
    }

    private void OnSpawnEnd(CharacterClass cc)
    {
        nameText.gameObject.SetActive(true);
        //skillImage.gameObject.SetActive(true);

        ConfirmButton.enabled = true;
        CancleButton.enabled = true;

        for(int i=0;i<stats.Count;i++)
        {
            stats[i].gameObject.SetActive(true);
            stats[i].OnSpawned(cc.stats[i]);
        }
    }

    public void OnCancleButtonPressed()
    {
        uiManager.DiasableSelectalbeUI();
        gameObject.SetActive(false);
    }

    public void OnConfirmButton()
    {
        uiManager.OnSelectableConfirm();
        gameObject.SetActive(false);
    }


    public void OnSkillICon(int num)
    {
        skillText.text = cc.skillExplain[num];
        skillNameText.text = cc.skillName[num];
        skillImage.gameObject.SetActive(true);        
    }

    public void OnSkillIconOut()
    {
        skillImage.gameObject.SetActive(false);
    }
}
