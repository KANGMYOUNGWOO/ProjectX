using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class CharacterClass : ScriptableObject
{
    public string characterName;
   
    public int attackPoint;
    public int healthPoint;
    public int ShieldPoint;
   
    //public int accuratePoint;

    public int[] stats = new int[3];
    public Sprite[] skillImage = new Sprite[3];
    public string[] skillName = new string[3];
    [TextArea]
    public string[] skillExplain = new string[3];

    public Sprite characterImage;

    public enum classSystem
    {
        HandGun,
        Riffle,
    }

    classSystem cs;
}
