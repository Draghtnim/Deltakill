using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using HarmonyLib;
using PluginConfig.API;
using PluginConfig.API.Fields;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


namespace Ultrapit
{

    [BepInPlugin("DarkFountains.draghtnim.ultrakill", "DarkFountains", "1.0")]
    public class Plugin : BaseUnityPlugin
    {
        private PluginConfigurator config;
        private AssetBundle terminal;
        public static bool IsCustomLevel = false;
        private UnityEngine.SceneManagement.Scene scene;
        private GameObject pitobj;
        private GameObject newpitobj;
        AssetBundle bundlepit;
        private static Plugin _instance;
        private static Shader MainShader;
        bool epmodeinternal = false;
        string OverrideFun;
        static string assemblyLocation = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        static AssetBundle PitBundle = AssetBundle.LoadFromFile(Path.Combine(assemblyLocation, "DATA_PACKAGE.resource"));
        static GameObject[] prefabs = PitBundle.LoadAllAssets<GameObject>();

        public static Plugin Instance => _instance;



        //Plonk code

        ///*
        public static void ReplaceShader(Material mat, Shader shader)
        {
            if ((UnityEngine.Object)(object)mat == (UnityEngine.Object)null || (UnityEngine.Object)(object)mat.shader == (UnityEngine.Object)null)
            {
                return;
            }
            int renderQueue = mat.renderQueue;
            Shader shader2 = mat.shader;
            if ((UnityEngine.Object)(object)Shader.Find(((UnityEngine.Object)shader2).name) != (UnityEngine.Object)null)
            {
                if (((UnityEngine.Object)mat.shader).name != "Standard")
                {
                    mat.shader = Shader.Find(((UnityEngine.Object)shader2).name);
                }
                else
                {
                    mat.shader = shader;
                }
                mat.renderQueue = renderQueue;
            }
            else if (((UnityEngine.Object)shader2).name == ((UnityEngine.Object)shader).name)
            {
                mat.shader = shader;
                mat.renderQueue = renderQueue;
            }
            else
            {
                mat.renderQueue = renderQueue;
            }
        }

