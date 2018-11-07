using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Common;


namespace QL_banQuyen_ngienCuu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region SHOW
        private void button_show_Click(object sender, EventArgs e)
        {
            SqlConnection conn = DBSQLServerUtils.GetDBConnection();

            try
            {
                conn.Open();
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("ERROR2!" + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            
        }
        #endregion

        #region LoadDATA
        private void LoadData()
        {
            try
            {
                string query = "Select ROW_NUMBER() OVER(ORDER BY deTai.[Mã đề tài] asc) as STT,[Mã đề tài],[Tên đề tài],[Cấp chủ đề],[Chủ nhiệm],[Tình trạng],[Ngày Nhận] FROM deTai";
                SqlConnection conn = DBSQLServerUtils.GetDBConnection();
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                SqlCommandBuilder cmd = new SqlCommandBuilder(adapter);
                adapter.InsertCommand = cmd.GetInsertCommand();
                adapter.UpdateCommand = cmd.GetUpdateCommand();
                adapter.DeleteCommand = cmd.GetDeleteCommand();

                DataSet data = new DataSet();
                adapter.Fill(data);
                
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = data.Tables[0];
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR!!!/n" + e.Message);
            }

        }
        #endregion

        #region ADD
        private void button_add_Click(object sender, EventArgs e)
        {

            Form_ADD formAdd = new Form_ADD();
            formAdd.ctn = ShowData;
            formAdd.ShowDialog();
        }
        #endregion

        #region showData
        private void ShowData(string textBox_ID, string textBox_name, DateTime dateTime,
            string comboBox_level, string comboBox_chairMan, Boolean radioButton)
        {
            this.textBox_ID.Text = textBox_ID;
            this.textBox_name.Text = textBox_name;
            this.dateTimePicker.Value = dateTime;
            this.comboBox_level.Text = comboBox_level;
            this.comboBox_chairMan.Text = comboBox_chairMan;
            if (radioButton == true)
            {
                this.radioButton_done.Checked = true;
            }
            else
            {
                this.radioButton_waiting.Checked = true;
            }


        }

        #endregion

        int rowIndex;
        #region RowHeaderMouserClick
        private void dataGridView1_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            rowIndex = e.RowIndex;
            try
            {

                textBox_ID.Text = dataGridView1.SelectedRows[0].Cells["Mã đề tài"].Value.ToString();
                textBox_name.Text = dataGridView1.SelectedRows[0].Cells["Tên đề tài"].Value.ToString();

                dateTimePicker.Value = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells["Ngày Nhận"].Value);
                comboBox_level.Text = dataGridView1.SelectedRows[0].Cells["Cấp chủ đề"].Value.ToString();
                comboBox_chairMan.Text = dataGridView1.SelectedRows[0].Cells["Chủ nhiệm"].Value.ToString();
                if (Convert.ToBoolean(dataGridView1.SelectedRows[0].Cells["Tình trạng"].Value))
                {
                    radioButton_done.Checked = true;
                }
                else
                {
                    radioButton_waiting.Checked = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("error!" + ex.Message);
            }

        }
        #endregion

        #region Update
        private void button_update_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM deTai";
            SqlConnection conn = DBSQLServerUtils.GetDBConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            SqlCommandBuilder cmd = new SqlCommandBuilder(adapter);
            adapter.InsertCommand = cmd.GetInsertCommand();
            adapter.UpdateCommand = cmd.GetUpdateCommand();
            adapter.DeleteCommand = cmd.GetDeleteCommand();

            DataSet data = new DataSet();
            adapter.Fill(data);
            int r = rowIndex;
            data.Tables[0].Rows[r]["Mã đề tài"] = textBox_ID.Text;
            data.Tables[0].Rows[r]["Tên đề tài"] = textBox_name.Text;
            data.Tables[0].Rows[r]["Cấp chủ đề"] = comboBox_level.Text;
            data.Tables[0].Rows[r]["Chủ nhiệm"] = comboBox_chairMan.Text;
            if (Convert.ToBoolean(radioButton_done.Checked))
            {
                data.Tables[0].Rows[r]["Tình trạng"] = true;
            }
            else
            {
                data.Tables[0].Rows[r]["Tình trạng"] = false;
            }
            data.Tables[0].Rows[r]["Ngày Nhận"] = Convert.ToDateTime(dateTimePicker.Value);
            adapter.Update(data);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = data.Tables[0];
        }
        #endregion

        #region Delete
        private void button_delete_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Đang xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                string query = "SELECT * FROM deTai";

                SqlConnection conn = DBSQLServerUtils.GetDBConnection();
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                SqlCommandBuilder cmd = new SqlCommandBuilder(adapter);
                adapter.InsertCommand = cmd.GetInsertCommand();
                adapter.UpdateCommand = cmd.GetUpdateCommand();
                adapter.DeleteCommand = cmd.GetDeleteCommand();
                DataSet data = new DataSet();
                adapter.Fill(data);
                

                //dataGridView1.Rows.RemoveAt(rowIndex);
                data.Tables[0].Rows[rowIndex].Delete();


                adapter.Update(data);
                dataGridView1.DataSource = null;
                LoadData();
            }
        }
        #endregion

        #region Search
        private void button_search_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM deTai WHERE [Mã đề tài] LIKE '%" + textBox_search.Text.ToString()+"%'";
            SqlConnection conn = DBSQLServerUtils.GetDBConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
            DataSet dt = new DataSet();
            adapter.Fill(dt);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = dt.Tables[0];
        }//Search theo ten de tai
        #endregion

        #region Sort
        private void button_sort_Click(object sender, EventArgs e)
        {
            string query = "SELECT [Mã đề tài],[Tên đề tài],deTai.[Cấp chủ đề],[Tên chủ đề],[Chủ nhiệm],[Tình trạng],[Ngày Nhận]" +
 "FROM deTai INNER JOIN capDeTai ON deTai.[Cấp chủ đề] = capDeTai.[Cấp chủ đề] ORDER BY [Tên chủ đề],[Chủ nhiệm];";
            SqlConnection conn = DBSQLServerUtils.GetDBConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(query, conn);

            DataSet data = new DataSet();
            adapter.Fill(data);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = data.Tables[0];
        }
        #endregion



        #region NotQT
        private void Form1_Load(object sender, EventArgs e)
        {
            //dataGridView1.Columns.Add("columnSTT", "STT");
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            /*try{

            textBox_ID.Text = dataGridView1.SelectedRows[0].Cells["ID_deTai"].Value.ToString();
            textBox_name.Text = dataGridView1.SelectedRows[0].Cells["name_deTai"].Value.ToString();
            dateTimePicker.Value = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells["date_receive_deTai"].Value);
            comboBox_level.Text = dataGridView1.SelectedRows[0].Cells["IDLevel_deTai"].Value.ToString();
            comboBox_chairMan.Text = dataGridView1.SelectedRows[0].Cells["chairMan_deTai"].Value.ToString();
            if(Convert.ToBoolean(dataGridView1.SelectedRows[0].Cells["status_deTai"].Value))
            {
                radioButton_done.Checked = true;
            }
            else
            {
                radioButton_waiting.Checked = true;
            }
            }catch(Exception ex)
            {
                MessageBox.Show("error!"+ex.Message);
            }*/
        }//bo
        #endregion

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {

            }
        }
    }
}
