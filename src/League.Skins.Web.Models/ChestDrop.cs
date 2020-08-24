using System;
using System.ComponentModel.DataAnnotations;

namespace League.Skins.Web.Models
{
    public class ChestDropAddRequest
    {
        [Required]
        public string OwnerId { get; set; }
        [Required]
        public string ChestType { get; set; }
        [Required]
        public string DropType { get; set; }
        public string AdditionalDropType { get; set; }
        public string SkinRarity { get; set; }
        public string Comment { get; set; }
    }

    public class ChestDropResponse
    {
        public string Id { get; set; }
        public string ChestType { get; set; }
        public string DropType { get; set; }
        public string AdditionalDropType { get; set; }
        public string SkinRarity { get; set; }
        public DateTime CreationTime { get; set; }
        public string Comment { get; set; }
    }
}
