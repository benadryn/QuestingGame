using UnityEngine;
using UnityEngine.UI;

public class EnemyMiniMapMarker : MonoBehaviour
{
    private string _id;
    private QuestManager _questManager;
    [SerializeField] private Image minimapImage;

    private bool _isDead;

    private void Start()
    {
        _questManager = QuestManager.Instance;
        _id = gameObject.tag;
    }

    private void Update()
    {
        if (_isDead) return;
        foreach (var quest in _questManager.activeQuests)
        {
            if (quest.id == _id)
            {
                minimapImage.gameObject.SetActive(true);
            }
            else
            {
                minimapImage.gameObject.SetActive(false);
            }
        }
    }

    public void RemoveMiniMapImage()
    {
        Debug.Log("hello");
        minimapImage.gameObject.SetActive(false);
        _isDead = true;
    }
}
