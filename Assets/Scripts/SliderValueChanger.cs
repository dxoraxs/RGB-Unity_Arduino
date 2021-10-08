using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderValueChanger : MonoBehaviour
{
    [SerializeField] private TypeSlider type;
    private Text value;
    private Slider slider;
        
    private void Awake()
    {
        value = GetComponentInChildren<Text>();
        slider = GetComponentInChildren<Slider>();
        slider.onValueChanged.AddListener(SetText);
    }

    public void OnPointerUp()
    {
        SelectorController.OnSliderPointerUp(type);
    }

    public void SetNewValue(Color32 newValue)
    {
        switch (type)
        {
            case TypeSlider.R:
                slider.value = newValue.r / 255f;
                value.text = newValue.r.ToString();
                break;
            case TypeSlider.G:
                slider.value = newValue.g / 255f;
                value.text = newValue.g.ToString();
                break;
            case TypeSlider.B:
                slider.value = newValue.b / 255f;
                value.text = newValue.b.ToString();
                break;
        }
    }

    public void SetText(float newValue)
    {
        if (!slider) return;
        value.text = ((byte)(newValue * 255)).ToString();
        SelectorController.OnChangeColorSlider((byte)(newValue * 255), type);
    }
}
