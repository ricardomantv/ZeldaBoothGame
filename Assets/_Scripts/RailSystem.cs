using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailSystem : MonoBehaviour {

    public List<GameObject> waypoints; // Every 3 should be actual stop points, points in between are used for cubic bezier calculations
    public bool isMoving;
    public float moveSpeed;

    private Camera cam;
    private int cur_waypoint;

    /*Spline calculation vars*/
    private Vector3 p0;
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;
    private Quaternion r0;
    private Quaternion r1;
    private float t;

	// Use this for initialization
	void Start () {
        isMoving = false;

        cam = Camera.main;
        cam.transform.position = waypoints[0].transform.position;
        cam.transform.rotation = waypoints[0].transform.rotation;

        cur_waypoint = 0;
        GetSplineVars(cur_waypoint);
    }

    private void FixedUpdate() {
        if (isMoving) {
            t += Time.deltaTime * moveSpeed;
        }        

        if (t >= 1.0f) {
            isMoving = false;
            t = 0.0f;
            cur_waypoint += 3;
            if (cur_waypoint == waypoints.Count - 1) {
                TriggerSceneChange();
            } else {
                GetSplineVars(cur_waypoint);
            }
        }
        
        if (isMoving) {
            cam.transform.position = TravelAlongSpline();
            cam.transform.rotation = Quaternion.Slerp(r0, r1, t);
        }

        // DEBUG COMMANDS
        if (Input.GetKeyDown(KeyCode.Space)) {
            // TODO: Replace this debug code with enemy trigger
            if (!isMoving) {
                isMoving = true;
            }
        }

        if (Input.GetMouseButtonDown(0)) {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100)) {
                if (hit.collider.gameObject.tag == "Enemy") {
                    GenericEnemy ge = hit.collider.gameObject.GetComponent<GenericEnemy>();
                    ge.DebugDeath();
                }
            }
        }
    }

    void GetSplineVars(int waypointIndex) {
        p0 = waypoints[waypointIndex].transform.position;
        p1 = waypoints[waypointIndex + 1].transform.position;
        p2 = waypoints[waypointIndex + 2].transform.position;
        p3 = waypoints[waypointIndex + 3].transform.position;
        t = 0.0f;

        r0 = waypoints[waypointIndex].transform.rotation;
        r1 = waypoints[waypointIndex + 3].transform.rotation;
    }

    Vector3 TravelAlongSpline() {
        float omt = 1 - t;
        float omtt = omt * omt;
        float omttt = omtt * omt;
        float tt = t * t;
        float ttt = tt * t;

        Vector3 pos = omttt * p0;
        pos += 3 * omtt * t * p1;
        pos += 3 * omt * tt * p2;
        pos += ttt * p3;

        // Debug.Log("pos: " + pos);
        return pos;
    }

    public void StartMovement() {
        isMoving = true;
    }

    void TriggerSceneChange() {

    }
}
