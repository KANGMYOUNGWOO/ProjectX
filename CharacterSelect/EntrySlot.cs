using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntrySlot : MonoBehaviour
{
    private int num;


    [SerializeField]private Image slotImage;

    public void SetImage(Sprite sprite)
    {
        if (sprite == null)
        {
            slotImage.gameObject.SetActive(false);
        }
        else
        {
            slotImage.sprite = sprite;
            slotImage.gameObject.SetActive(true);
        }
    }

}
