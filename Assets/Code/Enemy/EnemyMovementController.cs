using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class EnemyMovementController
{
    public static void MoveV2(Enemy enemy)
    {
        IncrementAggroTimer(enemy);

        if (CanAggro(enemy) || RecentlyHadAggro(enemy))
        {
            ToggleSeeker(enemy, true);
        }
        else
        {
            ToggleSeeker(enemy, false);

        }
    }

    public static void Move(Enemy enemy)
    {
        IncrementAggroTimer(enemy);

        if (CanAggro(enemy) || RecentlyHadAggro(enemy))
        {
            SetXSpeed(enemy);
            SetYSpeed(enemy);
        }
        else if (WithinAggroRange(enemy))
        {
            ToggleSeeker(enemy, true);
            return;
        }
        else
        {
            enemy.RigidBody.velocity = new Vector2(0, 0);
        }
        ToggleSeeker(enemy, false);
    }

    private static void ToggleSeeker(Enemy enemy, bool enabled)
    {
        var seeker = enemy.GetComponent<Seeker>();
        if (seeker != null)
        {
            seeker.enabled = enabled;
        }

        var aiPath = enemy.GetComponent<AIPath>();
        if (aiPath != null)
        {
            aiPath.enabled = enabled;
        }
    }

    private static bool CanAggro(Enemy enemy)
    {
        if (WithinAggroRange(enemy) && HasClearPath(enemy))
        {
            SetHasAggro(enemy);
            return true;
        }

        return false;
    }

    private static void IncrementAggroTimer(Enemy enemy)
    {
        if (enemy.AggroTimeRemaining > 0)
        {
            enemy.AggroTimeRemaining -= Time.deltaTime;
        }
        else
        {
            enemy.AggroTimeRemaining = 0;
            enemy.IsRecentlyAggro = false;
            enemy.SeenPlayer = false;
        }
    }

    private static void SetHasAggro(Enemy enemy)
    {
        enemy.IsRecentlyAggro = true;
        enemy.AggroTimeRemaining = 5f;
    }

    private static bool RecentlyHadAggro(Enemy enemy)
    {
        return enemy.IsRecentlyAggro;
    }

    private static bool HasClearPath(Enemy enemy)
    {
        var difference = enemy.Target.transform.position - enemy.transform.position;
        var direction = new Vector2(difference.x, difference.y);
        var raycastHit = Physics2D.Raycast(enemy.BoxCollider.bounds.center, direction, direction.magnitude, enemy._layerMask);
        var rayColor = Color.green;

        if (raycastHit.collider != null)
        {
            enemy.SeenPlayer = true;
            rayColor = Color.red;
        }
        Debug.DrawRay(enemy.BoxCollider.bounds.center, direction, rayColor);
        Debug.Log(raycastHit.collider);
        return raycastHit.collider == null;
    }

    public static bool WithinAggroRange(Enemy enemy)
    {
        var targetPos = enemy.Target.transform.position;
        var enemyPos = enemy.transform.position;

        return (targetPos - enemyPos).magnitude < enemy.AggroRange;
    }

    private static void SetXSpeed(Enemy enemy)
    {
        //Set horizontal speed
        //if player is left of enemy
        if (enemy.Target.transform.position.x < enemy.transform.position.x)
        {
            //current speed - accel*time
            var xSpeed = enemy.RigidBody.velocity.x - (enemy.xAccel * Time.deltaTime);

            //constrain to max speed
            if (xSpeed < -enemy.XMaxSpeed)
            {
                xSpeed = -enemy.XMaxSpeed;
            }

            //set velocity
            enemy.RigidBody.velocity = new Vector2(xSpeed, enemy.RigidBody.velocity.y);
        }
        //if player is right of enemy
        else if (enemy.Target.transform.position.x > enemy.transform.position.x)
        {
            var xSpeed = enemy.RigidBody.velocity.x + (enemy.xAccel * Time.deltaTime);

            if (xSpeed > enemy.XMaxSpeed)
            {
                xSpeed = enemy.XMaxSpeed;
            }
            enemy.RigidBody.velocity = new Vector2(xSpeed, enemy.RigidBody.velocity.y);
        }
    }

    private static void SetYSpeed(Enemy enemy)
    {
        //Set horizontal speed
        //if player is left of enemy
        if (enemy.Target.transform.position.y < enemy.transform.position.y)
        {
            //current speed - accel*time
            var ySpeed = enemy.RigidBody.velocity.y - (enemy.YAccel * Time.deltaTime);

            //constrain to max speed
            if (ySpeed < -enemy.YMaxSpeed)
            {
                ySpeed = -enemy.YMaxSpeed;
            }

            //set velocity
            enemy.RigidBody.velocity = new Vector2(enemy.RigidBody.velocity.x, ySpeed);
        }
        //if player is right of enemy
        else if (enemy.Target.transform.position.y > enemy.transform.position.y)
        {
            var ySpeed = enemy.RigidBody.velocity.y + (enemy.YAccel * Time.deltaTime);

            if (ySpeed > enemy.YMaxSpeed)
            {
                ySpeed = enemy.YMaxSpeed;
            }
            enemy.RigidBody.velocity = new Vector2(enemy.RigidBody.velocity.x, ySpeed);
        }
    }
}
