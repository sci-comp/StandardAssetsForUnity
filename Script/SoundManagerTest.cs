using UnityEngine;
using UnityEngine.UI;

public class SoundManagerTest : MonoBehaviour
{
    [SerializeField] Button playVelcro = null;

    private void Start()
    {
        playVelcro.onClick.AddListener(PlayVelcro);
    }

    private void PlayVelcro()
    {
        SoundManager.Instance.PlaySound("sfx_velcro");
    }
}
