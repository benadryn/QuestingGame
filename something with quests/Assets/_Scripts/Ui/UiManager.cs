using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Buttons")] 
    [SerializeField] private Button acceptQuestButton;
    [SerializeField] private Button declineQuestButton;
    [SerializeField] private Button completeQuestButton;
    [SerializeField] private Button cancelQuestButton;
    
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
        SetQuestButtonInteractable(true);
        questCanvas.enabled = true;
        questTitle.text = quest.questName;
        questDescription.text = quest.description;
        _currentQuest = quest;
    }

    public void AcceptQuest()
    {
        SetQuestButtonInteractable(false);
        _questManager.StartQuest(_currentQuest);
        questCanvas.enabled = false;
        NPC.PlayGoodbyeSfx?.Invoke();

    }

    public void HandInQuest(QuestInfoSo quest)
    {
        SetQuestButtonInteractable(true);
        finishQuestCanvas.enabled = true;
        finishQuestTitle.text = quest.questName;
        finishQuestDescription.text = quest.completedDescription;
        _currentQuest = quest;
        _questManager.questShowingOnUi = true;
        // NPC.playGoodbyeSfx?.Invoke();

    }

    public void HandInQuestButton()
    {
        // SetQuestButtonInteractable(false);
        _questManager.HandInQuest(_currentQuest);
        finishQuestCanvas.enabled = false;
        _questManager.questShowingOnUi = false;
    }

    public void DeclineQuest()
    {
        SetQuestButtonInteractable(false);
        NPC.PlayGoodbyeSfx?.Invoke();
        questCanvas.enabled = false;
        finishQuestCanvas.enabled = false;
        _currentQuest = null;
        _questManager.questShowingOnUi = false;
    }

    // Otherwise will not work with navigation
    private void SetQuestButtonInteractable(bool isInteractable)
    {
        acceptQuestButton.interactable = isInteractable;
        declineQuestButton.interactable = isInteractable;
        completeQuestButton.interactable = isInteractable;
        cancelQuestButton.interactable = isInteractable;
    }
}
