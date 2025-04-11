using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QUANLYDAILY.Models
{
    public class DaiLy
    {

        public int MaDaiLy { get; set; } = 0;
        public string Ten { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public string DienThoai { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;


        public int? MaLoaiDaiLy { get; set; }
        public int? MaQuan { get; set; }
        public DateTime? NgayTiepNhan { get; set; }

        //// Mối quan hệ với LOAIDAILY và QUAN
        //public LoaiDaiLy LoaiDaiLy { get; set; } = null!;
        //public Quan Quan { get; set; } = null!;

    }
}
