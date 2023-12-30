using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    private UiManager _uiManager;
    public List<QuestInfoSo> activeQuests = new List<QuestInfoSo>();
    public List<QuestInfoSo> completedQuests = new List<QuestInfoSo>();
    [SerializeField] private TextMeshProUGUI questDetailSide;
    public bool questShowingOnUi;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

    }


    private void Start()
    {
        _uiManager = UiManager.instance;
    }

    public void ShowQuest(QuestInfoSo quest)
    {
        if (!activeQuests.Contains(quest) && AreRequiredQuestsCompleted(quest) && !questShowingOnUi)
        {
            _uiManager.UpdateQuestText(quest);
            questShowingOnUi = true;
        }
    }
    
    public void StartQuest(QuestInfoSo quest)
    {
        if (!activeQuests.Contains(quest) && questShowingOnUi)
        {
            quest.isActive = true;
            activeQuests.Add(quest);
            UpdateSideQuestDetails(activeQuests);
            questShowingOnUi = false;
        }
        
    }

    private bool AreRequiredQuestsCompleted(QuestInfoSo quest)
    {
        foreach (var requiredQuest in quest.requiredQuests)
        {
            if (!completedQuests.Contains(requiredQuest))
            {
                return false;
            }
        }

        return true;
    }

    public void AdvanceKillQuest(string questId, EnemyType enemyType)
    {
        foreach (var quest in activeQuests)
        {
            if (questId == quest.id && enemyType == quest.enemyType)
            {
                AdvanceQuest(quest);
            }
        }
    }

    public void AdvanceCollectQuest(string questId)
    {
        foreach (var quest in activeQuests)
        {
            if (questId == quest.id)
            {
                AdvanceQuest(quest);
            }
        }
    }
    
    private void AdvanceQuest(QuestInfoSo quest)
    {
        foreach (var objective in quest.objectives)
        {
            if (objective.currentAmount < objective.targetAmount)
            {
                objective.currentAmount++;
            }

            if (objective.currentAmount >= objective.targetAmount)
            {
                CompleteQuest(quest);
            }
        }
        UpdateSideQuestDetails(activeQuests);
    }

    public void CompleteQuest(QuestInfoSo quest)
    {
        if (activeQuests.Contains(quest))
        {
            quest.isCompleted = true;
        }
    }

    public void HandInQuest(QuestInfoSo quest)
    {
        if (quest.isActive)
        {
            PlayerExperience.XpGain?.Invoke(quest.experience);
            NPC.PlayGoodbyeSfx?.Invoke();
        }
        quest.isActive = false;
        quest.isHandedIn = true;
        completedQuests.Add(quest);
        activeQuests.Remove(quest);
        RemoveSideQuestDetails();
    }

    private void UpdateSideQuestDetails(List<QuestInfoSo> quests)
    {
        string questDetailsText = "";
        
        foreach (var quest in quests)
        {
            if (quest.isCompleted)
            {
                questDetailsText += $"{quest.questName} - Completed \n \n";
            }
            else
            {
                foreach (var objective in quest.objectives)
                {
                    questDetailsText += $"{quest.questName} - {objective.description} - {objective.currentAmount}/{objective.targetAmount}\n \n";
                }
            }
        }

        questDetailSide.text = questDetailsText;
    }

    private void RemoveSideQuestDetails()
    {
        UpdateSideQuestDetails(activeQuests);
    }
}
