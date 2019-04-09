using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;

namespace c1tr00z.AssistLib.GoogleSpreadsheetImporter {
    public class GoogleSpreadsheetDocumentImpoter {

        private static System.Net.Security.RemoteCertificateValidationCallback allowCertificate = (sender, cert, chain, sslPolicyErrors) => true;

        public static Dictionary<string, Dictionary<string, string>> Import(GoogleSpreadsheetDocumentPageDBEntry page) {
            ServicePointManager.ServerCertificateValidationCallback += allowCertificate;
            string docFormat = "csv";
            string gDocsDownloadURL = "http://spreadsheets.google.com/feeds/download/spreadsheets/Export?key={0}&gid={1}&exportFormat={2}";
            string downloadUrl = string.Format(gDocsDownloadURL, page.document.documentId, page.pageId, docFormat);

            Debug.Log(downloadUrl);

            HttpWebRequest aRequest = (HttpWebRequest)WebRequest.Create(downloadUrl);
            HttpWebResponse aResponse = (HttpWebResponse)aRequest.GetResponse();

            WebHeaderCollection header = aResponse.Headers;

            var encoding = ASCIIEncoding.UTF8;

            var parsedPage = new Dictionary<string, Dictionary<string, string>>();
            using (var reader = new System.IO.StreamReader(aResponse.GetResponseStream(), encoding)) {
                string localizationsString = reader.ReadToEnd();

                var stringRows = localizationsString.Split('\r', '\n');


                Debug.Log(localizationsString);
                Debug.Log(stringRows.ToPlainString());

                string[] keyColumns = null;
                for (int r = 0; r < stringRows.Length; r++) {
                    var row = stringRows[r];
                    var stringCols = row.Split(',');
                    if (r == 0) {
                        keyColumns = stringCols;
                        Debug.Log(keyColumns.ToPlainString());
                        for (int l = 1; l < keyColumns.Length; l++) {
                            var lang = keyColumns[l];
                            if (!string.IsNullOrEmpty(lang)) {
                                parsedPage.AddOrSet(lang, new Dictionary<string, string>());
                            }
                        }
                    } else {
                        for (int c = 1; c < stringCols.Length; c++) {
                            parsedPage[keyColumns[c]].AddOrSet(stringCols[0], stringCols[c]);
                        }
                    }
                }
            }

            ServicePointManager.ServerCertificateValidationCallback -= allowCertificate;

            Debug.Log(JSONUtuls.Serialize(parsedPage));
            return parsedPage;
        }
    }
}