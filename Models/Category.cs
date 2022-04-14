using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SimpleTodoList.Models
{
    public partial class Category
    {
        public Category()
        {
            Lists = new HashSet<List>();
        }

        public Guid IdCategory { get; set; }
        [Required(ErrorMessage = "Empty Category")]
        [StringLength(15)]
        public string CategoryName { get; set; }

        public virtual ICollection<List> Lists { get; set; }
    }
}
