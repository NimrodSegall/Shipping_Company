using UnityEngine;
using TMPro;

public class DisplayHUD : MonoBehaviour
{
    private TMPro.TextMeshProUGUI timeTMP; 
    private int time;
    // Start is called before the first frame update
    void Start()
    {
        timeTMP = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateShownTime();
        DisplayTime();
    }

    private void UpdateShownTime()
    {
        time = GameState.current.time;
    }

    private void DisplayTime()
    {
        timeTMP.SetText(Utilities.FormatTime(time));
    }


}
