using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class WebHandler : MonoBehaviour
{
    public static WebHandler Instance;
    public delegate UnityWebRequest RequestCall();
    public string servAddress;
    public string registerEndpoint;
    public string loginEndpoint;
    public string loadPlacesEndpoint;
    public string loadDataEndpoint;
    public string getIdEndpoint;
    public string userEndpoint;
    public string updateUserEndpoint;
    public string sendfileEndpoint;
    public string sendDataEndpoint;
    public string checkInternet;
    public List<string> img_cache_string;
    public List<Texture2D> img_cache_tex;
    public GameObject loadingScreen;
    public GameObject noInternet;
    public TMP_InputField ip;
    public string needIP;

    private void Awake()
    {
        SingletonImplementation();
        string savedIP = PlayerPrefs.GetString("IP");
        if (savedIP != "")
        {
            needIP = savedIP;
        }
        // loadingScreen.SetActive(false);
    }

    #region Requests
    private async Task PostJson(string url, string dataString, UnityAction<string> DoIfSuccess = null, bool addTokenHeader = false)
    {
        var endUrl = needIP + url;
        var req = await IRequestSend(() =>
        {
            var request = new UnityWebRequest(endUrl, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(dataString);
            var uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.uploadHandler = uploadHandler;
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            if (!addTokenHeader)
                request.SetRequestHeader("content-type", "application/json");
            else
            {
                request.SetRequestHeader("content-type", "application/json");
                request.SetRequestHeader("Token", AppManager.Instance.userInfo.access_token);
            }
            print(request.GetRequestHeader("content-type") + "   " + (addTokenHeader ? request.GetRequestHeader("token") : "") + "    " + request.url + "    " + Encoding.UTF8.GetString(request.uploadHandler.data));
            return request;
        });
        Debug.Log("All OK");
        Debug.Log("Status Code: " + req.responseCode);
        DoIfSuccess?.Invoke(req.downloadHandler.text);
    }

    private async Task PostMultipartAsync(string url, string textSctring, byte[] image = null, UnityAction<string> DoIfSuccess = null, string tpe = "")
    {
        string endUrl = needIP + url;
        var req = await IRequestSend(() =>
        {
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            if (image != null)
            {
                print("sending image");
                formData.Add(new MultipartFormFileSection("file", image, "name.jpg", tpe));
                formData.Add(new MultipartFormDataSection("text", textSctring));
            }
            else
            {
                print("sending text");
                formData.Add(new MultipartFormDataSection("text", textSctring));
            }
            UnityWebRequest request = UnityWebRequest.Post(endUrl, formData);
            request.SetRequestHeader("Token", AppManager.Instance.userInfo.access_token);
            return request;
        });
        Debug.Log("Request sent");
        Debug.Log("Status code: " + req.responseCode);
        DoIfSuccess?.Invoke(req.downloadHandler.text);
    }

    private IEnumerator GetRequest(string url, UnityAction<string> DoIfSuccess = null, bool addToken = false, string getParameters = null)
    {
        var endUrl = needIP + url + (getParameters == null ? "" : getParameters);///////////
        var request = new UnityWebRequest(endUrl, "GET");
        request.downloadHandler = new DownloadHandlerBuffer();
        if (addToken)
            request.SetRequestHeader("token", AppManager.Instance.userInfo.access_token);

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.LogWarning("Error " + request.responseCode + " " + request.error);
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Request sent");
            Debug.Log("Status code: " + request.responseCode);
            DoIfSuccess?.Invoke(request.downloadHandler.text);
        }
        print(endUrl + "    " + AppManager.Instance.userInfo.access_token + "   " + request.downloadHandler.text);
    }

    private async Task LoadImage(string url, UnityAction<Texture2D> DoIfSuccess)
    {
        if (!(url.Contains(".jpg") || url.Contains(".png")))
        {
            DoIfSuccess(Resources.Load<Texture2D>("no-image"));
        }
        else
        {
            int i = 0;
            bool find = false;
            foreach (string s in img_cache_string)
            {
                if (s == url) { find = true; break; }
                i++;
            }
            if (find)
            {
                DoIfSuccess(img_cache_tex[i]);
            }
            else
            {
                var req = await IRequestSend(() =>
                {
                    UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
                    return request;
                });
                Debug.Log("Loading image");
                var tex = DownloadHandlerTexture.GetContent(req);
                tex.Apply();
                img_cache_string.Add(url);
                img_cache_tex.Add(tex);
                DoIfSuccess(tex);
            }
        }
    }

    public async Task<UnityWebRequest> IRequestSend(RequestCall data)
    {
        UnityWebRequest request;//= data();
                                //do
                                //{
        request = data();
        //   loadingScreen.SetActive(true);
        await request.SendWebRequest();
        //    loadingScreen.SetActive(false);
        Debug.Log(request.isNetworkError);
        //if (request.responseCode==403)
        //{
        //    AppManager.Instance.SwitchScreen(0);
        //    //AppManager.Instance.bottomBar.SetActive(false);
        //    //PlayerPrefs.DeleteAll();
        //   // AppManager.Instance.userInfo = new UserInfo();
        //    SceneManager.LoadScene(0);
        //    //break;
        //}            
        if (request.error != null)
        {
            print(request.error);
            print(request.downloadHandler.text);
            // noInternet.SetActive(true);
            await new WaitWhile(() => { return noInternet.activeSelf == true; });
        }
        //  }
        //while (request.error != null);
        Debug.Log(request.downloadHandler.text);
        return request;
    }
    #endregion

    public async void UpdateUserWrapper(UnityAction<string> afterFinish, string data)
    {
        PostJson(updateUserEndpoint, data, afterFinish, true);
    }

    public async void PostSendFileWrapper(UnityAction<string> afterFininish, string data, byte[] file = null, string tpe = "")
    {
        await PostMultipartAsync(sendfileEndpoint, data, file, afterFininish, tpe);
    }

    public async Task LoadImageWrapper(UnityAction<Texture2D> afterFinish, string imgUrl)
    {
        await LoadImage(imgUrl, afterFinish);
    }


    public async Task RegisterWraper(UnityAction<string> afterFinish)
    {
        var userJSON = JsonUtility.ToJson(AppManager.Instance.userInfo.user);
        await PostJson(registerEndpoint, userJSON, afterFinish);
    }

    public void LoginWraper(UnityAction<string> afterFinish)
    {
        var userJSON = JsonUtility.ToJson(AppManager.Instance.userInfo.user);
        PostJson(loginEndpoint, userJSON, afterFinish);
    }

    public void SendDataWraper(UnityAction<string> afterFinish)
    {
        var userJSON = JsonUtility.ToJson(AppManager.Instance.places);
        PostJson(sendDataEndpoint, userJSON, afterFinish, true);
    }

    public void SendFileWraper(string data, byte[] files, UnityAction<string> afterfinish, string tpe = "")
    {
        var userJSON = JsonUtility.ToJson(data);
        PostMultipartAsync(sendfileEndpoint, userJSON, files, afterfinish, tpe);
        //Debug.Log(data);
    }



    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public async Task GetLoadPlacesWraper(UnityAction<string> afterfinish, string id)
    {
        await GetRequest(loadPlacesEndpoint, afterfinish, true, id);
    }

    public async Task GetDataWraper(UnityAction<string> afterfinish)
    {
        await GetRequest(loadDataEndpoint, afterfinish);
    }

    public async Task GetIDWraper(UnityAction<string> afterfinish)
    {
        await GetRequest(getIdEndpoint, afterfinish);
    }

    public async Task CheckInternet(UnityAction<string> afterfinish)
    {
        await GetRequest(checkInternet, afterfinish);
    }

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public async Task GetUserDataWraper(string param, UnityAction<string> afterFinish)
    {
        await GetRequest(userEndpoint + param, afterFinish);
    }

    private void SingletonImplementation()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
            Destroy(this);
    }
}
