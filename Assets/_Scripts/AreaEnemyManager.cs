using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEnemyManager : MonoBehaviour {

    public float spawnRate;

    private List<GameObject> enemies;
    private int numEnemies;
    private RailSystem rs;

	// Use this for initialization
	void Start () {
        numEnemies = 0;
		foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
            numEnemies++;
        }

        rs = Camera.main.GetComponent<RailSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void EnemyDeath() {
        numEnemies--;
        if (numEnemies <= 0) {
            foreach (Transform enemy in transform) {
                Destroy(enemy.gameObject);
            }

            rs.StartMovement();
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "MainCamera") {
            StartCoroutine("SpawnEnemy");
        }
    }

    IEnumerator SpawnEnemy() {
        foreach (Transform enemy in transform) {
            Debug.Log(enemy.transform.name);
            enemy.gameObject.SetActive(true);
            yield return new WaitForSeconds(spawnRate);
        }
    }
}
