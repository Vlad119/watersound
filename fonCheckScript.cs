using UnityEngine;

public class fonCheckScript : MonoBehaviour
{
    public float timeRemaining = 2f;
    public GameObject fonCheck;
    public void OnEnable()
    {
        timeRemaining = 2f;
    }

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0)
            {
                fonCheck.SetActive(false);
            }
        }
    }
}
