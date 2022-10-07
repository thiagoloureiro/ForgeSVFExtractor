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
        }
    }
}