        public static void ReplaceAssets()
        {

            List<Material> list = new List<Material>();
            Dictionary<string, AudioMixer> dictionary = new Dictionary<string, AudioMixer>();
            dictionary["AllAudio"] = Addressables.LoadAssetAsync<AudioMixer>((object)"AllAudio").WaitForCompletion();
            dictionary["DoorAudio"] = Addressables.LoadAssetAsync<AudioMixer>((object)"DoorAudio").WaitForCompletion();
            dictionary["GoreAudio"] = Addressables.LoadAssetAsync<AudioMixer>((object)"GoreAudio").WaitForCompletion();
            dictionary["MusicAudio"] = Addressables.LoadAssetAsync<AudioMixer>((object)"MusicAudio").WaitForCompletion();
            dictionary["UnfreezeableAudio"] = Addressables.LoadAssetAsync<AudioMixer>((object)"UnfreezeableAudio").WaitForCompletion();
            GameObject[] array = PitBundle.LoadAllAssets<GameObject>();
            //Material[] sharedMaterials;
            foreach (GameObject val in array)
            {
                if (val.GetComponentsInChildren<AudioSource>(true) != null)
                {
                    AudioSource[] componentsInChildren = val.GetComponentsInChildren<AudioSource>(true);
                    foreach (AudioSource val2 in componentsInChildren)
                    {
                        if ((UnityEngine.Object)(object)val2.outputAudioMixerGroup != (UnityEngine.Object)null && dictionary.TryGetValue(((UnityEngine.Object)val2.outputAudioMixerGroup.audioMixer).name, out var value))
                        {
                            val2.outputAudioMixerGroup.audioMixer.outputAudioMixerGroup = value.FindMatchingGroups("Master").FirstOrDefault();
                        }
                    }
                }
                /*if (val.GetComponentsInChildren<Renderer>(true) != null)
                {
                    Renderer[] componentsInChildren2 = val.GetComponentsInChildren<Renderer>(true);
                    foreach (Renderer val3 in componentsInChildren2)
                    {
                        if ((UnityEngine.Object)(object)val3.sharedMaterial != (UnityEngine.Object)null)
                        {
                            ReplaceShader(val3.sharedMaterial, MainShader);
                        }
                        if (val3.sharedMaterials != null && val3.sharedMaterials.Length != 0)
                        {
                            sharedMaterials = val3.sharedMaterials;
                            foreach (Material val4 in sharedMaterials)
                            {
                                list.Add(val4);
                                ReplaceShader(val4, MainShader);
                            }
                        }
                    }
                }
                if (val.GetComponentsInChildren<ParticleSystemRenderer>(true) == null)
                {
                    continue;
                }
                ParticleSystemRenderer[] componentsInChildren3 = val.GetComponentsInChildren<ParticleSystemRenderer>(true);
                foreach (ParticleSystemRenderer val5 in componentsInChildren3)
                {
                    if ((UnityEngine.Object)(object)((Renderer)val5).sharedMaterial != (UnityEngine.Object)null)
                    {
                        ReplaceShader(((Renderer)val5).sharedMaterial, MainShader);
                    }
                    if (((Renderer)val5).sharedMaterials != null && ((Renderer)val5).sharedMaterials.Length != 0)
                    {
                        sharedMaterials = ((Renderer)val5).sharedMaterials;
                        foreach (Material val6 in sharedMaterials)
                        {
                            list.Add(val6);
                            ReplaceShader(val6, MainShader);
                        }
                    }
                }
            }
            sharedMaterials = PitBundle.LoadAllAssets<Material>();
            foreach (Material val7 in sharedMaterials)
            {
                if (!list.Contains(val7))
                {
                    list.Add(val7);
                    ReplaceShader(val7, MainShader);
                }
            }*/


            }
        }
        //*/

        public static GameObject FindObjectEvenIfDisabled(string rootName, string objPath = null, int childNum = 0, bool useChildNum = false)
        {
            GameObject obj = null;
            GameObject[] objs = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            bool gotRoot = false;
            foreach (GameObject obj1 in objs)
            {
                if (obj1.name == rootName)
                {
                    obj = obj1;
                    gotRoot = true;
                }
            }
            if (!gotRoot)
                goto returnObject;
            else
            {
                GameObject obj2 = obj;
                if (objPath != null)
                {
                    obj2 = obj.transform.Find(objPath).gameObject;
                    if (!useChildNum)
                    {
                        obj = obj2;
                    }
                }
                if (useChildNum)
                {
                    GameObject obj3 = obj2.transform.GetChild(childNum).gameObject;
                    obj = obj3;
                }
            }
        returnObject:
            return obj;
        }

        //end of plonk code

        enum FunEnum
        {
            Off,
            Random,
            Always_on
        }

