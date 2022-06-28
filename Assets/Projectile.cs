using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public LayerMask collisionMask;
    public float Speed { get => speed; set => speed = value; }
    float speed = 100;
    float damage = 1;


    void Update() {
        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }

    void CheckCollisions(float moveDistance) {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, moveDistance, collisionMask, QueryTriggerInteraction.Collide)) {
            OnHitObject(hitInfo);
        }
    }

    void OnHitObject(RaycastHit hitInfo) {
        IDamageable damageableObject = hitInfo.collider.GetComponent<IDamageable>();
        if (damageableObject != null) {
            damageableObject.TakeHit(damage, hitInfo);
        }
        Destroy(gameObject);
    }
}
