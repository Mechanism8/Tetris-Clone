using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty Level", menuName = "Configs/Difficulty Level", order = 1)]
public class DifficultyLevelScriptableObject : ScriptableObject
{
    public int difficultyLevel;
}
