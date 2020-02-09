using UnityEngine;

public class PlusPrefScript : MonoBehaviour
{

    public void PlusScript()
    {
        AppManager.Instance.screens[8].GetComponent<FonIDScript>().PlusSearchPref();
        Destroy(gameObject);
    }
}
