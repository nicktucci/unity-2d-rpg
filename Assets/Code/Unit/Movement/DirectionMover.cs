using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionMover : IMoveModule
{
    private Vector3 moveDirection = Vector3.zero;
    PhysicsBody2D body;

    private UnitDirection lastDirection = UnitDirection.None;
    public UnitDirection Direction
    {
        get
        {
            if (moveDirection.x < 0)
            {
                lastDirection = UnitDirection.Left;
                return lastDirection;
            }
            else if (moveDirection.x > 0)
            {
                lastDirection = UnitDirection.Right;
                return lastDirection;
            }
            return lastDirection;
        }
    }

    public float Magnitude { get; private set; }

    public Vector3 DirectionVector => moveDirection;

    public void Update(float speed, bool movementEnabled, bool inputEnabled)
    {
        if (movementEnabled && inputEnabled)
        {
            moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        else
        {
            moveDirection = Vector3.zero;
        }





        Vector3 realMagnitude = body.Move(moveDirection * speed * Time.deltaTime);

        float mX = Mathf.Abs(moveDirection.x * speed);
        float mY = Mathf.Abs(moveDirection.y * speed);

        Magnitude = mX > mY ? mX : mY;

        if (realMagnitude == Vector3.zero) Magnitude = 0;


    }

    public void Setup(GameObject owner)
    {
        body = new PhysicsBody2D(owner, 1 << LayerMask.NameToLayer("Collision"));

    }
}
