﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProjectRepository.DBModels
{
    public class Comment
    {
        public int id { get; set; }
        public int? commentId { get; set; }
        public int postId { get; set; }
        public Guid userToken { get;  set; }
        public string text { get; set; }
        public DateTime createdAt { get; set; }
    }
}
