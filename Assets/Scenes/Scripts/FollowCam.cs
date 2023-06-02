using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ;
    private void Awake()
    {
        camZ = this.transform.position.z;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //якщо нема потрібного об'єкта - повернути P:[0, 0, 0]
        Vector3 destination;
        if (POI == null)
            destination = Vector3.zero;
        else
        {
            //отримати позиції потрібного об'єкта
            destination = POI.transform.position;
            //якщо потрібний об'єкт - знаряд, впевнитися, що він зупинився
            if (POI.tag == "Projectile")
            {
                //якщо він стоїть на місці - повернути початкові налаштування поля зору камери в наступному кадрі
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    POI = null;
                    return;
                }
            }
        }
        //обмежити Х та У мінімальними значеннями
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = camZ;
        transform.position = destination;
        Camera.main.orthographicSize = destination.y + 10;
    }
}
