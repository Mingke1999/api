using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    [Table("Portfolios")]
    public class Portfolio
    {
        public string AppUserId {get; set;}
        public int StockId {get; set;}
        //navigation property
        public User AppUser {get; set;}
        public Stock Stock {get; set;}
    }
}