using FSPRO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Actions
{
    public class ADrawFromDiscard : CardAction
    {
        public Card exclude;
        public int count = 1;
        public override void Begin(G g, State s, Combat c)
        {
            bool flag = false;
            int num = 0;
            for (int i = 0; i < count; i++)
            {
                if (c.hand.Count >= 10)
                {
                    c.PulseFullHandWarning();
                    break;
                }

                if (c.discard.Count == 0)
                {
                    break;
                }

                if (c.discard[c.discard.Count-1-i] == exclude)
                {
                    continue;
                }

                if (!flag)
                {
                    Audio.Play(Event.CardHandling);
                    flag = true;
                }

                c.DrawCardIdx(s, c.discard.Count - 1, CardDestination.Discard).waitBeforeMoving = (double)i * 0.09;
                num++;
            }

            foreach (Artifact item2 in s.EnumerateAllArtifacts())
            {
                item2.OnDrawCard(s, c, num);
            }
        }

        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> list = new List<Tooltip>();
            list.Add(new TTGlossary(Mod.glossaries["DrawDiscard"].Head, count));
            return list;
        }

        public override Icon? GetIcon(State s)
        {
            return new Icon(Mod.sprites["ActionDrawFromDiscard"], count, Colors.textMain);
        }
    }
}