        private void Awake()
        {
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
            GameObject Fountain = prefabs[1];
            GameObject cache;
            GameObject Funobj;

            if (SceneHelper.CurrentScene != "Main Menu" || SceneHelper.CurrentScene != "Bootstrap" || SceneHelper.CurrentScene != "Intro")
            {
                ReplaceAssets();
            }
            Logger.LogWarning(epmodeinternal);

            switch (SceneHelper.CurrentScene)
            {



                case "Level 0-2":

                    FindObjectEvenIfDisabled("5B - Secret Arena", "5B Nonstuff/Pit (2)").transform.position = new UnityEngine.Vector3(0, 0, 798);
                    FindObjectEvenIfDisabled("5B - Secret Arena", "5B Nonstuff/Pit (3)").transform.position = new UnityEngine.Vector3(0, 0, 778);

                    Fountain = UnityEngine.Object.Instantiate(prefabs[7]);
                    break;

                case "Level 0-S":

                    FindObjectEvenIfDisabled("FinalRoom SecretExit").gameObject.SetActive(false);
                    Fountain = UnityEngine.Object.Instantiate(prefabs[0]);
                    FindObjectEvenIfDisabled("Hellgate Textless Variant", "DoorLeft").GetComponent<MeshRenderer>().allowOcclusionWhenDynamic = false;
                    FindObjectEvenIfDisabled("Hellgate Textless Variant", "DoorRight").GetComponent<MeshRenderer>().allowOcclusionWhenDynamic = false;
                    FindObjectEvenIfDisabled("0 - 0-S exit(Clone)", "closedoor").GetComponent<ObjectActivator>().events.toDisActivateObjects[0] = FindObjectEvenIfDisabled("Hellgate Textless Variant", "Opener");



                    break;

                case "Level 1-1":

                    FindObjectEvenIfDisabled("1 - First Field", "1 Stuff/Fountain").gameObject.SetActive(false);

                    Fountain = UnityEngine.Object.Instantiate(prefabs[1]);
                    Logger.LogInfo(FindObjectEvenIfDisabled("1 - Darker_Fountain(Clone)").name);
                    Logger.LogInfo(FindObjectEvenIfDisabled("1 - Darker_Fountain(Clone)", "fountain").name);
                    Logger.LogInfo(FindObjectEvenIfDisabled("1 - Darker_Fountain(Clone)", "fountain/muzzles/").name);
                    Logger.LogInfo(FindObjectEvenIfDisabled("1 - Darker_Fountain(Clone)", "fountain/muzzles/muzzleflash (4)").name);
                    Logger.LogInfo(FindObjectEvenIfDisabled("StatsManager").name);

                    Logger.LogInfo(FindObjectEvenIfDisabled("1 - Darker_Fountain(Clone)", "fountain/muzzles/muzzleflash (4)").GetComponent<ObjectActivator>().events);

                    FindObjectEvenIfDisabled("1 - Darker_Fountain(Clone)", "fountain/muzzles/muzzleflash (4)").GetComponent<ObjectActivator>().events.onActivate.AddListener(FindObjectEvenIfDisabled("StatsManager").GetComponent<StatsManager>().StopTimer);

                    FindObjectEvenIfDisabled("1 - Darker_Fountain(Clone)", "fountain/Cylinder (1)").GetComponent<CoinActivated>().events.onActivate.AddListener(FindObjectEvenIfDisabled("FirstRoom", "Room/FinalDoor/DoorLeft").GetComponent<Door>().LockClose);
                    FindObjectEvenIfDisabled("1 - Darker_Fountain(Clone)", "fountain/Cylinder (1)").GetComponent<CoinActivated>().events.onActivate.AddListener(FindObjectEvenIfDisabled("FirstRoom", "Room/FinalDoor/DoorRight").GetComponent<Door>().LockClose);
                    cache = FindObjectEvenIfDisabled("1 - Darker_Fountain(Clone)", "nopass");
                    cache.GetComponent<ObjectActivator>().events.toDisActivateObjects[0] = FindObjectEvenIfDisabled("LargeDoor", "OpenZone (1)");
                    cache.GetComponent<ObjectActivator>().events.toDisActivateObjects[1] = FindObjectEvenIfDisabled("LargeDoor (12)", "OpenZone (1)");
                    Fountain.transform.parent = FindObjectEvenIfDisabled("1 - First Field", "1 Stuff").transform;
                    Logger.LogMessage(FindObjectEvenIfDisabled("1 - Darker_Fountain(Clone)", "fountain/Cylinder (1)").name);

                    break;
                case "Level 1-S":

                    FindObjectEvenIfDisabled("5 - Finale", "FinalRoomSecretExit").gameObject.SetActive(false);
                    Fountain = UnityEngine.Object.Instantiate(prefabs[2]);
                    FindObjectEvenIfDisabled("5 - Finale", "InteractiveScreenPuzzle5x5 (2)/Canvas/Background").GetComponent<PuzzleController>().toActivate[0] = FindObjectEvenIfDisabled("2 - Witless Fountain(Clone)", "ActivateLimboDoor").gameObject;


                    break;

                case "Level 2-1":


                    if ((fun < 4 || OverrideFun == "Always_on") && OverrideFun != "Off")
                    {

                        Funobj = UnityEngine.Object.Instantiate(prefabs[13]);
                        Logger.LogWarning(string.Concat("Importing: ", Funobj.gameObject.name));


                        FindObjectEvenIfDisabled(Funobj.transform.name, SceneHelper.CurrentScene).gameObject.SetActive(true);
                        if (epmodeinternal)
                        {
                            FindObjectEvenIfDisabled(Funobj.transform.name, "Level 2-1/standard").gameObject.SetActive(false);
                            FindObjectEvenIfDisabled(Funobj.transform.name, "Level 2-1/eplmode").gameObject.SetActive(true);
                        }
                    }


                    break;

                case "Level 2-3":


                    FindObjectEvenIfDisabled("Exteriors", "Arch (1)").gameObject.SetActive(false);
                    FindObjectEvenIfDisabled("2 - Sewer Arena", "2 Nonstuff/Secret Level Entrance/FinalRoom SecretEntrance").gameObject.SetActive(false);
                    Fountain = UnityEngine.Object.Instantiate(prefabs[8]);
                    cache = FindObjectEvenIfDisabled("2 - Sewer Arena", "2 Nonstuff/Secret Level Entrance/FinalRoom SecretEntrance/Pit");
                    cache.gameObject.SetActive(true);
                    cache.transform.parent = null;
                    cache.transform.position = new UnityEngine.Vector3(5, -30, 800);

                    cache = FindObjectEvenIfDisabled("2 - Sewer Arena", "2 Nonstuff/Secret Level Entrance/FinalRoom SecretEntrance/Pit (2)");
                    cache.gameObject.SetActive(true);
                    cache.transform.parent = null;
                    cache.transform.position = new UnityEngine.Vector3(-15, -30, 800);
                    FindObjectEvenIfDisabled("8 - FountainLust(Clone)", "addmintofix").GetComponent<ObjectActivatorStay>().toDisActivate[0] = (FindObjectEvenIfDisabled("MinosBackground"));

                    break;
                case "Level 3-2":
                    if ((fun > 8 || OverrideFun == "Always_on") && OverrideFun != "Off")
                    {
                        FindObjectEvenIfDisabled("4 - Heart Chamber", "4 Stuff").transform.localPosition = new UnityEngine.Vector3(0, 0, 0);
                        FindObjectEvenIfDisabled("4 - Heart Chamber", "4 Stuff/Cube").SetActive(false);
                        FindObjectEvenIfDisabled("4 - Heart Chamber", "4 Nonstuff/MusicActivator").SetActive(false);


                        Funobj = UnityEngine.Object.Instantiate(prefabs[13]);
                        Logger.LogWarning(string.Concat("Importing: ", Funobj.gameObject.name));


                        FindObjectEvenIfDisabled(Funobj.transform.name, SceneHelper.CurrentScene).gameObject.SetActive(true);
                        FindObjectEvenIfDisabled(Funobj.transform.name, "Level 3-2/heart/FakeGabrielRouxl").GetComponent<MassAnimationReceiver>().realMass = FindObjectEvenIfDisabled("4 - Heart Chamber", "4 Stuff/Gabriel");
                        FindObjectEvenIfDisabled(Funobj.transform.name, "Level 3-2/heart/FakeGabrielRouxl/KillYourOST").GetComponent<ObjectActivator>().events.toDisActivateObjects[0] = FindObjectEvenIfDisabled("Music 1");
                        FindObjectEvenIfDisabled(Funobj.transform.name, "Level 3-2/heart/FakeGabrielRouxl/KillYourOST").GetComponent<ObjectActivator>().events.toDisActivateObjects[1] = FindObjectEvenIfDisabled("Music 2");

                        FindObjectEvenIfDisabled(Funobj.transform.name, "Level 3-2/heart/Cube").GetComponent<ObjectActivator>().events = FindObjectEvenIfDisabled("4 - Heart Chamber", "4 Stuff/Cube").GetComponent<ObjectActivator>().events;
                        FindObjectEvenIfDisabled(Funobj.transform.name, "Level 3-2/heart").transform.parent = FindObjectEvenIfDisabled("4 - Heart Chamber", "4 Stuff").transform;
                        FindObjectEvenIfDisabled(Funobj.transform.name, "Level 3-2/hold/MfixPutInGabe").GetComponent<ObjectActivator>().events.toActivateObjects[0] = FindObjectEvenIfDisabled("Music 3");
                        FindObjectEvenIfDisabled(Funobj.transform.name, "Level 3-2/hold/MfixPutInGabe").transform.parent = FindObjectEvenIfDisabled("4 - Heart Chamber", "4 Stuff/Gabriel").transform;

                    }
                    break;

                case "Level 4-2":

                    FindObjectEvenIfDisabled("OOB Stuff").transform.position = FindObjectEvenIfDisabled("OOB Stuff").transform.position - new UnityEngine.Vector3(0, 550, 0);

                    FindObjectEvenIfDisabled("FakeMoon").GetComponent<SphereCollider>().enabled = false;
                    FindObjectEvenIfDisabled("FakeMoon").GetComponent<MeshRenderer>().enabled = false;
                    FindObjectEvenIfDisabled("FakeMoon", "Point Light/Spot Light").gameObject.SetActive(false);
                    FindObjectEvenIfDisabled("FakeMoon", "FinalRoom SecretEntrance/Pit").transform.position = FindObjectEvenIfDisabled("FakeMoon", "FinalRoom SecretEntrance/Pit").transform.position - new UnityEngine.Vector3(0, 250, 0); ;
                    FindObjectEvenIfDisabled("FakeMoon", "FinalRoom SecretEntrance/Pit (2)").transform.position = FindObjectEvenIfDisabled("FakeMoon", "FinalRoom SecretEntrance/Pit (2)").transform.position - new UnityEngine.Vector3(0, 250, 0); ;
                    Fountain = UnityEngine.Object.Instantiate(prefabs[12]);
                    Fountain.transform.parent = FindObjectEvenIfDisabled("FakeMoon").transform;

                    break;

                case "Level 4-3":

                    if ((fun > 7 || OverrideFun == "Always_on") && OverrideFun != "Off")
                    {

                        Funobj = UnityEngine.Object.Instantiate(prefabs[13]);
                        FindObjectEvenIfDisabled(Funobj.transform.name, SceneHelper.CurrentScene).gameObject.SetActive(true);

                        FindObjectEvenIfDisabled("1 - First Chambers", "1 Nonstuff/Altar").gameObject.SetActive(false);



                        FindObjectEvenIfDisabled(Funobj.transform.name, "Level 4-3/patternphase1/soundenable").transform.parent = FindObjectEvenIfDisabled("1 - First Chambers", "1 Stuff/Torches/GreedTorch/OnLight").transform;
                        cache = FindObjectEvenIfDisabled(Funobj.transform.name, "Level 4-3/SoundwaveObject");
                        cache.transform.parent = FindObjectEvenIfDisabled("Player", "Agent").transform;
                        cache.SetActive(false);
                        cache.transform.SetSiblingIndex(0);
                        FindObjectEvenIfDisabled("0-S Check").gameObject.SetActive(false);
                        FindObjectEvenIfDisabled("7 - Generator Room", "7 Nonstuff/Altar/Altars").gameObject.SetActive(false);
                        FindObjectEvenIfDisabled("7 - Generator Room", "7 Nonstuff/Altar/Cube").gameObject.SetActive(false);

                        //ItemPlaceZone redaltar = FindObjectEvenIfDisabled("7 - Generator Room", "Agent").GetComponents<ItemPlaceZone>()[2];



                        FindObjectEvenIfDisabled(Funobj.transform.name, "Level 4-3/button/Cube").GetComponent<ObjectActivator>().events.toActivateObjects[0] = FindObjectEvenIfDisabled("7 - Generator Room", "7 Nonstuff/Altar/MusicStopper").gameObject;

                        FindObjectEvenIfDisabled(Funobj.transform.name, "Level 4-3/button/Cube/delay").GetComponent<ObjectActivator>().events.toActivateObjects[0] = (FindObjectEvenIfDisabled("7 - Generator Room", "7 Nonstuff/Spinning Cylinder/Lights"));

                    }


                    break;

                case "Level 4-S":

                    FindObjectEvenIfDisabled("4 - Boulder Run", "4 Stuff/FinalRoom SecretExit").gameObject.SetActive(false);
                    Fountain = UnityEngine.Object.Instantiate(prefabs[4]);
                    Fountain.transform.parent = FindObjectEvenIfDisabled("4 - Boulder Run").transform;

                    break;

                case "Level 5-1":
                    if ((fun == 6 || OverrideFun == "Always_on") && OverrideFun != "Off")
                    {
                        FindObjectEvenIfDisabled("IntroParent", "Intro/Intro B - First Tunnel/Cube(Clone)").gameObject.SetActive(false);
                        GameObject room_mysteryman = UnityEngine.Object.Instantiate(prefabs[6]);
                        room_mysteryman.transform.parent = FindObjectEvenIfDisabled("IntroParent", "Intro/Intro B - First Tunnel").transform;
                        room_mysteryman.transform.localPosition = new UnityEngine.Vector3(-22.6f, 0f, -7.5f);
                    }

                    Fountain = UnityEngine.Object.Instantiate(prefabs[9]);


                    FindObjectEvenIfDisabled("2 - Elevator", "2B Secret/FinalRoom SecretEntrance").gameObject.SetActive(false);

                    cache = FindObjectEvenIfDisabled("2 - Elevator", "2B Secret/FinalRoom SecretEntrance/Pit");
                    cache.gameObject.SetActive(true);
                    cache.transform.parent = null;
                    cache.transform.position = new UnityEngine.Vector3(5, -30, 800);

                    cache = FindObjectEvenIfDisabled("2 - Elevator", "2B Secret/FinalRoom SecretEntrance/Pit (2)");
                    cache.gameObject.SetActive(true);
                    cache.transform.parent = null;
                    cache.transform.position = new UnityEngine.Vector3(-15, -30, 800);



                    break;

                case "Level 5-S":

                    FindObjectEvenIfDisabled("FinalRoom SecretExit").gameObject.SetActive(false);
                    Fountain = UnityEngine.Object.Instantiate(prefabs[3]);

                    break;

                case "Level 7-3":


                    if ((fun < 4 || OverrideFun == "Always_on") && OverrideFun != "Off")
                    {

                        Funobj = UnityEngine.Object.Instantiate(prefabs[13]);
                        Logger.LogWarning(string.Concat("Importing: ", Funobj.gameObject.name));


                        FindObjectEvenIfDisabled(Funobj.transform.name, SceneHelper.CurrentScene).gameObject.SetActive(true);

                    }

                    FindObjectEvenIfDisabled("2 - Garden Maze", "Secret/FinalRoom SecretEntrance").gameObject.SetActive(false);
                    FindObjectEvenIfDisabled("2 - Garden Maze", "Secret", 3, true).gameObject.SetActive(false);
                    FindObjectEvenIfDisabled("2 - Garden Maze", "Secret", 4, true).gameObject.SetActive(false);
                    FindObjectEvenIfDisabled("2 - Garden Maze", "Secret", 5, true).gameObject.SetActive(false);
                    FindObjectEvenIfDisabled("2 - Garden Maze", "Secret", 6, true).gameObject.GetComponent<MeshRenderer>().enabled = false;
                    Fountain = UnityEngine.Object.Instantiate(prefabs[11]);

                    cache = FindObjectEvenIfDisabled("2 - Garden Maze", "Secret/FinalRoom SecretEntrance/Pit");
                    cache.gameObject.SetActive(true);
                    cache.transform.parent = null;
                    cache.transform.position = new UnityEngine.Vector3(15, -30, 800);

                    cache = FindObjectEvenIfDisabled("2 - Garden Maze", "Secret/FinalRoom SecretEntrance/Pit (2)");
                    cache.gameObject.SetActive(true);
                    cache.transform.parent = null;
                    cache.transform.position = new UnityEngine.Vector3(-5, -30, 800);




                    break;

                case "Level 7-S":



                    FindObjectEvenIfDisabled("Fake Exit").gameObject.SetActive(false);
                    FindObjectEvenIfDisabled("7-S_Unpaintable", "Interior/Blackout").gameObject.SetActive(false);
                    FindObjectEvenIfDisabled("7-S_Unpaintable", "Doorways/Door_009").gameObject.SetActive(false);



                    Fountain = UnityEngine.Object.Instantiate(prefabs[5]);
                    cache = FindObjectEvenIfDisabled("Fake Exit", "WasherPickup");
                    cache.transform.parent = FindObjectEvenIfDisabled("5 - fountainWash(Clone)", "enable/washreplace").transform;
                    cache.transform.localPosition = new UnityEngine.Vector3(0f, 0f, 0f);
                    cache.transform.rotation = UnityEngine.Quaternion.identity;

                    FindObjectEvenIfDisabled("5 - fountainWash(Clone)", "thumbsuptasque (1)").GetComponent<ObjectActivator>().events.onActivate.AddListener(FindObjectEvenIfDisabled("Checkpoints", "Checkpoint (2)").GetComponent<CheckPoint>().ReactivateCheckpoint);
                    FindObjectEvenIfDisabled("Checkpoints", "Checkpoint (2)").GetComponent<CheckPoint>().toActivate = FindObjectEvenIfDisabled("5 - fountainWash(Clone)");

                    cache = FindObjectEvenIfDisabled("Fake Exit", "VacuumPickup");
                    cache.transform.parent = FindObjectEvenIfDisabled("5 - fountainWash(Clone)", "enable/vacreplace").transform;
                    cache.transform.localPosition = new UnityEngine.Vector3(0f, 0f, 0f);
                    cache.transform.rotation = UnityEngine.Quaternion.identity;

                    cache = FindObjectEvenIfDisabled("5 - fountainWash(Clone)", "Swatches - parent/swatchLadder");
                    cache.transform.parent = FindObjectEvenIfDisabled("Interactives", "MainHall/MainHall Stuff/Sliding Ladder/Ladder_Collision/Ladder").transform;
                    cache.transform.localPosition = new UnityEngine.Vector3(0f, -0.8f, 21.35f);

                    cache = FindObjectEvenIfDisabled("5 - fountainWash(Clone)", "Swatches - parent/swatchWalk");
                    cache.transform.parent = FindObjectEvenIfDisabled("7-S_Unpaintable", "Interior/Library Chandelier").transform;
                    cache.transform.localPosition = new UnityEngine.Vector3(0f, -7.66f, 0f);
                    FindObjectEvenIfDisabled("7-S_Paintable").GetComponent<BloodCheckerManager>().finalDoorOpener = FindObjectEvenIfDisabled("5 - fountainWash(Clone)", "DoorOpenerReplacement");

                    FindObjectEvenIfDisabled("Interactives", "MainHall/MainHall NonStuff/Altar (Blue Skull) Variant/Cube").GetComponent<ItemPlaceZone>().activateOnSuccess[0] = FindObjectEvenIfDisabled("5 - fountainWash(Clone)", "3 Exit (2)/Open").gameObject;


                    break;
            }

            //FRIEND
            try
            {
                if (fun <= 3)
                {


                    GameObject friend = UnityEngine.Object.Instantiate(prefabs[10]);
                    FindObjectEvenIfDisabled(friend.transform.name, SceneHelper.CurrentScene).gameObject.SetActive(true);

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


