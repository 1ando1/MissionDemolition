using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    //статичне поле, доступне будь-якому іншому коду
    static public bool goalMet = false;

    void OnTriggerEnter(Collider other)
    {
        //коли область дії тригера попадає під щось - перевірити, чи це "щось" не є знарядом
        if (other.gameObject.tag == "Projectile")
        {
            //якщо це знаряд - присвоїти полю goalMet значення true, також змінити альфа-канал кольора, щоб збільщити
            //непрозорість
            Goal.goalMet = true;
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 1;
            mat.color = c;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
