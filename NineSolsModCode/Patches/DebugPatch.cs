namespace NineSolsMod.NineSolsModCode.Patches;

// [HarmonyPatch]
// public class FindNodePatch
// {
//     // This tells Harmony specifically WHICH generic version to patch
//     static MethodBase TargetMethod()
//     {
//         // 1. Use BindingFlags to search for Static AND Non-Public/Internal methods
//         var flags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
//         
//         MethodInfo? method = typeof(CustomAnimation).GetMethod("FindNode", flags);
//
//         // 2. Add a safety check! This stops the NullReferenceException 
//         // and gives you a clear error if the method still can't be found.
//         if (method is null)
//         {
//             throw new Exception("Harmony Patch Failed: Could not find the method 'FindNode' in CustomAnimation.");
//         }
//
//         // 3. Turn it into the specific generic version you want to patch.
//         // CHANGE typeof(Node) to whatever specific Godot type you are trying to intercept!
//         // (e.g. typeof(Sprite2D), typeof(Control), etc.)
//         return method.MakeGenericMethod(typeof(Node)); 
//     }
//
//     // Your standard Prefix/Postfix goes here
//     static void Postfix(Node root, string name, ref Node __result)
//     {
//         // Your mod logic here
//         MainFile.Logger.Info("Patching FindNode in BaseLib");
//         MainFile.Logger.Info($"Node: {__result}, root: {root}, name: {name}");
//     }
// }
//
// [HarmonyPatch(typeof(CustomAnimation), nameof(CustomAnimation.PlayCustomAnimation))]
// class CustomAnimationPlayCustomAnimationPatch
// { 
//     static void Postfix(Node n, bool __result, params string[] tryAnimNames)
//     {
//         MainFile.Logger.Info("Patching PlayCustomAnimation in CustomAnimation");
//         MainFile.Logger.Info($"Node: {n}, result: {__result}");
//         foreach (var name in tryAnimNames)  
//         {
//             MainFile.Logger.Info($"animName: {name}");
//         }
//     }
// }
//
// [HarmonyPatch(typeof(NCreature), nameof(NCreature.SetAnimationTrigger))]
// static class CustomAnimationPatch
// {
//     [HarmonyPrefix]
//     public static void Postfix(NCreature __instance, string trigger)
//     {
//         MainFile.Logger.Info("Patching SetAnimationTrigger in NCreature");
//         MainFile.Logger.Info($"Creature: {__instance}, trigger: {trigger}");
//     }
// }