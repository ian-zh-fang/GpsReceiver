using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiger.GpsReceiver
{
    public partial class Form1 : Form
    {
        private const int GPSPARAME = 360000;
        private const string LISTITEMKEYPREFIX = "LISTITEMKEYPREFIX";
        private const int GPSPORTNORMAL = 65530;
        private const string GPSPORTNAPPSETTINGNAME = "gpsport";
        private const string MESSAGEPREFIX = "#";

        private static readonly Tiger.GpsAnalyzer.GpsDatagramAnalyzer GpsDatagramAnalyzer;

        private readonly int _Port;

        private int _NextIndex;

        static Form1()
        {
            GpsDatagramAnalyzer = new GpsAnalyzer.GpsDatagramAnalyzer();
        }

        public Form1()
        {
            InitializeComponent();

            _NextIndex = 1;

            string gpsport = System.Configuration.ConfigurationManager.AppSettings[GPSPORTNAPPSETTINGNAME];
            int.TryParse(gpsport, out _Port);

            if (0 == _Port)
                _Port = GPSPORTNORMAL;

            this.txtPort.Text = string.Format("{0}", _Port);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GpsDatagramAnalyzer.OnOpened += GpsDatagramAnalyzer_OnOpened;
            GpsDatagramAnalyzer.OnClosed += GpsDatagramAnalyzer_OnClosed;
            GpsDatagramAnalyzer.OnReceived += GpsDatagramAnalyzer_OnReceived;
            GpsDatagramAnalyzer.OnAnalyzed += GpsDatagramAnalyzer_OnAnalyzed;
            GpsDatagramAnalyzer.OnException += GpsDatagramAnalyzer_OnException;

            this.txtIpv4.Text = GetLocalIpv4();
            InsertTipDisplayAt();
            ChangeStateMessage(OperatorCode.None);
        }

        private void ChangeBtnStatue(bool flag = true)
        {
            if(this.btnStart.InvokeRequired)
            {
                this.Invoke(new Action<bool>(ChangeBtnStatue), flag);
                return;
            }

            this.btnStop.Enabled = !flag;
            this.btnStart.Enabled = flag;
        }

        private void UpdateListView(GpsAnalyzer.GpsDatagramModel data)
        {
            if (this.listResult.InvokeRequired)
            {
                Action<GpsAnalyzer.GpsDatagramModel> action = new Action<GpsAnalyzer.GpsDatagramModel>(UpdateListView);
                this.Invoke(action, data);
                return;
            }

            string key = string.Format("{0}-{1}", LISTITEMKEYPREFIX, data.Params.DeviceID);
            //在此更新列表数据信息
            int index = this.listResult.Items.IndexOfKey(key);
            
            //此处添加
            if (-1 == index)
            {
                this.listResult.Items.Add(new ListViewItem(
                    new string[] 
                    { 
                        _NextIndex.ToString(),
                        data.Params.DeviceID, 
                        data.Params.Longitude.ToString(), 
                        data.Params.Latitude.ToString(), 
                        data.Params.Time.ToString("yyyy-MM-dd HH:mm:ss") 
                    })
                    {
                        Name = key,
                        BackColor = (0 == _NextIndex % 2) ? System.Drawing.Color.LightGray : System.Drawing.Color.White
                    });
                _NextIndex++;
                return;
            }

            //此处更新
            ListViewItem item = this.listResult.Items[index];
            item.SubItems[2].Text = data.Params.Longitude.ToString();
            item.SubItems[3].Text = data.Params.Latitude.ToString();
            item.SubItems[4].Text = data.Params.Time.ToString("yyyy-MM-dd HH:mm:ss");
        }

        void GpsDatagramAnalyzer_OnException(object sender, Ian.UdpClient.UdpEventArg e)
        {
            //此处代码执行发生异常
            //日志文件记录当前错误
            ChangeStateMessage(OperatorCode.Error);
        }

        void GpsDatagramAnalyzer_OnAnalyzed(object sender, GpsAnalyzer.GpsDatagramEventArg e)
        {
            ChangeStateMessage(OperatorCode.Analyzed);

            if(e.Data != null && e.Data.Params != null && !string.IsNullOrWhiteSpace(e.Data.Params.DeviceID))
            {
                UpdateListView(e.Data);

                //此处做数据调用处理//
                OperateDatagram(e.Data);
            }
        }

        private async void OperateDatagram(Tiger.GpsAnalyzer.GpsDatagramModel data)
        {
            Tiger.GpsAnalyzer.GpsDatagramModel.GpsParamsDatagramModel p = data.Params;
            Model.Officer officer = Program.GetOfficerId(p.DeviceID);
            Model.Point point = Program.ELatLng2EPoint((double)(p.Latitude / GPSPARAME), (double)(p.Longitude / GPSPARAME));

            Model.GpsDeviceTrack track = new Model.GpsDeviceTrack()
            {
                CurrentTime = p.Time,
                DeviceID = p.DeviceID,
                OfficerNum = (officer == null) ? 
                    null :
                    (string.IsNullOrWhiteSpace(officer.OfficerID) ? officer.CarNum : officer.OfficerID),
                X = point.X,
                Y = point.Y
            };

            try
            {
                await Task.Factory.StartNew(() =>
                {
                    Program.Client.Post<int, Model.GpsDeviceTrack>("AddDeviceTrack", "GlobalPositionSystem", track);
                });
            }
            catch (Exception) { }
        }

        void GpsDatagramAnalyzer_OnReceived(object sender, Ian.UdpClient.UdpEventArg e)
        {
            //此处标识接收到数据
            ChangeStateMessage(OperatorCode.Received);
        }

        void GpsDatagramAnalyzer_OnClosed(object sender, Ian.UdpClient.UdpEventArg e)
        {
            //此处标识网络数据服务关闭
            ChangeStateMessage(OperatorCode.Closed);
            ChangeBtnStatue(true);
        }

        void GpsDatagramAnalyzer_OnOpened(object sender, Ian.UdpClient.UdpEventArg e)
        {
            Ian.UdpClient.Client client = sender as Ian.UdpClient.Client;
            ChangeServiceIp(client == null ? null : client.LocalEndPoint);

            //此处标识网络数据服务打开
            ChangeStateMessage(OperatorCode.Opened);
            ChangeBtnStatue(false);
        }

        private void ChangeServiceIp(string localEP)
        {
            if (this.txtIpv4.InvokeRequired)
            {
                Action<string> action = new Action<string>(ChangeServiceIp);
                this.Invoke(action, localEP);
                return;
            }

            if (!string.IsNullOrWhiteSpace(localEP))
                this.txtIpv4.Text = localEP.Split(':')[0];
        }

        private void DisplayAt(string msg)
        {
            if(listMessage.InvokeRequired)
            {
                Action<string> action = new Action<string>(DisplayAt);
                this.Invoke(action, msg);
                return;
            }
            int count = this.listMessage.Items.Count;
            this.listMessage.Items[count - 1].SubItems[1].Text = msg;

            if (count == 8)
                this.listMessage.Items.RemoveAt(0);

            InsertTipDisplayAt();
        }

        private void InsertTipDisplayAt()
        {
            ListViewItem item = new ListViewItem(new string[] { MESSAGEPREFIX, "" });
            this.listMessage.Items.Add(item);
        }

        //获取本地Ipv4地址
        private string GetLocalIpv4()
        {
            string hostname = System.Net.Dns.GetHostName();
            System.Net.IPAddress[] addresses = System.Net.Dns.GetHostAddresses(hostname);
            System.Net.IPAddress addr = addresses.FirstOrDefault(t =>
                t.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
            return addr.ToString();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            GpsDatagramAnalyzer.Open(_Port);
            ChangeStateMessage(OperatorCode.Opening);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            GpsDatagramAnalyzer.Close();
            ChangeStateMessage(OperatorCode.Closing);
        }

        private void ChangeStateMessage(OperatorCode code)
        {
            if(this.statusStrip1.InvokeRequired)
            {
                Action<OperatorCode> action = new Action<OperatorCode>(ChangeStateMessage);
                this.Invoke(action, code);
                return;
            }

            switch (code)
            {
                case OperatorCode.Opening:
                    this.statueMessageLabel.Text = "正在打开网络服务 ...";
                    this.btnStart.Enabled = false;
                    break;
                case OperatorCode.Opened:
                    this.statueMessageLabel.Text = "网络服务已经就绪 .";
                    break;
                case OperatorCode.Closing:
                    this.statueMessageLabel.Text = "正在停止网络服务 ...";
                    this.btnStop.Enabled = false;
                    break;
                case OperatorCode.Closed:
                    this.statueMessageLabel.Text = "网络服务已经停止 .";
                    this.statueProgressbar.Value = 0;
                    break;
                case OperatorCode.Received:
                    this.statueMessageLabel.Text = "接收到网络协议（UDP）数据报，开始解析数据报 .";
                    this.statueProgressbar.Value = 0;
                    this.statueProgressbar.Value = 50;
                    break;
                case OperatorCode.Analyzed:
                    this.statueMessageLabel.Text = "接收网络协议（UDP）数据报，并解析 .";
                    this.statueProgressbar.Value = 100;
                    break;
                case OperatorCode.Error:
                    this.statueMessageLabel.Text = "发生错误，重新接收数据 .";
                    break;
                default:
                    this.statueMessageLabel.Text = "以就绪，等待操作 .";
                    break;
            }

            DisplayAt(this.statueMessageLabel.Text);
        }

        //操作方式
        enum OperatorCode : byte
        {
            None = 0x00,
            Closing,
            Closed,
            Opening,
            Opened,
            Received,
            Analyzed,
            Error,
        }
    }
}
