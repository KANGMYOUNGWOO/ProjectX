using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectableStat : MonoBehaviour
{
    [SerializeField] private List<Image> stats = new List<Image>();

    public void OnSpawned()
    {
        for (int i = 0; i < stats.Count; i++)
        {
            stats[i].gameObject.SetActive(false);
        }

    }

    public void OnSpawned(int d)
    {       
        WaitForSeconds wait = new WaitForSeconds(0.03f);

        StartCoroutine(sp(d));

        IEnumerator sp(int num)
        {
            for(int j = 0; j<num;j++)
            {
                yield return wait;
                stats[j].gameObject.SetActive(true);
            }
        }
    }
}
