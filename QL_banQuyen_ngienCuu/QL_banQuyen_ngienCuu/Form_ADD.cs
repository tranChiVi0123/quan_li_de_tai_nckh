using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace QL_banQuyen_ngienCuu
{
    public partial class Form_ADD : Form
    {
        public Form_ADD()
        {
            InitializeComponent();
        }
        
        public delegate void container(string textBox_ID,string textBox_name,DateTime dateTime,
            string comboBox_level,string comboBox_chairMan,Boolean radioButton);
        public container ctn;

        private void button_ok_Click(object sender, EventArgs e)
        {
            ctn.Invoke(textBox_ID.Text,textBox_name.Text,dateTimePicker.Value,
                    comboBox_level.Text,comboBox_chairMan.Text,radioButton_done.Checked);
           try{
                if(textBox_ID.Text!="" && textBox_name.Text!=""&&comboBox_level.Text!=""
                    &&comboBox_chairMan.Text!=""&&radioButton_done.Checked!=null){
                 string query = @"INSERT INTO deTai([Mã đề tài],[Tên đề tài],[Cấp chủ đề],[Chủ nhiệm],[Tình trạng],[Ngày Nhận]) VALUES ('"+textBox_ID.Text.ToString()+"','"+textBox_name.Text.ToString()+"','"+comboBox_level.Text.ToString()+"','"+comboBox_chairMan.Text.ToString()+"','"+radioButton_done.Checked.ToString()+"','"
                    +dateTimePicker.Value.ToString()+"');";
                SqlConnection conn = DBSQLServerUtils.GetDBConnection();
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataSet dt = new DataSet();
                adapter.Fill(dt);
                }else
                {
                    MessageBox.Show("Yêu Cầu Nhập Đầy Đủ");
                }

           }catch(Exception ex)
           {
                MessageBox.Show("ERROR!!! \n"+ex.Message);
           }
            finally
            {
                Close();
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
