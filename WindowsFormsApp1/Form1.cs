using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.bucket.manager.ForgeUtils;
using static WindowsFormsApp1.bucket.manager.ForgeUtils.Derivatives;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            //try
            // {
            //var x = await Derivatives.ExtractSVFAsync("", "eyJhbGciOiJSUzI1NiIsImtpZCI6IlU3c0dGRldUTzlBekNhSzBqZURRM2dQZXBURVdWN2VhIn0.eyJzY29wZSI6WyJkYXRhOndyaXRlIiwiZGF0YTpyZWFkIiwiYnVja2V0OmNyZWF0ZSIsImJ1Y2tldDpkZWxldGUiLCJidWNrZXQ6cmVhZCIsImRhdGE6c2VhcmNoIiwidmlld2FibGVzOnJlYWQiXSwiY2xpZW50X2lkIjoiYkpyc3FndkczQTd1VTdiRmtxRDRVZUVYUm0zMUQ2eUciLCJhdWQiOiJodHRwczovL2F1dG9kZXNrLmNvbS9hdWQvYWp3dGV4cDYwIiwianRpIjoiUlFJajBrZFd0aVcyMlBUTjV6a2NTRmpuQzFOdUtNTHljclFZM0dUdm8zY0V6ejVSMWtSTFRiNk95TXpTQUoxRCIsImV4cCI6MTY2NTA3NzAyOH0.cyvnauBn_tkKuQNdMBp7-DWHjpa3rJg2f72ANqZu2_uVUjcWnPXM64MbkVcpu9g9Jd6EuKXcWkc7ot1G65KSnOMYyYEYmIG1hkX2fmbqd_GzAXOo9EO1TpgxOqxD-joHZXL-uoot2x0z7BOxR6a6UwaRgkvC3eJ2XJbiUp8FFQaBLLBf6-OTJiNFIAK5WIr_BqZbHOGhe4jdroegdJWb8XbrKSQQ2kHOKhCp3KNJMKQzEK3XxaBR9BHH5e5WDOU3kau5fNz8mvpIXx7DC4QyaqqdUIwuyCxJKFRGDtwA7D_YvB4pP7IJg8CGqvYsRwVw2atLaG_Bs9BMbeompo5eVg");
            var urn = txtUrn.Text;
            var AccessToken = txtToken.Text;

            // get the list of resources to download
            List<Resource> resourcesToDownload = await ExtractSVFAsync(urn, AccessToken);

            var folderPath = $"c:\\tempforge\\{urn}";
            Directory.CreateDirectory(folderPath);

            var client = new RestClient("https://developer.api.autodesk.com/");
            foreach (bucket.manager.ForgeUtils.Derivatives.Resource resource in resourcesToDownload)
            {
                // prepare the GET to download the file
                RestRequest request = new RestRequest(resource.RemotePath, Method.Get);
                request.AddHeader("Authorization", "Bearer " + AccessToken);
                request.AddHeader("Accept-Encoding", "gzip, deflate");
                var response = await client.ExecuteAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // combine with selected local path
                    string pathToSave = Path.Combine(folderPath, resource.LocalPath);
                    // ensure local dir exists
                    Directory.CreateDirectory(Path.GetDirectoryName(pathToSave));
                    // save file
                    File.WriteAllBytes(pathToSave, response.RawBytes);
                }
                else
                {
                    // something went wrong for this file...
                }
            }

            MessageBox.Show("Process finished");
            // }
            // catch (Exception err)
            //{
            //    MessageBox.Show(err.Message);
            // }
        }
    }
}