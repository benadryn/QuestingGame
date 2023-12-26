using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    
    private QuestManager _questManager;
    [SerializeField] private Canvas questCanvas;
    [SerializeField] private TextMeshProUGUI questTitle;
    [SerializeField] private TextMeshProUGUI questDescription;
    [SerializeField] private Canvas finishQuestCanvas;
    [SerializeField] private TextMeshProUGUI finishQuestTitle;
    [SerializeField] private TextMeshProUGUI finishQuestDescription;
    
    private QuestInfoSo _currentQuest;

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
        _questManager = QuestManager.instance;
        
        questCanvas.enabled = false;
        finishQuestCanvas.enabled = false;
    }

    public void UpdateQuestText(QuestInfoSo quest)
    {
        questCanvas.enabled = true;
        questTitle.text = quest.questName;
        questDescription.text = quest.description;
        _currentQuest = quest;
    }

    public void AcceptQuest()
    {
        _questManager.StartQuest(_currentQuest);
        questCanvas.enabled = false;
        NPC.playGoodbyeSfx?.Invoke();

    }

    public void HandInQuest(QuestInfoSo quest)
    {
        finishQuestCanvas.enabled = true;
        finishQuestTitle.text = quest.questName;
        finishQuestDescription.text = quest.completedDescription;
        _currentQuest = quest;
        _questManager.questShowingOnUi = true;
        // NPC.playGoodbyeSfx?.Invoke();

    }

    public void HandInQuestButton()
    {
        _questManager.HandInQuest(_currentQuest);
        finishQuestCanvas.enabled = false;
        _questManager.questShowingOnUi = false;
    }

    public void DeclineQuest()
    {
        NPC.playGoodbyeSfx?.Invoke();
        questCanvas.enabled = false;
        finishQuestCanvas.enabled = false;
        _currentQuest = null;
        _questManager.questShowingOnUi = false;
    }
}
