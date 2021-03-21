using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;
using webservices.Models;

namespace webservices
{
    public class LoginController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public IHttpActionResult Post([FromBody] LoginRequest loginRequest)
        {
            if (loginRequest.nombreUsuario.Equals(null) && loginRequest.claveUsuario.Equals(null))
            {
                return BadRequest();
            }else if (loginRequest.nombreUsuario.Equals("marce") && loginRequest.claveUsuario.Equals("luna"))
            {
                return Ok(new { resultado = "ok"});
            }
            else
            {
                return Ok(new { resultado = "Error usuario no valido" });
            }
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}