using UnityEngine;
using UnityEngine.UI;

public class EraseButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(Click);
    }

    private void Click()
    {
        EventBus.InvokeEraseSelected();
    }
}