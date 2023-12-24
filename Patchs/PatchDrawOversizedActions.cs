﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APurpleApple_VoltMod.Actions;
using HarmonyLib;

namespace APurpleApple_VoltMod.Patchs
{
    [HarmonyPatch]
    public static class PatchDrawOversizedActions
    {
        [HarmonyPatch(typeof(Card), nameof(Card.RenderAction)), HarmonyPostfix]
        public static void RenderOversizedActionsPostfix(CardAction action, G g, bool dontDraw)
        {
            if (!dontDraw && action is IAOversized oversized)
            {
                Rect? rect = new Rect(-1 + oversized.offset, -1);
                Vec xy = g.Push(null, rect).rect.xy;
                Spr? id = oversized.icon.path;
                double x = xy.x;
                double y = xy.y;
                Color? color = new Color("ffffff");
                Draw.Sprite(id, x, y, flipX: false, flipY: false, 0.0, null, null, null, null, color);
                g.Pop();
            }
        }
    }
}
