using System;
using System.Collections;
using System.Collections.Generic;
using SQLite;
using UnityEngine;

/// <summary>
/// DB의 컬럼 매핑을 위한 Model 클래스
/// </summary>
[Table("player")]
public class PlayerModel
{
    [PrimaryKey]
    [Column("player_id")]
    public string PlayerId { get; set; }

    [Column("player_type")]
    public int PlayerType { get; set; }

    [Column("player_energy")]
    public int PlayerEnergy { get; set; }

    [Column("player_food")]
    public int PlayerFood { get; set; }

    [Column("player_workforce")]
    public int PlayerWorkforce { get; set; }

    [Column("player_fuel")]
    public int PlayerFuel { get; set; }

    [Column("player_research")]
    public int PlayerResearch { get; set; }

    [Column("player_currency")]
    public int PlayerCurrency { get; set; }

    [Column("player_powerplant_level")]
    public int PlayerPowerplantLevel { get; set; }

    [Column("player_biofarm_level")]
    public int PlayerBiofarmLevel { get; set; }

    [Column("player_quarters_level")]
    public int PlayerQuartersLevel { get; set; }

    [Column("player_fuelplant_level")]
    public int PlayerFuelplantLevel { get; set; }

    [Column("player_research_lab_level")]
    public int PlayerResearchLabLevel { get; set; }

    [Column("player_recovery_room_level")]
    public int PlayerRecoveryRoomLevel { get; set; }

    [Column("player_recreation_room_level")]
    public int PlayerRecreationRoomLevel { get; set; }
    
    [Column("player_d_day")]
    public int PlayerDDay { get; set; }
    
    [Column("player_hp")]
    public int PlayerHp { get; set; }
    
    [Column("player_stress")]
    public int PlayerStress { get; set; }
}

// id
// 타입, 자원들, 빌딩 업글 여부 V
// 다른 테이블 랜덤인카운터들 V
// 인벤토리 테이블. 