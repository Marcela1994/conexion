using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;

namespace LoginWebService
{
    public class RestService
    {
        private static byte[] keybytes = Encoding.UTF8.GetBytes("4$8mWQj?B/=N7Y,/");
        private static byte[] iv = Encoding.UTF8.GetBytes("4$8mWQj?B/=N7Y,/");
        public static void initCert()
        {
            System.Net.ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(
            (se, cert, chain, sslerror) =>
            {
                if (sslerror != SslPolicyErrors.None)
                {
                    return true;//Acepta de forma forzada el certificado de poseidon0404
                    //return (cert.GetIssuerName() == "DC=inet, DC=nh, CN=poseidon0404");//Acepta de forma forzada el certificado de poseidon0404
                }
                else
                {
                    return true;//deja pasar los certificados validos
                }

            });
        }
        public static T PostRequest<T>(string url, Object objeto)
        {

            HttpWebRequest request;
            request = WebRequest.Create(url) as HttpWebRequest;
            request.Timeout = 60 * 1000 * 20;
            request.Method = "POST";
            request.ContentType = "application/json";

            string json = JsonConvert.SerializeObject(objeto);//pasamos de objeto a json
            string respuesta = sendRequest(request, json);//Obtenemos la respuesta
            if (respuesta.Equals("Error en el servidor remoto: (404) No se encontró."))
            {
                string respuestaError = "{'Message': 'Not Found', '_httpCode': 404}";
                return JsonConvert.DeserializeObject<T>(respuestaError);
            }
            else
            {
                return JsonConvert.DeserializeObject<T>(respuesta);
            }
            //return JsonConvert.DeserializeObject<T>(respuesta); //pasamos de json a objeto
        }
        public static T PostRequest<T>(string url, Object objeto, string token)
        {

            HttpWebRequest request;
            request = WebRequest.Create(url) as HttpWebRequest;
            request.Timeout = 60 * 1000 * 20;
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", "Bearer " + token);
            string json = JsonConvert.SerializeObject(objeto);//pasamos de objeto a json
            //string json2 = JsonConvert.SerializeObject(json);
            string respuesta = sendRequest(request, json);//Obtenemos la respuesta
            return JsonConvert.DeserializeObject<T>(respuesta);
            //return JsonConvert.DeserializeObject<T>(respuesta); //pasamos de json a objeto
        }



        private static String sendRequest(HttpWebRequest request, string json)
        {
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
                ServicePointManager.ServerCertificateValidationCallback = (snder, cert, chain, error) => true;
                using (var httpResponse = (HttpWebResponse)request.GetResponse())
                {
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        String respuestaConCookie = "";
                        String setCookieHeader = httpResponse.Headers[HttpResponseHeader.SetCookie];

                        if (setCookieHeader != null)
                        {
                            respuestaConCookie = streamReader.ReadToEnd();
                            respuestaConCookie = respuestaConCookie.Substring(0, respuestaConCookie.Length - 1);

                            setCookieHeader = setCookieHeader.Replace("\"", "'");

                            respuestaConCookie = respuestaConCookie + ", \"valorCookie\":\"" + setCookieHeader + "\"}";

                            return respuestaConCookie;
                        }
                        return streamReader.ReadToEnd();
                    }
                }
            }
            catch (WebException wex)
            {

                return wex.Message;
            }
        }
        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            byte[] encrypted;
            // Create a RijndaelManaged object
            // with the specified key and IV.
            using (var rijAlg = new RijndaelManaged())
            {
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        public static string EncryptStringToBytes(string plainText)
        {
            var stringEncoded = EncryptStringToBytes(plainText, keybytes, iv);
            var cipherText = Convert.ToBase64String(stringEncoded);
            return cipherText;
        }
        private static readonly string ENCRYPTION_KEY = "4uLJXmU9gaMqu7yZnnRaH2hS9VCUUbzTNkbE8W5MLzLK2bL3x7WFxWvTd37rdYXAvA5J8jrGhDvF7D4XR3VYgU47EnyXGZqEHyxhwxS7bAU7kmuxRLfM6Ytc7p7C7vA7aD848gyTELEFONICAb7rjRjXBfZmsX9GFXwArRIoRStn2UBBUjNeYhVtURXDCKZGxHG8454KsySBhD4txrsJymh2wH9G7C9BD4sr78VztsFcGxjDzAkS7WfgT";

        public static string Encrypt(string clearText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(ENCRYPTION_KEY, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32); // 32 * 8
                encryptor.IV = pdb.GetBytes(16); // 16 * 8
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {

            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(ENCRYPTION_KEY, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32); // 32 * 8
                encryptor.IV = pdb.GetBytes(16); // 16 * 8
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

    }
}