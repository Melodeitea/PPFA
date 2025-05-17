using UnityEngine;

public class SplinePath : MonoBehaviour
{
    public Transform[] waypoints;

    public Vector3 GetPosition(float t)
    {
        t = Mathf.Clamp01(t);

        int segmentCount = waypoints.Length - 1;
        float totalLength = segmentCount;
        float scaledT = t * totalLength;
        int index = Mathf.FloorToInt(scaledT);
        index = Mathf.Clamp(index, 0, segmentCount - 1);

        float localT = scaledT - index;

        Vector3 p0 = waypoints[index].position;
        Vector3 p1 = waypoints[index + 1].position;

        return Vector3.Lerp(p0, p1, localT);
    }

    public Vector3 GetForward(float t)
    {
        float delta = 0.01f;
        Vector3 current = GetPosition(t);
        Vector3 ahead = GetPosition(Mathf.Clamp01(t + delta));
        return (ahead - current).normalized;
    }
}
