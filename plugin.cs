using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Bootstrap;
using HarmonyLib;
using PluginConfig.API;
using PluginConfig.API.Fields;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


namespace Deltakill
{

    [BepInPlugin("DarkFountains.draghtnim.ultrakill", "DarkFountains", "1.0")]
    [BepInDependency("com.earthlingOnFire.UltraFishing", BepInDependency.DependencyFlags.SoftDependency)]

    public class Plugin : BaseUnityPlugin
    {
        public static string modDir;
        private Material badfix;
        private PluginConfigurator config;
        public static bool IsCustomLevel = false;

        private static Plugin _instance;
        bool epmodeinternal = false;
        string OverrideFun;
        static string assemblyLocation = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        public static AssetBundle PitBundle = AssetBundle.LoadFromFile(Path.Combine(assemblyLocation, "DATA_PACKAGE.resource"));
        public static GameObject[] prefabs = PitBundle.LoadAllAssets<GameObject>();
        public static Plugin Instance => _instance;


        private void Start()
        {


        }

        enum FunEnum
        {
            Off,
            Random,
            Always_on
        }

        private void Awake()
        {
            string modPath = Assembly.GetExecutingAssembly().Location.ToString();
            modDir = Path.GetDirectoryName(modPath);
            badfix = GenericHelper.Fetch<Material>("Assets/Materials/Liquids/Limbo Water LowPriority V2Arena.mat");

            GenericHelper.DictonaryFill();

            string assemblyLocation = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);


            config = PluginConfigurator.Create("deltaKILL", "CONFIG_DATA_DELTA");

            config.SetIconWithURL(Path.Combine(assemblyLocation, "icon.png"));
            config.presetButtonHidden = true;

            BoolField EPMode = new BoolField(config.rootPanel, "Light Sensitive Mode", "EPMode", false);

            config.presetButtonInteractable = false;
            Harmony harmony = new Harmony("deltaKILL");
            harmony.PatchAll();

            _instance = this;
            epmodeinternal = EPMode.value;
            EnumField<FunEnum> field = new EnumField<FunEnum>(config.rootPanel, "Fun events", "FUN_OVERRIDE", FunEnum.Random);
            field.SetEnumDisplayName(FunEnum.Always_on, "Always on");

            OverrideFun = field.value.ToString();

            EPMode.onValueChange += (BoolField.BoolValueChangeEvent e) =>
            {
                // Note how only the division is disabled, but all the fields attached to it are affected as well
                epmodeinternal = !EPMode.value;

            };

            field.onValueChange += (EnumField<FunEnum>.EnumValueChangeEvent e) =>
            {

                OverrideFun = e.value.ToString();

            };

