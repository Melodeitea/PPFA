using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class FatigueEffect : MonoBehaviour
{
    public Volume postProcessingVolume;
    public float fatigueDuration = 15f;
    public float fatigueGripMultiplier = 0.7f;
    public float blurIntensity = 1f;
    public Light blindingLight;

    private float timer;
    private MotorbikeController bike;
    private DepthOfField dof;

    public void ActivateFatigue()
    {
        bike = FindObjectOfType<MotorbikeController>();
        if (bike != null) bike.SetGripMultiplier(fatigueGripMultiplier);

        postProcessingVolume.profile.TryGet(out dof);
        if (dof != null)
        {
            dof.active = true;
            dof.focusDistance.Override(blurIntensity);
        }

        if (blindingLight) blindingLight.enabled = true;

        timer = fatigueDuration;
        StartCoroutine(FatigueTimer());
    }

    IEnumerator FatigueTimer()
    {
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            if (blindingLight)
                blindingLight.intensity = Mathf.PingPong(Time.time * 10f, 3f) + 1f;

            yield return null;
        }

        DeactivateFatigue();
    }

    public void DeactivateFatigue()
    {
        if (bike != null) bike.ResetGripMultiplier();
        if (dof != null) dof.active = false;
        if (blindingLight) blindingLight.enabled = false;
    }
}
