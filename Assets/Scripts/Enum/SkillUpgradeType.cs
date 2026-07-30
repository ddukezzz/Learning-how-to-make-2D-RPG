using UnityEngine;

public enum SkillUpgradeType
{
    None,
    
    // ------ Dash Tree ------
    Dash,
    Dash_CloneOnStart,              // Create a Clone when Dash starts
    Dash_CloneOnStartAndArrival,    // Create a Clone when Dash starts and ends
    Dash_ShardOnStart,              // Create a Shard when Dash starts
    Dash_ShardOnStartAndArrival,    // Create a Shard when Dash starts and ends
    
    // ------ Shard Tree ------
    Shard,                  // The Shard explodes when touched by Enemy or time goes up
    Shard_MoveToEnemy,      // Shard will move to the nearest Enemy
    Shard_MultiCast,        // Shard ability can have up to N charges. You can cast them all in a row.
    Shard_Teleport,         // You can swap places with the last Shard you created
    Shard_TeleportHpRewind  // When you swap places with Shard, your HP% is same as it was when you created that Shard
}
