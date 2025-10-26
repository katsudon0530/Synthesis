using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardMove
{
    public class CardMove
    {

        public IEnumerator Slide(Card card, Vector2 goal, float slideDuration)
        {
            float elapsedTime = 0.0f;

            Vector2 start = card.transform.position;

            while (elapsedTime < slideDuration)
            {
                elapsedTime += Time.deltaTime;

                card.transform.position = Vector2.Lerp(start, goal, elapsedTime / slideDuration);
                yield return null;
            }
            card.transform.position = goal;
            yield break;
        }
    }
}

