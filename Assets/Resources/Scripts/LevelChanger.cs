using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    public Animator animator;
    private int levelIndex;

    // Update is called once per frame
    void Update()
    {

    }

    public void FadeToLevel(int levelIndex)
    {
        this.levelIndex = levelIndex;
        animator.SetTrigger("fadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelIndex);
    }
}
