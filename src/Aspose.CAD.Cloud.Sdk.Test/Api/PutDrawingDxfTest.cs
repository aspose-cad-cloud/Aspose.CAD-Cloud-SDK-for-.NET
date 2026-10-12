// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Aspose" file="SaveAsApiTests.cs">
//   Copyright (c) 2018 Aspose.CAD for Cloud
// </copyright>
// <summary>
//   Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
// 
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
// 
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Aspose.CAD.Cloud.Sdk.Test.Api
{
	using System.IO;
	using System.Collections.Generic;
	using NUnit.Framework;

	using Aspose.CAD.Cloud.Sdk.Model.Requests;
	using Aspose.CAD.Cloud.Sdk.Test.Base;
    using Aspose.CAD.Cloud.Sdk.Api;
    using Newtonsoft.Json;
    using System.Diagnostics;
    using System;
    using System.Reflection;
    using Aspose.CAD.Cloud.Sdk.Model;

    /// <summary>
    ///  Class for testing SaveAsApi
    /// </summary>
    [TestFixture]
    public class PutDrawingTests
    {
        private CadApi api;

        /// <summary>
        /// The local reference folder
        /// </summary>
        protected const string LocalReferenceFolder = @"../../../../../ReferenceData";

        /// <summary>
        /// The local test folder
        /// </summary>
        protected const string LocalTestFolder = @"../../../../../TestData";

        /// <summary>
        /// The server access file
        /// </summary>
        private const string ServerAccessFile = "serverAccess.json";

        protected static string LocalTestFolderFullPath => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), LocalTestFolder);

        /// <summary>
        /// Setup before each unit test
        /// </summary>
        [SetUp]
        public void Init()
        {        
            string serverAccessPath = Path.Combine(LocalTestFolderFullPath, ServerAccessFile);

            FileInfo serverFileInfo = new FileInfo(serverAccessPath);
            if (serverFileInfo.Exists && serverFileInfo.Length > 0)
            {
                var accessData = JsonConvert.DeserializeObject<ServerAccessData>(File.ReadAllText(serverAccessPath));
                api = new CadApi(
                    accessData.AppKey,
                    accessData.AppSid,
                    accessData.BaseURL,
                    AvailableApiVersions.V3,
                    true
                );
            }
            else
            {
                throw new ArgumentException("Please, specify valid access data (AppKey, AppSid, Base URL)");
            }
        }

        [Test]
        public void PutDrawingDxfTest()
        {
            string fileName = Path.Combine(LocalTestFolder, "input_simplified.svg"); 
            string outFile = "result.dxf";

            PostDrawingPropertiesRequest getProperties = new PostDrawingPropertiesRequest();
            FileStream fs = File.OpenRead(fileName);
            getProperties.DrawingData = fs;
            CadResponse responseStreamProperties = api.PostDrawingProperties(getProperties);
            fs.Close();

            double initialHeight = (double)responseStreamProperties.Height;
            double initialWidth = (double)responseStreamProperties.Width;

            Assert.AreEqual(2357, initialHeight, 0.01);
            Assert.AreEqual(2273, initialWidth, 0.01);

            float mmPerInch = 25.4f;

            double ratio = 1 / 0.9923130037;

            double newWidth = (initialWidth * ratio / mmPerInch);
            double newHeight = (initialHeight * ratio / mmPerInch);

            PutDrawingDXFRequest request = new PutDrawingDXFRequest();
            request.DrawingData = System.IO.File.OpenRead(fileName);

            request.ExportOptions = @"{""VectorRasterizationOptions"": { ""PageHeight"":"
                + newHeight.ToString(System.Globalization.CultureInfo.InvariantCulture) + @",""PageWidth"":" + newWidth.ToString(System.Globalization.CultureInfo.InvariantCulture) + @"}}";

            var responseStream = api.PutDrawingDXF(request);

            using (var fileStream = System.IO.File.Create(outFile))
            {
                responseStream.Seek(0, SeekOrigin.Begin);
                responseStream.CopyTo(fileStream);
                fileStream.Close();
            }

            getProperties = new PostDrawingPropertiesRequest();

            fs = File.OpenRead(outFile);
            getProperties.DrawingData = fs;
            responseStreamProperties = api.PostDrawingProperties(getProperties);
            fs.Close();

            Assert.AreEqual(11, (double)responseStreamProperties.Height, 0.01);
            Assert.AreEqual(15, (double)responseStreamProperties.Width, 0.01);

            File.Delete(outFile);
        }

        [Test]
        public void PutDrawingPngTest()
        {
            string fileName = Path.Combine(LocalTestFolder, "910609.dxf");
            string outFile = "result.png";

            double newWidth = 200;
            double newHeight = 200;

            PutDrawingPngRequest request = new PutDrawingPngRequest();
            request.DrawingData = System.IO.File.OpenRead(fileName);

            request.ExportOptions = @"{""VectorRasterizationOptions"": { ""PageHeight"":"
            + newHeight.ToString(System.Globalization.CultureInfo.InvariantCulture) + @",""PageWidth"":" + newWidth.ToString(System.Globalization.CultureInfo.InvariantCulture) + @"}}";

            var responseStream = api.PutDrawingPng(request);

            using (var fileStream = System.IO.File.Create(outFile))
            {
                responseStream.Seek(0, SeekOrigin.Begin);
                responseStream.CopyTo(fileStream);
                fileStream.Close();
            }

            PostDrawingPropertiesRequest getProperties = new PostDrawingPropertiesRequest();
            FileStream fs = File.OpenRead(outFile);
            getProperties.DrawingData = fs;
            CadResponse responseStreamProperties = api.PostDrawingProperties(getProperties);
            fs.Close();

            Assert.AreEqual(200, responseStreamProperties.Height, 0.1);
            Assert.AreEqual(200, responseStreamProperties.Width, 0.1);

            File.Delete(outFile);
        }

        [Test]
        public void PutDrawingBmpTest()
        {
            string fileName = Path.Combine(LocalTestFolder, "910609.dxf");
            string outFile = "result.bmp";

            double newWidth = 200;
            double newHeight = 200;

            PutDrawingBmpRequest request = new PutDrawingBmpRequest();
            request.DrawingData = System.IO.File.OpenRead(fileName);
            request.ExportOptions = @"{""VectorRasterizationOptions"": { ""PageHeight"":"
                + newHeight.ToString(System.Globalization.CultureInfo.InvariantCulture) + @",""PageWidth"":" + newWidth.ToString(System.Globalization.CultureInfo.InvariantCulture) + @"}}";

            var responseStream = api.PutDrawingBmp(request);

            using (var fileStream = System.IO.File.Create(outFile))
            {
                responseStream.Seek(0, SeekOrigin.Begin);
                responseStream.CopyTo(fileStream);
                fileStream.Close();
            }

            PostDrawingPropertiesRequest getProperties = new PostDrawingPropertiesRequest();
            FileStream fs = File.OpenRead(outFile);
            getProperties.DrawingData = fs;
            CadResponse responseStreamProperties = api.PostDrawingProperties(getProperties);
            fs.Close();

            Assert.AreEqual(200, responseStreamProperties.Height, 0.1);
            Assert.AreEqual(200, responseStreamProperties.Width, 0.1);

            File.Delete(outFile);
        }

        [Test]
        public void PutDrawingCgmTest()
        {
            string fileName = Path.Combine(LocalTestFolder, "910609.dxf");
            string outFile = "result.cgm";

            double newWidth = 200;
            double newHeight = 200;

            PutDrawingCgmRequest request = new PutDrawingCgmRequest();
            request.DrawingData = System.IO.File.OpenRead(fileName);
            request.ExportOptions = @"{""VectorRasterizationOptions"": { ""PageHeight"":"
              + newHeight.ToString(System.Globalization.CultureInfo.InvariantCulture) + @",""PageWidth"":" + newWidth.ToString(System.Globalization.CultureInfo.InvariantCulture) + @"}}";

            var responseStream = api.PutDrawingCgm(request);

            using (var fileStream = System.IO.File.Create(outFile))
            {
                responseStream.Seek(0, SeekOrigin.Begin);
                responseStream.CopyTo(fileStream);
                fileStream.Close();
            }

            PostDrawingPropertiesRequest getProperties = new PostDrawingPropertiesRequest();
            FileStream fs = File.OpenRead(outFile);
            getProperties.DrawingData = fs;
            CadResponse responseStreamProperties = api.PostDrawingProperties(getProperties);
            fs.Close();

            Assert.AreEqual(414, responseStreamProperties.Height, 0.1);
            Assert.AreEqual(617, responseStreamProperties.Width, 0.1);

            File.Delete(outFile);
        }

        [Test]
        public void PutDrawingDwfTest()
        {
            string fileName = Path.Combine(LocalTestFolder, "910609.dxf");
            string outFile = "result.dwf";

            double newWidth = 200;
            double newHeight = 200;

            PutDrawingDwfRequest request = new PutDrawingDwfRequest();
            request.DrawingData = System.IO.File.OpenRead(fileName);
            request.ExportOptions = @"{""VectorRasterizationOptions"": { ""PageHeight"":"
                + newHeight.ToString(System.Globalization.CultureInfo.InvariantCulture) + @",""PageWidth"":" + newWidth.ToString(System.Globalization.CultureInfo.InvariantCulture) + @"}}";

            var responseStream = api.PutDrawingDwf(request);

            using (var fileStream = System.IO.File.Create(outFile))
            {
                responseStream.Seek(0, SeekOrigin.Begin);
                responseStream.CopyTo(fileStream);
                fileStream.Close();
            }

            PostDrawingPropertiesRequest getProperties = new PostDrawingPropertiesRequest();
            FileStream fs = File.OpenRead(outFile);
            getProperties.DrawingData = fs;
            CadResponse responseStreamProperties = api.PostDrawingProperties(getProperties);
            fs.Close();
         
            Assert.AreEqual(150, responseStreamProperties.Height, 0.1);
            Assert.AreEqual(150, responseStreamProperties.Width, 0.1);

            File.Delete(outFile);
        }

        [Test]
        public void PutDrawingFbxTest()
        {
            string fileName = Path.Combine(LocalTestFolder, "910609.dxf");
            string outFile = "result.fbx";

            double newWidth = 200;
            double newHeight = 200;

            PutDrawingFbxRequest request = new PutDrawingFbxRequest();
            request.DrawingData = System.IO.File.OpenRead(fileName);
            request.ExportOptions = @"{""VectorRasterizationOptions"": { ""PageHeight"":"
                + newHeight.ToString(System.Globalization.CultureInfo.InvariantCulture) + @",""PageWidth"":" + newWidth.ToString(System.Globalization.CultureInfo.InvariantCulture) + @"}}";

            var responseStream = api.PutDrawingFbx(request);

            using (var fileStream = System.IO.File.Create(outFile))
            {
                responseStream.Seek(0, SeekOrigin.Begin);
                responseStream.CopyTo(fileStream);
                fileStream.Close();
            }

            PostDrawingPropertiesRequest getProperties = new PostDrawingPropertiesRequest();
            FileStream fs = File.OpenRead(outFile);
            getProperties.DrawingData = fs;
            CadResponse responseStreamProperties = api.PostDrawingProperties(getProperties);
            fs.Close();

            Assert.AreEqual(98, responseStreamProperties.Height, 0.1);
            Assert.AreEqual(149, responseStreamProperties.Width, 0.1);

            File.Delete(outFile);
        }

        [Test]
        public void PutDrawingGifTest()
        {
            string fileName = Path.Combine(LocalTestFolder, "910609.dxf");
            string outFile = "result.gif";

            double newWidth = 200;
            double newHeight = 200;

            PutDrawingGifRequest request = new PutDrawingGifRequest();
            request.DrawingData = System.IO.File.OpenRead(fileName);
            request.ExportOptions = @"{""VectorRasterizationOptions"": { ""PageHeight"":"
                + newHeight.ToString(System.Globalization.CultureInfo.InvariantCulture) + @",""PageWidth"":" + newWidth.ToString(System.Globalization.CultureInfo.InvariantCulture) + @"}}";

            var responseStream = api.PutDrawingGif(request);

            using (var fileStream = System.IO.File.Create(outFile))
            {
                responseStream.Seek(0, SeekOrigin.Begin);
                responseStream.CopyTo(fileStream);
                fileStream.Close();
            }

            PostDrawingPropertiesRequest getProperties = new PostDrawingPropertiesRequest();
            FileStream fs = File.OpenRead(outFile);
            getProperties.DrawingData = fs;
            CadResponse responseStreamProperties = api.PostDrawingProperties(getProperties);
            fs.Close();

            Assert.AreEqual(200, responseStreamProperties.Height, 0.1);
            Assert.AreEqual(200, responseStreamProperties.Width, 0.1);

            File.Delete(outFile);
        }

        [Test]
        public void PutDrawingGlbTest()
        {
            string fileName = Path.Combine(LocalTestFolder, "910609.dxf");
            string outFile = "result.glb";

            double newWidth = 200;
            double newHeight = 200;

            PutDrawingGlbRequest request = new PutDrawingGlbRequest();
            request.DrawingData = System.IO.File.OpenRead(fileName);
            request.ExportOptions = @"{""VectorRasterizationOptions"": { ""PageHeight"":"
                + newHeight.ToString(System.Globalization.CultureInfo.InvariantCulture) + @",""PageWidth"":" + newWidth.ToString(System.Globalization.CultureInfo.InvariantCulture) + @"}}";

            var responseStream = api.PutDrawingGlb(request);

            using (var fileStream = System.IO.File.Create(outFile))
            {
                responseStream.Seek(0, SeekOrigin.Begin);
                responseStream.CopyTo(fileStream);
                fileStream.Close();
            }

            PostDrawingPropertiesRequest getProperties = new PostDrawingPropertiesRequest();
            FileStream fs = File.OpenRead(outFile);
            getProperties.DrawingData = fs;
            CadResponse responseStreamProperties = api.PostDrawingProperties(getProperties);
            fs.Close();

            Assert.AreEqual(97, responseStreamProperties.Height, 0.1);
            Assert.AreEqual(149, responseStreamProperties.Width, 0.1);

            File.Delete(outFile);
        }

        [Test]
        public void PutDrawingGltfTest()
        {
            string fileName = Path.Combine(LocalTestFolder, "910609.dxf");
            string outFile = "result.gltf";

            double newWidth = 200;
            double newHeight = 200;

            PutDrawingGltfRequest request = new PutDrawingGltfRequest();
            request.DrawingData = System.IO.File.OpenRead(fileName);
            request.ExportOptions = @"{""VectorRasterizationOptions"": { ""PageHeight"":"
                + newHeight.ToString(System.Globalization.CultureInfo.InvariantCulture) + @",""PageWidth"":" + newWidth.ToString(System.Globalization.CultureInfo.InvariantCulture) + @"}}";

            var responseStream = api.PutDrawingGltf(request);

            using (var fileStream = System.IO.File.Create(outFile))
            {
                responseStream.Seek(0, SeekOrigin.Begin);
                responseStream.CopyTo(fileStream);
                fileStream.Close();
            }

            PostDrawingPropertiesRequest getProperties = new PostDrawingPropertiesRequest();
            FileStream fs = File.OpenRead(outFile);
            getProperties.DrawingData = fs;
            CadResponse responseStreamProperties = api.PostDrawingProperties(getProperties);
            fs.Close();

            Assert.AreEqual(97, responseStreamProperties.Height, 0.1);
            Assert.AreEqual(149, responseStreamProperties.Width, 0.1);

            File.Delete(outFile);
        }

        [Test]
        public void PutDrawingJpegTest()
        {
            string fileName = Path.Combine(LocalTestFolder, "910609.dxf");
            string outFile = "result.jpg";

            double newWidth = 200;
            double newHeight = 200;

            PutDrawingJpegRequest request = new PutDrawingJpegRequest();
            request.DrawingData = System.IO.File.OpenRead(fileName);
            request.ExportOptions = @"{""VectorRasterizationOptions"": { ""PageHeight"":"
                + newHeight.ToString(System.Globalization.CultureInfo.InvariantCulture) + @",""PageWidth"":" + newWidth.ToString(System.Globalization.CultureInfo.InvariantCulture) + @"}}";

            var responseStream = api.PutDrawingJpeg(request);

            using (var fileStream = System.IO.File.Create(outFile))
            {
                responseStream.Seek(0, SeekOrigin.Begin);
                responseStream.CopyTo(fileStream);
                fileStream.Close();
            }

            PostDrawingPropertiesRequest getProperties = new PostDrawingPropertiesRequest();
            FileStream fs = File.OpenRead(outFile);
            getProperties.DrawingData = fs;
            CadResponse responseStreamProperties = api.PostDrawingProperties(getProperties);
            fs.Close();

            Assert.AreEqual(200, responseStreamProperties.Height, 0.1);
            Assert.AreEqual(200, responseStreamProperties.Width, 0.1);

            File.Delete(outFile);
        }

        [Test]
        public void PutDrawingObjTest()
        {
            string fileName = Path.Combine(LocalTestFolder, "910609.dxf");
            string outFile = "result.obj";

            double newWidth = 200;
            double newHeight = 200;

            PutDrawingObjRequest request = new PutDrawingObjRequest();
            request.DrawingData = System.IO.File.OpenRead(fileName);
            request.ExportOptions = @"{""VectorRasterizationOptions"": { ""PageHeight"":"
                + newHeight.ToString(System.Globalization.CultureInfo.InvariantCulture) + @",""PageWidth"":" + newWidth.ToString(System.Globalization.CultureInfo.InvariantCulture) + @"}}";

            var responseStream = api.PutDrawingObj(request);

            using (var fileStream = System.IO.File.Create(outFile))
            {
                responseStream.Seek(0, SeekOrigin.Begin);
                responseStream.CopyTo(fileStream);
                fileStream.Close();
            }

            PostDrawingPropertiesRequest getProperties = new PostDrawingPropertiesRequest();
            FileStream fs = File.OpenRead(outFile);
            getProperties.DrawingData = fs;
            CadResponse responseStreamProperties = api.PostDrawingProperties(getProperties);
            fs.Close();

            Assert.AreEqual(97, responseStreamProperties.Height, 0.1);
            Assert.AreEqual(149, responseStreamProperties.Width, 0.1);

            File.Delete(outFile);
        }
      
        [Test]
        public void PutDrawingPsdTest()
        {
            string fileName = Path.Combine(LocalTestFolder, "910609.dxf");
            string outFile = "result.psd";

            double newWidth = 200;
            double newHeight = 200;

            PutDrawingPsdRequest request = new PutDrawingPsdRequest();
            request.DrawingData = System.IO.File.OpenRead(fileName);
            request.ExportOptions = @"{""VectorRasterizationOptions"": { ""PageHeight"":"
                + newHeight.ToString(System.Globalization.CultureInfo.InvariantCulture) + @",""PageWidth"":" + newWidth.ToString(System.Globalization.CultureInfo.InvariantCulture) + @"}}";

            var responseStream = api.PutDrawingPsd(request);

            using (var fileStream = System.IO.File.Create(outFile))
            {
                responseStream.Seek(0, SeekOrigin.Begin);
                responseStream.CopyTo(fileStream);
                fileStream.Close();
            }

            PostDrawingPropertiesRequest getProperties = new PostDrawingPropertiesRequest();
            FileStream fs = File.OpenRead(outFile);
            getProperties.DrawingData = fs;
            CadResponse responseStreamProperties = api.PostDrawingProperties(getProperties);
            fs.Close();

            Assert.AreEqual(200, responseStreamProperties.Height, 0.1);
            Assert.AreEqual(200, responseStreamProperties.Width, 0.1);

            File.Delete(outFile);
        }

        [Test]
        public void PutDrawingSTPTest()
        {
            string fileName = Path.Combine(LocalTestFolder, "910609.dxf");
            string outFile = "result.stp";

            double newWidth = 200;
            double newHeight = 200;

            PutDrawingSTPRequest request = new PutDrawingSTPRequest();
            request.DrawingData = System.IO.File.OpenRead(fileName);
            request.ExportOptions = @"{""VectorRasterizationOptions"": { ""PageHeight"":"
                + newHeight.ToString(System.Globalization.CultureInfo.InvariantCulture) + @",""PageWidth"":" + newWidth.ToString(System.Globalization.CultureInfo.InvariantCulture) + @"}}";

            var responseStream = api.PutDrawingSTP(request);

            using (var fileStream = System.IO.File.Create(outFile))
            {
                responseStream.Seek(0, SeekOrigin.Begin);
                responseStream.CopyTo(fileStream);
                fileStream.Close();
            }

            PostDrawingPropertiesRequest getProperties = new PostDrawingPropertiesRequest();
            FileStream fs = File.OpenRead(outFile);
            getProperties.DrawingData = fs;
            CadResponse responseStreamProperties = api.PostDrawingProperties(getProperties);
            fs.Close();

            Assert.AreEqual(97, responseStreamProperties.Height, 0.1);
            Assert.AreEqual(149, responseStreamProperties.Width, 0.1);

            File.Delete(outFile);
        }

        [Test]
        public void PutDrawingSvgTest()
        {
            string fileName = Path.Combine(LocalTestFolder, "910609.dxf");
            string outFile = "result.svg";

            double newWidth = 200;
            double newHeight = 200;

            PutDrawingSvgRequest request = new PutDrawingSvgRequest();
            request.DrawingData = System.IO.File.OpenRead(fileName);
            request.ExportOptions = @"{""VectorRasterizationOptions"": { ""PageHeight"":"
                + newHeight.ToString(System.Globalization.CultureInfo.InvariantCulture) + @",""PageWidth"":" + newWidth.ToString(System.Globalization.CultureInfo.InvariantCulture) + @"}}";

            var responseStream = api.PutDrawingSvg(request);

            using (var fileStream = System.IO.File.Create(outFile))
            {
                responseStream.Seek(0, SeekOrigin.Begin);
                responseStream.CopyTo(fileStream);
                fileStream.Close();
            }

            PostDrawingPropertiesRequest getProperties = new PostDrawingPropertiesRequest();
            FileStream fs = File.OpenRead(outFile);
            getProperties.DrawingData = fs;
            CadResponse responseStreamProperties = api.PostDrawingProperties(getProperties);
            fs.Close();

            Assert.AreEqual(200, responseStreamProperties.Height, 0.1);
            Assert.AreEqual(200, responseStreamProperties.Width, 0.1);

            File.Delete(outFile);
        }

        [Test]
        public void PutDrawingThreeDSTest()
        {
            string fileName = Path.Combine(LocalTestFolder, "910609.dxf");
            string outFile = "result.3ds";

            double newWidth = 200;
            double newHeight = 200;

            PutDrawingThreeDSRequest request = new PutDrawingThreeDSRequest();
            request.DrawingData = System.IO.File.OpenRead(fileName);
            request.ExportOptions = @"{""VectorRasterizationOptions"": { ""PageHeight"":"
                + newHeight.ToString(System.Globalization.CultureInfo.InvariantCulture) + @",""PageWidth"":" + newWidth.ToString(System.Globalization.CultureInfo.InvariantCulture) + @"}}";

            var responseStream = api.PutDrawingThreeDS(request);

            using (var fileStream = System.IO.File.Create(outFile))
            {
                responseStream.Seek(0, SeekOrigin.Begin);
                responseStream.CopyTo(fileStream);
                fileStream.Close();
            }

            PostDrawingPropertiesRequest getProperties = new PostDrawingPropertiesRequest();
            FileStream fs = File.OpenRead(outFile);
            getProperties.DrawingData = fs;
            CadResponse responseStreamProperties = api.PostDrawingProperties(getProperties);
            fs.Close();

            Assert.AreEqual(97, responseStreamProperties.Height, 0.1);
            Assert.AreEqual(149, responseStreamProperties.Width, 0.1);

            File.Delete(outFile);
        }

        [Test]
        public void PutDrawingTiffTest()
        {
            string fileName = Path.Combine(LocalTestFolder, "910609.dxf");
            string outFile = "result.tif";

            double newWidth = 200;
            double newHeight = 200;

            PutDrawingTiffRequest request = new PutDrawingTiffRequest();
            request.DrawingData = System.IO.File.OpenRead(fileName);
            request.ExportOptions = @"{""VectorRasterizationOptions"": { ""PageHeight"":"
                + newHeight.ToString(System.Globalization.CultureInfo.InvariantCulture) + @",""PageWidth"":" + newWidth.ToString(System.Globalization.CultureInfo.InvariantCulture) + @"}}";

            var responseStream = api.PutDrawingTiff(request);

            using (var fileStream = System.IO.File.Create(outFile))
            {
                responseStream.Seek(0, SeekOrigin.Begin);
                responseStream.CopyTo(fileStream);
                fileStream.Close();
            }

            PostDrawingPropertiesRequest getProperties = new PostDrawingPropertiesRequest();
            FileStream fs = File.OpenRead(outFile);
            getProperties.DrawingData = fs;
            CadResponse responseStreamProperties = api.PostDrawingProperties(getProperties);
            fs.Close();

            Assert.AreEqual(200, responseStreamProperties.Height, 0.1);
            Assert.AreEqual(200, responseStreamProperties.Width, 0.1);

            File.Delete(outFile);
        }
    }
}
