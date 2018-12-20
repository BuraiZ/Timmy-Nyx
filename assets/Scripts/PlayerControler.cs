using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour {
    // Déclaration des constantes
    private static readonly Vector3 FlipRotation = new Vector3(0, 180, 0);

    // Déclaration des variables
    public bool _Grounded { get; set; }
    protected bool _Walled { get; set; }
    protected bool _Flipped { get; set; }
    protected Animator _Anim { get; set; }
    protected Rigidbody _Rb { get; set; }
    protected bool isSelected;

    // Component
    public Transform spawner;
    public GameManager gm;

    // Valeurs exposées
    [SerializeField]
    protected float MoveSpeed = 5.0f;

    [SerializeField]
    protected float JumpForce = 10f;

    [SerializeField]
    protected LayerMask WhatIsGround;

    RaycastHit hit;

    // Awake se produit avant le Start. Il peut être bien de régler les références dans cette section.
    protected virtual void Awake() {
        _Rb = GetComponent<Rigidbody>();
    }

    // Utile pour régler des valeurs aux objets
    protected virtual void Start() {
        _Grounded = false;
        _Flipped = false;
        isSelected = false;
    }

    // Vérifie les entrées de commandes du joueur
    protected virtual void Update() {
        var horizontal = Input.GetAxis("Horizontal") * MoveSpeed;
        CheckGrounded();
        HorizontalMove(horizontal);
        FlipCharacter(horizontal);
    }

    // Gère le mouvement horizontal
    protected virtual void HorizontalMove(float horizontal) {
        float horizontalSpeed = _Walled ? 0 : horizontal;
        _Rb.velocity = new Vector3(_Rb.velocity.x, _Rb.velocity.y, horizontalSpeed);
    }

    // Gère l'orientation du joueur et les ajustements de la camera
    void FlipCharacter(float horizontal) {
        if (horizontal < 0 && !_Flipped) {
            _Flipped = true;
            transform.Rotate(FlipRotation);
        } else if (horizontal > 0 && _Flipped) {
            _Flipped = false;
            transform.Rotate(-FlipRotation);
        }
    }

    private void CheckGrounded() {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 0.1f)) {
            _Grounded = true;
        } else {
            _Grounded = false;
        }
    }

    public void IsSelected(bool isSelected) {
        this.isSelected = isSelected;
    }

    public void Respawn() {
        transform.position = spawner.position;
    }
}
