using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "HintAction", menuName = "Scriptable Objects/Hint Action Data")]
public class HintActionData : ScriptableObject
{
    public enum Language { ENGLISH, SPANISH };

    [field: SerializeField]
    public string HintId { get; private set; }

    [field: SerializeField, SerializedDictionary("Language", "Hint")]
    public SerializedDictionary<Language, string> hintTexts { get; private set; }
}
