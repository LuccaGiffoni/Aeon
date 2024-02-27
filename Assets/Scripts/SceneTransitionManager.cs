using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public void MoveToSceneByIndex(int sceneIndex)
    {
        StartCoroutine(MoveToScene(sceneIndex));
    }

    private IEnumerator MoveToScene(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        yield return operation.isDone;

        operation.allowSceneActivation = true;
    }
}