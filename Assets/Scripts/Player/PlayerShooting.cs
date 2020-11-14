using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Shots")]
    public LineRenderer shotTrail;

    private PlayerStats stats = PlayerStats.Instance;

    private void Shoot()
    {
        //Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Ray ray = default;
        RaycastHit rch;
        Vector2 endPointForLineRenderer;

        if (Physics.Raycast(ray, out rch, stats.shotRange, stats.layersCanShoot.value))
        {
            IDamageable damagableTarger = rch.transform.GetComponent<IDamageable>();
            if (damagableTarger != null)
            {
                damagableTarger.TakeDamage(stats.shotDamage);
            }

            endPointForLineRenderer = rch.point;

            Debug.Log("Player hit: " + rch.transform.name + " for " + stats.shotDamage + " dmg");
        }
        else
        {
            endPointForLineRenderer = ray.origin + ray.direction;
        }

        shotTrail.SetPositions(new Vector3[] { ray.origin, endPointForLineRenderer });
        shotTrail.gameObject.SetActive(true);

        stats.timeCanNextShoot = Time.time + stats.rateOfFire;
    }

    private IEnumerable DisableBulletTrail()
    {
        Debug.Log("Coroutine func started");
        Debug.Log("next line");
        yield return null;

        Debug.Log("Update 1");
        yield return null;

        Debug.Log("Update 2");

    }
}
