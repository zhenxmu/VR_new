using UnityEngine;
using NodeCanvas.DialogueTrees;

public class DialogueTrigger1 : MonoBehaviour
{
    public GameObject player;
    public DialogueTreeController dialogueTreeController;
    public bool isChating=false;
    public GameObject interactionUI;
    public GameObject ChatUI;
    private void Start()
    {
         interactionUI.SetActive(false);
         ChatUI.SetActive(false);
    }
    private void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 3f&&!isChating)
        {
            Debug.Log("可以触发对话了");
            interactionUI.SetActive(true);
        }else{
            interactionUI.SetActive(false);
        }
    }
    public void StartChat()
    {
        ChatUI.SetActive(true);
        dialogueTreeController.StartDialogue();
         interactionUI.SetActive(false);
         isChating=true;
    }
    public void StoptChat()
    {
         isChating=false;
         ChatUI.SetActive(false);
    }

}