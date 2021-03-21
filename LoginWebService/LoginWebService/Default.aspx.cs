using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace LoginWebService
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void login_Click(object sender, EventArgs e)
        {
            LoginData loginData = new LoginData();
            loginData.nombreUsuario = userName.Text;
            loginData.claveUsuario = passUser.Text;
            string url = "http://localhost/webservices/api/login";
            LoginResponse respuesta = RestService.PostRequest<LoginResponse>(url, loginData);

            if (respuesta.resultado.Equals("ok"))
            {
                Response.Redirect("~/Home.aspx");

            }
            else
            {
                DialogResult mensaje;
                mensaje = MessageBox.Show("Datos ingresados no validos");
                Response.Redirect("~/Registrarse.aspx");
            }

        }

        public static void validar(Object loginData)
        {
            var url = $"http://localhost:51893/api/login";
            var request = (HttpWebRequest)WebRequest.Create(url);
            //string json = $"{{\"data\":\"{loginData}\"}}";
            request.Method = "POST";
            request.ContentType = "application/json";

            string json = JsonConvert.SerializeObject(loginData);
            //string respuesta = sendRequest(request, json);

            DialogResult mensaje;
            mensaje = MessageBox.Show("DATOS INGRESADOS: Nombre Usuario: " + json);

        }

        
    }


}
