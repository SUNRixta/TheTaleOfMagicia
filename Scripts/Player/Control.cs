using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Control : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private float Speed = 5f;
    
    public DialogueUI DialogueUI => dialogueUI;
    public IInteractable Interactable {get; set;}

    public LayerMask enemyTrigger;

    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void OnMove(InputValue value)
    {
        CheckForEncounters();
        if (dialogueUI.isOpen)
        {
            animator.SetBool("IsWalking", false);
        }
        else
        {
            moveDirection = value.Get<Vector2>();
            CheckForEncounters();

            if (moveDirection.x != 0 || moveDirection.y != 0)
            {
                animator.SetFloat("Horizontal", moveDirection.x);
                animator.SetFloat("Vertical", moveDirection.y);

                animator.SetBool("IsWalking", true);
            }
            else
            {
                animator.SetBool("IsWalking", false);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Speed = 9f;
            animator.speed = 1.5f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            Speed = 5f;
            animator.speed = 1f;
        }

        if (dialogueUI.isOpen == false)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interactable?.Interact(this);
            }
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
    

    private void FixedUpdate()
    {
        if (dialogueUI.isOpen) return;

        rb.MovePosition(rb.position + moveDirection * Speed * Time.fixedDeltaTime);
    }

    private void CheckForEncounters()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.2f, enemyTrigger) != null)
        {
            int randInt = Random.Range(1, 101);
            Debug.Log(randInt);
            if (randInt <= 25)
            {
                FindObjectOfType<SceneSwitcher>().LoadBattleScene();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (collision.CompareTag("SceneChangeTrigger"))
        {
            FindObjectOfType<SceneSwitcher>().LoadNextScene();
        }

        if (collision.CompareTag("SceneEnterTrigger"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                FindObjectOfType<SceneSwitcher>().LoadNextScene();
            }
        }
    }
}
