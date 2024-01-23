using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMiniMapMarker : MonoBehaviour
{
    private string _id;
    private QuestManager _questManager;
    [SerializeField] private Image minimapImage;
    private bool _questObjectiveFinished;

    private bool _isDead;

    private void Start()
    {
        _questManager = QuestManager.Instance;
        _id = gameObject.tag;
    }

    private void Update()
    {
        if (_isDead || _questManager.activeQuests.All(quest => quest.id != _id || quest.isCompleted))
        {
            minimapImage.gameObject.SetActive(false);
        }
        else
        {
            minimapImage.gameObject.SetActive(true);
        }
    }

    public void RemoveMiniMapImage()
    {
        _isDead = true;
    }

  
}
