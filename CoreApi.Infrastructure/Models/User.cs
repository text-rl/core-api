﻿using System;
using System.Collections.Generic;

namespace CoreApi.Infrastructure.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}