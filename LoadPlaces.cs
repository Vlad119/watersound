using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadPlaces : MonoBehaviour
{
    public TMP_Dropdown Place;
    public TMP_Dropdown Surface;
    public TMP_Dropdown Box;
    public TMP_Dropdown Pipes;
    public TMP_Dropdown Channels;
    public TMP_Text comment;
    public int val;
    public Button PlaceEdit;
    public Button SurfaceEdit;
    public Button BoxEdit;
    public Button ChannelEdit;
    public Button PipeEdit;
    public Button AddPlace;
    public Button AddSurface;
    public Button AddBox;
    public Button AddPipes;
    public Button AddChannels;
    public Button DelPlace;
    public Button DelSurface;
    public Button DelBox;
    public Button DelPipe;
    public Button DelChannel;
    string fileType;


    async public void OnEnable()
    {
        Debug.Log("on_enable");
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        await new WaitUntil(() =>
        {
            return WebHandler.Instance && AM && (P.Count > 0);
        });
        AllControlsDisable(); // блокировка всех элементов управления
        UpdatePlaceDropdownOptions(); //обновить опции у Place dropdown 
        DelPlace.interactable = false;
        PlaceEdit.interactable = false;
        ChangeControllersStatus();
        if (P.Count > 0)
        {
            PlaceEdit.interactable = true;
            DelPlace.interactable = true;
            AddSurface.interactable = true;
            AddBox.interactable = true;
            AddPipes.interactable = true;
            AddChannels.interactable = true;
        }
        if (P.Count == 1)
        {
            SelectPlace(0);
        }
        else
        {
            SelectPlace(AM.PlaceN);
        }
    }


    private async void Start()
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        await WebHandler.Instance.GetDataWraper((repl) =>
        {
            AppManager.Instance.allowed_values = JsonUtility.FromJson<AnswerValues>(repl);
        }); //загрузка данных для dropdown у объектов осмотра
        await WebHandler.Instance.GetIDWraper((repl) =>
        {
            AppManager.Instance.search = JsonUtility.FromJson<Search>(repl);
        }); //загрузка данных для получения ID синхронизации
        JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(AM.phone_number), AM.places);
        if (AM.places.places.Count > 0)
        {
            Debug.Log("player prefs succesfully loaded");
        }

        if (P.Count != 0)
        {
            SelectPlace(0);
        }
        else
        {
            AllControlsDisable();
            MakeNewPlace();
        }
        try
        {
            AM.choiseSurface = P[AM.PlaceN].surface[0].name;
            AM.choiseBox = P[AM.BoxN].box[0].name;
            AM.choisePipe = P[AM.PipeN].pipe[0].name;
            AM.choiseChannel = P[AM.ChannelN].channel[0].name;
        }
        catch { }
        DelPlace.interactable = false;
        PlaceEdit.interactable = false;
        ChangeControllersStatus();
        if (P.Count > 0)
        {
            PlaceEdit.interactable = true;
            DelPlace.interactable = true;
            AddSurface.interactable = true;
            AddBox.interactable = true;
            AddPipes.interactable = true;
            AddChannels.interactable = true;
        }
        if (P.Count == 1)
        {
            SelectPlace(0);
        }
        else
        {
            SelectPlace(AM.PlaceN);
        }
        if (P.Count == 0)
        {
            PlaceEdit.interactable = false;
            DelPlace.interactable = false;
            AddSurface.interactable = false;
            AddBox.interactable = false;
            AddPipes.interactable = false;
            AddChannels.interactable = false;
        }
    }

    public void InitiateSelection()
    {
        try
        {
            var P = AppManager.Instance.places.places;
            var AM = AppManager.Instance;
            Place.captionText.text = P[AM.PlaceN].name;
            Place.value = Convert.ToInt32(P[AM.PlaceN]);
            SelectSurface(AM.SurfaceN);
            SelectBox(AM.BoxN);
            SelectPipe(AM.PipeN);
            SelectChannel(AM.ChannelN);
        }
        catch { }
    }


    public void SelectPlace(int i)
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        if (P.Count != 0)
        {
            AM.PlaceN = i;
            if (P[AM.PlaceN].comment == "")
            {
                comment.text = "Комментарий";
            }
            else
            {
                comment.text = P[AM.PlaceN].comment;
            }
            AllDropdownsOptionsClear(); // очистка вариантов выбора выпадающих списков поверхностей, камер, труб и каналов
            SurfaceCheckZero();
            BoxCheckZero();
            PipeCheckZero();
            ChannelCheckZero();
            InitiateSelection();
            AM.choisePlace = P[i].name;
        }
    }


    public void ChangeControllersStatus() // блокировка или разблокировка элементов управления
    {
        Debug.Log("смена параметров interactable");
        var AM = AppManager.Instance;
        var P = AM.places.places;
        AllControlsDisable();
        PlaceOn();
        PlaceEdit.interactable = true;
        DelPlace.interactable = true;
        try
        {
            if (P[AM.PlaceN].way_type == "неизвестно"
            || P[AM.PlaceN].way_type == "Канальная"
            || P[AM.PlaceN].way_type == "0"
            || P[AM.PlaceN].way_type == "1")
            {
                Debug.Log("Включаю все элементы управления");
                AllOnline();
            }
            if (P[AM.PlaceN].way_type == "Бесканальная"
                || P[AM.PlaceN].way_type == "Гильза/футляр"
                || P[AM.PlaceN].way_type == "Надземная на высоких опорах"
                || P[AM.PlaceN].way_type == "Надземная на низких опорах"
                || P[AM.PlaceN].way_type == "2"
                || P[AM.PlaceN].way_type == "5"
                || P[AM.PlaceN].way_type == "7"
                || P[AM.PlaceN].way_type == "8")
            {
                Debug.Log("Включение элементов поверхности");
                SurfaceOn();
                SurfaceEdit.interactable = true;
                DelSurface.interactable = true;
            }
            if (P[AM.PlaceN].way_type == "Подвал"
            || P[AM.PlaceN].way_type == "Камера"
            || P[AM.PlaceN].way_type == "12"
            || P[AM.PlaceN].way_type == "13")
            {
                Debug.Log("Включение элементов камеры");
                BoxOn();
                BoxEdit.interactable = true;
                DelBox.interactable = true;
            }
        }
        catch { }
    }



    public void SurfaceCheckZero()
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        if (P[AM.PlaceN].surface.Count > 0)
        {
            for (int sval = 0; sval < P[AM.PlaceN].surface.Count; sval++)
            {
                Surface.options.Add(new TMP_Dropdown.OptionData(P[AM.PlaceN].surface[sval].name));
            }
            Surface.RefreshShownValue();
            SurfaceOn(); // включение элементов управления поверхности
            SurfaceEdit.interactable = true;
            DelSurface.interactable = true;
        }
        else
        {
            MakeNewSurface();
        }
    }



    public void BoxCheckZero()
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        if (P[AM.PlaceN].box.Count > 0)
        {
            for (int bval = 0; bval < P[AM.PlaceN].box.Count; bval++)
            {
                Box.options.Add(new TMP_Dropdown.OptionData(P[AM.PlaceN].box[bval].name));
                Box.RefreshShownValue();
            }
            BoxOn(); //включение элементов управления камеры
            BoxEdit.interactable = true;
            DelBox.interactable = true;
        }
        else
        {
            MakeNewBox();
        }
    }

    public void ChannelCheckZero()
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        if (P[AM.PlaceN].channel.Count > 0)
        {
            for (int cval = 0; cval < P[AM.PlaceN].channel.Count; cval++)
            {
                Channels.options.Add(new TMP_Dropdown.OptionData(P[AM.PlaceN].channel[cval].name));
                Channels.RefreshShownValue();
            }
            ChannelOn();
            ChannelEdit.interactable = true;
            DelChannel.interactable = true;
        }
        else
        {
            MakeNewChannel();
        }
    }

    public void PipeCheckZero()
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        if (P[AM.PlaceN].pipe.Count > 0)
        {
            for (int pval = 0; pval < P[AM.PlaceN].pipe.Count; pval++)
            {
                Pipes.options.Add(new TMP_Dropdown.OptionData(P[AM.PlaceN].pipe[pval].name));
                Pipes.RefreshShownValue();
            }
            PipeOn(); //включение элементов управления трубы
            PipeEdit.interactable = true;
            DelPipe.interactable = true;
        }
        else
        {
            MakeNewPipe();
        }
    }

    public void AllDropdownsOptionsClear()
    {
        Surface.options.Clear();
        Box.options.Clear();
        Channels.options.Clear();
        Pipes.options.Clear();
    } //очистка опций в dropdown

    #region Select
    public void SelectSurface(int j)
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        try
        {
            AM.choiseSurface = P[AM.PlaceN].surface[j].name;
            AM.SurfaceN = j;
            Surface.RefreshShownValue();
        }
        catch { }
    }

    public void SelectBox(int j)
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        try
        {
            Box.RefreshShownValue();
            AM.choiseBox = P[AM.PlaceN].box[j].name;
            AM.BoxN = j;
        }
        catch { }
    }

    public void SelectPipe(int j)
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        Pipes.RefreshShownValue();
        AM.choisePipe = P[AM.PlaceN].pipe[j].name;
        AM.PipeN = j;
    }

    public void SelectChannel(int j)
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        try
        {
            Channels.RefreshShownValue();
            AM.choiseChannel = P[AM.PlaceN].channel[j].name;
            AM.ChannelN = j;
        }
        catch { }
    }
    #endregion

    #region Add
    public void AddNewPlace()
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        Debug.Log("создание нового участка");
        P.Add(new Place(null, null, null, null, null));
        AM.PlaceN = P.Count - 1;
    }

    public void AddNewSurfase()
    {
        AppManager.Instance.nSurface = true;
        AppManager.Instance.SwitchScreen(4);
    }

    public void AddNewBox()
    {
        AppManager.Instance.nBox = true;
        AppManager.Instance.SwitchScreen(5);
    }

    public void AddNewPipe()
    {
        AppManager.Instance.nPipe = true;
        AppManager.Instance.SwitchScreen(6);
    }

    public void AddNewChannel()
    {
        AppManager.Instance.nChannel = true;
        AppManager.Instance.SwitchScreen(7);
    }
    #endregion

    #region Delete
    public void DeletePlace()
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        if (P.Count == 1)
        {
            MakeNewSurface();
            MakeNewBox();
            MakeNewPipe();
            MakeNewChannel();
            AllControlsDisable();
            MakeNewPlace();
        }
        P.Remove(P[AM.PlaceN]);
        AM.PlaceN--;
        SaveChanges();
    }

    public void DeleteSurface()
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        if (P[AM.PlaceN].surface.Count == 1)
        {
            MakeNewSurface();
        }
        var DS = P[AM.PlaceN].surface;
        DS.Remove(DS[AM.SurfaceN]);
        SaveChanges();
    }

    public void DeleteBox()
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        if (P[AM.PlaceN].box.Count == 1)
        {
            MakeNewBox();
        }
        var DB = P[AM.PlaceN].box;
        DB.Remove(DB[AM.BoxN]);
        SaveChanges();
    }

    public void DeletePipe()
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        if (P[AM.PlaceN].pipe.Count == 1)
        {
            MakeNewPipe();
        }
        var DP = P[AM.PlaceN].pipe;
        DP.Remove(DP[AM.PipeN]);
        SaveChanges();
    }

    public void DeleteChannel()
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        if (P[AM.PlaceN].channel.Count == 1)
        {
            MakeNewChannel();
        }
        var DC = P[AM.PlaceN].channel;
        DC.Remove(DC[AM.ChannelN]);
        SaveChanges();
    }
    #endregion


    public void SaveChanges()
    {
        var AM = AppManager.Instance;
        var place = AM.places;
        string data = JsonUtility.ToJson(place);
        PlayerPrefs.SetString(AM.phone_number, data);
        Debug.Log("player prefs saved");
        OnEnable();
    }


    #region View 
    public void BoxOff()
    {
        Box.interactable = false;
        BoxEdit.interactable = false;
        AddBox.interactable = false;
        DelBox.interactable = false;
    }

    public void BoxOn()
    {
        Debug.Log("Включение элементов камеры");
        AddBox.interactable = true;
        Box.interactable = true;
    }

    public void AllControlsDisable()
    {
        PlaceOff();
        SurfaceOff();
        BoxOff();
        PipeOff();
        ChannelOff();
    }

    public void AllOnline()
    {
        PlaceOn();
        SurfaceOn();
        BoxOn();
        PipeOn();
        ChannelOn();
    }

    public void PlaceOn()
    {
        Place.interactable = true;
        AddPlace.interactable = true;
    }

    public void PlaceOff()
    {
        PlaceEdit.interactable = false;
        DelPlace.interactable = false;
    }

    public void SurfaceOff()
    {
        Surface.interactable = false;
        SurfaceEdit.interactable = false;
        AddSurface.interactable = false;
        DelSurface.interactable = false;
    }


    public void SurfaceOn()
    {
        Debug.Log("Включение элементов поверхности");
        Surface.interactable = true;
        AddSurface.interactable = true;
    }


    public void ChannelOff()
    {
        Channels.interactable = false;
        ChannelEdit.interactable = false;
        AddChannels.interactable = false;
        DelChannel.interactable = false;
    }

    public void ChannelOn()
    {
        Debug.Log("Включение элементов канала");
        Channels.interactable = true;
        AddChannels.interactable = true;
    }

    public void PipeOff()
    {
        Pipes.interactable = false;
        PipeEdit.interactable = false;
        AddPipes.interactable = false;
        DelPipe.interactable = false;
    }

    public void PipeOn()
    {
        Debug.Log("Включение элементов труб");
        Pipes.interactable = true;
        AddPipes.interactable = true;
    }
    #endregion

    public void SendData()
    {
        SetTargetIDsToMediaFiles(() =>
        {
            SetTargetIDsToNoteMediaFiles(() =>
            {
                WebHandler.Instance.SendDataWraper((repl) =>
                {
                    Debug.Log("Senderf");
                    AppManager.Instance.SwitchScreen(2);
                    OnEnable();
                });
            }
            );
        });
    }

    public async void SetTargetIDsToMediaFiles(Action callContinue = null)
    {
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        List<bool> check = new List<bool>();
        foreach (string path in AppManager.Instance._mediaPaths)
        {
            check.Add(false);
            int n = check.Count - 1;
            var f = ReadFile(path);
            if (path.Contains(".jpg") || path.Contains(".jpeg") || path.Contains(".JPG")) fileType = "image/jpg";
            if (path.Contains(".png") || path.Contains(".PNG")) fileType = "image/png";
            if (path.Contains(".wav") || path.Contains(".WAV")) fileType = "audio/wav";
            if (path.Contains(".mp4") || path.Contains(".MP4")) fileType = "video/mp4";
            WebHandler.Instance.SendFileWraper(AM.userInfo.access_token, f, (repl) =>
            {
                Debug.Log("mediaRepl=" + repl);
                if (P != null)
                {
                    foreach (var place in P)
                    {
                        if (place.surface != null)
                        {
                            place.ForeachSurface(path, repl);
                        }

                        if (place.box != null)
                        {
                            place.ForeachBox(path, repl);
                        }

                        if (place.pipe != null)
                        {
                            place.ForeachPipe(path, repl);
                        }

                        if (place.channel != null)
                        {
                            place.ForeachChannel(path, repl);
                        }
                    }
                }
                Debug.Log(n);
                check[n] = true;
            }, fileType);
        }
        await new WaitWhile(() =>
        {
            bool rez = true;
            foreach (var b in check)
            {
                rez = rez && b;
            }
            Debug.Log("rez " + rez);
            return !rez;
        });
        callContinue?.Invoke();
    }

    public async void SetTargetIDsToNoteMediaFiles(Action callContinue = null)
    {
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        List<bool> check = new List<bool>();
        foreach (string path in AppManager.Instance._noteMediaPaths)
        {
            check.Add(false);
            int m = check.Count - 1;
            var f = ReadFile(path);
            if (path.Contains(".jpg") || path.Contains(".jpeg") || path.Contains(".JPG")) fileType = "image/jpg";
            if (path.Contains(".png") || path.Contains(".PNG")) fileType = "image/png";
            if (path.Contains(".wav") || path.Contains(".WAV")) fileType = "audio/wav";
            if (path.Contains(".mp4") || path.Contains(".MP4")) fileType = "video/mp4";
            WebHandler.Instance.SendFileWraper(AM.userInfo.access_token, f, (repl) =>
            {
                Debug.Log("NoteMediaRepl=" + repl);
                if (P != null)
                {
                    foreach (var place in P)
                    {
                        if (place.surface != null)
                        {
                            place.ForeachNoteSurface(path, repl);
                        }

                        if (place.box != null)
                        {
                            place.ForeachNoteBox(path, repl);
                        }

                        if (place.pipe != null)
                        {
                            place.ForeachNotePipe(path, repl);
                        }

                        if (place.channel != null)
                        {
                            place.ForeachNoteChannel(path, repl);
                        }
                    }
                }
                Debug.Log("m " + m);
                check[m] = true;
            }, fileType);
        }
        await new WaitWhile(() =>
        {
            bool rez = true;
            foreach (var b in check)
            {
                rez = rez && b;
            }
            return !rez;
        });
        callContinue?.Invoke();
    }

    public byte[] ReadFile(string _path)
    {
        byte[] bytes = File.ReadAllBytes(_path);
        return bytes;
    }

    #region Way 

    public void WayOne()
    {
        AppManager.Instance.way = 1;
    }

    public void WayTwo()
    {
        AppManager.Instance.way = 2;
    }

    public void WayThree()
    {
        AppManager.Instance.way = 3;
    }


    public void WayFour()
    {
        AppManager.Instance.way = 4;
    }
    #endregion

    public void DownLoadPlace()
    {
        AppManager.Instance.needDownload = true;
        AppManager.Instance.SwitchScreen(3);
    }


    public void LogOut()
    {
        PlayerPrefs.DeleteKey("access_token");
        PlayerPrefs.DeleteKey("code");
        PlayerPrefs.DeleteKey("phoneNumber");
        Application.Quit();
    }

    public void UpdatePlaceDropdownOptions()
    {
        var P = AppManager.Instance.places.places;
        Place.options.Clear();
        for (val = 0; val < P.Count; val++)
        {
            Place.options.Add(new TMP_Dropdown.OptionData(P[val].name));
            P[val].saved = true;
        }
    }



    public void MakeNewPlace()
    {
        Place.captionText.text = "Создать осмотр участка";
        Place.options.Clear();
        PlaceEdit.interactable = false;
        DelPlace.interactable = false;
        comment.text = "";
    }

    public void MakeNewSurface()
    {
        Surface.captionText.text = "Создать осмотр поверхности";
        DelSurface.interactable = false;
        SurfaceEdit.interactable = false;
        AddSurface.interactable = true;
    }

    public void MakeNewBox()
    {
        Box.captionText.text = "Создать осмотр камеры";
        DelBox.interactable = false;
        BoxEdit.interactable = false;
        AddBox.interactable = true;
    }

    public void MakeNewChannel()
    {
        Channels.captionText.text = "Создать осмотр канала";
        DelChannel.interactable = false;
        ChannelEdit.interactable = false;
        AddChannels.interactable = true;
    }

    public void MakeNewPipe()
    {
        Pipes.captionText.text = "Создать осмотр трубы";
        DelPipe.interactable = false;
        PipeEdit.interactable = false;
        AddPipes.interactable = true;
    }
}

