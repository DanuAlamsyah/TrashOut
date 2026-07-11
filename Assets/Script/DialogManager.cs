using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject eqoPanel;
    public GameObject verelPanel;

    [Header("Texts")]
    public TextMeshProUGUI eqoNameText;
    public TextMeshProUGUI eqoDialogueText;

    public TextMeshProUGUI verelNameText;
    public TextMeshProUGUI verelDialogueText;

    [Header("Characters")]
    public GameObject eqo;
    public GameObject verel;

    [Header("Dialog Data")]
    public DialogData[] dialogues;

    [Header("Typing")]
    public float typingSpeed = 0.03f;

    private int currentIndex = 0;
    private bool isTyping = false;

    [Header("Next Scene")]
    public string nextScene;

   void Start()
    {
        eqoPanel.SetActive(false);
        verelPanel.SetActive(false);
    }

    public void StartDialogue()
    {
        ShowDialogue();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextDialogue();
        }
    }

    void ShowDialogue()
    {
        string speaker = dialogues[currentIndex].speaker;
        string sentence = dialogues[currentIndex].sentence;

        // reset text
        eqoDialogueText.text = "";
        verelDialogueText.text = "";

        if (speaker == "EQO")
        {
            eqoPanel.SetActive(true);
            verelPanel.SetActive(false);

            eqoNameText.text = speaker;

            StopAllCoroutines();
            StartCoroutine(TypeSentence(eqoDialogueText, sentence));
        }
        else
        {
            verelPanel.SetActive(true);
            eqoPanel.SetActive(false);

            verelNameText.text = speaker;

            StopAllCoroutines();
            StartCoroutine(TypeSentence(verelDialogueText, sentence));
        }
    }

    IEnumerator TypeSentence(TextMeshProUGUI targetText, string sentence)
    {
        isTyping = true;

        targetText.text = "";

        foreach (char letter in sentence)
        {
            targetText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    void NextDialogue()
    {
        if (isTyping)
        {
            StopAllCoroutines();

            if (dialogues[currentIndex].speaker == "EQO")
            {
                eqoDialogueText.text = dialogues[currentIndex].sentence;
            }
            else
            {
                verelDialogueText.text = dialogues[currentIndex].sentence;
            }

            isTyping = false;
            return;
        }

        currentIndex++;

        if (currentIndex < dialogues.Length)
        {
            ShowDialogue();
        }
        else
        {
            EndCutscene();
        }
    }

    void EndCutscene()
    {
        SceneManager.LoadScene(nextScene);
    }
}