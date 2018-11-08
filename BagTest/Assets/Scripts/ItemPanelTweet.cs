using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ItemPanelTweet : MonoBehaviour {
    public RectTransform ItemPanel;
    public RectTransform OriginAnchor;
    public DetailPanelTweet tweet;
    float ItemDestPosY;
    public bool ItemPanelIsOn = false;
    // Use this for initialization
    private void Awake()
    {
        ItemDestPosY = ItemPanel.localPosition.y;
        ItemPanel.localPosition = OriginAnchor.localPosition;
        ItemPanel.gameObject.SetActive(false);
    }
    void Start ()
    {

        Tweener ItemPanelTweener = ItemPanel.DOLocalMoveY(ItemDestPosY, 0.5f);
        ItemPanelTweener.OnPlay(() =>
        {
            ItemPanel.gameObject.SetActive(true);
        });
        ItemPanelTweener.OnComplete(() =>
        {
            ItemPanelIsOn = !ItemPanelIsOn;
        });
        ItemPanelTweener.SetAutoKill(false);
        ItemPanelTweener.Pause();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!ItemPanelIsOn)
                PanelOn();
            else
                PanelOff();
        }
	}
    public void PanelOn()
    {
        ItemPanel.DOPlayForward();
    }
    public void PanelOff()
    {
        ItemPanel.DOPlayBackwards();
        if (tweet.DetailPanelIsOn)
        {
            Invoke("DetailOffInvoke", 0.3f);
        }
        Invoke("SetFalse", 0.6f);
    }
    void SetFalse()
    {
        ItemPanelIsOn = false;
        ItemPanel.gameObject.SetActive(false);
    }
    void DetailOffInvoke()
    {
        tweet.PanelOff();
    }
}
