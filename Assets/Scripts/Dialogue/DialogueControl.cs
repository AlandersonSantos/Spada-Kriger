using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public GameObject dialogueBox;

    public TMP_Text nameText;
    public TMP_Text dialogueText;

    private string[] lines;

    private int index;

    void Awake()
    {
        instance=this;
    }

    public void StartDialogue(
        string npcName,
        string[] dialogueLines)
    {
        dialogueBox.SetActive(true);

        nameText.text=npcName;

        lines=dialogueLines;

        index=0;

        dialogueText.text=lines[index];
    }

    void Update()
    {
        if(dialogueBox.activeSelf &&
           Input.GetKeyDown(KeyCode.Space))
        {
            index++;

            if(index<lines.Length)
            {
                dialogueText.text=
                lines[index];
            }
            else
            {
                dialogueBox.SetActive(false);
            }
        }
    }
}