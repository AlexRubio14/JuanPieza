using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/DialogueData")]

public class DialogueData : ScriptableObject
{
	public enum DialogueType { START, DIALOGUE, ACTION, END }
    [Serializable]
	public struct Dialogue
	{
		public DialogueType type;
        public SerializedDictionary<LanguageManager.Language, string> dialogues;
        public string actionId;
	}

	[field: SerializeField]
	public List<Dialogue> sequence {  get; private set; }



	
    public Dialogue GetDialogue(int _sequenceIndex)
	{
		return sequence[_sequenceIndex];
	}
}
