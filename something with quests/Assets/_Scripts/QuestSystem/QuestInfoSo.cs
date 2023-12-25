using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests", fileName = "New Quest")]
public class QuestInfoSo : ScriptableObject
{
   public string id;
   public string questName;
   public int levelRequired;
   public int experience;
   [TextArea(3, 10)] public string description;
   [TextArea(3, 10)] public string completedDescription;
   public List<QuestInfoSo> requiredQuests;
   public bool isCompleted;
   public bool isActive;
   public bool isHandedIn;
   public bool resetQuest;
   public EnemyType enemyType;
   
   
   
   public List<Objective> objectives;

   [System.Serializable]
   public class Objective
   {
      public string description;
      public int targetAmount;
      public int currentAmount;
   }
   
   public enum ObjectiveTask
   {
      Kill,
      Collect,
   }

   public void Reset()
   {
      if (resetQuest)
      {
         isHandedIn = false;
         isActive = false;
         isCompleted = false;
         foreach (var objective in objectives)
         {
            objective.currentAmount = 0;
         }
      }
   }
}
