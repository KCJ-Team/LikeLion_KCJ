using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactionManager : SceneSingleton<FactionManager>
{
   public List<Faction> factions;

   [Header("UIs")] 
   public Image imageFaction;
   public Text textFaction;
   
   // 가장 지지도가 높은 팩션 반환
   public Faction GetLeadingFaction()
   {
      Faction leadingFaction = factions.OrderByDescending(f => f.supportLevel).First();
      UpdateFactionUI(leadingFaction.type); // UI 업데이트 호출
      return leadingFaction;
   }

   // 특정 타입의 팩션 지지도를 변경
   public void ChangeFactionSupport(FactionType factionType, float amount)
   {
      Faction faction = factions.Find(f => f.type == factionType);

      if (faction != null)
      {
         faction.ChangeSupport(amount);
         UpdateFactionUI(factionType); // UI 업데이트 호출
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

   // FactionManager 클래스 내에 있는 UpdateFactionUI 메서드 수정
   public void UpdateFactionUI(FactionType factionType)
   {
      Faction faction = factions.Find(f => f.type == factionType);

      if (faction == null) return;

      if (imageFaction != null)
         imageFaction.sprite = faction.icon;

      if (textFaction != null)
      {
         textFaction.text = $"{faction.type}";

         // 텍스트 색상을 팩션 타입에 따라 변경
         switch (faction.type)
         {
            case FactionType.Red:
               textFaction.color = Color.red;
               break;
            case FactionType.Black:
               textFaction.color = Color.black;
               break;
            case FactionType.Green:
               textFaction.color = Color.green;
               break;
            case FactionType.Yellow:
               textFaction.color = Color.yellow;
               break;
            default:
               textFaction.color = Color.white;
               break;
         }
      }
   }

} // end class
