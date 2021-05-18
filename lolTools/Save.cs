using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

using Newtonsoft.Json;

namespace lolTools {
    public static class Save {

        /*
            save data every 15 sec

            max key length?
        */

        private static Timer saveTimer = new Timer();
        private static Dictionary<string, string> data = new Dictionary<string, string>();
        private static string savePath = "saveData.json";

        #region PASS FUNC
        public static void dump() => data.Clear();
        private static string dataToStr(object obj) => JsonConvert.SerializeObject(obj);
        #endregion

        public static void init() {

            if(File.Exists(savePath)) {
                try {
                    readFromDisk();
                } catch {
                    Debug.WriteLine("Cant read from save file");
                }
            }

            #if DEBUG
            if(!Save.hasKey("DBG")) {
                Save.addKey("DBG",true);
            }
            #elif RELEASE
            if(Save.hasKey("DBG")) {
            Save.dump();
            }
            #endif

            saveTimer.Tick += Save.autoSave;
            saveTimer.Interval = 15000;
            saveTimer.Enabled = true;
        }

        public static bool hasKey(string key) {
            // TODO: test if data is null
            if(data == null) {
                return false;
            }
            return data.ContainsKey(key);
        }

        public static void addKey(string key, object obj = null) {
            if (hasKey(key)) {
                throw new Exception("Key is already made");
            } else {
                data.Add(key, dataToStr(obj));
            }
        }

        public static void writeData(string key, string obj) {
            if (!hasKey(key)) {
                Debug.WriteLine($"Thre is no key: {key}. Making it now");
                Save.addKey(key);
            }
            data[key] = obj;
        }

        public static void writeData(string key, object obj) {
            if (!hasKey(key)) {
                Debug.WriteLine($"Thre is no key: {key}. Making it now");
                Save.addKey(key,obj);
            } else {
                data[key] = dataToStr(obj);
            }
        }

        public static string readData(string key) {
            if(data.ContainsKey(key)) {
                return data[key];
            } else {
                return string.Empty;
            }
        }

        public static T readData<T>(string key) {
            if (!data.ContainsKey(key)) {
                throw new Exception($"{key} is not a key");
            } else {
                return JsonConvert.DeserializeObject<T>(data[key]);
            }
        }

        #region DISK SHIT
        public static void autoSave(object sender, EventArgs args) {
            saveToDisk();
        }

        public static void saveToDisk() {
            string newData = JsonConvert.SerializeObject(data);
            byte[] bytes = Encoding.UTF8.GetBytes(newData);
            FileStream fs = File.Open(savePath, FileMode.Create);
            fs.Write(bytes);
            fs.Close();
        }


        public static void readFromDisk() {
            string savedData = File.ReadAllText(savePath,Encoding.UTF8);
            if(savedData == "") {
                return;
            }
            data = JsonConvert.DeserializeObject<Dictionary<string, string>>(savedData);
        }
        #endregion
    }
}
