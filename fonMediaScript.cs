using SA.CrossPlatform.App;
using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class fonMediaScript : MonoBehaviour
{
    public MediaEdit mediaEdit;
    public Action<Files> callback;
    public int editIndex;
    public bool enableCompited = false;
    public GameObject recordBTN;
    public TMP_InputField parent;
    public TMP_Text p;
    public LoopScrollRect scroll;
    public GameObject scrolll;
    public string _path;
    public bool voiceRecorded;
    [SerializeField] public string audioPath;
    [SerializeField] public GameObject recordedMessage;
    private Coroutine timer;
    public Button SendMessageButton;
    public Button SendVoiceMessageButton;
    public GameObject BlockChat;
    [SerializeField] public TMP_Text time;
    [SerializeField] public GameObject ShowRecordProcess;
    public AudioSource audio;
    public AudioClip audioMessage;
    public static fonMediaScript instance;
    public GameObject record;
    public int count = 0;
    [SerializeField] private AudioClip recordedClip;
    public TMP_Text timerr;
    public int s;
    public int b;
    public int pp;
    public int c;

    /// <param name="audS"></param>
    /// <param name="deviceName"></param>
    /// 

    private void Start()
    {
        instance = this;
    }

    public void OnEnable()
    {
        var AM = AppManager.Instance;
        SendVoiceMessageButton.onClick.AddListener(() => RecordVoice());
        parent.text = AM.path2;
        scroll.ClearCells();
        if (AM.openNoteMedia)
        {
            ViewNoteMediaPref();
        }
        else
        {
            ViewPref();
        }
    }

    public void ChooseImageFromGallery()
    {
        var camera = UM_Application.CameraService;
        var AM = AppManager.Instance;
        int maxThumbnailSize = 4096;
        camera.TakePicture(maxThumbnailSize, (result) =>
        {
            if (result.IsSucceeded)
            {
                UM_Media mdeia = result.Media;
                Texture2D image = mdeia.Thumbnail;
                _path = mdeia.Path;
                if (AM.openNoteMedia)
                {
                    AppManager.Instance._noteMediaPaths.Add(mdeia.Path);
                }
                else
                {
                    AppManager.Instance._mediaPaths.Add(mdeia.Path);
                }
                Debug.Log("Thumbnail width: " + image.width + " / height: " + image.height);
                Debug.Log("mdeia.Type: " + mdeia.Type);
                Debug.Log("mdeia.Path: " + mdeia.Path);
                AddFile();
            }
            else
            {
                Debug.Log("failed to take a picture: " + result.Error.FullMessage);
            }
        });
    }// выбрать фото


    public void Videos()
    {
        var camera = UM_Application.CameraService;
        var AM = AppManager.Instance;
        int maxThumbnailSize = 1024;
        camera.TakeVideo(maxThumbnailSize, (result) =>
        {
            if (result.IsSucceeded)
            {
                UM_Media media = result.Media;
                _path = media.Path;
                if (AM.openNoteMedia)
                {
                    AppManager.Instance._noteMediaPaths.Add(media.Path);
                }
                else
                {
                    AppManager.Instance._mediaPaths.Add(media.Path);
                }
                Texture2D image = media.Thumbnail;
                Debug.Log("Thumbnail width: " + image.width + " / height: " + image.height);
                Debug.Log("mdeia.Type: " + media.Type);
                Debug.Log("mdeia.Path: " + media.Path);
                AddFile();
            }
            else
            {
                Debug.Log("failed to take a picture: " + result.Error.FullMessage);
            }
        });
    }//выбрать видео


    public void RecordVoice()
    {
        var AM = AppManager.Instance;
        count++;
        Debug.Log("Recording voice");
        audio.clip = Microphone.Start(Microphone.devices[0], false, 30, 44100);
        voiceRecorded = false;
        SendVoiceMessageButton.transform
            .GetChild(0)
            .GetComponent<Image>()
            .sprite = Resources.Load<Sprite>("AudioMessage/StopRecording");
        timer = StartCoroutine(ShowTimer());
        SendVoiceMessageButton.onClick.RemoveAllListeners();
        SendVoiceMessageButton.onClick.AddListener(() =>
        {
            EndRecording(audio, Microphone.devices[0]);
            audioMessage = audio.clip;
            voiceRecorded = true;
            var RecordedMessage = Instantiate(recordedMessage, recordBTN.transform);
            RecordedMessage.SetActive(true);
            StopCoroutine(timer);
            RecordedMessage.GetComponent<AudioMessage>().isOnline = false;
            Debug.Log("Saving");
            byte[] test = WavUtility.FromAudioClip(audioMessage);
            audioPath = System.String.Format(@"{0}/file{1}.wav", Application.persistentDataPath, PlayerPrefs.GetInt("Photo"));
            _path = audioPath;
            if (AM.openNoteMedia)
            {
                AppManager.Instance._noteMediaPaths.Add(audioPath);
            }
            else
            {
                AppManager.Instance._mediaPaths.Add(audioPath);
            }
            PlayerPrefs.SetInt("Photo", PlayerPrefs.GetInt("Photo") + 1);
            PlayerPrefs.Save();
            Debug.Log(audioPath);
            FileStream fileStream = new FileStream(audioPath, FileMode.Create, FileAccess.ReadWrite);
            fileStream.Write(test, 0, test.Length - 1);
            fileStream.Close();
            SendVoiceMessageButton.onClick.RemoveAllListeners();
            SendVoiceMessageButton.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("microphone");
            SendVoiceMessageButton.onClick.AddListener(() =>
            {
                RecordVoice();
            });
            Debug.Log(count);
            AddFile();
        });
    } // запись звука


    public byte[] ReadFile(string _path)
    {
        byte[] bytes = File.ReadAllBytes(_path);
        return bytes;
    }

    void EndRecording(AudioSource audS, string deviceName)
    {
        recordedClip = audS.clip;
        if (deviceName != null)
        {
            var position = Microphone.GetPosition(deviceName);
            var soundData = new float[recordedClip.samples * recordedClip.channels];
            Debug.Log("recordedClip.samples " + recordedClip.samples);
            Debug.Log("recordedClip.channels " + recordedClip.channels);
            recordedClip.GetData(soundData, 0);
            var newData = new float[position * recordedClip.channels];
            Debug.Log("position " + position);
            Debug.Log("recordedClip.channels " + recordedClip.channels);
            for (int i = 0; i < newData.Length; i++)
            {
                newData[i] = soundData[i];
            }
            var newClip = AudioClip.Create(recordedClip.name, position, recordedClip.channels, recordedClip.frequency, false);
            newClip.SetData(newData, 0);
            AudioClip.Destroy(recordedClip);
            audS.clip = newClip;
            timerr.text = "Начать запись";
        }
    } // запись звука

    private IEnumerator ShowTimer()
    {
        float time = 0.0f;
        while (!voiceRecorded)
        {
            yield return new WaitForSeconds(0.1f);
            time += 0.1f;
            this.time.text = System.String.Format("{0:0.0} с", time);
        }
        Debug.Log("Timer stoped");
        yield return this.time.text = "";
        yield return null;
    }

    public async void GetLink(MediaEdit _mediaEdit, int _index, Action<Files> _callback)
    {
        await new WaitUntil(() =>
        {
            return enableCompited;
        });
        var AM = AppManager.Instance;
        Debug.Log("recieved index = " + _index);
        editIndex = _index;
        callback = _callback;
        mediaEdit = _mediaEdit;
    }//ссылки для префабов

    public void SurfaceAddFile(string path)
    {
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        AM.nFile = 1;
        try
        {
            if (!AM.nNoteMedia)
            {
                P[AM.PlaceN].surface[AM.SurfaceN].files.Add(new Files(path));
                Debug.Log("_count " + P[AM.PlaceN].surface[AM.SurfaceN].files.Count);
                Debug.Log("_path333 " + P[AM.PlaceN].surface[AM.SurfaceN].files[0].path);
                ViewPref();
            }
            else
            {
                P[AM.PlaceN].surface[AM.SurfaceN].note[AM.NoteN].files.Add(new Files(path));
                ViewNoteMediaPref();
            }
        }
        catch { }
    }

    public void BoxAddFile(string path)
    {
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        AM.nFile = 2;
        try
        {
            if (!AM.nNoteMedia)
            {
                P[AM.PlaceN].box[AM.BoxN].files.Add(new Files(path));
                ViewPref();
            }
            else
            {
                P[AM.PlaceN].box[AM.BoxN].note[AM.NoteN].files.Add(new Files(path));
                ViewNoteMediaPref();
            }
        }
        catch { }
    }

    public void PipeAddFile(string path)
    {
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        AM.nFile = 3;
        try
        {
            if (!AM.nNoteMedia)
            {
                P[AM.PlaceN].pipe[AM.PipeN].files.Add(new Files(path));
                ViewPref();
            }
            else
            {
                P[AM.PlaceN].pipe[AM.PipeN].note[AM.NoteN].files.Add(new Files(path));
                ViewNoteMediaPref();
            }
        }
        catch { }
    }

    public void ChannelAddFile(string path)
    {
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        AM.nFile = 4;
        try
        {
            if (!AM.nNoteMedia)
            {
                P[AM.PlaceN].channel[AM.ChannelN].files.Add(new Files(path));
                ViewPref();
            }
            else
            {
                P[AM.PlaceN].channel[AM.ChannelN].note[AM.NoteN].files.Add(new Files(path));
                ViewNoteMediaPref();
            }
        }
        catch { }
    }

    public void AddFile()
    {
        var AM = AppManager.Instance;
        switch (AM.way)
        {
            case 1: Debug.Log("_path222 " + _path); SurfaceAddFile(_path); break;
            case 2: BoxAddFile(_path); break;
            case 3: PipeAddFile(_path); break;
            case 4: ChannelAddFile(_path); break;
        }
        scroll.RefillCells();
    }//файл

    public void ViewPref()
    {
        var AM = AppManager.Instance;
        switch (AM.way)
        {
            case 1: ViewSurfacePref(); break;
            case 2: ViewBoxPref(); break;
            case 3: ViewPipePref(); break;
            case 4: ViewChannelPref(); break;
        }
    }

    public void ViewNoteMediaPref()
    {
        var AM = AppManager.Instance;
        switch (AM.way)
        {
            case 1: ViewNoteMediaSurfacePref(); break;
            case 2: ViewNoteMediaBoxPref(); break;
            case 3: ViewNoteMediaPipePref(); break;
            case 4: ViewNoteMediaChannelPref(); break;
        }
    }

    public void ViewSurfacePref()
    {
        try
        {
            var AM = AppManager.Instance;
            var P = AppManager.Instance.places.places;
            if (P[AM.PlaceN].surface[AM.SurfaceN].files.Count > 0)
            {
                scroll.totalCount = P[AM.PlaceN].surface[AM.SurfaceN].files.Count;
                scroll.RefillCells();
            }     
        }
        catch { }
    }

    public void ViewNoteMediaSurfacePref()
    {
        try
        {
            var AM = AppManager.Instance;
            var P = AppManager.Instance.places.places;
            if (P[AM.PlaceN].surface[AM.SurfaceN].note[AM.NoteN].files.Count > 0)
            {
                scroll.totalCount = P[AM.PlaceN].surface[AM.SurfaceN].note[AM.NoteN].files.Count;
                scroll.RefillCells();
            }
        }
        catch { }
    }

    public void ViewBoxPref()
    {
        try
        {
            var AM = AppManager.Instance;
            var P = AppManager.Instance.places.places;
            if (P[AM.PlaceN].box[AM.BoxN].files.Count > 0)
            {
                scroll.totalCount = P[AM.PlaceN].box[AM.BoxN].files.Count;
                scroll.RefillCells();
            }
        }
        catch { }
    }

    public void ViewNoteMediaBoxPref()
    {
        try
        {
            var AM = AppManager.Instance;
            var P = AppManager.Instance.places.places;
            if (P[AM.PlaceN].box[AM.BoxN].note[AM.NoteN].files.Count > 0)
            {
                scroll.totalCount = P[AM.PlaceN].box[AM.BoxN].note[AM.NoteN].files.Count;
                scroll.RefillCells();
            }
        }
        catch { }
    }

    public void ViewPipePref()
    {
        try
        {
            var AM = AppManager.Instance;
            var P = AppManager.Instance.places.places;
            if (P[AM.PlaceN].pipe[AM.PipeN].files.Count > 0)
            {
                scroll.totalCount = P[AM.PlaceN].pipe[AM.PipeN].files.Count;
                scroll.RefillCells();
            }
        }
        catch { }
    }

    public void ViewNoteMediaPipePref()
    {
        try
        {
            var AM = AppManager.Instance;
            var P = AppManager.Instance.places.places;
            if (P[AM.PlaceN].pipe[AM.PipeN].note[AM.NoteN].files.Count > 0)
            {
                scroll.totalCount = P[AM.PlaceN].pipe[AM.PipeN].note[AM.NoteN].files.Count;
                scroll.RefillCells();
            }
        }
        catch { }
    }

    public void ViewChannelPref()
    {
        try
        {
            var AM = AppManager.Instance;
            var P = AppManager.Instance.places.places;
            if (P[AM.PlaceN].channel[AM.ChannelN].files.Count > 0)
            {
                scroll.totalCount = P[AM.PlaceN].channel[AM.ChannelN].files.Count;
                scroll.RefillCells();
            }
        }
        catch { }
    }

    public void ViewNoteMediaChannelPref()
    {
        try
        {
            var AM = AppManager.Instance;
            var P = AppManager.Instance.places.places;
            if (P[AM.PlaceN].channel[AM.ChannelN].note[AM.NoteN].files.Count > 0)
            {
                scroll.totalCount = P[AM.PlaceN].channel[AM.ChannelN].note[AM.NoteN].files.Count;
                scroll.RefillCells();
            }
        }
        catch { }
    }

    public void SaveMedia()
    {
        var AM = AppManager.Instance;
        if (AM.openNoteMedia)
        {
            UpdatePlayerPrefs();
            AM.openNoteMedia = false;
            AM.media.SetActive(false);
        }
        else
        {
            UpdatePlayerPrefs();
            AM.BackButton();
        }
    }

    public void UpdatePlayerPrefs()
    {
        var AM = AppManager.Instance;
        var place = AM.places;
        string data = JsonUtility.ToJson(place);
        PlayerPrefs.SetString(AM.phone_number, data);
        Debug.Log("player prefs saved");
    }

    public void ChooseImageFromNativeGallery()
    {
        PickImage(2000);
    }

    private void PickImage(int maxSize)
    {
        var AM = AppManager.Instance;
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (path != null)
            {
                this._path = path;
                Debug.Log("_path111 " + _path);
                if (AM.openNoteMedia)
                {
                    AppManager.Instance._noteMediaPaths.Add(path);
                }
                else
                {
                    AppManager.Instance._mediaPaths.Add(path);
                }
                // Create Texture from selected image
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                AddFile();
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }
                //Destroy(texture, 5f);
            }
        }, "Выберите изображения для объявления", "image/png", maxSize);
        Debug.Log("Permission result: " + permission); 
    }
}
