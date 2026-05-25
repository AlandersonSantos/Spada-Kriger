using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    public string npcName;

    [TextArea]
    public string[] lines;

    bool playerNear;

    void Update()
    {
       if(
        playerNear &&
        Input.GetKeyDown(KeyCode.L) &&
        !DialogueManager.instance.IsDialogueOpen() &&
        !DialogueManager.instance.justClosed
        )
        {
            DialogueManager.instance
            .StartDialogue(
                npcName,
                lines
            );
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerNear=true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerNear=false;
        }
    }
}