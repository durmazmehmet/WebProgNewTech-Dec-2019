﻿using System.Web;
using System.Web.Mvc;

namespace _006_StudentBirthDayApp
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
