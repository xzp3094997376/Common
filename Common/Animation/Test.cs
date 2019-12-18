using UnityEngine;

public class Test : MonoBehaviour
{
    public Animation ani;
    public Animator ator;
    private void Start()
    {
        AnimatorEventComponent acom = ator.gameObject.AddComponent<AnimatorEventComponent>();
        acom.AddEvent("cube2", 29, () =>
        {
            ator.speed = 0f;
            Debug.LogError("fdsfs");
        });
    }
}
