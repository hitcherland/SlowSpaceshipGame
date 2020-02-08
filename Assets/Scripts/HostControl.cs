
using UnityEngine;
using UnityEngine.UI;

public class HostControl : MonoBehaviour
{
    public Server server;
    public InputField inputField;

    private bool done = false;

    public void Update()
    {
        if (!done && server.publicEndPoint != null)
        {
            done = true;
            inputField.text = server.publicEndPoint.ToString();
        }
    }

    public void CopyToClipboard()
    {
        TextEditor textEditor = new TextEditor();
        textEditor.text = server.publicEndPoint.ToString();
        textEditor.SelectAll();
        textEditor.Copy();
    }
}
