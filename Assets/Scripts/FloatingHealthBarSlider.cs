using UnityEngine;
using UnityEngine.UI;
public class FloatingHealthBarSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }
    public void ShowHealthBar()
    {
        if (!slider.IsActive()) slider.gameObject.SetActive(true);
    }

    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
