using StackOverflow.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StackOverflow.Web.Models
{
    public class ShowQuestionVM
    {
        public Question Question { get; set; }
        public bool AlreadyLiked { get; set; }
    }
}
