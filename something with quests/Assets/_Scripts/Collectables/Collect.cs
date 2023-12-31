using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Collect : MonoBehaviour
{
    private Collider _collider;
    private QuestManager _questManager;
    private string _id;

    private void Start()
    {
        _collider = GetComponent<Collider>();
        _questManager = QuestManager.Instance;
        _id = gameObject.tag;
    }

    private void Update()
    {
        foreach (var unused in _questManager.activeQuests.Where(quest => quest.id == _id))
        {
            SetCollectable();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            QuestManager.Instance.AdvanceCollectQuest(GetQuestId());
            Destroy(gameObject);
        }
    }

    private string GetQuestId()
    {
        return gameObject.tag;
    }

    private void SetCollectable()
    {
        _collider.enabled = true;
    }
}
