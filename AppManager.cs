using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AppManager : MonoBehaviour
{
    [HideInInspector] public static AppManager Instance;
    public string phone_number;
    public GameObject[] screens;
    public GameObject logoActivity;
    public int activeScreenIndex;
    public List<int> prevScreenIndex;
    public string my_push_token;
    public Texture2D user_maptex;
    public int codeLength = 4;
    public string access_token;
    public List<F> Files = new List<F>();
    public List<string> _mediaPaths = new List<string>();
    public List<string> _noteMediaPaths = new List<string>();
    public UserInfo userInfo = new UserInfo();
    public AnswerPlace places = new AnswerPlace();
    public AnswerValues allowed_values = new AnswerValues();
    public GameObject media;
    public Search search;
    public FindID fID = new FindID();
    public string orgName;
    public string needID;
    public bool editIndex = false;
    public TMP_InputField codeText;
    public string choisePlace;
    public string choiseSurface;
    public string choiseBox;
    public string choisePipe;
    public string choiseChannel;

    public int PlaceN = 0;
    public int SurfaceN = 0;
    public int BoxN = 0;
    public int PipeN = 0;
    public int ChannelN = 0;
    public int NoteN = 0;
    public int MediaN = 0;

    public bool nPlace = false;
    public bool nSurface = false;
    public bool nBox = false;
    public bool nPipe = false;
    public bool nChannel = false;
    public bool nNote = false;
    public bool nNoteMedia = false;
    public bool openNoteMedia = false;

    public string path1;
    public string path2;

    public int way = 1;
    public int nFile;
    public string ID;
    public bool needDownload = false;
    public string original;
    public string mainValue;

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (activeScreenIndex == 0)
                {
                    Application.Quit();
                }
                else if (activeScreenIndex == 2)
                {
                    Application.Quit();
                }
                else
                {
                    BackButton();
                }
            }
        }
    }

    private void Start()
    {
        Input.location.Start();
    }

    private void Awake()
    {

        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }


    public void CheckCode(TMP_InputField field)
    {
        if (field.text.Length == codeLength)
        {
            userInfo.user.code = Convert.ToInt32(field.text);
            {
                WebHandler.Instance.LoginWraper((repl) =>
                {
                    if (repl.Contains("wrong") || repl.Contains("Cannot connect to destination host"))
                    {
                        codeText.text = "";
                    }
                    else
                    {
                        JsonUtility.FromJsonOverwrite(repl, userInfo);
                        string s = PlayerPrefs.GetString(phone_number);
                        Debug.Log("phoneNumber " + s);
                        if (!string.IsNullOrEmpty(s))
                        {
                            JsonUtility.FromJsonOverwrite(s, places);
                        }
                        SaveUserInfo(userInfo);
                        userInfo.user.firstTime = false;
                        PlayerPrefs.SetString("access_token", userInfo.access_token);
                        PlayerPrefs.SetString("code", field.text);
                        SwitchScreen(2);
                    }
                });
            }
        }
    }

    public void SwitchScreen(int nextScreenIndex)
    {
        if (activeScreenIndex < nextScreenIndex)
        {
            //play left to right animation
        }
        else if (activeScreenIndex < nextScreenIndex)
        {
            //play right to left animation
        }
        else
        {
            //do nothing
        }
        //yield return transferAnimationLength;

        if (nextScreenIndex == -1)
        {
            activeScreenIndex = prevScreenIndex[prevScreenIndex.Count - 1];
            prevScreenIndex.RemoveAt(prevScreenIndex.Count - 1);
        }
        else
        {
            prevScreenIndex.Add(activeScreenIndex);
            activeScreenIndex = nextScreenIndex;
        }
        for (int i = 0; i < screens.Length; i++)
        {
            if (i != activeScreenIndex)
                screens[i].SetActive(false);
        }
        screens[activeScreenIndex].SetActive(true);
    }

    public void SaveUserInfo(UserInfo userInfo)
    {
        var toSave = JsonUtility.ToJson(userInfo);
        PlayerPrefs.SetString("userInfo", toSave);
    }

    public void LoadUser()
    {
        var toUnravel = PlayerPrefs.GetString("userInfo");
        userInfo = JsonUtility.FromJson<UserInfo>(toUnravel);
    }

    public void BackButton()
    {
        SwitchScreen(-1);
    }
}
