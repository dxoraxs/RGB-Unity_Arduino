using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class SliderEditorText : MonoBehaviour, IPointerDownHandler
{
    private Text editText;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Slider slider;
    [SerializeField] private Vector2 canvasWorldPosition;
    [SerializeField] private Image line;

    private void Start()
    {
        editText = GetComponentInChildren<Text>();
        rectTransform = GetComponent<RectTransform>();
        
        editText.text = canvasWorldPosition.y.ToString();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        MainBodySelectorColor.OnClickSlider(line, slider, editText, transform.position.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(rectTransform.rect.width, rectTransform.rect.height));
    }

    [Button]
    private void FindPosition()
    {
        Debug.Log(transform.position + " || " + Camera.main.ScreenToWorldPoint(transform.position));
        canvasWorldPosition = transform.position;
    }
}