using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualObject : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;


    public void Show(Material material)
    {
         gameObject.SetActive(true);
         meshRenderer.material = material;
    }

    public void Hide()
    {
        gameObject.SetActive(false);       
    }
}
