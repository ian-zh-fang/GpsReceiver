using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tiger.GpsReceiver
{
    static class Program
    {
        //Http 请求服务
        public readonly static Ian.HttpClient.SimpleClient Client = new Ian.HttpClient.SimpleClient();

        private readonly static double Lat_Intercept = 28.1295295988415d;
        private readonly static double Lat_Variable1 = 0.0000013749824444732d;
        private readonly static double Lat_Variable2 = -1.96814028627638E-06d;

        private readonly static double Lng_Intercept = 106.779551091342d;
        private readonly static double Lng_Variable1 = 1.65108712853934E-06d;
        private readonly static double Lng_Variable2 = 2.57289803037774E-06d;

        private readonly static double X_Intercept = -41625228.1212446d;
        private readonly static double X_Variable1 = 379077.48425634d;
        private readonly static double X_Variable2 = 289961.487137384d;

        private readonly static double Y_Intercept = -14787167.9659441d;
        private readonly static double Y_Variable1 = -243261.514199763d;
        private readonly static double Y_Variable2 = 202566.881067585d;

        //警员信息
        private static List<Model.Officer> _Officers = new List<Model.Officer>();

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Action action = new Action(LoadOfficer);
                FormLoad form = new FormLoad(action, 30000);
                form.Text = "正在连接远程服务器，请稍等 ...";
                form.OnCompleted += form_OnCompleted;
                form.OnTimeout += form_OnTimeout;
                Application.Run(form);
            }
            catch (Exception) { }
        }

        static void form_OnTimeout()
        {
            MessageBox.Show("远程服务器连接失败，请查看网络是否正常。", "系统提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        static void form_OnCompleted()
        {
            Form1 form = new Form1();
            form.ShowDialog();
        }

        //加载警员数据
        static void LoadOfficer()
        {
            Ian.HttpClient.SimpleClient.ExecuteResult<List<Model.Officer>> result
                = Client.Get<List<Model.Officer>>("GetAllDevices", "GlobalPositionSystem", null);
            if (result.Status == 200)
                _Officers = result.Result;
        }

        /// <summary>
        /// 获取指定设备ID的警员绑定信息
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public static Model.Officer GetOfficerId(string deviceId)
        {
            if (string.IsNullOrWhiteSpace(deviceId))
                return null;

            return _Officers.FirstOrDefault(t => t.DeviceID == deviceId);
        }

        public static Model.Point ELatLng2EPoint(double x, double y)
        {
            Model.Point point = new Model.Point()
            {
                X = (X_Variable1 * x) + (X_Variable2 * y) + X_Intercept,
                Y = (Y_Variable1 * y) + (Y_Variable2 * y) + Y_Intercept
            };            

            return point;
        }

        public static Model.Point EPoint2ELatLng(double x, double y)
        {
            Model.Point point = new Model.Point()
            {
                X = (Lat_Variable1 * x) + (Lat_Variable2 * y) + Lat_Intercept,
                Y = (Lng_Variable1 * y) + (Lng_Variable1 * y) + Lng_Intercept
            };

            return point;
        }
    }
}
