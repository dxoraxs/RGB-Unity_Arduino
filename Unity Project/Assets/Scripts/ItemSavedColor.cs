using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSavedColor : MonoBehaviour
{
    [SerializeField] private RawImage preview;
    [SerializeField] private Text name;
    public ItemColor item;
    private Tweener animation;
    private RectTransform rectTransform;
    private const float defaultHeight = 300;
    private const float defaultWidth = 1323;

    private void StopAnimation()
    {
        if (animation != null && animation.active) animation.Kill();
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Enable();
    }

    public void Disable()
    {
        StopAnimation();

        item = null;
        animation = rectTransform.DOSizeDelta(new Vector2(defaultWidth, -50), 0.5f).OnComplete(() => { gameObject.SetActive(false); });
        //canvasGroup.DOFade(0, canvasGroup.alpha / 2).OnComplete(() => { gameObject.SetActive(false); });
    }

    public void Enable()
    {
        var defaultParent = transform.parent;
        transform.SetParent(null);
        transform.SetParent(defaultParent);

        StopAnimation();
        gameObject.SetActive(true);
        animation = rectTransform.DOSizeDelta(new Vector2(defaultWidth, defaultHeight), 0.5f);
        //canvasGroup.DOFade(1, canvasGroup.alpha / 2);
    }

    public void InitItem(ItemColor item)
    {
        this.item = item;
        name.text = item.name;

        Texture2D newTexture = new Texture2D(1, 1);
        newTexture.SetPixel(0, 0, item.color);
        newTexture.Apply();
        preview.texture = newTexture;        
    }

    public void OnClickDelete()
    {
        MainController.Remove(item);
    }

    public void OnClickEditColor()
    {
        MainController.OpenSelectorColor(item);
    }

    public void OnClickSetCurrentColor()
    {
        MainController.SetCurrentColor(item);
    }
}