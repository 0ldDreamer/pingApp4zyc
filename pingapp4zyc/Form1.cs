using System;
using System.Net.NetworkInformation;
using System.Text;
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

        // Ping 方法，发送 ICMP 回显请求
        public static IPStatus Ping(string hostNameOrAddress)
        {
            IPStatus sta = IPStatus.TimedOut;
            Ping p = new Ping();
            PingOptions options = new PingOptions();
            options.DontFragment = true;  // 不分段
            string data = "abc";  // 数据包内容
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 200;  // 超时时间设置为200毫秒

            try
            {
                PingReply reply = p.Send(hostNameOrAddress, timeout, buffer, options);
                sta = reply.Status;  // 返回 Ping 的状态
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ping 过程中出现异常: " + ex.Message);
            }
            finally
            {
                if (p != null)
                {
                    // 释放 Ping 实例
                    IDisposable disposable = p;
                    disposable.Dispose();
                }
            }

            return sta;
        }

        // 点击按钮时执行 Ping 逻辑
        private void btnOK_Click_1(object sender, EventArgs e)
        {
            // 判断是否输入了目标主机地址或 IP
            if (!String.IsNullOrEmpty(txtHostNameOrAddress.Text.Trim()))
            {
                string strPing = txtHostNameOrAddress.Text.Trim();
                IPStatus iPStatus = Ping(strPing);

                // 根据 Ping 状态判断结果，并输出到 richTextBox
                switch (iPStatus)
                {
                    case IPStatus.Unknown:
                        richTextBox1.AppendText(strPing + ": " + iPStatus.ToString() + " :由于未知原因，ICMP 回送请求失败。\n");
                        break;
                    case IPStatus.Success:
                        richTextBox1.AppendText(strPing + ": " + iPStatus.ToString() + ": ICMP 回送请求成功；收到一个 ICMP 回送答复。\n");
                        break;
                    case IPStatus.DestinationNetworkUnreachable:
                        richTextBox1.AppendText(strPing + ": " + iPStatus.ToString() + ":由于无法访问包含目标计算机的网络，ICMP 回送请求失败。\n");
                        break;
                    case IPStatus.DestinationHostUnreachable:
                        richTextBox1.AppendText(strPing + ": " + iPStatus.ToString() + ":由于无法访问目标计算机，ICMP 回送请求失败。\n");
                        break;
                    case IPStatus.TimedOut:
                        richTextBox1.AppendText(strPing + ": " + iPStatus.ToString() + ":在所分配的时间内未收到 ICMP 回送答复。允许的默认答复时间为 5 秒。\n");
                        break;
                    case IPStatus.BadRoute:
                        richTextBox1.AppendText(strPing + ": " + iPStatus.ToString() + ":由于在源计算机和目标计算机之间没有有效的路由，ICMP 回送请求失败。\n");
                        break;
                    case IPStatus.TtlExpired:
                        richTextBox1.AppendText(strPing + ": " + iPStatus.ToString() + ":由于数据包的生存时间 (TTL) 值达到零，导致转发节点（路由器或网关）将数据包丢弃，ICMP 回送请求失败。\n");
                        break;
                    case IPStatus.TimeExceeded:
                        richTextBox1.AppendText(strPing + ": " + iPStatus.ToString() + ":由于数据包的生存时间 (TTL) 值达到零，导致转发节点（路由器或网关）将数据包丢弃，ICMP 回送请求失败。\n");
                        break;
                    default:
                        richTextBox1.AppendText(strPing + ": " + iPStatus.ToString() + ":其他原因！没有列出！\n");
                        break;
                }
            }
        }
    }
}
