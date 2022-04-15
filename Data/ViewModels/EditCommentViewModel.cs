using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

//TODO: merge create/ edit blog viewmodels an create/ edit comment viewmodel

namespace BlogApp.Data.ViewModel
{
    [Obsolete]
    public class EditCommentViewModel
    {
        public string Content { get; set; }
    }
}