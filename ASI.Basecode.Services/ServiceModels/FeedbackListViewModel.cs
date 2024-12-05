﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASI.Basecode.Services.ServiceModels
{
    public class FeedbackListViewModel
    {
        public List<FeedbackViewModel> Feedbacks { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchQuery { get; set; }
    }

}