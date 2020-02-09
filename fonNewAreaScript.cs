using System;
using TMPro;
using UnityEngine;

public class fonNewAreaScript : MonoBehaviour
{
    public GameObject nameTxt;
    public GameObject typeTxt;
    public GameObject commentTxt;
    public GameObject name;
    public GameObject ID_sinc;
    public GameObject comment;
    public GameObject way_type;
    public Allowed_values AV;
    public bool loadValue = false;
    public GameObject fonCheck;
    public Place place;
    public TMP_Text buttonText;
    public GameObject scroll;
    public TMP_Text originText;
    public TMP_Text lastSaveText;
    public TMP_Text length;
    public TMP_Text diametr;
    public TMP_Text type;
    public TMP_Text isolation;

    async public void OnEnable()
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        AV = AM.allowed_values.allowed_values;
        await new WaitUntil(() =>
        {
            return WebHandler.Instance && AM;
        });
        Rephrase(); // замена числовых значений на наименование типов прокладки
        Clear(); //очистка полей ввода
        ChangeWayType(); // наименование типов прокладки
        if (P.Count > 0)
        {
            LoadPlace(); //то загружаю ранее созданный участок
        }
        buttonText.text = "Сохранить";
        commentTxt.SetActive(true);
        nameTxt.SetActive(true);
        typeTxt.SetActive(true);
        scroll.SetActive(true);
        name.SetActive(true);
        ID_sinc.SetActive(true);
        comment.SetActive(true);
        way_type.SetActive(true);
        ID_sinc.GetComponent<TMP_InputField>().text = P[AM.PlaceN].id;
    }

    public void Place()
    {
        buttonText.text = "Сохранить";
        commentTxt.SetActive(true);
        nameTxt.SetActive(true);
        typeTxt.SetActive(true);
        scroll.SetActive(true);
        name.SetActive(true);
        ID_sinc.SetActive(true);
        comment.SetActive(true);
        way_type.SetActive(true);
        way_type.GetComponent<TMP_Dropdown>().options.Clear();
    }


    public void ChangeWayType()
    {
        way_type.GetComponent<TMP_Dropdown>().options.Clear();
        for (int val = 0; val < AV.way_type.Count; val++)
        {
            way_type.GetComponent<TMP_Dropdown>().options.Add(new TMP_Dropdown.OptionData(AV.way_type[val].value));
        }
        way_type.GetComponent<TMP_Dropdown>().RefreshShownValue();
    }

    public void LoadPlace() // загружаю ранее созданный участок
    {
        var AM = AppManager.Instance;
        var P = AM.places.places;
        name.GetComponent<TMP_InputField>().text = P[AM.PlaceN].name;
        ID_sinc.GetComponent<TMP_InputField>().text = P[AM.PlaceN].id;
        comment.GetComponent<TMP_InputField>().text = P[AM.PlaceN].comment;
        way_type.GetComponent<TMP_Dropdown>().captionText.text = P[AM.PlaceN].way_type;
    }

    private void Clear() //очищаю поля для ввода
    {
        name.GetComponent<TMP_InputField>().text = "";
        ID_sinc.GetComponent<TMP_InputField>().text = "";
        comment.GetComponent<TMP_InputField>().text = "";
    }

    public void RecordPlace() //сохраняю изменения
    {
        var AM = AppManager.Instance;
        SavePlace();
        var place = AM.places;
        string data = JsonUtility.ToJson(place);
        PlayerPrefs.SetString(AM.phone_number, data);
        Debug.Log("player prefs saved");
        AM.SwitchScreen(2);
    }


    public void SavePlace()
    {
        Debug.Log("редактирование участка");
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        if (P.Count > 0)
        {
            P[AM.PlaceN].name = name.GetComponent<TMP_InputField>().text;
            P[AM.PlaceN].id = ID_sinc.GetComponent<TMP_InputField>().text;
            P[AM.PlaceN].comment = comment.GetComponent<TMP_InputField>().text;
            P[AM.PlaceN].way_type = way_type.GetComponent<TMP_Dropdown>().captionText.text;
            P[AM.PlaceN].saved = true;
            P[AM.PlaceN].status = "1";
        }
        AM.ID = ID_sinc.GetComponent<TMP_InputField>().text;
    }


    public void OpenFonID()
    {
        AppManager.Instance.screens[8].SetActive(true);
    }


    public void Rephrase()
    {
        var P = AppManager.Instance.places.places;
        var AM = AppManager.Instance;
        try
        {
            if (P[AM.PlaceN].way_type == "0")
            { P[AM.PlaceN].way_type = "неизвестно"; }
            if (P[AM.PlaceN].way_type == "1")
            { P[AM.PlaceN].way_type = "Канальная"; }
            if (P[AM.PlaceN].way_type == "2")
            { P[AM.PlaceN].way_type = "Бесканальная"; }
            if (P[AM.PlaceN].way_type == "3")
            { P[AM.PlaceN].way_type = "Коллектор"; }
            if (P[AM.PlaceN].way_type == "4")
            { P[AM.PlaceN].way_type = "Щитовая проходка/туннель"; }
            if (P[AM.PlaceN].way_type == "5")
            { P[AM.PlaceN].way_type = "Гильза/футляр"; }
            if (P[AM.PlaceN].way_type == "6")
            { P[AM.PlaceN].way_type = "Штольня"; }
            if (P[AM.PlaceN].way_type == "7")
            { P[AM.PlaceN].way_type = "Надземная на высоких опорах"; }
            if (P[AM.PlaceN].way_type == "8")
            { P[AM.PlaceN].way_type = "Надземная на низких опорах"; }
            if (P[AM.PlaceN].way_type == "9")
            { P[AM.PlaceN].way_type = "Мостовой переход"; }
            if (P[AM.PlaceN].way_type == "10")
            { P[AM.PlaceN].way_type = "Эстакада"; }
            if (P[AM.PlaceN].way_type == "11")
            { P[AM.PlaceN].way_type = "Дюкер"; }
            if (P[AM.PlaceN].way_type == "12")
            { P[AM.PlaceN].way_type = "Подвал"; }
            if (P[AM.PlaceN].way_type == "13")
            { P[AM.PlaceN].way_type = "Камера"; }
        }
        catch { }
    }
}
