using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace JJBA
{
    public class DOTweenTest : MonoBehaviour
    {
        public Transform target; // Объект, за которым будем следовать
        public float duration = 1f; // Время, за которое происходит движение к цели
        public float followDistance = 1f; // Дистанция до объекта

        void Update()
        {
            // Проверяем расстояние до цели
            if (Vector3.Distance(transform.position, target.position) > followDistance)
            {
                // Плавно перемещаем объект к цели с помощью DOTween
                transform.DOMove(target.position, duration);
            }
        }
    }
}
