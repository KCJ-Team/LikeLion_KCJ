using System.Collections;
using System.Collections.Generic;
using SQLite;
using UnityEngine;

[Table("equipment")]
public class EquipmentModel
{
    [PrimaryKey]
    [Column("equipment_id")]
    public int EquipmentId { get; set; }
}
