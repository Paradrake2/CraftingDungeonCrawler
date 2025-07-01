using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string skillID;
    public Button button;
    public Image icon;
    public GameObject lockOverlay;
    void OnEnable()
    {
        StartCoroutine(DelayedInit());
    }
    private IEnumerator DelayedInit()
    {
        yield return new WaitUntil(() => SkillTreeManager.Instance != null && SkillTreeManager.Instance.skillLookup.ContainsKey(skillID));
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            SkillTreeManager.Instance.UnlockSkill(skillID);
            Debug.Log("Clicked Button");
        });

        RefreshState();
    }

    public void RefreshState()
    {
        Debug.Log("Called refresh state");
        Debug.LogWarning(SkillTreeManager.Instance.skillLookup[skillID]);
        bool unlocked = SkillTreeManager.Instance.skillLookup[skillID].unlocked;
        bool canUnlock = SkillTreeManager.Instance.CanUnlock(skillID);
        bool acquired = SkillTreeManager.Instance.Acquired(skillID);
        Debug.Log(canUnlock);
        Debug.Log(skillID + (!unlocked && !canUnlock));
        lockOverlay.SetActive(!(unlocked && !acquired) || !canUnlock);
        button.interactable = canUnlock;
        if (acquired) button.onClick.RemoveAllListeners();
        icon.sprite = SkillTreeManager.Instance.skillLookup[skillID].icon;
        if (SkillTreeManager.Instance.skillLookup[skillID].acquired == true) lockOverlay.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("ENTERED");
        if (SkillTreeManager.Instance != null && SkillTreeManager.Instance.skillLookup.ContainsKey(skillID))
        {
            string desc = SkillTreeManager.Instance.skillLookup[skillID].description;
            TooltipUI.Instance.ShowTooltip(desc, Input.mousePosition);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.HideTooltip();
    }
    // Update is called once per frame
    void Update()
    {
    }
}
