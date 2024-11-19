using System.Collections;
using System.Collections.Generic;
using SQLite;
using UnityEngine;

[Table("store")]
public class StoreModel
{
    [PrimaryKey]
    [Column("store_id")]
    public int StoreId { get; set; }
}
