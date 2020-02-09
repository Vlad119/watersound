using TMPro;
using UnityEngine;

public class FonRegistrationScript : MonoBehaviour
{
    public TMP_InputField inputPhone;
    public TMP_InputField inputIP;

    private void Start()
    {
        var AM = AppManager.Instance;
        string s = PlayerPrefs.GetString("phoneNumber");
        AM.userInfo.user.phone = s;
        if (s != "")
        {
            AM.SwitchScreen(1);
            AM.phone_number = s;
        }
    }

    public async void SendPass()
    {
        var AM = AppManager.Instance;
        AM.userInfo.user.phone = inputPhone.text;
        AM.phone_number = inputPhone.text;
        if (inputIP.text != "" && inputIP.text.Contains("http") && inputPhone.text!="")
        {
            WebHandler.Instance.needIP = inputIP.text;
            await WebHandler.Instance.RegisterWraper((repl) =>
            {
                if (string.IsNullOrEmpty(repl))
                {
                    Debug.Log("check phone or IP");
                }
                else
                {
                    Debug.Log("No errors");
                    PlayerPrefs.SetString("phoneNumber", inputPhone.text);
                    PlayerPrefs.SetString("IP", inputIP.text);
                    AM.SwitchScreen(1);
                }
            });
        }
    }
}
