
using System.Collections.ObjectModel;
using System.Windows;
using System;
using QUANLYDAILY.Datahelper;
using System.Windows.Controls;
using QUANLYDAILY.Models;
using System.Data;


namespace QUANLYDAILY
{
    public partial class MainWindow : Window
    {
        Databasehelper db = new Databasehelper();

        public MainWindow()
        {
            InitializeComponent();

            LoadLoaiDaiLy();
            LoadQuan();
            LoadDaiLy();
        }

        private void LoadLoaiDaiLy()
        {
            string query = "SELECT * FROM LOAIDAILY";
            DataTable dt = db.ExecuteQuery(query);
            cbLoaiDaiLy.ItemsSource = dt.DefaultView;

        }

        private void LoadQuan()
        {
            string query = "SELECT * FROM QUAN";
            DataTable dt = db.ExecuteQuery(query);
            cbQuan.ItemsSource = dt.DefaultView;
        }

        private void LoadDaiLy()
        {
            string query = "SELECT * FROM DAILY";
            DataTable dt = db.ExecuteQuery(query);
            dgDaiLy.ItemsSource = dt.DefaultView;
        }

        private string TaoMaDaiLyMoi()
        {
            string query = "SELECT MaDaiLy FROM DAILY ORDER BY MaDaiLy DESC LIMIT 1";
            DataTable dt = db.ExecuteQuery(query);

            if (dt.Rows.Count == 0)
                return "DL001"; // Nếu chưa có dữ liệu

            string maCuoi = dt.Rows[0]["MaDaiLy"].ToString(); // VD: "DL007"
            int so = int.Parse(maCuoi.Substring(2)); // lấy "007" => 7
            so++; // tăng lên => 8
            return "DL" + so.ToString("D3"); // => "DL008"
        }
        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            // Lấy dữ liệu từ form
            string ten = txtTen.Text;
            string dienthoai = txtDienThoai.Text;
            string diachi = txtDiaChi.Text;
            string email = txtEmail.Text;
            string ngayTN = dpNgayTiepNhan.SelectedDate?.ToString("yyyy-MM-dd") ?? "";
            string maloai = cbLoaiDaiLy.SelectedValue?.ToString() ?? "";
            string maquan = cbQuan.SelectedValue?.ToString() ?? "";

            string maDL = TaoMaDaiLyMoi();
            // Kiểm tra ràng buộc THAMSO: SoLuongDaiLyToiDa, nếu cần
            //int soLuongDaiLyToiDa = GetSoLuongDaiLyToiDa();
            //if (ListDaiLy.Count >= soLuongDaiLyToiDa)
            //{
            //    MessageBox.Show($"Vượt quá số lượng đại lý tối đa ({soLuongDaiLyToiDa}). Không thể thêm.");
            //    return;
            //}

            string query = $"INSERT INTO DAILY (MaDaiLy, Ten, DienThoai, DiaChi, Email, NgayTiepNhan, MaLoaiDaiLy, MaQuan) " +
                   $"VALUES ('{maDL}', '{ten}', '{dienthoai}', '{diachi}', '{email}', '{ngayTN}', '{maloai}', '{maquan}')";

            int rows = db.ExecuteNonQuery(query);
            if (rows > 0)
            {
                MessageBox.Show("Thêm thành công!");
                LoadDaiLy();
            }
            else
            {
                MessageBox.Show("Thêm thất bại!");
            }
        }

        private void dgDaiLy_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgDaiLy.SelectedItem == null) return;

            DataRowView row = dgDaiLy.SelectedItem as DataRowView;

            txtTen.Text = row["Ten"].ToString();
            txtDienThoai.Text = row["DienThoai"].ToString();
            txtDiaChi.Text = row["DiaChi"].ToString();
            txtEmail.Text = row["Email"].ToString();
            dpNgayTiepNhan.SelectedDate = DateTime.Parse(row["NgayTiepNhan"].ToString());
            cbLoaiDaiLy.SelectedValue = row["MaLoaiDaiLy"].ToString();
            cbQuan.SelectedValue = row["MaQuan"].ToString();
        }

        private void btnSua_Click(object sender, RoutedEventArgs e)
        {
            if (dgDaiLy.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn đại lý để sửa.");
                return;
            }

            DataRowView row = dgDaiLy.SelectedItem as DataRowView;
            string maDL = row["MaDaiLy"].ToString();
            string ten = txtTen.Text;
            string dienthoai = txtDienThoai.Text;
            string diachi = txtDiaChi.Text;
            string email = txtEmail.Text;
            string ngayTN = dpNgayTiepNhan.SelectedDate?.ToString("yyyy-MM-dd") ?? "";
            string maloai = cbLoaiDaiLy.SelectedValue?.ToString() ?? "";
            string maquan = cbQuan.SelectedValue?.ToString() ?? "";

            string query = $"UPDATE DAILY SET Ten='{ten}', DienThoai='{dienthoai}', DiaChi='{diachi}', Email='{email}', " +
                           $"NgayTiepNhan='{ngayTN}', MaLoaiDaiLy='{maloai}', MaQuan='{maquan}' WHERE MaDaiLy='{maDL}'";

            int rows = db.ExecuteNonQuery(query);
            if (rows > 0)
            {
                MessageBox.Show("Sửa thành công!");
                LoadDaiLy();
            }
            else
            {
                MessageBox.Show("Sửa thất bại!");
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            if (dgDaiLy.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn đại lý để xoá.");
                return;
            }

            DataRowView row = dgDaiLy.SelectedItem as DataRowView;
            string maDL = row["MaDaiLy"].ToString();

            var result = MessageBox.Show("Bạn có chắc muốn xoá đại lý này?", "Xác nhận", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                string query = $"DELETE FROM DAILY WHERE MaDaiLy='{maDL}'";
                int rows = db.ExecuteNonQuery(query);

                if (rows > 0)
                {
                    MessageBox.Show("Xoá thành công!");
                    LoadDaiLy();
                }
                else
                {
                    MessageBox.Show("Xoá thất bại!");
                }
            }
        }


        //private int GetSoLuongDaiLyToiDa()
        //{
        //    // Hàm đọc bảng THAMSO, trả về số lượng đại lý tối đa
        //    using (var con = _db.GetConnection())
        //    {
        //        con.Open();
        //        var cmd = con.CreateCommand();
        //        cmd.CommandText = "SELECT SoLuongDaiLyToiDa FROM THAMSO LIMIT 1";
        //        var result = cmd.ExecuteScalar();
        //        if (result != null && int.TryParse(result.ToString(), out int val))
        //        {
        //            return val;
        //        }
        //    }
        //    return int.MaxValue; // Nếu ko tìm thấy thì trả về giá trị rất lớn
        //}
    }

}
