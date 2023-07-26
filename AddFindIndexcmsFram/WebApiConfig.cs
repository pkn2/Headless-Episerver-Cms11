﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace AddFindIndexcmsFram
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var cors = new EnableCorsAttribute("http://localhost:63234", "*", "*");

            config.EnableCors(cors);
            //config.MapHttpAttributeRoutes();

        }
    }
}