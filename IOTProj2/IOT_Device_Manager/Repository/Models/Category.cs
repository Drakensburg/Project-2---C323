using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace IOT_Device_Manager.Repository.Models
{
    public partial class Category
    {
        [Key]
        [Column("CategoryID")]
        public Guid CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
    }
}
