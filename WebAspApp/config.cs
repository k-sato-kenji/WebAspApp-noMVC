﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebAspApp
{
    public class config
    {
        static string _constr = null;

        public static string ConnStr
        {
            get
            {
                if(_constr==null)
                {
                    _constr = ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
                }

                return _constr;
            
            }
        }
    }
}