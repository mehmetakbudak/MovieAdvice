using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieAdvice.Storage.Entities
{
    public class WebsiteParameter : BaseEntity
    {
        public int? ParentId { get; set; }

        public string Code { get; set; }

        public string Value { get; set; }

        public bool Required { get; set; }

        public bool Visible { get; set; }
    }
}
