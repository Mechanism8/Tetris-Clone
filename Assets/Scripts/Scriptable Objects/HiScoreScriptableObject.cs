using UnityEngine;

[CreateAssetMenu(fileName = "Hi Score", menuName = "Configs/Hi Score", order = 2)]
public class HiScoreScriptableObject : ScriptableObject
{
    public int hiScore;
}
