﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace UnityModManagerNet
{
    public partial class UnityModManager
    {
        public class ModSettings
        {
            public virtual void Save(ModEntry modEntry)
            {
                Save(this, modEntry);
            }

            public virtual string GetPath(ModEntry modEntry)
            {
                return Path.Combine(modEntry.Path, "Settings.xml");
            }

            public static void Save(ModSettings data, ModEntry modEntry)
            {
                Save(data, modEntry, null);
            }

            public static void Save(ModSettings data, ModEntry modEntry, XmlAttributeOverrides attributes)
            {
                var filepath = data.GetPath(modEntry);
                try
                {
                    using (var writer = new StreamWriter(filepath))
                    {
                        var serializer = new XmlSerializer(data.GetType(), attributes);
                        serializer.Serialize(writer, data);
                    }
                }
                catch (Exception e)
                {
                    modEntry.Logger.Error($"Can't save {filepath}.");
                    modEntry.Logger.LogException(e);
                }
            }

            public static T Load<T>(ModEntry modEntry) where T : ModSettings, new()
            {
                var t = new T();
                var filepath = t.GetPath(modEntry);
                if (File.Exists(filepath))
                {
                    try
                    {
                        using (var stream = File.OpenRead(filepath))
                        {
                            var serializer = new XmlSerializer(typeof(T));
                            var result = (T)serializer.Deserialize(stream);
                            return result;
                        }
                    }
                    catch (Exception e)
                    {
                        modEntry.Logger.Error($"Can't read {filepath}.");
                        modEntry.Logger.LogException(e);
                    }
                }

                return t;
            }

            public static T Load<T>(ModEntry modEntry, XmlAttributeOverrides attributes) where T : ModSettings, new()
            {
                var t = new T();
                var filepath = t.GetPath(modEntry);
                if (File.Exists(filepath))
                {
                    try
                    {
                        using (var stream = File.OpenRead(filepath))
                        {
                            var serializer = new XmlSerializer(typeof(T), attributes);
                            var result = (T)serializer.Deserialize(stream);
                            return result;
                        }
                    }
                    catch (Exception e)
                    {
                        modEntry.Logger.Error($"Can't read {filepath}.");
                        modEntry.Logger.LogException(e);
                    }
                }

                return t;
            }

            /* Kept for backwards compatibility */

            [Obsolete("This method is deprecated and may be removed in a future release. Use the non-generic version instead.")]
            public static void Save<T>(T data, ModEntry modEntry) where T : ModSettings, new()
            {
                Save((ModSettings)data, modEntry, null);
            }

            /// <summary>
            /// [0.20.0]
            /// </summary>
            [Obsolete("This method is deprecated and may be removed in a future release. Use the non-generic version instead.")]
            public static void Save<T>(T data, ModEntry modEntry, XmlAttributeOverrides attributes) where T : ModSettings, new()
            {
                Save((ModSettings)data, modEntry, attributes);
            }
        }
    }
}
