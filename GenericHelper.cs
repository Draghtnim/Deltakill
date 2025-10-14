using System.Collections.Generic;
using System.IO;
using System.Linq;
using BepInEx;
using BepInEx.Logging;
using GameConsole.pcon;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;


namespace Ultrapit
{

    public class GenericHelper : BaseUnityPlugin
    {

        public static bool IsCustomLevel = false;
        private static Plugin _instance;
        private static GameObject pref = null;



        public static Plugin Instance => _instance;

        private static Dictionary<string, GameObject> BundleDictionary = new Dictionary<string, GameObject>();

        //Plonk code

        public static void DictonaryFill()
        {

            foreach (var pref in Plugin.prefabs)
            {
                if (BundleDictionary.ContainsKey(pref.name)){
                    int i = 0;
                    for (i = 0; BundleDictionary.ContainsKey(pref.name+i); i++)
                    {

                    }
                    BundleDictionary.Add(pref.name+i, pref);

                }
                else
                {
                    BundleDictionary.Add(pref.name, pref);
                }
            }
        }



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
        public static Type Fetch<Type>(string name)
        {
            return Addressables.LoadAssetAsync<Type>((object)name).WaitForCompletion();
        }

        public static GameObject FetchAsset(string name)
        {
            GameObject gameObject = null;
            BundleDictionary.TryGetValue(name, out gameObject);
            return gameObject;
        }

        public static void ReplaceAssets()
        {
            Shader MainShader = Fetch<Shader>("Assets/Shaders/MasterShader/ULTRAKILL-Standard.shader");
            List<Material> list = new List<Material>();
            Dictionary<string, AudioMixer> dictionary = new Dictionary<string, AudioMixer>();
            dictionary["AllAudio"] = Addressables.LoadAssetAsync<AudioMixer>((object)"AllAudio").WaitForCompletion();
            dictionary["DoorAudio"] = Addressables.LoadAssetAsync<AudioMixer>((object)"DoorAudio").WaitForCompletion();
            dictionary["GoreAudio"] = Addressables.LoadAssetAsync<AudioMixer>((object)"GoreAudio").WaitForCompletion();
            dictionary["MusicAudio"] = Addressables.LoadAssetAsync<AudioMixer>((object)"MusicAudio").WaitForCompletion();
            dictionary["UnfreezeableAudio"] = Addressables.LoadAssetAsync<AudioMixer>((object)"UnfreezeableAudio").WaitForCompletion();
            GameObject[] array = Plugin.PitBundle.LoadAllAssets<GameObject>();
            Material[] sharedMaterials;
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

                //study this
                if (val.GetComponentsInChildren<Renderer>(true) != null)
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
            sharedMaterials = Plugin.PitBundle.LoadAllAssets<Material>();
            foreach (Material val7 in sharedMaterials)
            {
                if (!list.Contains(val7))
                {
                    list.Add(val7);
                    ReplaceShader(val7, MainShader);
                }
            }



        }

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
    }
}


