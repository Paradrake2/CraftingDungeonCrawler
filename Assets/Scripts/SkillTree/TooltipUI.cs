using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;
    public GameObject tooltipPanel;
    public TextMeshProUGUI descriptionText;
    public Vector2 offset = new Vector2(150f, 150f);
    void Awake()
    {
        tooltipPanel.SetActive(false);
        Instance = this;
    }

    public void ShowTooltip(string description, Vector3 position)
    {
        tooltipPanel.SetActive(true);
        descriptionText.text = description;
        tooltipPanel.transform.position = position + (Vector3)offset;
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tooltipPanel.transform.position = Input.mousePosition + (Vector3)offset;
    }
}
