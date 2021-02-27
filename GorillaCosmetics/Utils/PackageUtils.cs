using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO.Compression;
using System.IO;
using System.Linq;
using GorillaCosmetics.Data.Descriptors;
using GorillaCosmetics.Data;

namespace GorillaCosmetics.Utils
{
    public static class PackageUtils
    {
        public static (AssetBundle bundle, PackageJSON json) AssetBundleAndJSONFromPackage(string path)
        {
            AssetBundle bundle = null;
            PackageJSON json = null;
            using (ZipArchive archive = ZipFile.OpenRead(path))
            {
                var jsonEntry = archive.Entries.First(i => i.Name == "package.json");
                if (jsonEntry != null)
                {
                    var stream = new StreamReader(jsonEntry.Open(), Encoding.Default);
                    string jsonString = stream.ReadToEnd();
                    json = JsonUtility.FromJson<PackageJSON>(jsonString);
                    // potentially the worst thing i've ever written, please do not let this see the light of day ever
                    // jesus christ
                    string descriptorString = jsonString.Split(new string[] { "\"descriptor\": " }, StringSplitOptions.None)[1];
                    descriptorString = descriptorString.Substring(0, descriptorString.IndexOf("},") + 1);
                    string configString = jsonString.Split(new string[] { "\"config\": " }, StringSplitOptions.None)[1];
                    configString = configString.Substring(0, configString.IndexOf("}") + 1);
                    json.descriptor = JsonUtility.FromJson<Descriptor>(descriptorString);
                    json.config = JsonUtility.FromJson<Config>(configString);
                }
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    if (json != null && entry.Name == json.pcFileName)
                    {
                        //here the file
                        var SeekableStream = new MemoryStream();
                        entry.Open().CopyTo(SeekableStream);
                        SeekableStream.Position = 0;
                        bundle = AssetBundle.LoadFromStream(SeekableStream);
                    }
                }
            }
            return (bundle, json);
        }

        public static AssetBundle AssetBundleFromPackage(string path)
        {
            return AssetBundleAndJSONFromPackage(path).bundle;
        }

        public static GorillaMaterialDescriptor ConvertJsonToMaterial(PackageJSON json)
        {
            GorillaMaterialDescriptor Descriptor = new GorillaMaterialDescriptor();
            Descriptor.MaterialName = json.descriptor.objectName;
            Descriptor.AuthorName = json.descriptor.author;
            Descriptor.Description = json.descriptor.description;
            Descriptor.CustomColors = json.config.customColors;
            Descriptor.DisablePublicLobbies = json.config.disableInPublicLobbies;
            return Descriptor;
        }
        
        public static HatDescriptor ConvertJsonToHat(PackageJSON json)
        {
            HatDescriptor Descriptor = new HatDescriptor();
            Descriptor.HatName = json.descriptor.objectName;
            Descriptor.AuthorName = json.descriptor.author;
            Descriptor.Description = json.descriptor.description;
            Descriptor.CustomColors = json.config.customColors;
            Descriptor.DisablePublicLobbies = json.config.disableInPublicLobbies;
            return Descriptor;
        }
    }
}
