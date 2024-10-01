using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimer;
    private float shakeTimerTotal;
    private float startingAmplitude;
    private float startingFrequency;

    private void Awake()
    {
        Instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();

        if (cinemachineVirtualCamera == null)
        {
            Debug.LogError("CinemachineVirtualCamera component not found on this GameObject.");
        }
        else
        {
            CinemachineBasicMultiChannelPerlin cinemachinePerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (cinemachinePerlin == null)
            {
                Debug.LogError("CinemachineBasicMultiChannelPerlin component not found on CinemachineVirtualCamera.");
            }
            else
            {
                Debug.Log("CinemachineBasicMultiChannelPerlin component successfully found and assigned.");
            }
        }
    }

    public void ShakeCamera(float amplitude, float frequency, float time)
    {
        if (cinemachineVirtualCamera == null)
        {
            Debug.LogError("CinemachineVirtualCamera component not assigned.");
            return;
        }

        CinemachineBasicMultiChannelPerlin cinemachinePerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (cinemachinePerlin == null)
        {
            Debug.LogError("CinemachineBasicMultiChannelPerlin component not found on CinemachineVirtualCamera.");
            return;
        }

        cinemachinePerlin.m_AmplitudeGain = amplitude;
        cinemachinePerlin.m_FrequencyGain = frequency;
        shakeTimer = time;
        shakeTimerTotal = time;
        startingAmplitude = amplitude;
        startingFrequency = frequency;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin cinemachinePerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (cinemachinePerlin != null)
            {
                float amplitude = Mathf.Lerp(startingAmplitude, 0f, 1 - (shakeTimer / shakeTimerTotal));
                float frequency = Mathf.Lerp(startingFrequency, 0f, 1 - (shakeTimer / shakeTimerTotal));
                cinemachinePerlin.m_AmplitudeGain = amplitude;
                cinemachinePerlin.m_FrequencyGain = frequency;
            }
        }
    }
}
