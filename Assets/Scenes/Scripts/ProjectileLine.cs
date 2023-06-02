using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; //одининий об'єкт

    [Header("Set in Inspector")]
    public float minDist = 1.0f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    // Start is called before the first frame update
    void Awake()
    {
        S = this; //встановити посилання на одининий об'єкт
        //отримати посилання на LineRender
        line = GetComponent<LineRenderer>();
        //виключити LineRender, поки він не знадобиться
        line.enabled = false;
        //ініціалізувати список точок
        points = new List<Vector3>();
    }
    public GameObject poi
    {
        get { return (_poi); }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                //якщо поле _poi має дійсне посилання, скинути всі остальні параметри в початковий стан
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }
    //цей метод можна викликати, щоб стерти лінію
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }
    public void AddPoint()
    {
        //викликається для додавання точки в лінії
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
            //якщо точка недостатньо далека від попередньої - просто вийти
            return;

        if (points.Count == 0)
        {
            //якщо це точка запуску, додати додатковий фрагмент лінії, щоб допомогти краще прицілитися надалі
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;

            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            //встановити перші 2 точки
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            //вклюити LineRender
            line.enabled = true;
        }
        else
        {
            //звичайна послідовність додавання точки
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }
    //повертає місцезнаходження останньої точки
    public Vector3 lastPoint
    { 
        get
        {
            if (points == null)
                return (Vector3.zero); //якщо точок нема - повернути Vector3.zero
            return (points[points.Count - 1]);
        }
    }
    private void FixedUpdate()
    {
        if (poi == null)
            //якщо poi має пусте значення, знайти потрібний об'єкт
            if (FollowCam.POI != null)
                if (FollowCam.POI.tag == "Projectile")
                    poi = FollowCam.POI;
                else
                    return; //вийти, якщо об'єкт не знайдений
            else
                return; //вийти, якщо об'єкт не знайдений

        //якщо потрібний об'єкт знайдено - спробувати додати точку з його координатами в кожному FixedUpdate
        AddPoint();
        if (FollowCam.POI == null)
            poi = null; //якщо FollowCam.POI має null, записати null в poi
    }
}
