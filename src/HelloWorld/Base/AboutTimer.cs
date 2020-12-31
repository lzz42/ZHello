namespace ZHello.Base
{
    public class AboutTimer
    {
        private System.Windows.Forms.Timer FormTimer;
        private System.Threading.Timer ThreadTimer;
        private System.Timers.Timer TimersTimer;

        private void InitTimer()
        {
            FormTimer = new System.Windows.Forms.Timer();
            FormTimer.Interval = 1000;
            //在主线程上执行
            FormTimer.Tick += (s, e) =>
            {
                FormTimerCallBack();
            };
            //在ThreadPool上申请线程执行响应函数
            ThreadTimer = new System.Threading.Timer(ThreadTimerCallBack);
            ThreadTimer.Change(1000, 1000);
            //
            TimersTimer = new System.Timers.Timer();
            TimersTimer.Interval = 1000d;
            TimersTimer.Elapsed += (s, e) =>
            {
                TimersTimerCallBack();
            };
        }

        private void FormTimerCallBack()
        {
        }

        private void ThreadTimerCallBack(object state)
        {
        }

        private void TimersTimerCallBack()
        {
        }
    }
}