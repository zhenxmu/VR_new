using UnityEngine;
using NodeCanvas.DialogueTrees;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject player;
    public DialogueTreeController dialogueTreeController;
    public bool isChating=false;
    public GameObject interactionUI;
    private void Start()
    {
         interactionUI.SetActive(false);
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
        dialogueTreeController.StartDialogue();
         interactionUI.SetActive(false);
         isChating=true;
    }
    public void StoptChat()
    {
         isChating=false;
    }

}