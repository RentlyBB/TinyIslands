using UnityEngine;
using UnityEngine.SceneManagement;

public class BuildManager : MonoBehaviour {
    public GameObject WebGLCanvas;
    public GameObject WarningPanel;

    private void Awake() {
#if UNITY_WEBGL && !UNITY_EDITOR
        WebGLCanvas.SetActive(true);
#endif
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F1)) {
            SceneManager.LoadScene(0);
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        if(Input.GetKeyDown(KeyCode.H))
        {
            WarningPanel.SetActive(!WarningPanel.activeSelf);
        }
#endif
    }
}