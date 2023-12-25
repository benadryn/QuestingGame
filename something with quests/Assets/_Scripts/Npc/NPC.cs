using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private List<QuestInfoSo> quests = new List<QuestInfoSo>();
    private QuestManager _questManager;
    private IInteractable _interactableImplementation;
    [SerializeField] private NpcQuestMarker npcQuestMarker;
    private bool _hasUnacceptedQuests;
    private bool _hasFinishedQuests;

    private void Start()
    {
        _questManager = QuestManager.instance;
    }

    private void Update()
    {
            SetQuestFloater();
    }
    private void SetQuestFloater()
    {
        _hasUnacceptedQuests = false;
        _hasFinishedQuests = false;

        foreach (var quest in quests)
        {
            bool allRequiredQuestsCompleted = AreAllRequiredQuestsCompleted(quest);

            if (allRequiredQuestsCompleted && !quest.isHandedIn && !quest.isActive)
            {
                if (!_questManager.activeQuests.Contains(quest))
                {
                    _hasUnacceptedQuests = true;
                    // break;
                }
            }
            else if (quest.isActive && !quest.isCompleted || quest.isHandedIn || !allRequiredQuestsCompleted || _questManager.completedQuests.Contains(quest))
            {
                npcQuestMarker.HideAllImages();
            }

            if (quest.isCompleted && quest.isActive && !quest.isHandedIn)
            {
                npcQuestMarker.ShowQuestionMark();
                _hasFinishedQuests = true;
                // break;
            }
        }

        if (_hasFinishedQuests)
        {
            npcQuestMarker.ShowQuestionMark();
        }
        else if (_hasUnacceptedQuests)
        {
            npcQuestMarker.ShowExclamation();
        }
    }
    
    private bool AreAllRequiredQuestsCompleted(QuestInfoSo quest)
    {
        foreach (var requiredQuest in quest.requiredQuests)
        {
            if (!_questManager.completedQuests.Contains(requiredQuest))
            {
                return false;
            }
        }

        return true;
    }

    public void OfferQuest(QuestInfoSo quest)
    {
        if (quest != null)
        {
            if (!_questManager.activeQuests.Contains(quest) && !quest.isCompleted && !_hasFinishedQuests)
            {
                _questManager.ShowQuest(quest);
            }

            else if(_questManager.activeQuests.Contains(quest) && quest.isCompleted && _hasFinishedQuests)
            {
                HandInQuest(quest);
            }
        }
        
    }

    
    public void HandInQuest(QuestInfoSo quest)
    {
            if (quest.isActive && quest.isCompleted)
            {
                UiManager.instance.HandInQuest(quest);
            }
    }

    public void Interact(Vector3 playerPosition, float distanceForDialog)
    {
        float distance = Vector3.Distance(transform.position, playerPosition);
        if (distance <= distanceForDialog)
        {
            foreach (var quest in quests)
            {
                OfferQuest(quest);
            }
        }
    }
}
