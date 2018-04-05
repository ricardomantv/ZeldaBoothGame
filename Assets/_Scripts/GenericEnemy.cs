using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GenericEnemy : MonoBehaviour {

    public int health;
    public float moveSpeed;
    public float stopDist;

    private Camera cam;
    private NavMeshAgent agent;
    private AreaEnemyManager aem;

	// Use this for initialization
	void OnEnable () {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        aem = this.transform.parent.gameObject.GetComponent<AreaEnemyManager>();

        agent.destination = cam.transform.position;
        agent.speed = moveSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        float curDist = Vector3.Distance(this.transform.position, cam.transform.position);
        if (curDist <= stopDist) {
            agent.isStopped = true;
        }

        if (agent.isStopped) {
            // TODO: play idle animation
        }
	}

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Projectile") {
            health--;
            if (health == 0) {
                StartCoroutine("Die");
            }
        }
    }

    public void DebugDeath() {
        health--;
        if (health == 0) {
            StartCoroutine("Die");
        }
    }

    IEnumerator Die() {
        // TODO: Spawn enemy death explosion
        yield return new WaitForSeconds(0.5f);
        this.gameObject.SetActive(false);
        aem.EnemyDeath();
    }
}
