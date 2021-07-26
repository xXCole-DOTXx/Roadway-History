using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roadway_History.Models
{
    public class ViewModel
    {
        public Statewide Statewide { get; set; }
        public List<Document> Document { get; set; }
    }
}