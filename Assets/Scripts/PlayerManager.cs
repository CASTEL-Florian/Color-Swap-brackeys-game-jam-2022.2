using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private List<Player> players;
    [SerializeField] private float speed = 400;
    [SerializeField] private GameObject controlIndicator;
    [SerializeField] private Vector3 controlIndicatorOffset;
    private PlayerInputs controls;
    private MyCamera cam;
    private int currentIndex = 0;
    private Dictionary<string, List<Transform>> colorToTransforms;
    private List<Rigidbody2D> rigidbodies;

    private void Start()
    {
        cam = FindObjectOfType<MyCamera>();
        colorToTransforms = new Dictionary<string, List<Transform>>();
        rigidbodies = new List<Rigidbody2D>();
        foreach (Player player in players)
        {
            string color = player.GetComponent<UnitColor>().color;
            if (!colorToTransforms.ContainsKey(color))
                colorToTransforms[color] = new List<Transform>();
            colorToTransforms[color].Add(player.transform);
            rigidbodies.Add(player.GetComponent<Rigidbody2D>());
        }
    }


    private void Awake()
    {
        controls = new PlayerInputs();
        controls.Player.Swap.performed += ctx => Swap();
        controls.Player.Dash.performed += ctx => Dash();
    }

    private void FixedUpdate()
    {
        Vector2 move = controls.Player.Move.ReadValue<Vector2>();
        for (int i = 0; i < players.Count; i++)
        {
            if (i == currentIndex)
                players[i].Move(move * Time.fixedDeltaTime * speed);
            else
                players[i].Move(Vector2.zero);
        }
        controlIndicator.transform.position = players[currentIndex].transform.position + controlIndicatorOffset;
    }

    private void Update()
    {
        players[currentIndex].RotateTowardMouse();
        players[currentIndex].SetGunActive(controls.Player.Shoot.ReadValue<float>() == 1);
    }

    private void Swap()
    {
        players[currentIndex].SetGunActive(false);
        currentIndex = (currentIndex + 1) % players.Count;
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    public Transform FindClosePlayer(Transform from, string col)
    {
        Transform dest = transform;
        float minDist = Mathf.Infinity;
        foreach (Transform playerTransform in colorToTransforms[col])
        {
            float dist = Vector3.Distance(playerTransform.position, from.position);
            if (dist < minDist)
            {
                dest = playerTransform;
                minDist = dist;
            }
        }
                

        return dest;
    }

    private void Dash()
    {
        
            StartCoroutine(players[currentIndex].DashCoroutine());
    }

    
}
