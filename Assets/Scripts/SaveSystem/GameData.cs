using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class GameData
{
    public int gold;

    public List<Inventory_Item> itemList;
    public SerializableDictionary<string, int> inventory;                     // itemSaveID -> stackSize
    public SerializableDictionary<string, int> storageItems;
    public SerializableDictionary<string, int> storageMaterials;

    public SerializableDictionary<string, ItemType> equippedItems;            // itemSaveID -> slotType

    public int skillPoints;
    public SerializableDictionary<string, bool> skillTreeUI;                  // skillName -> unlockStatus
    public SerializableDictionary<SkillType, SkillUpgradeType> skillUpgrades; // skillType -> upgradeType

    public Vector3 savedCheckpoint;

    public SerializableDictionary<string, bool> unlockedCheckpoints;          // Checkpoint ID -> Unlocked status
    public SerializableDictionary<string, Vector3> inScenePortals;            // Scene name -> portal position

    public string portalDestinationSceneName;
    public bool returningFromTown;

    public string lastScenePlayed;
    public Vector3 lastPlayerPosition;
    
    public GameData()
    {
        inventory = new SerializableDictionary<string, int>();
        storageItems = new SerializableDictionary<string, int>();
        storageMaterials = new SerializableDictionary<string, int>();

        equippedItems = new SerializableDictionary<string, ItemType>();
        
        skillTreeUI = new SerializableDictionary<string, bool>();
        skillUpgrades = new SerializableDictionary<SkillType, SkillUpgradeType>();
        
        unlockedCheckpoints = new SerializableDictionary<string, bool>();
        inScenePortals = new SerializableDictionary<string, Vector3>();
    }
}
