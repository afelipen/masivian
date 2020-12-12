using System;
using System.Collections.Generic;
using System.Text;

namespace Masivian.Roulette.Domain.Entities
{
    [Serializable]
    public class SpinWheel
    {
        public string Id { get; set; }
        public bool isOpen { get; set; }
        public DateTime? dateOpen { get; set; }
        public DateTime? dateClose{ get; set; }
    }
}
