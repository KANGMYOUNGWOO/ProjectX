using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using UnityEngine.UI;
using DG.Tweening;

public class AccuracyUI : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _damageText;
    [SerializeField] private RectTransform rect;
    [SerializeField] private Button button;
    [SerializeField] private Button canclebutton;
    [SerializeField] private Image HPBackGround;
    [SerializeField] private Image InfoBackGround;
   

    

    private float ScreenX;
    private float ScreenY;
    public float ScreenWidth;
    public float ScreenHeight;
    public UIManager uiManager { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        ScreenWidth = Screen.width;
        ScreenHeight = Screen.height;
        
     
        gameObject.SetActive(false);
    }

    public void SpanwUI(Vector2 pos, int damage, ShootAction shootAction)
    {
        button.onClick.RemoveAllListeners();
        canclebutton.onClick.RemoveAllListeners();
        
        gameObject.SetActive(true);
        
        ScreenX = ScreenWidth * Camera.main.WorldToViewportPoint(pos).x;
        ScreenY = ScreenHeight * Camera.main.WorldToViewportPoint(pos).y;
        rect.anchoredPosition = new Vector2(ScreenX, ScreenY);
        transform.SetSiblingIndex(0);

        _damageText.text = string.Format("Accuracy : {0}", damage);

        
        button.onClick.AddListener(()=> { shootAction.Fire(); });
        canclebutton.onClick.AddListener(() => shootAction.EndAction());
        canclebutton.onClick.AddListener(() => gameObject.SetActive(false));
    
        _damageText.gameObject.SetActive(false);

        HPBackGround.rectTransform.DOSizeDelta(new Vector2(1800, 2500f), 0.3f).From(new Vector2(1800, 1)).SetEase(Ease.InOutFlash);
        InfoBackGround.rectTransform.DOSizeDelta(new Vector2(1800,2500f) , 0.3f).From(new Vector2(1800,1)).SetEase(Ease.InOutFlash);

        
    }



    public void DesPawnUI()
    {
        gameObject.SetActive(false);
    }

    
}
