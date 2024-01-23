using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Harvest : MonoBehaviour, IHarvestable
{
    [SerializeField] private float harvestTime = 1.5f;
    private BoxCollider _collider;
    private string _id;
    [SerializeField] private Image minimapImage;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _id = gameObject.tag;
        _collider.enabled = false;
    }

    private void Update()
    {
        foreach (var quest in QuestManager.Instance.activeQuests.Where(quest => quest.id == _id))
        {
            if (quest.isCompleted)
            {
                _collider.enabled = false;
            }
            else
            {
                SetHarvestable();
            }
        }
    }

    private void SetHarvestable()
    {
        _collider.enabled = true;
        minimapImage.gameObject.SetActive(true);
    }

    public (bool, float) Harvestable(Vector3 playerPosition, float harvestDistance)
    {
        float distance = Vector3.Distance(transform.position, playerPosition);
        if (distance <= harvestDistance)
        {
            return (true, harvestTime);
        }

        return (false, 0.0f);
    }
}
