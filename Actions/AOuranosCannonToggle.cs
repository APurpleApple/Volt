using FMOD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APurpleApple_VoltMod.Actions
{
    internal class AOuranosCannonToggle : CardAction
    {
        public bool targetPlayer = true;
        public bool mute;
        public bool active = true;

        public override void Begin(G g, State s, Combat c)
        {
            List<Part> parts = s.ship.parts;
            for (int index = 0; index < parts.Count; ++index)
            {
                if (active)
                {
                    if (parts[index].skin == "@mod_part:APurpleApple.VoltMod.Part.OuranosCannonInactive")
                    {
                        parts[index].skin = "@mod_part:APurpleApple.VoltMod.Part.OuranosCannon";
                    }
                }
                else
                {
                    if (parts[index].skin == "@mod_part:APurpleApple.VoltMod.Part.OuranosCannon")
                    {
                        parts[index].skin = "@mod_part:APurpleApple.VoltMod.Part.OuranosCannonInactive";
                    }
                }
            }
            Audio.Play(new GUID?(FSPRO.Event.TogglePart));
        }
    }
}
