using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace SimpleTodoList.Models
{
    public partial class List
    {
        public Guid IdList { get; set; }
        
        [Required(ErrorMessage = "Empty Category")]
        public Guid? CategoryId { get; set; }
        
        [Required(ErrorMessage = "Empty Title")]
        [StringLength(60)]
        public string TitleList { get; set; }
        
        [Required(ErrorMessage = "Empty Description")]
        [StringLength(1000)]
        public string DescriptionList { get; set; }
        
        [Required(ErrorMessage = "Empty State")]
        public bool Complete { get; set; }
        
        public DateTime CreateDate { get; set; }

        public virtual Category Category { get; set; }
    }
}
