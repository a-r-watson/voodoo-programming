using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Web.Hosting;

namespace One2CatchThemAll.Controllers
{
    public class QRController : ApiController
    {


        //FTP Server URL.
        string ftp = "ftp://yourdomain.com/";

        //FTP Folder name. Leave blank if you want to upload to root folder.
        string ftpFolder = "";

        byte[] fileBytes = null;




        // GET api/qr
        public string Get()
        {
            return "OK";
            
        }

        // GET api/qr/5
        public string Get(int id)
        {
            return "value";
        }




        // POST api/qr
        [HttpPost]
        public string Post(HttpRequestMessage inRequest)
        {
            string workstring = "";
            string QuizName = "";
            string workstring2 = "";
            string workstring3 = "";
            string SaveWholeString = "";
            string body = "";
        
            try { 
       
             body = inRequest.Content.ReadAsStringAsync().Result;



            // Create a string array with the lines of text
            workstring = "length of body " + body.Length.ToString() + " document " + body.ToString() ;  // - - - get the details of the request body into a string
            byte[] byteArray = Encoding.ASCII.GetBytes(workstring.ToString());
            MemoryStream stream = new MemoryStream(byteArray);


            //Read the FileName and convert it to Byte array.
            using (StreamReader fileStream = new StreamReader(stream))
            {
                fileBytes = Encoding.UTF8.GetBytes(fileStream.ReadToEnd());
                fileStream.Close();
            }

            try
            {
                //Create FTP Request.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftp + ftpFolder + "quizresults.txt");
                request.Method = WebRequestMethods.Ftp.UploadFile;

                //Enter FTP Server credentials.
                request.Credentials = new NetworkCredential("username", "password");
                request.ContentLength = fileBytes.Length;
                request.UsePassive = true;
                request.UseBinary = true;
                request.ServicePoint.ConnectionLimit = fileBytes.Length;
                request.EnableSsl = false;

                using (Stream requestStream = request.GetRequestStream())
                {
                    requestStream.Write(fileBytes, 0, fileBytes.Length);
                    requestStream.Close();
                }

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();


                response.Close();
            }
            catch (WebException ex)
            {
                throw new Exception((ex.Response as FtpWebResponse).StatusDescription);
            }



            return "OK"; // -- or the plugin complains about not being able to send results to the server
        }




        // PUT api/qr/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/qr/5
        public void Delete(int id)
        {
        }
    }
}
