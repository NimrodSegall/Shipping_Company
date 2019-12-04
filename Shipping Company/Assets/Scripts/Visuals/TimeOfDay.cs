using UnityEngine;

public class TimeOfDay : MonoBehaviour
{
    [SerializeField]
    private Transform sunTransform = null;

    private float skyRotationSpeed = 10f;
    private Light sunLight;
    // Start is called before the first frame update
    void Start()
    {
        skyRotationSpeed =  360 / (GameState.current.secsPerHour * 24);
        sunLight = sunTransform.GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        RotateSky();
    }

    private void RotateSky()
    {
        sunTransform.rotation *= Quaternion.AngleAxis(skyRotationSpeed * Time.deltaTime, Vector3.up);
    }

    private void ModifySunColor()
    {
        sunLight.color = Color.Lerp(sunLight.color, GameState.current.sunColors[GameState.current.time], 1);
    }
}
