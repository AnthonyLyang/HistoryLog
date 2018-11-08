using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;



public class DetailPanelTweet : MonoBehaviour {

    public RectTransform DetailPanel;
    public RectTransform OriginAnchor;
    float DetailDestPosY;
    public bool DetailPanelIsOn=false;
    private void Awake()
    {
        DetailDestPosY = DetailPanel.localPosition.y;
        DetailPanel.localPosition = OriginAnchor.localPosition;
        DetailPanel.gameObject.SetActive(false);
    }
    // Use this for initialization
    void Start ()
    {
        Tweener DetailPanelTweener = DetailPanel.DOLocalMoveY(DetailDestPosY, 0.5f);
        DetailPanelTweener.OnPlay(() => 
        {
            DetailPanel.gameObject.SetActive(true);
        });
        DetailPanelTweener.OnComplete(() =>
        {
            DetailPanelIsOn = !DetailPanelIsOn;
        });
        DetailPanelTweener.SetAutoKill(false);
        DetailPanelTweener.Pause();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void PanelOn()
    {
        DetailPanel.DOPlayForward();
    }
    public void PanelOff()
    {
        DetailPanel.DOPlayBackwards();
        Invoke("SetFalse", 0.6f);
    }
    void SetFalse()
    {
        DetailPanelIsOn = false;
        DetailPanel.gameObject.SetActive(false);
    }

}
