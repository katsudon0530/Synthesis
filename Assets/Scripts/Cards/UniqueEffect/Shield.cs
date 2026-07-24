using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UniqueEffects/Shield")]
public class ShieldEffect : UniqueEffect
{
    public override void PlayCondition(Card card, Enemy enemy)
    {
        card.PlayCondition = true;
    }

    //カードの効果処理
    public override IEnumerator Execute(BattleContext battle)
    {
        int difenseValue = (int)FlontBuff(battle.card, battle.flontCard);
        Player player = Player.Instance;

        player.Defens += difenseValue;
        
        if (player.Defens > 100)
        {
            player.Defens = 100;
        }
        MessageText.TextIn($"{player.Defens}ぼうぎょがあがった");

        yield break;
    }

    //一枚前のカードの追加効果処理
    public float FlontBuff(Card card, Card flontCard)
    {
        float difenseValue = card.Base.CardStatus.Defense_Status;

        if (flontCard == null)
        {
            return (int)difenseValue;
        }
        else
        {
            string cardName = flontCard.Base.CardName;
            FlontBuff foundBuff = card.Base.FlontBuff.Find(buff => buff.flontCard == cardName);

            if (foundBuff == null)
            {
                return (int)difenseValue;
            }
            difenseValue *= foundBuff.buff;
            return (int)difenseValue;
        }

    }
}

