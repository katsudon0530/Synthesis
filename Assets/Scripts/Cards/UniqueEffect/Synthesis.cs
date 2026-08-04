using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UniqueEffects/Synthesis")]
public class SynthesisEffect : UniqueEffect
{
    public override void PlayCondition(Card card, Enemy enemy)
    {
        card.PlayCondition = true;
    }

    //カードの効果処理
    public override IEnumerator Execute(BattleContext battle)
    {
        GameData.Instance.synthesisCount++;
        battle.log.SendMessage($"合成可能回数が1回増えた！");
        yield return new WaitForSeconds(1.0f);

        int index = Deck.Instance.DeckAll.FindIndex(number => number == battle.card.Base.ID);
        Deck.Instance.DeckAll.RemoveAt(index);
        battle.log.SendMessage($"カードがデッキから消滅した");
        yield break;
    }
}

