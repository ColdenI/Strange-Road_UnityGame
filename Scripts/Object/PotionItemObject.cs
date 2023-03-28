using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionItemObject : MonoBehaviour
{

    public TypePotionItem typePotion = TypePotionItem.SpeedPotion;

    public enum TypePotionItem
    {
        ZombiePotion = 0,
        SpeedPotion = 1,
        LivePotion = 2,
        JumpPotion = 3,
        PowerPotion = 4
    }
}
