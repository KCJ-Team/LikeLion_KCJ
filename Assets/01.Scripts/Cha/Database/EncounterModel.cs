using System;
using System.Collections;
using System.Collections.Generic;
using SQLite;
using UnityEngine;

/// <summary>
/// DB의 컬럼 매핑을 위한 Model 클래스
/// </summary>
[Table("encounter")]
public class EncounterModel
{
    [PrimaryKey]
    [Column("encounter_id")]
    public int EncounterId { get; set; }
}

// 인벤토리 테이블. 