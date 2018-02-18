using A1.Business;
using A1.Settings;
using Jig.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var options = CmdParser.Instance<CmdOptions>(Environment.GetCommandLineArgs());

            LblTitle.Text = options.TitleName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // input error
            if (TbxID.Text == "")
            {
                LblResult.Text = "検索条件が空白です、値を入力してください";
                return;
            }

            // business logic
            LblResult.Text = new SerchLogic().SearchName(TbxID.Text);
        }
    }
}
