using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

//TODO: merge create/ edit blog viewmodels an create/ edit comment viewmodel

namespace RazorBlog.Data.ViewModel
{
    [Obsolete]
    public class EditCommentViewModel
    {
        public string Content { get; set; }
    }
}