﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data.Common;
using System.Runtime.Remoting.Messaging;

namespace CRUD
{
    public partial class Form1 : Form
    {
        public static List<string> addList = new List<string>();
        public static int RowCnt = 0;
        public static Boolean sw =false;
  
        public Form1()
        {
            
            InitializeComponent();

            toolStripLabel1.Text = "";
            
        }
        private MySqlConnection DbConn()
        {
            MySqlConnection conn; string strconn = "Server=localhost;Database=stu;Uid=root;Pwd=1234;";
            conn = new MySqlConnection(strconn);
            conn.Open();

            return conn;
        }

        private Boolean NullCheck(int i)
        {
            if (dgv1.Rows[i].Cells["no"].Value == null)
                return true;
            if (dgv1.Rows[i].Cells["grade"].Value == null)
                return true;
            if (dgv1.Rows[i].Cells["cclass"].Value == null)
                return true;
            if (dgv1.Rows[i].Cells["name"].Value == null)
                return true;

            return false;
        }

        private Boolean NumberCheck(int i)
        {
            int s = 0;
            if (!int.TryParse(dgv1.Rows[i].Cells["no"].Value.ToString(),out s))
                return true;
            if (!int.TryParse(dgv1.Rows[i].Cells["grade"].Value.ToString(), out s))
                return true;
            if (!int.TryParse(dgv1.Rows[i].Cells["cclass"].Value.ToString(), out s))
                return true;

            return false;
        }

        private void AllStore()
        {
            for (int i = 0; i < dgv1.RowCount; i++)
            {
                if (dgv1.Rows[i].HeaderCell.Value != null)
                {
                    if (dgv1.Rows[i].HeaderCell.Value.ToString() == "UP")
                    {
                        dgv1.CurrentCell = dgv1.Rows[i].Cells[0];
                        btnUpdate.PerformClick();

                    }
                    else if (dgv1.Rows[i].HeaderCell.Value.ToString() == "NEW")
                    {
                        dgv1.CurrentCell = dgv1.Rows[i].Cells[0];
                        btnCreate.PerformClick();
                    }
                }
            }
        }


