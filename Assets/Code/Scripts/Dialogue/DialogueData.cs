using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Scriptable Objects/DialogueData")]

public class DialogueData : ScriptableObject
{
	public enum DialogueType { DIALOGUE, ACTION, END }
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
	[HideInInspector]
	public int sequenceIndex = 0;

	public Dialogue GetCurrentDialogue()
	{
        return sequence[sequenceIndex];
    }
    public Dialogue GetNextDialogue()
	{
		sequenceIndex++;

		if (sequenceIndex >= sequence.Count)
			return new Dialogue(DialogueType.END, "", null);

		return sequence[sequenceIndex];
	}
}