            //bundlepit = AssetBundle.LoadFromMemory(Properties.Resources.Crusher1);
            UltraFishingCompat.RegisterExtraFish();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDestroy()
        {
            Logger.LogInfo(SceneManager.GetActiveScene().name);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {


            int fun = (int)UnityEngine.Random.Range(1, 10);
            GameObject[] prefabs = PitBundle.LoadAllAssets<GameObject>();
            GameObject Fountain = GenericHelper.FetchAsset("1 - Darker_Fountain");
            GameObject cache;
            GameObject Funobj;

            if (SceneHelper.CurrentScene != "Main Menu" || SceneHelper.CurrentScene != "Bootstrap" || SceneHelper.CurrentScene != "Intro")
            {
                GenericHelper.ReplaceAssets();
            }
            Logger.LogWarning(epmodeinternal);

            switch (SceneHelper.CurrentScene)
            {



                case "Level 0-2":

                    GenericHelper.FindObjectEvenIfDisabled("5B - Secret Arena", "5B Nonstuff/Pit (2)").transform.position = new UnityEngine.Vector3(0, 0, 798);
                    GenericHelper.FindObjectEvenIfDisabled("5B - Secret Arena", "5B Nonstuff/Pit (3)").transform.position = new UnityEngine.Vector3(0, 0, 778);

                    Fountain = UnityEngine.Object.Instantiate(GenericHelper.FetchAsset("7 - 02IntroFountain"));
                    break;

                case "Level 0-S":

                    GenericHelper.FindObjectEvenIfDisabled("FinalRoom SecretExit").gameObject.SetActive(false);
                    Fountain = UnityEngine.Object.Instantiate(GenericHelper.FetchAsset("0 - 0-S exit"));
                    GenericHelper.FindObjectEvenIfDisabled("Hellgate Textless Variant", "DoorLeft").GetComponent<MeshRenderer>().allowOcclusionWhenDynamic = false;
                    GenericHelper.FindObjectEvenIfDisabled("Hellgate Textless Variant", "DoorRight").GetComponent<MeshRenderer>().allowOcclusionWhenDynamic = false;
                    GenericHelper.FindObjectEvenIfDisabled("0 - 0-S exit(Clone)", "closedoor").GetComponent<ObjectActivator>().events.toDisActivateObjects[0] = GenericHelper.FindObjectEvenIfDisabled("Hellgate Textless Variant", "Opener");



                    break;

                case "Level 1-1":

                    if ((fun < 4 || OverrideFun == "Always_on") && OverrideFun != "Off")
                    {

                        Funobj = UnityEngine.Object.Instantiate(GenericHelper.FetchAsset("C1 - Fun"));
                        Logger.LogWarning(string.Concat("Importing: ", Funobj.gameObject.name));


                        GenericHelper.FindObjectEvenIfDisabled(Funobj.transform.name, SceneHelper.CurrentScene).gameObject.SetActive(true);
                    }


                    GenericHelper.FindObjectEvenIfDisabled("1 - First Field", "1 Stuff/Fountain").gameObject.SetActive(false);

                    Fountain = UnityEngine.Object.Instantiate(GenericHelper.FetchAsset("1 - Darker_Fountain"));
                    Logger.LogInfo(GenericHelper.FindObjectEvenIfDisabled("1 - Darker_Fountain(Clone)").name);
                    Logger.LogInfo(GenericHelper.FindObjectEvenIfDisabled("1 - Darker_Fountain(Clone)", "fountain").name);
                    Logger.LogInfo(GenericHelper.FindObjectEvenIfDisabled("1 - Darker_Fountain(Clone)", "fountain/muzzles/").name);
                    Logger.LogInfo(GenericHelper.FindObjectEvenIfDisabled("1 - Darker_Fountain(Clone)", "fountain/muzzles/muzzleflash (4)").name);
                    Logger.LogInfo(GenericHelper.FindObjectEvenIfDisabled("StatsManager").name);

                    Logger.LogInfo(GenericHelper.FindObjectEvenIfDisabled("1 - Darker_Fountain(Clone)", "fountain/muzzles/muzzleflash (4)").GetComponent<ObjectActivator>().events);

                    GenericHelper.FindObjectEvenIfDisabled("1 - Darker_Fountain(Clone)", "fountain/muzzles/muzzleflash (4)").GetComponent<ObjectActivator>().events.onActivate.AddListener(GenericHelper.FindObjectEvenIfDisabled("StatsManager").GetComponent<StatsManager>().StopTimer);

                    GenericHelper.FindObjectEvenIfDisabled("1 - Darker_Fountain(Clone)", "fountain/Cylinder (1)").GetComponent<CoinActivated>().events.onActivate.AddListener(GenericHelper.FindObjectEvenIfDisabled("FirstRoom", "Room/FinalDoor/DoorLeft").GetComponent<Door>().LockClose);
                    GenericHelper.FindObjectEvenIfDisabled("1 - Darker_Fountain(Clone)", "fountain/Cylinder (1)").GetComponent<CoinActivated>().events.onActivate.AddListener(GenericHelper.FindObjectEvenIfDisabled("FirstRoom", "Room/FinalDoor/DoorRight").GetComponent<Door>().LockClose);
                    cache = GenericHelper.FindObjectEvenIfDisabled("1 - Darker_Fountain(Clone)", "nopass");
                    cache.GetComponent<ObjectActivator>().events.toDisActivateObjects[0] = GenericHelper.FindObjectEvenIfDisabled("LargeDoor", "OpenZone (1)");
                    cache.GetComponent<ObjectActivator>().events.toDisActivateObjects[1] = GenericHelper.FindObjectEvenIfDisabled("LargeDoor (12)", "OpenZone (1)");
                    Fountain.transform.parent = GenericHelper.FindObjectEvenIfDisabled("1 - First Field", "1 Stuff").transform;
                    Logger.LogMessage(GenericHelper.FindObjectEvenIfDisabled("1 - Darker_Fountain(Clone)", "fountain/Cylinder (1)").name);

                    break;
                case "Level 1-S":



                    GenericHelper.FindObjectEvenIfDisabled("5 - Finale", "FinalRoomSecretExit").gameObject.SetActive(false);
                    Fountain = UnityEngine.Object.Instantiate(GenericHelper.FetchAsset("2 - Witless Fountain"));
                    GenericHelper.FindObjectEvenIfDisabled("5 - Finale", "InteractiveScreenPuzzle5x5 (2)/Canvas/Background").GetComponent<PuzzleController>().toActivate[0] = GenericHelper.FindObjectEvenIfDisabled("2 - Witless Fountain(Clone)", "ActivateLimboDoor").gameObject;

                    GenericHelper.FindObjectEvenIfDisabled("2 - Witless Fountain(Clone)", "WaterReplace").gameObject.GetComponent<MeshRenderer>().material = badfix;

                    break;

                case "Level 2-1":


                    if ((fun < 4 || OverrideFun == "Always_on") && OverrideFun != "Off")
                    {

                        Funobj = UnityEngine.Object.Instantiate(GenericHelper.FetchAsset("C1 - Fun"));
                        Logger.LogWarning(string.Concat("Importing: ", Funobj.gameObject.name));


                        GenericHelper.FindObjectEvenIfDisabled(Funobj.transform.name, SceneHelper.CurrentScene).gameObject.SetActive(true);
                        if (epmodeinternal)
                        {
                            GenericHelper.FindObjectEvenIfDisabled(Funobj.transform.name, "Level 2-1/standard").gameObject.SetActive(false);
                            GenericHelper.FindObjectEvenIfDisabled(Funobj.transform.name, "Level 2-1/eplmode").gameObject.SetActive(true);
                        }
                    }


                    break;

                case "Level 2-3":


                    GenericHelper.FindObjectEvenIfDisabled("Exteriors", "Arch (1)").gameObject.SetActive(false);
                    GenericHelper.FindObjectEvenIfDisabled("2 - Sewer Arena", "2 Nonstuff/Secret Level Entrance/FinalRoom SecretEntrance").gameObject.SetActive(false);
                    Fountain = UnityEngine.Object.Instantiate(GenericHelper.FetchAsset("8 - FountainLust"));
                    cache = GenericHelper.FindObjectEvenIfDisabled("2 - Sewer Arena", "2 Nonstuff/Secret Level Entrance/FinalRoom SecretEntrance/Pit");
                    cache.gameObject.SetActive(true);
                    cache.transform.parent = null;
                    cache.transform.position = new UnityEngine.Vector3(5, -30, 800);
                    cache.transform.GetChild(0).position = cache.transform.GetChild(0).position - new Vector3(0, 20, 0);

                    cache = GenericHelper.FindObjectEvenIfDisabled("2 - Sewer Arena", "2 Nonstuff/Secret Level Entrance/FinalRoom SecretEntrance/Pit (2)");
                    cache.gameObject.SetActive(true);
                    cache.transform.parent = null;
                    cache.transform.position = new UnityEngine.Vector3(-15, -30, 800);
                    GenericHelper.FindObjectEvenIfDisabled("8 - FountainLust(Clone)", "addmintofix").GetComponent<ObjectActivatorStay>().toDisActivate[0] = (GenericHelper.FindObjectEvenIfDisabled("MinosBackground"));

                    break;


                case "Level 2-S":

                    GameObject mask = GenericHelper.FindObjectEvenIfDisabled("Canvas", "PowerUpVignette/Panel/Aspect Ratio Mask/");
                    Fountain = UnityEngine.Object.Instantiate(GenericHelper.FetchAsset("2-Sstuff"));


                    //make this a for loop, otherwise it skips over
                    foreach (Transform child in GenericHelper.FindObjectEvenIfDisabled("2-Sstuff(Clone)", "Canvas/AdditionalScreens/").transform)
                    {
                        child.parent = mask.transform;
                        Logger.LogWarning("moved " + child.name);

                    }



                    break;
                case "Level 3-2":
                    if ((fun > 8 || OverrideFun == "Always_on") && OverrideFun != "Off")
                    {
                        GenericHelper.FindObjectEvenIfDisabled("4 - Heart Chamber", "4 Stuff").transform.localPosition = new UnityEngine.Vector3(0, 0, 0);
                        GenericHelper.FindObjectEvenIfDisabled("4 - Heart Chamber", "4 Stuff/Cube").SetActive(false);
                        GenericHelper.FindObjectEvenIfDisabled("4 - Heart Chamber", "4 Nonstuff/MusicActivator").SetActive(false);


                        Funobj = UnityEngine.Object.Instantiate(GenericHelper.FetchAsset("C1 - Fun"));//.GetComponents<ObjectActivator>().First();
;
                        Logger.LogWarning(string.Concat("Importing: ", Funobj.gameObject.name));


                        GenericHelper.FindObjectEvenIfDisabled(Funobj.transform.name, SceneHelper.CurrentScene).gameObject.SetActive(true);
                        GenericHelper.FindObjectEvenIfDisabled(Funobj.transform.name, "Level 3-2/heart/FakeGabrielRouxl").GetComponent<MassAnimationReceiver>().realMass = GenericHelper.FindObjectEvenIfDisabled("4 - Heart Chamber", "4 Stuff/Gabriel");
                        GenericHelper.FindObjectEvenIfDisabled(Funobj.transform.name, "Level 3-2/heart/FakeGabrielRouxl/KillYourOST").GetComponent<ObjectActivator>().events.toDisActivateObjects[0] = GenericHelper.FindObjectEvenIfDisabled("Music 1");
                        GenericHelper.FindObjectEvenIfDisabled(Funobj.transform.name, "Level 3-2/heart/FakeGabrielRouxl/KillYourOST").GetComponent<ObjectActivator>().events.toDisActivateObjects[1] = GenericHelper.FindObjectEvenIfDisabled("Music 2");

                        GenericHelper.FindObjectEvenIfDisabled(Funobj.transform.name, "Level 3-2/heart/Cube").GetComponent<ObjectActivator>().events = GenericHelper.FindObjectEvenIfDisabled("4 - Heart Chamber", "4 Stuff/Cube").GetComponent<ObjectActivator>().events;
                        GenericHelper.FindObjectEvenIfDisabled(Funobj.transform.name, "Level 3-2/heart").transform.parent = GenericHelper.FindObjectEvenIfDisabled("4 - Heart Chamber", "4 Stuff").transform;
                        GenericHelper.FindObjectEvenIfDisabled(Funobj.transform.name, "Level 3-2/hold/MfixPutInGabe").GetComponent<ObjectActivator>().events.toActivateObjects[0] = GenericHelper.FindObjectEvenIfDisabled("Music 3");
                        GenericHelper.FindObjectEvenIfDisabled(Funobj.transform.name, "Level 3-2/hold/MfixPutInGabe").transform.parent = GenericHelper.FindObjectEvenIfDisabled("4 - Heart Chamber", "4 Stuff/Gabriel").transform;

                    }
                    break;

                case "Level 4-2":

                    GenericHelper.FindObjectEvenIfDisabled("OOB Stuff").transform.position = GenericHelper.FindObjectEvenIfDisabled("OOB Stuff").transform.position - new UnityEngine.Vector3(0, 550, 0);

                    GenericHelper.FindObjectEvenIfDisabled("FakeMoon").GetComponent<SphereCollider>().enabled = false;
                    GenericHelper.FindObjectEvenIfDisabled("FakeMoon").GetComponent<MeshRenderer>().enabled = false;
                    GenericHelper.FindObjectEvenIfDisabled("FakeMoon", "Point Light/Spot Light").gameObject.SetActive(false);
                    GenericHelper.FindObjectEvenIfDisabled("FakeMoon", "FinalRoom SecretEntrance/Pit").transform.position = GenericHelper.FindObjectEvenIfDisabled("FakeMoon", "FinalRoom SecretEntrance/Pit").transform.position - new UnityEngine.Vector3(0, 250, 0); ;
                    GenericHelper.FindObjectEvenIfDisabled("FakeMoon", "FinalRoom SecretEntrance/Pit").transform.GetChild(0).gameObject.transform.position = GenericHelper.FindObjectEvenIfDisabled("FakeMoon", "FinalRoom SecretEntrance/Pit").transform.GetChild(0).gameObject.transform.position - new Vector3(0, 50, 0);
                    GenericHelper.FindObjectEvenIfDisabled("FakeMoon", "FinalRoom SecretEntrance/Pit (2)").transform.position = GenericHelper.FindObjectEvenIfDisabled("FakeMoon", "FinalRoom SecretEntrance/Pit (2)").transform.position - new UnityEngine.Vector3(0, 250, 0); ;
                    Fountain = UnityEngine.Object.Instantiate(GenericHelper.FetchAsset("C - FountainGreed"));
                    Fountain.transform.parent = GenericHelper.FindObjectEvenIfDisabled("FakeMoon").transform;

                    break;

                case "Level 4-3":

                    if ((fun > 7 || OverrideFun == "Always_on") && OverrideFun != "Off")
                    {

                        Funobj = UnityEngine.Object.Instantiate(GenericHelper.FetchAsset("C1 - Fun"));
                        GenericHelper.FindObjectEvenIfDisabled(Funobj.transform.name, SceneHelper.CurrentScene).gameObject.SetActive(true);

                        GenericHelper.FindObjectEvenIfDisabled("1 - First Chambers", "1 Nonstuff/Altar").gameObject.SetActive(false);



                        GenericHelper.FindObjectEvenIfDisabled(Funobj.transform.name, "Level 4-3/patternphase1/soundenable").transform.parent = GenericHelper.FindObjectEvenIfDisabled("1 - First Chambers", "1 Stuff/Torches/GreedTorch/OnLight").transform;
                        cache = GenericHelper.FindObjectEvenIfDisabled(Funobj.transform.name, "Level 4-3/SoundwaveObject");
                        cache.transform.parent = GenericHelper.FindObjectEvenIfDisabled("Player", "Agent").transform;
                        cache.SetActive(false);
                        cache.transform.SetSiblingIndex(0);
                        GenericHelper.FindObjectEvenIfDisabled("0-S Check").gameObject.SetActive(false);
                        GenericHelper.FindObjectEvenIfDisabled("7 - Generator Room", "7 Nonstuff/Altar/Altars").gameObject.SetActive(false);
                        GenericHelper.FindObjectEvenIfDisabled("7 - Generator Room", "7 Nonstuff/Altar/Cube").gameObject.SetActive(false);

                        //ItemPlaceZone redaltar = GenericHelper.FindObjectEvenIfDisabled("7 - Generator Room", "Agent").GetComponents<ItemPlaceZone>()[2];



                        GenericHelper.FindObjectEvenIfDisabled(Funobj.transform.name, "Level 4-3/button/Cube").GetComponent<ObjectActivator>().events.toActivateObjects[0] = GenericHelper.FindObjectEvenIfDisabled("7 - Generator Room", "7 Nonstuff/Altar/MusicStopper").gameObject;

                        GenericHelper.FindObjectEvenIfDisabled(Funobj.transform.name, "Level 4-3/button/Cube/delay").GetComponent<ObjectActivator>().events.toActivateObjects[0] = (GenericHelper.FindObjectEvenIfDisabled("7 - Generator Room", "7 Nonstuff/Spinning Cylinder/Lights"));

                    }


                    break;

                case "Level 4-S":

                    GenericHelper.FindObjectEvenIfDisabled("4 - Boulder Run", "4 Stuff/FinalRoom SecretExit").gameObject.SetActive(false);
                    Fountain = UnityEngine.Object.Instantiate(GenericHelper.FetchAsset("4 - greedFountain"));
                    Fountain.transform.parent = GenericHelper.FindObjectEvenIfDisabled("4 - Boulder Run").transform;

                    break;

                case "Level 5-1":
                    if ((fun == 6 || OverrideFun == "Always_on") && OverrideFun != "Off")
                    {
                        GenericHelper.FindObjectEvenIfDisabled("IntroParent", "Intro/Intro B - First Tunnel/Cube(Clone)").gameObject.SetActive(false);
                        GameObject room_mysteryman = UnityEngine.Object.Instantiate(GenericHelper.FetchAsset("6 - room_mysteryman"));
                        room_mysteryman.transform.parent = GenericHelper.FindObjectEvenIfDisabled("IntroParent", "Intro/Intro B - First Tunnel").transform;
                        room_mysteryman.transform.localPosition = new UnityEngine.Vector3(-22.6f, 0f, -7.5f);
                    }

                    Fountain = UnityEngine.Object.Instantiate(GenericHelper.FetchAsset("9 - doorFountain"));


                    GenericHelper.FindObjectEvenIfDisabled("2 - Elevator", "2B Secret/FinalRoom SecretEntrance").gameObject.SetActive(false);

                    cache = GenericHelper.FindObjectEvenIfDisabled("2 - Elevator", "2B Secret/FinalRoom SecretEntrance/Pit");
                    cache.gameObject.SetActive(true);
                    cache.transform.parent = null;
                    cache.transform.position = new UnityEngine.Vector3(5, -30, 800);
                    cache.transform.GetChild(0).position = cache.transform.GetChild(0).position - new Vector3(0, 20, 0);

                    cache = GenericHelper.FindObjectEvenIfDisabled("2 - Elevator", "2B Secret/FinalRoom SecretEntrance/Pit (2)");
                    cache.gameObject.SetActive(true);
                    cache.transform.parent = null;
                    cache.transform.position = new UnityEngine.Vector3(-15, -30, 800);



                    break;

                case "Level 5-S":

                    GenericHelper.FindObjectEvenIfDisabled("FinalRoom SecretExit").gameObject.SetActive(false);
                    Logger.LogError(GenericHelper.FetchAsset("3- Fishing-Fountain"));
                    Fountain = UnityEngine.Object.Instantiate(GenericHelper.FetchAsset("3- Fishing-Fountain"));

                    break;

                case "Level 7-3":


                    if ((fun < 4 || OverrideFun == "Always_on") && OverrideFun != "Off")
                    {

                        Funobj = UnityEngine.Object.Instantiate(GenericHelper.FetchAsset("C1 - Fun"));
                        Logger.LogWarning(string.Concat("Importing: ", Funobj.gameObject.name));


                        GenericHelper.FindObjectEvenIfDisabled(Funobj.transform.name, SceneHelper.CurrentScene).gameObject.SetActive(true);

                    }

                    GenericHelper.FindObjectEvenIfDisabled("2 - Garden Maze", "Secret/FinalRoom SecretEntrance").gameObject.SetActive(false);
                    GenericHelper.FindObjectEvenIfDisabled("2 - Garden Maze", "Secret", 3, true).gameObject.SetActive(false);
                    GenericHelper.FindObjectEvenIfDisabled("2 - Garden Maze", "Secret", 4, true).gameObject.SetActive(false);
                    GenericHelper.FindObjectEvenIfDisabled("2 - Garden Maze", "Secret", 5, true).gameObject.SetActive(false);
                    GenericHelper.FindObjectEvenIfDisabled("2 - Garden Maze", "Secret", 6, true).gameObject.GetComponent<MeshRenderer>().enabled = false;
                    Fountain = UnityEngine.Object.Instantiate(GenericHelper.FetchAsset("B - ViolentEntrance"));

                    cache = GenericHelper.FindObjectEvenIfDisabled("2 - Garden Maze", "Secret/FinalRoom SecretEntrance/Pit");
                    cache.gameObject.SetActive(true);
                    cache.transform.parent = null;
                    cache.transform.position = new UnityEngine.Vector3(-5, -30, 800);

                    cache.transform.GetChild(0).position = cache.transform.GetChild(0).position - new Vector3(0, 20, 0);

                    cache = GenericHelper.FindObjectEvenIfDisabled("2 - Garden Maze", "Secret/FinalRoom SecretEntrance/Pit (2)");
                    cache.gameObject.SetActive(true);
                    cache.transform.parent = null;
                    cache.transform.position = new UnityEngine.Vector3(15, -30, 800);




                    break;

                case "Level 7-S":



                    GenericHelper.FindObjectEvenIfDisabled("Fake Exit").gameObject.SetActive(false);
                    GenericHelper.FindObjectEvenIfDisabled("7-S_Unpaintable", "Interior/Blackout").gameObject.SetActive(false);
                    GenericHelper.FindObjectEvenIfDisabled("7-S_Unpaintable", "Doorways/Door_009").gameObject.SetActive(false);



                    Fountain = UnityEngine.Object.Instantiate(GenericHelper.FetchAsset("5 - fountainWash"));
                    cache = GenericHelper.FindObjectEvenIfDisabled("Fake Exit", "WasherPickup");
                    cache.transform.parent = GenericHelper.FindObjectEvenIfDisabled("5 - fountainWash(Clone)", "enable/washreplace").transform;
                    cache.transform.localPosition = new UnityEngine.Vector3(0f, 0f, 0f);
                    cache.transform.rotation = UnityEngine.Quaternion.identity;

                    GenericHelper.FindObjectEvenIfDisabled("5 - fountainWash(Clone)", "thumbsuptasque (1)").GetComponent<ObjectActivator>().events.onActivate.AddListener(GenericHelper.FindObjectEvenIfDisabled("Checkpoints", "Checkpoint (2)").GetComponent<CheckPoint>().ReactivateCheckpoint);
                    GenericHelper.FindObjectEvenIfDisabled("Checkpoints", "Checkpoint (2)").GetComponent<CheckPoint>().toActivate = GenericHelper.FindObjectEvenIfDisabled("5 - fountainWash(Clone)");

                    cache = GenericHelper.FindObjectEvenIfDisabled("Fake Exit", "VacuumPickup");
                    cache.transform.parent = GenericHelper.FindObjectEvenIfDisabled("5 - fountainWash(Clone)", "enable/vacreplace").transform;
                    cache.transform.localPosition = new UnityEngine.Vector3(0f, 0f, 0f);
                    cache.transform.rotation = UnityEngine.Quaternion.identity;

                    cache = GenericHelper.FindObjectEvenIfDisabled("5 - fountainWash(Clone)", "Swatches - parent/swatchLadder");
                    cache.transform.parent = GenericHelper.FindObjectEvenIfDisabled("Interactives", "MainHall/MainHall Stuff/Sliding Ladder/Ladder_Collision/Ladder").transform;
                    cache.transform.localPosition = new UnityEngine.Vector3(0f, -0.8f, 21.35f);

                    cache = GenericHelper.FindObjectEvenIfDisabled("5 - fountainWash(Clone)", "Swatches - parent/swatchWalk");
                    cache.transform.parent = GenericHelper.FindObjectEvenIfDisabled("7-S_Unpaintable", "Interior/Library Chandelier").transform;
                    cache.transform.localPosition = new UnityEngine.Vector3(0f, -7.66f, 0f);
                    GenericHelper.FindObjectEvenIfDisabled("7-S_Paintable").GetComponent<BloodCheckerManager>().finalDoorOpener = GenericHelper.FindObjectEvenIfDisabled("5 - fountainWash(Clone)", "DoorOpenerReplacement");

                    GenericHelper.FindObjectEvenIfDisabled("Interactives", "MainHall/MainHall NonStuff/Altar (Blue Skull) Variant/Cube").GetComponent<ItemPlaceZone>().activateOnSuccess[0] = GenericHelper.FindObjectEvenIfDisabled("5 - fountainWash(Clone)", "3 Exit (2)/Open").gameObject;


                    break;
            }

            //FRIEND
            try
            {
                if (fun <= 3)
                {


                    GameObject friend = UnityEngine.Object.Instantiate(GenericHelper.FetchAsset("A - OBJECT_FRIEND"));
                    GenericHelper.FindObjectEvenIfDisabled(friend.transform.name, SceneHelper.CurrentScene).gameObject.SetActive(true);

                }
            }
            catch (Exception)
            {
                Logger.LogInfo("VERY INTERESTING");
            }


            Logger.LogWarning(string.Concat("Importing: ", Fountain.gameObject.name));
        }





    }




    [HarmonyPatch] // <--- this is **ESSENTIAL** for this to work 
    public class PlayerAnimationsPatch
    {


        [HarmonyPatch(typeof(PlayerAnimations), "PlayFootstepClip")]
        [HarmonyPostfix]
        public static void FootStepPatch(PlayerAnimations __instance)
        {

            if (__instance.transform.GetChild(0).name == "SoundwaveObject")
            {

                GameObject test = GameObject.Instantiate(__instance.transform.GetChild(0).gameObject);
                test.transform.parent = null;
                test.transform.position = __instance.transform.position;


                test.transform.GetChild(0).GetComponent<AddForce>().force = test.transform.GetChild(0).GetComponent<PlayerTracker>().GetPlayerVelocity();
                test.SetActive(true);

            }

        }




    }


}


