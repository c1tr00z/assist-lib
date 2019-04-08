using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.IO;
using System.Text;

namespace c1tr00z.AssistLib.Localization {
    public class LocalizationEditor {

        private static System.Net.Security.RemoteCertificateValidationCallback allowCertificate = (sender, cert, chain, sslPolicyErrors) => true;

        [MenuItem("Assist/Get Localizations")]
        public static void GetLocalizations() {

            ServicePointManager.ServerCertificateValidationCallback += allowCertificate;
            string docFormat = "csv";
            string gDocsDownloadURL = "http://spreadsheets.google.com/feeds/download/spreadsheets/Export?key={0}&exportFormat={1}";
            string downloadUrl = string.Format(gDocsDownloadURL, AssistLibSettings.localizationDocumentKey, docFormat);

            Debug.Log(downloadUrl);

            HttpWebRequest aRequest = (HttpWebRequest)WebRequest.Create(downloadUrl);
            HttpWebResponse aResponse = (HttpWebResponse)aRequest.GetResponse();

            WebHeaderCollection header = aResponse.Headers;

            var encoding = ASCIIEncoding.UTF8;
            using (var reader = new System.IO.StreamReader(aResponse.GetResponseStream(), encoding)) {
                string localizationsString = reader.ReadToEnd();

                var stringRows = localizationsString.Split('\r', '\n');


                Debug.Log(localizationsString);
                Debug.Log(stringRows.ToPlainString());
                var localizations = new Dictionary<string, Dictionary<string, string>>();

                string[] langCols = null;
                for (int r = 0; r < stringRows.Length; r++) {
                    var row = stringRows[r];
                    var stringCols = row.Split(',');
                    if (r == 0) {
                        langCols = stringCols;
                        Debug.Log(langCols.ToPlainString());
                        for (int l = 1; l < langCols.Length; l++) {
                            var lang = langCols[l];
                            if (!string.IsNullOrEmpty(lang)) {
                                localizations.AddOrSet(lang, new Dictionary<string, string>());
                            }
                        }
                    } else {
                        for (int c = 1; c < stringCols.Length; c++) {
                            localizations[langCols[c]].AddOrSet(stringCols[0], stringCols[c]);
                        }
                    }
                }

                Debug.Log(Application.dataPath);

                foreach (KeyValuePair<string, Dictionary<string, string>> kvp in localizations) {
                    var lang = ItemsEditor.CreateOrGetItem<LanguageItem>(Path.Combine(Path.Combine("Assets", "Localizations"), "Resources"), kvp.Key);
                    var json = MiniJSON.Json.Serialize(kvp.Value).DecodeEncodedNonAscii();
                    Debug.LogError(json);
                    var file = new FileInfo(Path.Combine(Path.Combine(Path.Combine(Application.dataPath, "Localizations"), "Resources"), kvp.Key + "@text.txt"));
                    if (!file.Exists) {
                        file.Create().Close();
                    }
                    Debug.Log(file.ToString());
                    using (StreamWriter sw = new StreamWriter(file.ToString())) {
                        sw.Write(json);
                        sw.Close();
                    }
                }

                Debug.Log(localizations.ToPlainString());
            }

            //byte[] buffer = new byte[16 * 1024];
            //using (MemoryStream ms = new MemoryStream()) {
            //    int read;
            //    while ((read = aResponse.GetResponseStream().Read(buffer, 0, buffer.Length)) > 0) {
            //        ms.Write(buffer, 0, read);
            //    }

            //    ms.Position = 0;

            //    var localizationsString = System.Text.Encoding.UTF8.GetString(ms.ToArray());

            //}

            ServicePointManager.ServerCertificateValidationCallback -= allowCertificate;
            ItemsEditor.CollectItems();
        }

    }
}
