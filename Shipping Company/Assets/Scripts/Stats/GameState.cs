using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState current = null;
    public int time = 12;

    public float secsPerHour = 60f;

    public Color[] sunColors = new Color[24];

    private float lastTimeTick = 0f;

    private void Awake()
    {
        current = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTime();
    }

    private void UpdateTime()
    {
        if(Time.time >= lastTimeTick + secsPerHour)
        {
            time = (time + 1) % 24;
            lastTimeTick = Time.time;
        }
    }
}
