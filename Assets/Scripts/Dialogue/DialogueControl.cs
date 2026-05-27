using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public GameObject dialogueBox;

    public TMP_Text nameText;
    public TMP_Text dialogueText;

    private DialogueLine[] lines;

    private int index;

    private bool canAdvance;

    public bool justClosed;

    void Awake()
    {
        instance = this;
    }

    public bool IsDialogueOpen()
    {
        return dialogueBox.activeSelf;
    }

    public void StartDialogue(DialogueLine[] dialogueLines)
    {
        dialogueBox.SetActive(true);

        PlayerMovement player =
        FindFirstObjectByType<PlayerMovement>();

        if (player != null)
        {
            player.enabled = false;
        }

        lines = dialogueLines;

        index = 0;

        ShowLine();

        canAdvance = false;

        Invoke(nameof(EnableAdvance), 0.15f);
    }

    void ShowLine()
    {
        nameText.text = lines[index].speaker;

        dialogueText.text = lines[index].text;
    }

    void EnableAdvance()
    {
        canAdvance = true;
    }

    void Update()
    {
        if (
            dialogueBox.activeSelf &&
            canAdvance &&
            Input.GetKeyDown(KeyCode.L)
        )
        {
            index++;

            if (index < lines.Length)
            {
                ShowLine();
            }
            else
            {
                CloseDialogue();
            }
        }
    }

    void CloseDialogue()
    {
        dialogueBox.SetActive(false);

        PlayerMovement player =
        FindFirstObjectByType<PlayerMovement>();

        if (player != null)
        {
            player.enabled = true;
        }

        justClosed = true;

        Invoke(nameof(ResetCloseFlag), 0.2f);
    }

    void ResetCloseFlag()
    {
        justClosed = false;
    }
}