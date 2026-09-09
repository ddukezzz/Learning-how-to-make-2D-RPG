using TMPro;
using UnityEngine;

public class UI_QuestPreview : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private TextMeshProUGUI questDescription;
    [SerializeField] private TextMeshProUGUI questGoal;
    [SerializeField] private UI_QuestRewardSlot[] questReward;

    [SerializeField] private GameObject[] additionalObjects;

    public void SetupQuestPreview(QuestDataSO questDataSO)
    {
        EnableAdditionalObjects(true);
        EnableQuestRewardObjects(true);
        
        questName.text = questDataSO.questName;
        questDescription.text = questDataSO.description;
        questGoal.text = questDataSO.questGoal;

        for (int i = 0; i < questDataSO.rewardItems.Length; i++)
        {
            Inventory_Item rewardItem = new Inventory_Item(questDataSO.rewardItems[i].itemData);
            rewardItem.stackSize = questDataSO.rewardItems[i].stackSize;
            
            questReward[i].gameObject.SetActive(true);
            questReward[i].UpdateSlot(questDataSO.rewardItems[i]);
        }
    }

    public void MakeQuestPreviewEmpty()
    {
        questName.text = "";    
        questDescription.text = "";

        EnableAdditionalObjects(false);
        EnableQuestRewardObjects(false);
    }

    private void EnableAdditionalObjects(bool enable)
    {
        foreach (var obj in additionalObjects)
            obj.SetActive(enable);
    }
    
    private void EnableQuestRewardObjects(bool enable)
    {
        foreach (var obj in questReward)
            obj.gameObject.SetActive(false);
    }
}
