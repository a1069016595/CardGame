using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public delegate void CreateRoomResult(string mes);
public class CreateRoomPanel : MonoBehaviour
{
    public InputField roomNameInputField;
    public Button applyButton;
    public Button closeButton;

    CreateRoomResult er;

    public void Init()
    {
        this.gameObject.SetActive(false);
        roomNameInputField = transform.FindChild("RoomNameInputField").GetComponent<InputField>();
        applyButton = transform.FindChild("ApplyButton").GetComponent<Button>();
        closeButton = transform.FindChild("CloseButton").GetComponent<Button>();

        applyButton.onClick.AddListener(OnApplyButton);
        closeButton.onClick.AddListener(OnCloseButton);
    }

    public void Show(CreateRoomResult e)
    {
        roomNameInputField.text = "ff";
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);

            er = e;
        }
    }

    void Hide()
    {
        gameObject.SetActive(false);
    }

    void OnApplyButton()
    {
        string mes = roomNameInputField.text;
        if (mes == "")
        {
            return;
        }
        Hide();
        if (er != null)
            er(mes);
    }

    void OnCloseButton()
    {
        roomNameInputField.text = "";
        Hide();
    }
}
