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
    public partial class FormLoad : Form
    {
        public event Action OnCompleted;
        public event Action OnTimeout;

        private const int TIMERINTERVAL = 100;
        private readonly int _MaxTimeSize;
        private readonly int _StepLength;
        private readonly Action _Action;

        private bool _IsCompleted;
        private int _CompleteLength;

        public FormLoad()
        {
            InitializeComponent();
            this.timer1.Interval = TIMERINTERVAL;

            _MaxTimeSize = 10000;
            _StepLength = _MaxTimeSize / TIMERINTERVAL;
            this.progressBar1.Maximum = _MaxTimeSize;
            _Action = new Action(ActionCallback);
        }

        public FormLoad(Action action, int maxTimeSize)
        {
            InitializeComponent();
            this.timer1.Interval = TIMERINTERVAL;

            _Action = action;
            _MaxTimeSize = maxTimeSize;
            _StepLength = _MaxTimeSize / TIMERINTERVAL;
            this.progressBar1.Maximum = _MaxTimeSize;
        }

        //空回调函数
        private void ActionCallback() {  }

        private void FormLoad_Load(object sender, EventArgs e)
        {
            timer1.Start();
            ExecuteAsync();
        }

        private async void ExecuteAsync()
        {
            try
            {
                Action action = new Action(ExecureCoreAsync);
                await Task.Factory.StartNew(action);
            }
            catch (Exception){ }
        }

        private void ExecureCoreAsync()
        {
            try
            {
                Task t = Task.Factory.StartNew(_Action);
                _IsCompleted = t.Wait(_MaxTimeSize);
            }
            catch (AggregateException e)
            {
                e.Flatten().Handle(t => true);
            }
            catch (Exception)
            {
                //在此记录日志信息
            }
            finally
            {

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Task t = null;
            if (!_IsCompleted)
            {
                _CompleteLength += _StepLength;
                if (_MaxTimeSize >= _CompleteLength)
                {
                    this.progressBar1.Value = _CompleteLength;
                    return;
                }

                timer1.Stop();
                //触发 Timeout 事件
                t = Task.Factory.StartNew(() =>
                {
                    if (null != OnTimeout)
                        OnTimeout();
                });
                t.Wait();
                this.Close();
                return;
            }

            //此处标识程序调用结束，需要释放当前资源            
            timer1.Stop();
            this.Hide();

            if (null != OnCompleted)
                OnCompleted();
            this.Close();
        }
    }
}
