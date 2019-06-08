using SHLoaderTester.Workers;
using System;
using System.Windows.Forms;

namespace SHLoaderTester
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.ConnectionString.Text = "data source=10.20.5.2;initial catalog=RK7_SH4_3_10;integrated security=False;MultipleActiveResultSets=True;App=EntityFramework;User ID=sa;Password=Rkeeper001";
            this.SHServerName.Text = "10.20.1.2";
            this.SHServerPort.Text = "1005";
            this.SHUserName.Text = "VLADAS";
            this.SHPassword.Text = "123";
        }

        private void WorkerInit()
        {
            SHWorker.settings.ConnectionString = this.ConnectionString.Text;
            SHWorker.settings.SHServerAddress = this.SHServerName.Text;
            SHWorker.settings.SHServerPort = Convert.ToUInt16(this.SHServerPort.Text);
            SHWorker.settings.SHUserName = this.SHUserName.Text;
            SHWorker.settings.SHPassword = this.SHPassword.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WorkerInit();
            var duration = SHWorker.LoadComplectsTree();
            this.Output.Text += $"Load complects complete - it takes {duration}{Environment.NewLine}";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            WorkerInit();
            string errorInfo = "";
            try
            {
                var duration = SHWorker.LoadBaseComplects(groupRid.Text == "" ? 0 : Convert.ToInt64(groupRid.Text), out errorInfo);
                this.Output.Text += $"Load base complects - it takes {duration}{Environment.NewLine}";
            }
            catch (Exception error)
            {
                this.Output.Text += $"Load base complects - error: {errorInfo}{Environment.NewLine}";
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            WorkerInit();
            var duration = SHWorker.LoadHDRComplects();
            this.Output.Text += $"Load HDR complects - it takes {duration}{Environment.NewLine}";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