        private void btnRead_Click(object sender, EventArgs e)
        {

            MySqlConnection conn = DbConn();

            DataSet ds = new DataSet();
            addList.Clear();

            string sql = "select * from student;";
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.SelectCommand = new MySqlCommand(sql, conn);
            adapter.Fill(ds);

            sw = false;

            int i = 0;
            RowCnt = ds.Tables[0].Rows.Count;
            dgv1.RowCount = ds.Tables[0].Rows.Count + 1;
            

            foreach (DataRow s in ds.Tables[0].Rows)
            {
                // 학번
                if (s["no"] != System.DBNull.Value)
                    dgv1.Rows[i].Cells["no"].Value = s["no"].ToString();
                //학년
                if (s["grade"] != System.DBNull.Value)
                    dgv1.Rows[i].Cells["grade"].Value = s["grade"].ToString();
                //반
                if (s["cclass"] != System.DBNull.Value)
                    dgv1.Rows[i].Cells["cclass"].Value = s["cclass"].ToString();
                //이름
                if (s["name"] != System.DBNull.Value)
                    dgv1.Rows[i].Cells["name"].Value = s["name"].ToString();
                //점수
                if (s["score"] != System.DBNull.Value)
                    dgv1.Rows[i].Cells["score"].Value = s["score"].ToString();
                i++;
            }
            sw = true;
            toolStripLabel1.Text = "조회 완료";
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = DbConn();
            DataSet ds = new DataSet();
            addList.Clear();

            try
            {
                if (NullCheck(dgv1.CurrentRow.Index))
                {
                    MessageBox.Show("필수항목 비어있음");
                    return;
                }
                if (NumberCheck(dgv1.CurrentRow.Index))
                {
                    MessageBox.Show("학년, 반, 학번은 숫자로 기입해주세요");
                    return;
                }

                sw = false;
               
                addList.Add(dgv1.CurrentRow.Cells["no"].Value.ToString());
                addList.Add(dgv1.CurrentRow.Cells["grade"].Value.ToString());
                addList.Add(dgv1.CurrentRow.Cells["cclass"].Value.ToString());
                addList.Add(dgv1.CurrentRow.Cells["name"].Value.ToString());
                if (dgv1.CurrentRow.Cells["score"].Value == null)
                    addList.Add(" ");
                else
                    addList.Add(dgv1.CurrentRow.Cells["score"].Value.ToString());

                string sql = "insert into student values(" + addList[0] + "," + addList[1] + "," + addList[2] + ",'" + addList[3] + "','" + addList[4] + "');";
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = new MySqlCommand(sql, conn);
                adapter.Fill(ds);
                
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return;
            }

            
            dgv1.CurrentRow.HeaderCell.Value = null;
            sw = true;
            toolStripLabel1.Text = "추가 완료"; 
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = DbConn();
            DataSet ds = new DataSet();
            addList.Clear();

            try
            {
                if (NullCheck(dgv1.CurrentRow.Index))
                {
                    MessageBox.Show("필수항목 비어있음");
                    return;
                }
                if (NumberCheck(dgv1.CurrentRow.Index))
                {
                    MessageBox.Show("학년, 반, 학번은 숫자로 기입해주세요");
                    return;
                }
                sw = false;
                
                addList.Add(dgv1.CurrentRow.Cells["no"].Value.ToString());
                addList.Add(dgv1.CurrentRow.Cells["grade"].Value.ToString());
                addList.Add(dgv1.CurrentRow.Cells["cclass"].Value.ToString());
                addList.Add(dgv1.CurrentRow.Cells["name"].Value.ToString());
                if (dgv1.CurrentRow.Cells["score"].Value == null)
                    addList.Add(" ");
                else
                    addList.Add(dgv1.CurrentRow.Cells["score"].Value.ToString());

                string sql = "update student set no = " + addList[0] +
                                                 ",grade = " + addList[1] +
                                                 ",cclass =" + addList[2] +
                                                 ",name = '" + addList[3] + "' " +
                                                 ",score = '" + addList[4] + "' " +
                                                 "where no = " + addList[0] + ";";
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = new MySqlCommand(sql, conn);
                adapter.Fill(ds);

              
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return;
            }

            dgv1.CurrentRow.HeaderCell.Value = null;
            sw = true;
            toolStripLabel1.Text = "수정 완료";
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = DbConn();
            DataSet ds = new DataSet();
            addList.Clear();
            try
            {
                addList.Add(dgv1.CurrentRow.Cells["no"].Value.ToString());

                string sql = "delete from student " +
                             "where no = " + addList[0] + ";";

                MySqlDataAdapter adapter = new MySqlDataAdapter();
                adapter.SelectCommand = new MySqlCommand(sql, conn);
                adapter.Fill(ds);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
                return;
            }

            btnRead.PerformClick();
            toolStripLabel1.Text = "제거 완료";
        }

        private void dgv1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (sw)
            {
                if (RowCnt > dgv1.CurrentRow.Index)
                {
                    dgv1.CurrentRow.HeaderCell.Value = "UP";
                }
                else
                {
                    dgv1.CurrentRow.HeaderCell.Value = "NEW";
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
            for(int j = 0;j < dgv1.Rows.Count; j++)
            {
                if (dgv1.Rows[j].HeaderCell.Value != null ) {
                    DialogResult result = MessageBox.Show("저장되어있지않는 데이터가 있습니다. 저장할까요?", this.Text, MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        AllStore();

                        e.Cancel = false;
                    }
                    else if(result == DialogResult.No)
                    {
                        e.Cancel = false;
                    }
                    else
                    {
                        e.Cancel = true;
                    }
                    return;
                }
            }
        }
    }
}
