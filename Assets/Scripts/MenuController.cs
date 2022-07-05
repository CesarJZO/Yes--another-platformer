using UnityEngine;
using UnityEngine.SceneManagement;

namespace mastermind
{
    public class MenuController : MonoBehaviour
    {
        public void LoadScene(int sceneId) => SceneManager.LoadScene(sceneId);

        public void QuitGame() => Application.Quit(0);
    }
}