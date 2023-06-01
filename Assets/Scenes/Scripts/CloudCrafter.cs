using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in Inspector")]
    public int numClouds = 40;  //число хмар
    public GameObject cloudPrefab;  //шаблон для хмар
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1; //мінімальний масштаб кожної хмари
    public float cloudScaleMax = 3; //макссимальний масштаб кожної хмари
    public float cloudSpeedMult = 0.5f; //коефіцієнт швидкості хмар

    private GameObject[] cloudInstrnces;

    // Start is called before the first frame update
    void Awake()
    {
        //створити масив для збереження всіх екземплярів хмар
        cloudInstrnces = new GameObject[numClouds];
        //знайти батьківський ігровий об'єкт CloudAnchor
        GameObject anchor = GameObject.Find("CloudAnchor");
        //створити в циклі задану кількість хмар
        GameObject cloud;
        for (int i = 0; i < numClouds; i++)
        {
            //створити екземпляр cloudPrefab
            cloud = Instantiate<GameObject>(cloudPrefab);
            //вибрати позицію для хмари
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            //масштабувати хмару
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //менші хмари (з меншим значенням scaleU) повинні бути ближче до землі
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            //менші хмари повинні бути далі
            cPos.z = 100 - 90 * scaleU;
            //застосувати отримані значення координат і масштаба хмар
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            //зробити хмару дочернім anchor
            cloud.transform.SetParent(anchor.transform);
            //добавити хмару в масив cloudInstances
            cloudInstrnces[i] = cloud;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //обійти в циклі всі значення
        foreach (GameObject cloud in cloudInstrnces)
        {
            //отримати масштаб і координати хмари
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            //збільшити швидкість для ближніх хмар
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            //якщо хмара змістилась занадто далеко вліво - перемістити її вправо
            if (cPos.x <= cloudPosMin.x)
                cPos.x = cloudPosMax.x;
            //застосувати нові координати
            cloud.transform.position = cPos;
        }
    }
}
