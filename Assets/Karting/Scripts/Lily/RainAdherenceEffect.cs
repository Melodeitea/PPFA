using UnityEngine;

public class RainAdherenceEffect : MonoBehaviour
{
    public float reducedGripFactor = 0.5f;
    public ParticleSystem rainParticles;
    public AudioSource rainSound;

    private MotorbikeController bike;

    public void ActivateRain()
    {
        bike = FindObjectOfType<MotorbikeController>();
        if (bike != null)
        {
            bike.SetGripMultiplier(reducedGripFactor);
        }

        if (rainParticles) rainParticles.Play();
        if (rainSound) rainSound.Play();
    }

    public void DeactivateRain()
    {
        if (bike != null)
        {
            bike.ResetGripMultiplier();
        }

        if (rainParticles) rainParticles.Stop();
        if (rainSound) rainSound.Stop();
    }
}
