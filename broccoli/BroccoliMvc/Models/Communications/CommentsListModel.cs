using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BroccoliTrade.Web.BroccoliMvc.Models.Communications
{
    public class CommentsListModel
    {
        public IEnumerable<CommentModel> CommentList { get; set; } 
    }
}