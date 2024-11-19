using System.Collections;
using System.Collections.Generic;
using SQLite;
using UnityEngine;

[Table("inventory")]
public class InventoryModel
{
    [PrimaryKey]
    [Column("inventory_id")]
    public int InventoryId { get; set; }
}
