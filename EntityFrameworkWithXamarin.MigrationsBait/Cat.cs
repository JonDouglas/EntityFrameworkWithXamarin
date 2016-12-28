using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkWithXamarin.MigrationsBait
{
    public class Cat
    {
        [Key]
        public int CatId { get; set; }
        public string Name { get; set; }
        public int MeowsPerSecond { get; set; }
    }
}
