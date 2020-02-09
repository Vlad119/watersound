using System.Collections;
using UnityEngine;

public class AudioMessage : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    public string url;
    [SerializeField] private UnityEngine.UI.Image icon;
    public bool isOnline;
    [SerializeField] private UnityEngine.UI.Text time;
    private float timeSeconds;
    private Coroutine timer;
    public float length;
    public UnityEngine.UI.Image status;

    private void OnEnable()
    {
        GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(() => Play());
        timeSeconds = 0;
        Debug.Log("Audio message recived!");
        StartCoroutine(LoadClipFromSource());
    }

    private IEnumerator ShowTimer()
    {
        while (timeSeconds <= length) 
        {
            yield return new WaitForSeconds(0.1f);
            timeSeconds += 0.1f;
            this.time.text = System.String.Format("{0:0.0} с", timeSeconds);
        }
        yield return icon.sprite = Resources.Load<Sprite>("AudioMessage/playIcon");
        yield return  this.time.text = "";
        GetComponentInChildren<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(() => Play());
        timeSeconds = 0;
    }

    private IEnumerator LoadClipFromSource()
    {
        yield return new WaitForSeconds(1.0f);
        if (isOnline)
        {
            Debug.Log("Clip is Online");
            WWW request = new WWW(url);
            yield return request;
            source.clip = request.GetAudioClip(false, false, AudioType.WAV);
        }
        else
        {
            Debug.Log("Clip is on the device");
            yield return new WaitForSeconds(1.0f);
            source.clip = fonMediaScript.instance.audioMessage;
        }
    }

    private async void Play()
    {
        while (source.clip == null)
        {
            await new WaitForSeconds(0.01f);
        }
        length = source.clip.length;
        Debug.Log("Playing audio!");
        icon.sprite = Resources.Load<Sprite>("AudioMessage/StopIcon");
        source.Play();
        timer = StartCoroutine(ShowTimer());
        GetComponentInChildren<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(() => Stop());
    }

    private void Stop()
    {
        Debug.Log("Stoping audio!");
        icon.sprite = Resources.Load<Sprite>("AudioMessage/playIcon");
        source.Stop();
        StopCoroutine(timer);
        timeSeconds = 0;
        this.time.text = "";
        GetComponentInChildren<UnityEngine.UI.Button>().onClick.RemoveAllListeners();
        GetComponentInChildren<UnityEngine.UI.Button>().onClick.AddListener(() => Play());
    }

    public void DeleteMessage()
    {
        Destroy(fonMediaScript.instance.audioMessage);
        Destroy(fonMediaScript.instance.recordBTN.transform.Find("RecordedMessage(Clone)").gameObject);
        fonMediaScript.instance.voiceRecorded = false;
    }
}
