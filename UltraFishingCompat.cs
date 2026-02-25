using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using BepInEx;
using UltraFishing;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;


namespace Deltakill
{

    public class UltraFishingCompat : BaseUnityPlugin
    {

        public static bool IsCustomLevel = false;
        private static Plugin _instance;
        private static GameObject pref = null;



        public static Plugin Instance => _instance;

        private static Dictionary<string, GameObject> BundleDictionary = new Dictionary<string, GameObject>();

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

            List<FishObject> fish = Plugin.PitBundle.LoadAllAssets<FishObject>().ToList<FishObject>();

            for (int i = 0; i < fish.Count; i++)
            {
                collection.RegisterFish(fish[i], savePath, i);

            }

            GlobalFishManager.RegisterCollection(collection);

        }
    }
}


