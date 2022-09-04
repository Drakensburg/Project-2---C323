using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace IOT_Device_Manager.Repository.Models
{
    public partial class Device
    {
        [Key]
        [Column("DeviceID")]
        public Guid DeviceId { get; set; }
        public string DeviceName { get; set; }
        [Column("CategoryID")]
        public Guid CategoryId { get; set; }
        [Column("ZoneID")]
        public Guid ZoneId { get; set; }
        public string Status { get; set; }
        public bool IsActvie { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateCreated { get; set; }
    }
}
