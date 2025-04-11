using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLYDAILY.Models
{
    public class LoaiDaiLy
    {

        public int MaLoaiDaiLy { get; set; } = 0;
        public string TenLoaiDaiLy { get; set; } = string.Empty;

        //// Mối quan hệ với DAILY
        //public ICollection<DaiLy> Dailies { get; set; }
    }
}
