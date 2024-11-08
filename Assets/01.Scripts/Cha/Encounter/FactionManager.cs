using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class FactionManager : SceneSingleton<FactionManager>
{
   public List<Faction> factions;

   [Header("Test UI...")] 
   public TextMeshProUGUI textFaction;
   
   // 가장 지지도가 높은 팩션 반환
   public Faction GetLeadingFaction()
   {
      return factions.OrderByDescending(f => f.supportLevel).First();
   }

   // 특정 타입의 팩션 지지도를 변경
   public void ChangeFactionSupport(FactionType factionType, float amount)
   {
      Faction faction = factions.Find(f => f.type == factionType);
      
      if (faction != null)
      {
         faction.ChangeSupport(amount);
         UpdateUI(faction);
      }
   }
   
   // 전체 팩션의 지지도 출력
   public void PrintFactionSupportLevels()
   {
      foreach (var faction in factions)
      {
         Debug.Log($"{faction.name} ({faction.type}): {faction.supportLevel}");
      }
   }
   
   // 특정 타입의 팩션 아이콘을 반환
   public Sprite GetFactionIcon(FactionType factionType)
   {
      Faction faction = factions.Find(f => f.type == factionType);
      return faction != null ? faction.icon : null;
   }

   private void UpdateUI(Faction faction)
   {
      if (textFaction != null)
         textFaction.text = $"Faction : {faction.type}\n FactionSupport : {faction.supportLevel}";
   }
} // end class
