using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] public string charName;
    [SerializeField] public Sprite charSprite;
    [SerializeField][TextArea] private string[] dialogue;
    [SerializeField] private Response[] responses;

    public string CharacterName => charName;
    public string[] Dialogue => dialogue;
    public Sprite CharSprites => charSprite;
    public bool HasResponses => Responses != null && Responses.Length > 0;
    public Response[] Responses => responses;
}
