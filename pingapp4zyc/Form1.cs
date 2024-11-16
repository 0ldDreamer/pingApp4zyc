using System;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pingapp4zyc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // 开始连续 Ping
        private async void btnOK_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtHostNameOrAddress.Text.Trim()))
            {
                MessageBox.Show("请输入目标主机地址或 IP！");
                return;
            }

            string strPing = txtHostNameOrAddress.Text.Trim();
            richTextBox1.AppendText($"正在 Ping {strPing} 具有 32 字节的数据:\n");

            try
            {
                Ping ping = new Ping();
                PingOptions options = new PingOptions
                {
                    DontFragment = true  // 不分段
                };
                string data = "abcdefghijklmnopqrstuvwxyz123456"; // 32 字节数据包
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 1000; // 超时时间设置为1秒

                for (int i = 0; i < 4; i++) // 连续发送 4 次 Ping
                {
                    PingReply reply = await ping.SendPingAsync(strPing, timeout, buffer, options);

                    if (reply.Status == IPStatus.Success)
                    {
                        richTextBox1.AppendText(
                            $"来自 {reply.Address}: 字节={buffer.Length} 时间={reply.RoundtripTime}ms TTL={reply.Options.Ttl}\n");
                    }
                    else
                    {
                        richTextBox1.AppendText($"请求超时。\n");
                    }

                    await Task.Delay(1000); // 等待 1 秒后继续
                }

                ping.Dispose();
            }
            catch (Exception ex)
            {
                richTextBox1.AppendText($"Ping 出现异常: {ex.Message}\n");
            }
        }
    }
}
