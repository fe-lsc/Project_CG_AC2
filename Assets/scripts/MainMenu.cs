using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private RectTransform scoreRectTransform;

    [SerializeField]
    private GameManager gameManager;
    void Start(){
        scoreRectTransform.anchoredPosition = new Vector2(
            scoreRectTransform.anchoredPosition.x,
            7
            );
        GetComponentInChildren<TMPro.TextMeshProUGUI>().gameObject
            .LeanScale(new Vector3(1.2f, 1.2f),0.5f).setLoopPingPong();
    }
    public void Play(){
        GetComponent<CanvasGroup>().LeanAlpha(0,0.5f)
            .setOnComplete(OnComplete);
    }

    private void OnComplete(){
        scoreRectTransform.LeanMoveY(-72f, 0.75f).setEaseOutBounce();

        gameManager.Enable();
        Destroy(gameObject);
    }
    
}
