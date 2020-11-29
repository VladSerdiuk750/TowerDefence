using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private bool isFirst;

    private Animator anim;
    
    private static readonly int First = Animator.StringToHash("First");

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        anim.SetBool(First, isFirst);
    }
}
