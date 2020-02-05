using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackendOrganizationManagement.Main.Dto
{
    public class Filter
    {

        public int limit
        {
            get; set;
        }

        public int page
        {
            get; set;
        }
        public String orderType
        {
            get; set;
        }
        public String orderBy
        {
            get; set;
        }

        public bool contains
        {
            get; set;
        }

        public bool beginsWith
        {
            get; set;
        }

        public bool exacts
        {
            get; set;
        }

        public int year
        {
            get; set;
        }

        public int month
        {
            get; set;
        }

        public String module
        {
            get; set;
        }

        public Dictionary<String, Object> fieldsFilter
        {
            get; set;
        }

        public int monthTo
        {
            get; set;
        }
        public int yearTo
        {
            get; set;
        }
    }
}