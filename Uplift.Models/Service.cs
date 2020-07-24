﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Uplift.Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Service Name")]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }

        [Display(Name="Description")]
        public string LongDesc { get; set; }

        [DataType(DataType.ImageUrl)]
        [Display(Name ="Image")]
        public string ImageUrl { get; set; }



    }
}
