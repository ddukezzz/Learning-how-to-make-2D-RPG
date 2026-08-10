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
    Shard_TeleportHpRewind, // When you swap places with Shard, your HP% is same as it was when you created that Shard
    
    // ------ Sword Throw Tree ------
    SwordThrow,         // You can throw your Sword to deal Damage to Enemies from afar
    SwordThrow_Spin,    // Your Sword will spin at one point and deal Damage
    SwordThrow_Pierce,  // Your Sword will Pierce through N targets
    SwordThrow_Bounce,  // Bounce Sword will bounce between enemies
    
    // ------ Time Echo Tree ------
    TimeEcho, // Create a Clone of a Player, It can take Damage from Enemies
    TimeEcho_SingleAttack,      // Time Echo can perform a Single Attack
    TimeEcho_MultiAttack,       // Time Echo can perform N Attacks
    TimeEcho_ChanceToMultiply,  // Time Echo has a chance to create another Time Echo
    TimeEcho_HealWisp,          // When Time Echo dies, It creates a Wisp that flies towards the Player to heal
    TimeEcho_CleanseWisp,       // Wisp will now remove Negative Effects from Player
    TimeEcho_CooldownWisp,      // Wisp will reduce cooldown of all Skills by N seconds
    
    // ------ Domain Expansion ------
    Domain_SlowingDown,     // Create an area in which You can slow down all Enemies by 90-100%. You can freely move and fight
    Domain_EchoSpam,        // You can no longer move, but You can spam Enemies with Time Echo ability
    Domain_ShardSpam,       // You can no loner move, but You can spam Enemies with Time Shard ability
}
