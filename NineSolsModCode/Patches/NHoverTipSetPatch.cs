using System.Reflection;
using BaseLib.Utils;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.HoverTips;

namespace NineSolsMod.NineSolsModCode.Patches;

[HarmonyPatch(typeof(NHoverTipSet), "CorrectVerticalOverflow")]
public static class NHoverTipSet_CorrectVerticalOverflow_Patch
{
    // 传入私有字段 _textHoverTipContainer 和 _cardHoverTipContainer，Harmony 会自动映射
    public static bool Prefix(NHoverTipSet __instance,
                              VFlowContainer ____textHoverTipContainer,
                              NHoverTipCardContainer ____cardHoverTipContainer)
    {
        float y = NGame.Instance.GetViewportRect().Size.Y;
        float padding = 10f;

        // 重置缩放
        ____cardHoverTipContainer.Scale = Vector2.One;
        ____textHoverTipContainer.Scale = Vector2.One;

        // === 处理 Text 容器 ===
        if (____textHoverTipContainer.GlobalPosition.Y + ____textHoverTipContainer.Size.Y > y)
        {
            ____textHoverTipContainer.GlobalPosition = new Vector2(____textHoverTipContainer.GlobalPosition.X, y - ____textHoverTipContainer.Size.Y - padding);
        }
        if (____textHoverTipContainer.GlobalPosition.Y < padding)
        {
            ____textHoverTipContainer.GlobalPosition = new Vector2(____textHoverTipContainer.GlobalPosition.X, padding);
        }

        // === 处理 Card 容器 ===
        if (____cardHoverTipContainer.GlobalPosition.Y + ____cardHoverTipContainer.Size.Y > y)
        {
            ____cardHoverTipContainer.GlobalPosition = new Vector2(____cardHoverTipContainer.GlobalPosition.X, y - ____cardHoverTipContainer.Size.Y - padding);
        }

        // 防止顶部超出屏幕并进行缩放
        if (____cardHoverTipContainer.GlobalPosition.Y < padding)
        {
            ____cardHoverTipContainer.GlobalPosition = new Vector2(____cardHoverTipContainer.GlobalPosition.X, padding);

            float availableHeight = y - (padding * 2);
            if (____cardHoverTipContainer.Size.Y > availableHeight)
            {
                float scaleRatio = availableHeight / ____cardHoverTipContainer.Size.Y;
                ____cardHoverTipContainer.Scale = new Vector2(scaleRatio, scaleRatio);
            }
        }

        // 返回 false 拦截原方法的执行
        return false;
    }
}

// ==========================================
// Patch 2: 修复水平越界 (CorrectHorizontalOverflow)
// ==========================================
[HarmonyPatch(typeof(NHoverTipSet), "CorrectHorizontalOverflow")]
public static class NHoverTipSet_CorrectHorizontalOverflow_Patch
{
    public static bool Prefix(NHoverTipSet __instance,
                              VFlowContainer ____textHoverTipContainer,
                              NHoverTipCardContainer ____cardHoverTipContainer)
    {
        float x = NGame.Instance.GetViewportRect().Size.X;
        float padding = 10f;

        Vector2 globalPosition = ____cardHoverTipContainer.GlobalPosition;
        // 获取时乘上缩放值
        float x2 = ____cardHoverTipContainer.Size.X * ____cardHoverTipContainer.Scale.X;

        Vector2 globalPosition2 = ____textHoverTipContainer.GlobalPosition;
        float x3 = ____textHoverTipContainer.Size.X * ____textHoverTipContainer.Scale.X;

        if (globalPosition.X + x2 <= x && globalPosition2.X + x3 > x)
        {
            float x4 = globalPosition.X - x3;
            ____textHoverTipContainer.GlobalPosition = new Vector2(x4, globalPosition.Y);
        }
        else if (globalPosition.X + x2 > x || globalPosition2.X + x3 > x)
        {
            float x5 = globalPosition2.X + x3 - x2;
            ____cardHoverTipContainer.GlobalPosition = new Vector2(x5, globalPosition.Y);
            ____textHoverTipContainer.GlobalPosition += Vector2.Left * x2;
        }
        else if (globalPosition.X < padding || globalPosition2.X < padding)
        {
            float x6 = Mathf.Max(globalPosition2.X, padding);
            ____cardHoverTipContainer.GlobalPosition = new Vector2(x6, globalPosition.Y);
            ____textHoverTipContainer.GlobalPosition += Vector2.Right * x2;
        }

        // 返回 false 拦截原方法的执行
        return false;
    }
}