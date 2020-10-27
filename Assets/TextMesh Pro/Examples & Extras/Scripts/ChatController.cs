using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TextMesh_Pro.Scripts
{
    public class ChatController : MonoBehaviour {


        public TMP_InputField tmpChatInput;

        public TMP_Text tmpChatOutput;

        public Scrollbar chatScrollbar;

        void OnEnable()
        {
            tmpChatInput.onSubmit.AddListener(AddToChatOutput);

        }

        void OnDisable()
        {
            tmpChatInput.onSubmit.RemoveListener(AddToChatOutput);

        }


        void AddToChatOutput(string newText)
        {
            // Clear Input Field
            tmpChatInput.text = string.Empty;

            var timeNow = System.DateTime.Now;

            tmpChatOutput.text += "[<#FFFF80>" + timeNow.Hour.ToString("d2") + ":" + timeNow.Minute.ToString("d2") + ":" + timeNow.Second.ToString("d2") + "</color>] " + newText + "\n";

            tmpChatInput.ActivateInputField();

            // Set the scrollbar to the bottom when next text is submitted.
            chatScrollbar.value = 0;

        }

    }
}
