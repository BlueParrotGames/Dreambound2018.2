using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] float speed = 6f;
    [SerializeField] float jumpHeight = 8f;
    [SerializeField] float gravity = 20.0f;
    [SerializeField] float deadzone = 0.25f;

    [SerializeField] Animator anim;

    [Header("Gear animating")]
    [SerializeField] SkinnedMeshRenderer root;
    [ReadOnly] public List<SkinnedMeshRenderer> skinnedMeshes;
    [ReadOnly] public List<Animator> animators;

    Camera mainCam;
    CharacterController controller;

    Vector3 dir = Vector3.zero;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCam = Camera.main;
        anim = GetComponent<Animator>();

        //skinnedMeshes = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

        UpdateEquippedGear();

    }

    public void UpdateEquippedGear()
    {
        skinnedMeshes.Clear();
        animators.Clear();
        Debug.Log("Updating equipped gear!");
        foreach (SkinnedMeshRenderer s in gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            skinnedMeshes.Add(s);
            s.rootBone = root.rootBone;
            s.bones = root.bones;
        }

        foreach(Animator a in gameObject.GetComponentsInChildren<Animator>())
        {
            animators.Add(a);
            a.avatar = anim.avatar;
            a.runtimeAnimatorController = anim.runtimeAnimatorController;
        }
    }

    private void SetValues(string axisX, string axisZ)
    {
        float x = Input.GetAxis(axisX);
        float z = Input.GetAxis(axisZ);

        dir = new Vector3(x, 0, z);
        dir *= speed;

        anim.SetFloat("VelocityX", x);
        anim.SetFloat("VelocityZ", z);
    }

    private void MoveLegacy()
    {
        if (!GameManager.useController)
        {
            SetValues("Horizontal", "Vertical");

            if (Input.GetButtonDown("Jump") && controller.isGrounded)
            {
                Debug.Log(gameObject.name + " is Jumping");
                dir.y = jumpHeight * speed;
            }

            #region Cursor Direction
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLength;
            if (groundPlane.Raycast(camRay, out rayLength))
            {
                Vector3 lookPoint = camRay.GetPoint(rayLength);
                transform.LookAt(new Vector3(lookPoint.x, transform.position.y, lookPoint.z));
            }
            #endregion
        }
    }

    private void Update()
    {
        /// Mouse & Keyboard
        MoveLegacy();

        dir.y -= ((gravity * speed) * Time.deltaTime);
        controller.Move(transform.TransformDirection( dir * Time.deltaTime));

        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);
            }
        }
    }
}