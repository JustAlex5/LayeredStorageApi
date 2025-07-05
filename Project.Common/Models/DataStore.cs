using Project.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Common.Models
{
    public class DataStore
    {
        public int Id { get; set; }
        public string Data { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public SourceTypeEnum SourceType { get; set; } = SourceTypeEnum.Manual;
    }
}
