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
		public Dialogue(DialogueType _type, string _dialogue, string _action)
		{
			type = _type;
			dialogue = _dialogue;
            actionId = _action;
		}
		public DialogueType type;
		[TextArea]
		public string dialogue;
		public string actionId;
	}

	[field: SerializeField]
	public List<Dialogue> sequence {  get; private set; }



	
    public Dialogue GetDialogue(int _sequenceIndex)
	{
		return sequence[_sequenceIndex];
	}
}
