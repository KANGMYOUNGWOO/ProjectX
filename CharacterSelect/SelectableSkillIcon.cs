using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class SelectableSkillIcon : MonoBehaviour
{
    [SerializeField] private SelectableUI select;
    [SerializeField] private Image backGroundImage;
    [SerializeField] private Image IconImage;
    [SerializeField] private int num;

    void Start()
    {
        EventTrigger eventTrigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry_PointerEnter = new EventTrigger.Entry();
        entry_PointerEnter.eventID = EventTriggerType.PointerEnter;
        entry_PointerEnter.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
        eventTrigger.triggers.Add(entry_PointerEnter);

        EventTrigger.Entry entry_PointerExit = new EventTrigger.Entry();
        entry_PointerExit.eventID = EventTriggerType.PointerExit;
        entry_PointerExit.callback.AddListener((data) => { OnPointerExit((PointerEventData)data); });
        eventTrigger.triggers.Add(entry_PointerExit);

    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        select.OnSkillICon(num);
        backGroundImage.color = Color.gray;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        select.OnSkillIconOut();
        backGroundImage.color = Color.white;
    }

    public void OnSpawned(Sprite sprite)
    {
        IconImage.gameObject.SetActive(false);
        backGroundImage.rectTransform.DOSizeDelta(new Vector2(254.4f,249.8f),0.3f).SetEase(Ease.InOutFlash).From(new Vector2(254.4f, 0)).OnComplete(() => {
            IconImage.gameObject.SetActive(true);
            IconImage.sprite = sprite;
        });
    }
}
