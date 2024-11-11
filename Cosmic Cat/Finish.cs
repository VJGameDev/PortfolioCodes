using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour {
    private bool levelCompleted = false;

    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.name == "Player 1" || collision.gameObject.name == "Player 2" && !levelCompleted) {
            levelCompleted = true;
            Invoke("CompleteLevel", 1f);
        }
    }

    private void CompleteLevel() {
        SceneManager.LoadScene("Winner");
    }
}
