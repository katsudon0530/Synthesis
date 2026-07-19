using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "UniqueEffects/Reflector")]
public class ReflectorEffect : UniqueEffect
{
    [SerializeField] StatusEffectBase effectBase;
    public override void PlayCondition(Card card, Enemy enemy)
    {
        card.PlayCondition = true;
    }

    //カードの効果処理
    public override IEnumerator Execute(Card card, Card flontCard, Enemy enemy)
    {
        int difenseValue = (int)FlontBuff(card, flontCard);
        Player player = Player.Instance;

        player.Defens += difenseValue;

        if (player.Defens > 100)
        {
            player.Defens = 100;
        }
        MessageText.TextIn($"{player.Defens}ぼうぎょがあがった");

        //すでに反射状態が付与されていた場合
        if (enemy.Base.Effects.Exists(e => e.Base == effectBase))
        {
            //StatusEffect effect = enemy.Base.Effects.Find(e => e.Base == effectBase);
            MessageText.TextIn($"すでに反射状態だ！");
            yield return new WaitForSeconds(1.0f);
            yield break;
        }

        MessageText.TextIn(enemy.EffectGenerator.PlayerGrantEffect(player, effectBase));


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
