using UnityEngine;

public class Waypoint : MonoBehaviour
{
    public Waypoint nextWaypoint;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.5f);

        if (nextWaypoint != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, nextWaypoint.transform.position);
        }
    }
}