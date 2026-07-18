using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.CullingGroup;
using static UnityEngine.UIElements.VisualElement;

public class DungeonManager : MonoBehaviour
{
    [SerializeField] GameMaster gameMaster;
    [SerializeField] Dungeon dungeon;
    [SerializeField] Text floorText;
    [SerializeField] CanvasGroup floorPanel;
    private Enemy enemy;
    float fadeInTime = 0.3f;
    float fadeOutTime = 0.5f;

    private void Awake()
    {
        if (gameMaster == null)
        {
            gameMaster = FindAnyObjectByType<GameMaster>();
            if (gameMaster == null)
                Debug.LogWarning("シーン上にgameMasterが存在していません");
        }
    }

    private void Start()
    {
        StartCoroutine(DungeonMode());
    }

    public IEnumerator DungeonMode()
    {
        for (int i = 0; i < dungeon.Hierachies.Count; i++)
        {
            yield return StartCoroutine(Move(i, dungeon.Hierachies.Count));
            while (enemy　!= null)
            {
                yield return StartCoroutine(gameMaster.TurnStart());
            }
        }
        yield return StartCoroutine(gameMaster.ResultTurn());
    }


    private void DungeonMove(int hierachy)
    {
        int dungeonEnemy = dungeon.Hierachies[hierachy].EnemyProbabilities[0].enemyBase.EnemyID;
        enemy = gameMaster.EnemySet(dungeonEnemy);
    }

    public IEnumerator Move(int nowFloor, int allFloor)
    {
        floorText.text = ($"フロア　{nowFloor + 1}/{allFloor}");
        if(nowFloor != 0)
            yield return StartCoroutine(FadeIn());
        else
            floorPanel.alpha = 1f;

        DungeonMove(nowFloor);
        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(FadeOut());
        yield break;
    }

    public IEnumerator FadeOut()
    {
        float time = 0f;
        while (time < fadeOutTime)
        {
            time += Time.deltaTime;
            floorPanel.alpha = Mathf.Lerp(1f, 0f, time / fadeOutTime);
            yield return null;
        }
        floorPanel.alpha = 0f;
        floorPanel.interactable = false;
        floorPanel.blocksRaycasts = false;
    }

    public IEnumerator FadeIn()
    {
        float time = 0f;
        while (time < fadeInTime)
        {
            time += Time.deltaTime;
            floorPanel.alpha = Mathf.Lerp(0f, 1f, time / fadeInTime);
            yield return null;
        }
        floorPanel.alpha = 1f;
        floorPanel.interactable = true;
        floorPanel.blocksRaycasts = true;
    }
}
