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
    public string playerId { get; set; }
}
