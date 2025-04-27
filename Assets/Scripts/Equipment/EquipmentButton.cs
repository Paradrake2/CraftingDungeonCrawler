using UnityEngine;
using UnityEngine.UI;

public class EquipmentButton : MonoBehaviour
{
    public Equipment equipmentData;
    private Button button;
    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }
    public void Setup(Equipment equipment) {
        equipmentData = equipment;
    }
    private void OnClick() {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
