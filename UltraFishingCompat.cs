using UltraFishing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace Deltakill
{

    public static class UltraFishingCompat
    {
        private static bool? _enabled;

        public static bool enabled
        {
            get
            {
                if (_enabled == null)
                {
                    _enabled = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.earthlingOnFire.UltraFishing");
                }
                return (bool)_enabled;
            }
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        public static void RegisterExtraFish()
        {
            string savePath = Path.Combine(Plugin.modDir, "DATA_FISH.save");

            FishCollection collection = new FishCollection("DELTAKILL");

            List<FishObject> fish = new List<FishObject> { };
            fish.Add(GenericHelper.Fetch<FishObject>("ralsei"));
            for (int i = 0; i < fish.Count; i++) {
                collection.RegisterFish(fish[i], savePath, i);
            }
            GlobalFishManager.RegisterCollection(collection);
        }
    }
}