using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;

public class SlotBar : MonoBehaviour
{
    [SerializeField] private List<Image> SlotList = new List<Image>();
    [SerializeField] private Image gagueBar;
    [SerializeField] private TextMeshProUGUI statText;

    private int maxStat = 0;

    private float prevShowStat = 0;
    private float currentShowStat = 0;

    private Sequence FadeSequence = DOTween.Sequence(); 
   
    public void ShowDamage(int stat, int index)
    {
        float prevStat = (float)stat / (float)maxStat;
        float currentStat = (float)(stat - index) / (float)maxStat;


    }

    public void FadeEnd()
    {
        FadeSequence.Pause();
    }

    public void OnSpawnInitliaze(int stat)
    {
        maxStat = stat;
        statText.text = string.Format("{0}",stat);
        FadeSequence.Append(gagueBar.DOFillAmount(currentShowStat,0.3f).From(prevShowStat).SetLoops(-1,LoopType.Restart).SetAutoKill(false));
        FadeSequence.Pause();
    }

    
    public void DamageAdjust(int stat, int index)
    {
        float prevStat    = (float)stat  / (float)maxStat;
        float currentStat = (float)(stat-index) / (float)maxStat;
        
        FadeSequence.Pause();
        gagueBar.DOFillAmount(currentStat,0.1f).From(prevStat).SetEase(Ease.InOutBounce).OnComplete(()=>statText.text =string.Format("{0}",stat-index));
    }

    
}
