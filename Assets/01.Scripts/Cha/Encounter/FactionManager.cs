using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FactionManager : SceneSingleton<BuildingManager>
{
   public List<Faction> factions;

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
      }
   }
} // end class
